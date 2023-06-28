using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#if !NET35 && !NET20

using System.Threading.Tasks;

#endif

namespace HslCommunication.MQTT
{
    /// <summary>
    /// Mqtt协议的辅助类，提供了一些协议相关的基础方法，方便客户端和服务器端一起调用。<br />
    /// The auxiliary class of the Mqtt protocol provides some protocol-related basic methods for the client and server to call together.
    /// </summary>
    public class MqttHelper
    {
        #region Static Helper Method

        public static List<MqttRpcApiInfo> GetSyncServicesApiInformationFromObject(object obj)
        {
            if (obj is Type type)
                return GetSyncServicesApiInformationFromObject(type.Name, type);
            else
                return GetSyncServicesApiInformationFromObject(obj.GetType().Name, obj);
        }

        /// <summary>
        /// 根据当前的对象定义的方法信息，获取到所有支持ApiTopic的方法列表信息，包含API名称，示例参数数据，描述信息。<br />
        /// According to the method information defined by the current object, the list information of all methods that support ApiTopic is obtained,
        /// including the API name, sample parameter data, and description information.
        /// </summary>
        /// <param name="api">指定的ApiTopic的前缀，可以理解为控制器，如果为空，就不携带控制器。</param>
        /// <param name="obj">实际的等待解析的对象</param>
        /// <param name="permissionAttribute">默认的权限特性</param>
        /// <returns>返回所有API说明的列表，类型为<see cref="MqttRpcApiInfo"/></returns>
        public static List<MqttRpcApiInfo> GetSyncServicesApiInformationFromObject(string api, object obj, HslMqttPermissionAttribute permissionAttribute = null)
        {
            Type objType = null;
            if (obj is Type type)   // 表示注册的是静态的方法或是静态属性
            {
                objType = type;
                obj = null;
            }
            else
            {
                objType = obj.GetType();
            }

            MethodInfo[] methodInfos = objType.GetMethods();
            List<MqttRpcApiInfo> mqttSyncServices = new List<MqttRpcApiInfo>();
            foreach (var method in methodInfos)
            {
                var apiResult = GetMqttSyncServicesApiFromMethod(api, method, obj, permissionAttribute);
                if (!apiResult.IsSuccess) continue;

                mqttSyncServices.Add(apiResult.Content);
            }
            PropertyInfo[] propertyInfos = objType.GetProperties();
            foreach (var property in propertyInfos)
            {
                var apiResult = GetMqttSyncServicesApiFromProperty(api, property, obj, permissionAttribute);
                if (!apiResult.IsSuccess) continue;

                if (!apiResult.Content1.PropertyUnfold)
                    mqttSyncServices.Add(apiResult.Content2);
                else
                {
                    if (property.GetValue(obj, null) == null) continue;
                    var apis = GetSyncServicesApiInformationFromObject(apiResult.Content2.ApiTopic, property.GetValue(obj, null), permissionAttribute);
                    mqttSyncServices.AddRange(apis);
                }
            }

            return mqttSyncServices;
        }

        private static string GetReturnTypeDescription(Type returnType)
        {
            if (returnType.IsSubclassOf(typeof(OperateResult)))
            {
                if (returnType == typeof(OperateResult)) return returnType.Name;
                if (returnType.GetProperty("Content") != null)
                {
                    return $"OperateResult<{returnType.GetProperty("Content").PropertyType.Name}>";
                }
                else
                {
                    StringBuilder sb = new StringBuilder("OperateResult<");
                    for (int i = 1; i <= 10; i++)
                    {
                        if (returnType.GetProperty("Content" + i.ToString()) != null)
                        {
                            if (i != 1) sb.Append(",");
                            sb.Append(returnType.GetProperty("Content" + i.ToString()).PropertyType.Name);
                        }
                        else
                        {
                            break;
                        }
                    }
                    sb.Append(">");
                    return sb.ToString();
                }
            }
            else
            {
                return returnType.Name;
            }
        }

