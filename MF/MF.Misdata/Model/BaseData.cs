using System.Collections.Generic;
using System.Reflection;

namespace Common.Model
{
    /// <summary>
    /// 存放基础数据的各种字典
    /// </summary>
    public class BaseData
    {
        #region public

        /// <summary>
        /// 工位号对应的dll名称集合
        /// </summary>
        /// <typeparam name="string">工位名称</typeparam>
        /// <typeparam name="string">dll名称</typeparam>
        public static Dictionary<string, string> DllNames { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 工位号对应的类名称集合
        /// </summary>
        /// <typeparam name="string">工位名称</typeparam>
        /// <typeparam name="string">类名称</typeparam>
        public static Dictionary<string, string> ClassNames { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 工位号对应所有Tag
        /// </summary>
        public static Dictionary<string, List<QualityDataType>> OPName2TagsMap { get; set; } = new Dictionary<string, List<QualityDataType>>();

        /// <summary>
        /// 所有TagID对象
        /// </summary>
        public static Dictionary<string, QualityDataType> AllTagsMap { get; set; } = new Dictionary<string, QualityDataType>();

        /// <summary>
        /// 程序集名称对应的dll实例集合
        /// </summary>
        /// <typeparam name="string">程序集名称</typeparam>
        /// <typeparam name="Assembly">dll程序集</typeparam>
        public static Dictionary<string, Assembly> Assemblys { get; set; } = new Dictionary<string, Assembly>();

        #endregion public
    }
}