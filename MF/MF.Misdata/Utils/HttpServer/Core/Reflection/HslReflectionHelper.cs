using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using HslCommunication.Core;
using Newtonsoft.Json.Linq;
using HslCommunication.MQTT;

#if !NET35 && !NET20

using System.Threading.Tasks;

#endif

namespace HslCommunication.Reflection
{
    /// <summary>
    /// 反射的辅助类
    /// </summary>
    public class HslReflectionHelper
    {
        #region Parameters From json

        /// <summary>
        /// 从Json数据里解析出真实的数据信息，根据方法参数列表的类型进行反解析，然后返回实际的数据数组<br />
        /// Analyze the real data information from the Json data, perform de-analysis according to the type of the method parameter list,
        /// and then return the actual data array
        /// </summary>
        /// <param name="context">当前的会话内容</param>
        /// <param name="parameters">提供的参数列表信息</param>
        /// <param name="json">参数变量信息</param>
        /// <returns>已经填好的实际数据的参数数组对象</returns>
        public static object[] GetParametersFromJson(ISessionContext context, ParameterInfo[] parameters, string json)
        {
            JObject jObject = string.IsNullOrEmpty(json) ? new JObject() : JObject.Parse(json);
            object[] paras = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(byte)) paras[i] = jObject.Value<byte>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(short)) paras[i] = jObject.Value<short>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(ushort)) paras[i] = jObject.Value<ushort>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(int)) paras[i] = jObject.Value<int>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(uint)) paras[i] = jObject.Value<uint>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(long)) paras[i] = jObject.Value<long>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(ulong)) paras[i] = jObject.Value<ulong>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(double)) paras[i] = jObject.Value<double>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(float)) paras[i] = jObject.Value<float>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(bool)) paras[i] = jObject.Value<bool>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(string)) paras[i] = jObject.Value<string>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(DateTime)) paras[i] = jObject.Value<DateTime>(parameters[i].Name);
                else if (parameters[i].ParameterType == typeof(byte[])) paras[i] = jObject.Value<string>(parameters[i].Name).ToHexBytes();
                else if (parameters[i].ParameterType == typeof(short[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<short>()).ToArray();
                else if (parameters[i].ParameterType == typeof(ushort[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<ushort>()).ToArray();
                else if (parameters[i].ParameterType == typeof(int[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<int>()).ToArray();
                else if (parameters[i].ParameterType == typeof(uint[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<uint>()).ToArray();
                else if (parameters[i].ParameterType == typeof(long[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<long>()).ToArray();
                else if (parameters[i].ParameterType == typeof(ulong[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<ulong>()).ToArray();
                else if (parameters[i].ParameterType == typeof(float[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<float>()).ToArray();
                else if (parameters[i].ParameterType == typeof(double[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<double>()).ToArray();
                else if (parameters[i].ParameterType == typeof(bool[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<bool>()).ToArray();
                else if (parameters[i].ParameterType == typeof(string[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<string>()).ToArray();
                else if (parameters[i].ParameterType == typeof(DateTime[])) paras[i] = jObject[parameters[i].Name].ToArray().Select(m => m.Value<DateTime>()).ToArray();
                else if (parameters[i].ParameterType == typeof(ISessionContext)) paras[i] = context;
                else if (parameters[i].ParameterType.IsArray) paras[i] = ((JArray)jObject[parameters[i].Name]).ToObject(parameters[i].ParameterType);
                else if (parameters[i].ParameterType == typeof(JObject))
                {
                    // 如果定义了JSON类型的对象，就尝试再json对象及其字符串表述形式下都解析一次。
                    try
                    {
                        paras[i] = (JObject)jObject[parameters[i].Name];
                    }
                    catch
                    {
                        paras[i] = JObject.Parse(jObject.Value<string>(parameters[i].Name));
                    }
                }
                else
                {
                    try
                    {
                        paras[i] = jObject[parameters[i].Name].ToObject(parameters[i].ParameterType);
                    }
                    catch
                    {
                        paras[i] = JObject.Parse(jObject.Value<string>(parameters[i].Name)).ToObject(parameters[i].ParameterType);
                    }
                }
                //else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}"  );
            }
            return paras;
        }

        /// <summary>
        /// 从url数据里解析出真实的数据信息，根据方法参数列表的类型进行反解析，然后返回实际的数据数组<br />
        /// Analyze the real data information from the url data, perform de-analysis according to the type of the method parameter list,
        /// and then return the actual data array
        /// </summary>
        /// <param name="context">当前的会话内容</param>
        /// <param name="parameters">提供的参数列表信息</param>
        /// <param name="url">参数变量信息</param>
        /// <returns>已经填好的实际数据的参数数组对象</returns>
        public static object[] GetParametersFromUrl(ISessionContext context, ParameterInfo[] parameters, string url)
        {
            if (url.IndexOf('?') > 0) url = url.Substring(url.IndexOf('?') + 1);
            string[] splits = url.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> dict = new Dictionary<string, string>(splits.Length);
            for (int i = 0; i < splits.Length; i++)
            {
                if (!string.IsNullOrEmpty(splits[i]))
                {
                    if (splits[i].IndexOf('=') > 0)
                    {
                        dict.Add(splits[i].Substring(0, splits[i].IndexOf('=')).Trim(' '), splits[i].Substring(splits[i].IndexOf('=') + 1));
                    }
                }
            }

            object[] paras = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(byte)) paras[i] = byte.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(short)) paras[i] = short.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(ushort)) paras[i] = ushort.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(int)) paras[i] = int.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(uint)) paras[i] = uint.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(long)) paras[i] = long.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(ulong)) paras[i] = ulong.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(double)) paras[i] = double.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(float)) paras[i] = float.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(bool)) paras[i] = bool.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(string)) paras[i] = dict[parameters[i].Name];
                else if (parameters[i].ParameterType == typeof(DateTime)) paras[i] = DateTime.Parse(dict[parameters[i].Name]);
                else if (parameters[i].ParameterType == typeof(byte[])) paras[i] = dict[parameters[i].Name].ToHexBytes();
                else if (parameters[i].ParameterType == typeof(short[])) paras[i] = dict[parameters[i].Name].ToStringArray<short>();
                else if (parameters[i].ParameterType == typeof(ushort[])) paras[i] = dict[parameters[i].Name].ToStringArray<ushort>();
                else if (parameters[i].ParameterType == typeof(int[])) paras[i] = dict[parameters[i].Name].ToStringArray<int>();
                else if (parameters[i].ParameterType == typeof(uint[])) paras[i] = dict[parameters[i].Name].ToStringArray<uint>();
                else if (parameters[i].ParameterType == typeof(long[])) paras[i] = dict[parameters[i].Name].ToStringArray<long>();
                else if (parameters[i].ParameterType == typeof(ulong[])) paras[i] = dict[parameters[i].Name].ToStringArray<ulong>();
                else if (parameters[i].ParameterType == typeof(float[])) paras[i] = dict[parameters[i].Name].ToStringArray<float>();
                else if (parameters[i].ParameterType == typeof(double[])) paras[i] = dict[parameters[i].Name].ToStringArray<double>();
                else if (parameters[i].ParameterType == typeof(bool[])) paras[i] = dict[parameters[i].Name].ToStringArray<bool>();
                else if (parameters[i].ParameterType == typeof(string[])) paras[i] = dict[parameters[i].Name].ToStringArray<string>();
                else if (parameters[i].ParameterType == typeof(DateTime[])) paras[i] = dict[parameters[i].Name].ToStringArray<DateTime>();
                else if (parameters[i].ParameterType == typeof(ISessionContext)) paras[i] = context;
                else paras[i] = JToken.Parse(dict[parameters[i].Name]).ToObject(parameters[i].ParameterType);
                //else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}"  );
            }
            return paras;
        }

        /// <summary>
        /// 从方法的参数列表里，提取出实际的示例参数信息，返回一个json对象，注意：该数据是示例的数据，具体参数的限制参照服务器返回的数据声明。<br />
        /// From the parameter list of the method, extract the actual example parameter information, and return a json object. Note: The data is the example data,
        /// and the specific parameter restrictions refer to the data declaration returned by the server.
        /// </summary>
        /// <param name="method">当前需要解析的方法名称</param>
        /// <param name="parameters">当前的参数列表信息</param>
        /// <returns>当前的参数对象信息</returns>
        public static JObject GetParametersFromJson(MethodInfo method, ParameterInfo[] parameters)
        {
            JObject jObject = new JObject();
            for (int i = 0; i < parameters.Length; i++)
            {
#if NET20 || NET35
				if      (parameters[i].ParameterType == typeof( byte ))       jObject.Add( parameters[i].Name, new JValue( default( byte ) ) );
				else if (parameters[i].ParameterType == typeof( short ))      jObject.Add( parameters[i].Name, new JValue( default( short ) ) );
				else if (parameters[i].ParameterType == typeof( ushort ))     jObject.Add( parameters[i].Name, new JValue( default( ushort ) ) );
				else if (parameters[i].ParameterType == typeof( int ))        jObject.Add( parameters[i].Name, new JValue( default( int ) ) );
				else if (parameters[i].ParameterType == typeof( uint ))       jObject.Add( parameters[i].Name, new JValue( default( uint ) ) );
				else if (parameters[i].ParameterType == typeof( long ))       jObject.Add( parameters[i].Name, new JValue( default( long ) ) );
				else if (parameters[i].ParameterType == typeof( ulong ))      jObject.Add( parameters[i].Name, new JValue( default( ulong ) ) );
				else if (parameters[i].ParameterType == typeof( double ))     jObject.Add( parameters[i].Name, new JValue( default( double ) ) );
				else if (parameters[i].ParameterType == typeof( float ))      jObject.Add( parameters[i].Name, new JValue( default( float ) ) );
				else if (parameters[i].ParameterType == typeof( bool ))       jObject.Add( parameters[i].Name, new JValue( default( bool ) ) );
				else if (parameters[i].ParameterType == typeof( string ))     jObject.Add( parameters[i].Name, new JValue( "" ) );
				else if (parameters[i].ParameterType == typeof( DateTime ))   jObject.Add( parameters[i].Name, new JValue( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ) );
				else if (parameters[i].ParameterType == typeof( byte[] ))     jObject.Add( parameters[i].Name, new JValue( "00 1A 2B 3C 4D" ) );
				else if (parameters[i].ParameterType == typeof( short[] ))    jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( ushort[] ))   jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( int[] ))      jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( uint[] ))     jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( long[] ))     jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( ulong[] ))    jObject.Add( parameters[i].Name, new JArray( new int[] { 1, 2, 3 } ) );
				else if (parameters[i].ParameterType == typeof( float[] ))    jObject.Add( parameters[i].Name, new JArray( new float[] { 1f, 2f, 3f } ) );
				else if (parameters[i].ParameterType == typeof( double[] ))   jObject.Add( parameters[i].Name, new JArray( new double[] { 1d, 2d, 3d } ) );
				else if (parameters[i].ParameterType == typeof( bool[] ))     jObject.Add( parameters[i].Name, new JArray( new bool[] { true, false, false } ) );
				else if (parameters[i].ParameterType == typeof( string[] ))   jObject.Add( parameters[i].Name, new JArray( new string[] { "1", "2", "3" } ) );
				else if (parameters[i].ParameterType == typeof( DateTime[] )) jObject.Add( parameters[i].Name, new JArray( new string[] { DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) } ) );
				else if (parameters[i].ParameterType == typeof( ISessionContext )) continue;
				else if (parameters[i].ParameterType.IsArray)                 jObject.Add( parameters[i].Name,  JToken.FromObject( GetObjFromArrayParameterType( parameters[i].ParameterType ) ) );
				else jObject.Add( parameters[i].Name, JToken.FromObject( Activator.CreateInstance( parameters[i].ParameterType ) ) );
				//else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}" );
#else
                if (parameters[i].ParameterType == typeof(byte)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (byte)parameters[i].DefaultValue : default(byte)));
                else if (parameters[i].ParameterType == typeof(short)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (short)parameters[i].DefaultValue : default(short)));
                else if (parameters[i].ParameterType == typeof(ushort)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (ushort)parameters[i].DefaultValue : default(ushort)));
                else if (parameters[i].ParameterType == typeof(int)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (int)parameters[i].DefaultValue : default(int)));
                else if (parameters[i].ParameterType == typeof(uint)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (uint)parameters[i].DefaultValue : default(uint)));
                else if (parameters[i].ParameterType == typeof(long)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (long)parameters[i].DefaultValue : default(long)));
                else if (parameters[i].ParameterType == typeof(ulong)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (ulong)parameters[i].DefaultValue : default(ulong)));
                else if (parameters[i].ParameterType == typeof(double)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (double)parameters[i].DefaultValue : default(double)));
                else if (parameters[i].ParameterType == typeof(float)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (float)parameters[i].DefaultValue : default(float)));
                else if (parameters[i].ParameterType == typeof(bool)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (bool)parameters[i].DefaultValue : default(bool)));
                else if (parameters[i].ParameterType == typeof(string)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? (string)parameters[i].DefaultValue : ""));
                else if (parameters[i].ParameterType == typeof(DateTime)) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((DateTime)parameters[i].DefaultValue).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                else if (parameters[i].ParameterType == typeof(byte[])) jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((byte[])parameters[i].DefaultValue).ToHexString() : "00 1A 2B 3C 4D"));
                else if (parameters[i].ParameterType == typeof(short[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (short[])parameters[i].DefaultValue : new short[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(ushort[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (ushort[])parameters[i].DefaultValue : new ushort[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(int[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (int[])parameters[i].DefaultValue : new int[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(uint[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (uint[])parameters[i].DefaultValue : new uint[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(long[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (long[])parameters[i].DefaultValue : new long[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(ulong[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (ulong[])parameters[i].DefaultValue : new ulong[] { 1, 2, 3 }));
                else if (parameters[i].ParameterType == typeof(float[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (float[])parameters[i].DefaultValue : new float[] { 1f, 2f, 3f }));
                else if (parameters[i].ParameterType == typeof(double[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (double[])parameters[i].DefaultValue : new double[] { 1d, 2d, 3d }));
                else if (parameters[i].ParameterType == typeof(bool[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (bool[])parameters[i].DefaultValue : new bool[] { true, false, false }));
                else if (parameters[i].ParameterType == typeof(string[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? (string[])parameters[i].DefaultValue : new string[] { "1", "2", "3" }));
                else if (parameters[i].ParameterType == typeof(DateTime[])) jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((DateTime[])parameters[i].DefaultValue).Select(m => m.ToString("yyyy-MM-dd HH:mm:ss")).ToArray() : new string[] { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }));
                else if (parameters[i].ParameterType == typeof(ISessionContext)) continue;
                else if (parameters[i].ParameterType.IsArray) jObject.Add(parameters[i].Name, parameters[i].HasDefaultValue ? JToken.FromObject(parameters[i].DefaultValue) : JToken.FromObject(GetObjFromArrayParameterType(parameters[i].ParameterType)));
                else jObject.Add(parameters[i].Name, JToken.FromObject(parameters[i].HasDefaultValue ? parameters[i].DefaultValue : Activator.CreateInstance(parameters[i].ParameterType)));
                // else throw new Exception( $"Can't support parameter [{parameters[i].Name}] type : {parameters[i].ParameterType}" );
#endif
            }
            return jObject;
        }

        private static object GetObjFromArrayParameterType(Type parameterType)
        {
            Type actualType = null;
            Type[] types = parameterType.GetGenericArguments();
            if (types.Length > 0)
            {
                actualType = types[0];
            }
            else
            {
                actualType = parameterType.GetElementType();
            }
            Array array = Array.CreateInstance(actualType, 3);
            for (int j = 0; j < 3; j++)
            {
                array.SetValue(Activator.CreateInstance(actualType), j);
            }
            return array;
        }

        /// <summary>
        /// 将一个对象转换成 <see cref="OperateResult{T}"/> 的string 类型的对象，用于远程RPC的数据交互
        /// </summary>
        /// <param name="obj">自定义的对象</param>
        /// <returns>转换之后的结果对象</returns>
        public static OperateResult<string> GetOperateResultJsonFromObj(object obj)
        {
            if (obj is OperateResult result)
            {
                OperateResult<string> ret = new OperateResult<string>();
                ret.IsSuccess = result.IsSuccess;
                ret.ErrorCode = result.ErrorCode;
                ret.Message = result.Message;

                if (result.IsSuccess)
                {
                    var property = obj.GetType().GetProperty("Content");
                    if (property != null)
                    {
                        var retObject = property.GetValue(obj, null);
                        if (retObject != null) ret.Content = retObject.ToJsonString();
                        return ret;
                    }

                    var propertyContent1 = obj.GetType().GetProperty("Content1");
                    if (propertyContent1 == null) return ret;

                    var propertyContent2 = obj.GetType().GetProperty("Content2");
                    if (propertyContent2 == null)
                    {
                        ret.Content = new { Content1 = propertyContent1.GetValue(obj, null) }.ToJsonString();
                        return ret;
                    }

                    var propertyContent3 = obj.GetType().GetProperty("Content3");
                    if (propertyContent3 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent4 = obj.GetType().GetProperty("Content4");
                    if (propertyContent4 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent5 = obj.GetType().GetProperty("Content5");
                    if (propertyContent5 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent6 = obj.GetType().GetProperty("Content6");
                    if (propertyContent6 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent7 = obj.GetType().GetProperty("Content7");
                    if (propertyContent7 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent8 = obj.GetType().GetProperty("Content8");
                    if (propertyContent8 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent9 = obj.GetType().GetProperty("Content9");
                    if (propertyContent9 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                            Content8 = propertyContent8.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }

                    var propertyContent10 = obj.GetType().GetProperty("Content10");
                    if (propertyContent10 == null)
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                            Content8 = propertyContent8.GetValue(obj, null),
                            Content9 = propertyContent9.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }
                    else
                    {
                        ret.Content = new
                        {
                            Content1 = propertyContent1.GetValue(obj, null),
                            Content2 = propertyContent2.GetValue(obj, null),
                            Content3 = propertyContent3.GetValue(obj, null),
                            Content4 = propertyContent4.GetValue(obj, null),
                            Content5 = propertyContent5.GetValue(obj, null),
                            Content6 = propertyContent6.GetValue(obj, null),
                            Content7 = propertyContent7.GetValue(obj, null),
                            Content8 = propertyContent8.GetValue(obj, null),
                            Content9 = propertyContent9.GetValue(obj, null),
                            Content10 = propertyContent10.GetValue(obj, null),
                        }.ToJsonString();
                        return ret;
                    }
                }
                return ret;
            }
            else
            {
                return OperateResult.CreateSuccessResult(obj == null ? string.Empty : obj.ToJsonString());
            }
        }

        #endregion Parameters From json
    }
}