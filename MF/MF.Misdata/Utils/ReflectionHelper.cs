using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Common.Utils
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 实例缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> instances = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 方法缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> methods = new ConcurrentDictionary<string, object>();

        private ReflectionHelper()
        {
        }

        /// <summary>
        /// 通过反射调用动态连接库方法
        /// </summary>
        /// <param name="assembly">程序集(DLL)</param>
        /// <param name="Namespace">命名空间名称，如果无命名空间写“”</param>
        /// <param name="Classname">类名</param>
        /// <param name="Method">方法名</param>
        /// <param name="Paramsters">函数参数数组</param>
        /// <param name="errmsg">错误消息</param>
        public static bool ReflectionMethod(Assembly assembly,
                                            string Namespace,
                                            string Classname,
                                            string Method,
                                            object[] Paramsters,
                                            out string errmsg)
        {
            string instancekey = $"{assembly.FullName}{Namespace}{Classname}";
            string methodkey = $"{assembly.FullName}{Namespace}{Classname}{Method}";
            errmsg = "";
            //缓存方法
            if (instances.ContainsKey(instancekey))
            {
                if (methods.ContainsKey(methodkey))
                {
                    try
                    {
                        object instanceObject = instances[instancekey];
                        MethodInfo mi = methods[methodkey] as MethodInfo;
                        if (mi != null)
                        {
                            mi.Invoke(instanceObject, Paramsters);
                            return true;
                        }
                        else
                        {
                            errmsg = "方法不存在";
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        errmsg = e.Message;
                        return false;
                    }
                }
            }

            try
            {
                Type t;
                //类
                if (Namespace == "")
                {
                    t = assembly.GetType(Classname);
                }
                else
                {
                    t = assembly.GetType($"{Namespace}.{Classname}");
                }
                if (t == null)
                {
                    errmsg = "类不存在";
                    return false;
                }
                MethodInfo mi = t.GetMethod(Method); //获取方法
                methods[methodkey] = mi;

                if (mi == null)
                {
                    errmsg = "方法不存在";
                    return false;
                }
                ParameterInfo[] paramsInfo = mi.GetParameters();
                for (int i = 0; i < paramsInfo.Length; i++)
                {
                    Type tType = paramsInfo[i].ParameterType;
                    //改变参数类型
                    Paramsters[i] = SD_ChanageType(Paramsters[i], tType);
                }

                // 实例化
                object instanceObject = Activator.CreateInstance(t);
                instances[instancekey] = instanceObject;
                mi.Invoke(instanceObject, Paramsters);
            }
            catch (Exception e)
            {
                methods.TryRemove(methodkey, out _);
                instances.TryRemove(instancekey, out _);
                errmsg = e.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertsionType"></param>
        /// <returns></returns>
        private static object SD_ChanageType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类, //判断convertsionType是否为nullable泛型类
            if (convertsionType.IsGenericType && convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }
    }
}