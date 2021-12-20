using SqlSugar;

using Common.DBUtils;
using System;

namespace Common.Entities
{
    /// <summary>
    /// 流水策略
    /// </summary>
    [SugarTable("infra_serialno")]
    public class Serialno : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///当前流水号
        /// </summary>
        [SugarColumn(ColumnName = "current_no")]
        public decimal CurrentNo { get; set; }

        /// <summary>
        ///类别
        /// </summary>
        [SugarColumn(ColumnName = "category")]
        public string Category { get; set; }

        /// <summary>
        ///当前流水号
        /// </summary>
        [SugarColumn(ColumnName = "serialno_pattern")]
        public string SerialNoPattern { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }

        /// <summary>
        ///当前天数
        /// </summary>
        [SugarColumn(ColumnName = "current_day")]
        public DateTime? CurrentDay { get; set; }

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="serialNoPattern"></param>
        /// <param name="remark"></param>
        /// <param name="nickName"></param>
        public void ChangeInfo(string name, string category, string serialNoPattern, string remark, string nickName)
        {
            this.InitEntiy(nickName);
            this.Name = name;
            this.Category = category;
            this.SerialNoPattern = serialNoPattern;
            this.Remark = remark;
        }
    }
}