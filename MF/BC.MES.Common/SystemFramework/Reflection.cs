using System;
using System.Reflection;
using System.ComponentModel;

namespace SystemFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class Reflection
    {
        /// <summary>
        /// 通过反射调用动态连接库方法
        /// </summary>
        /// <param name="assembly">程序集(DLL)</param>
        /// <param name="Namespace">命名空间名称，如果无命名空间写“”</param>
        /// <param name="Classname">类名</param>
        /// <param name="Method">方法名</param>
        /// <param name="Paramsters">函数参数数组</param>
        static public string ReflectionMethod(Assembly assembly, string Namespace, string Classname, string Method, object[] Paramsters)
        {
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
                    t = assembly.GetType(Namespace + "." + Classname);
                }
                if (t == null)
                {
                    return "类不存在";
                }
                ////方法
                MethodInfo mi = t.GetMethod(Method); //获取方法
                if (mi == null)
                {
                    return "方法不存在";
                }
                ParameterInfo[] paramsInfo = mi.GetParameters();
                for (int i = 0; i < paramsInfo.Length; i++)
                {
                    Type tType = paramsInfo[i].ParameterType;
                    //改变参数类型   
                    Paramsters[i] = SD_ChanageType(Paramsters[i], tType);
                }
                /// 建一个实例化的类
                object instanceObject = Activator.CreateInstance(t);
                mi.Invoke(instanceObject, Paramsters);
                return "ok";
            }
            catch (Exception err)
            {
                return err.ToString();

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertsionType"></param>
        /// <returns></returns>
        public static object SD_ChanageType(object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&
                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
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