        /// <summary>
        /// 根据当前的方法的委托信息和类对象，生成<see cref="MqttRpcApiInfo"/>的API对象信息。
        /// </summary>
        /// <param name="api">Api头信息</param>
        /// <param name="method">方法的委托</param>
        /// <param name="obj">当前注册的API的源对象</param>
        /// <param name="permissionAttribute">默认的权限特性</param>
        /// <returns>返回是否成功的结果对象</returns>
        public static OperateResult<MqttRpcApiInfo> GetMqttSyncServicesApiFromMethod(string api, MethodInfo method, object obj, HslMqttPermissionAttribute permissionAttribute = null)
        {
            object[] attrs = method.GetCustomAttributes(typeof(HslMqttApiAttribute), false);
            if (attrs == null || attrs.Length == 0) return new OperateResult<MqttRpcApiInfo>($"Current Api ：[{method}] not support Api attribute");

            HslMqttApiAttribute apiAttribute = (HslMqttApiAttribute)attrs[0];
            MqttRpcApiInfo apiInformation = new MqttRpcApiInfo();
            apiInformation.SourceObject = obj;
            apiInformation.Method = method;
            apiInformation.Description = apiAttribute.Description;
            apiInformation.HttpMethod = apiAttribute.HttpMethod.ToUpper();
            if (string.IsNullOrEmpty(apiAttribute.ApiTopic)) apiAttribute.ApiTopic = method.Name;

            if (permissionAttribute == null)
            {
                attrs = method.GetCustomAttributes(typeof(HslMqttPermissionAttribute), false);
                if (attrs?.Length > 0) apiInformation.PermissionAttribute = (HslMqttPermissionAttribute)attrs[0];
            }
            else
            {
                apiInformation.PermissionAttribute = permissionAttribute;
            }

            if (string.IsNullOrEmpty(api))
                apiInformation.ApiTopic = apiAttribute.ApiTopic;
            else
                apiInformation.ApiTopic = api + "/" + apiAttribute.ApiTopic;
            var parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
#if NET20 || NET35
			sb.Append( GetReturnTypeDescription( method.ReturnType ) );
#else
            if (method.ReturnType.IsSubclassOf(typeof(Task)))
                sb.Append($"Task<{GetReturnTypeDescription(method.ReturnType.GetProperty("Result").PropertyType)}>");
            else
                sb.Append(GetReturnTypeDescription(method.ReturnType));
#endif
            sb.Append(" ");
            sb.Append(apiInformation.ApiTopic);
            sb.Append("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != typeof(ISessionContext))
                {
                    sb.Append(parameters[i].ParameterType.Name);
                    sb.Append(" ");
                    sb.Append(parameters[i].Name);
                    if (i != parameters.Length - 1)
                        sb.Append(",");
                }
            }
            sb.Append(")");
            apiInformation.MethodSignature = sb.ToString();

            apiInformation.ExamplePayload = HslReflectionHelper.GetParametersFromJson(method, parameters).ToString();
            return OperateResult.CreateSuccessResult(apiInformation);
        }

        /// <summary>
        /// 根据当前的方法的委托信息和类对象，生成<see cref="MqttRpcApiInfo"/>的API对象信息。
        /// </summary>
        /// <param name="api">Api头信息</param>
        /// <param name="property">方法的委托</param>
        /// <param name="obj">当前注册的API的源对象</param>
        /// <param name="permissionAttribute">默认的权限特性</param>
        /// <returns>返回是否成功的结果对象</returns>
        public static OperateResult<HslMqttApiAttribute, MqttRpcApiInfo> GetMqttSyncServicesApiFromProperty(string api, PropertyInfo property, object obj, HslMqttPermissionAttribute permissionAttribute = null)
        {
            object[] attrs = property.GetCustomAttributes(typeof(HslMqttApiAttribute), false);
            if (attrs == null || attrs.Length == 0) return new OperateResult<HslMqttApiAttribute, MqttRpcApiInfo>($"Current Api ：[{property}] not support Api attribute");

            HslMqttApiAttribute apiAttribute = (HslMqttApiAttribute)attrs[0];
            MqttRpcApiInfo apiInformation = new MqttRpcApiInfo();
            apiInformation.SourceObject = obj;
            apiInformation.Property = property;
            apiInformation.Description = apiAttribute.Description;
            apiInformation.HttpMethod = apiAttribute.HttpMethod.ToUpper();
            if (string.IsNullOrEmpty(apiAttribute.ApiTopic)) apiAttribute.ApiTopic = property.Name;

            if (permissionAttribute == null)
            {
                attrs = property.GetCustomAttributes(typeof(HslMqttPermissionAttribute), false);
                if (attrs?.Length > 0) apiInformation.PermissionAttribute = (HslMqttPermissionAttribute)attrs[0];
            }
            else
            {
                apiInformation.PermissionAttribute = permissionAttribute;
            }

            if (string.IsNullOrEmpty(api))
                apiInformation.ApiTopic = apiAttribute.ApiTopic;
            else
                apiInformation.ApiTopic = api + "/" + apiAttribute.ApiTopic;

            StringBuilder sb = new StringBuilder();
            sb.Append(GetReturnTypeDescription(property.PropertyType));
            sb.Append(" ");
            sb.Append(apiInformation.ApiTopic);
            sb.Append(" { ");
            if (property.CanRead) sb.Append("get; ");
            if (property.CanWrite) sb.Append("set; ");
            sb.Append("}");
            apiInformation.MethodSignature = sb.ToString();
            apiInformation.ExamplePayload = string.Empty;
            return OperateResult.CreateSuccessResult(apiAttribute, apiInformation);
        }

        #endregion Static Helper Method
    }
}