using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 物料拉动记录 表
    /// </summary>
    [SugarTable("sfc_pull", "物料拉动记录")]
    public class Pull : BaseEntity
    {
        public Pull()
        {
        }
        
        /// <summary>
        /// Desc:产线代码
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_code", ColumnDescription = "产线代码", IsNullable = true, Length = 100)]
        public string LineCode{ get; set; }

        /// <summary>
        /// Desc:产线名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线名称", IsNullable = true, Length = 255)]
        public string LineName{ get; set; }

        /// <summary>
        /// Desc:订单编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "订单编号", IsNullable = true, Length = 100)]
        public string OrderNum{ get; set; }

        /// <summary>
        /// Desc:工位名称
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_name", ColumnDescription = "工位名称", IsNullable = true, Length = 100)]
        public string OpName{ get; set; }

        /// <summary>
        /// Desc:工位描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "op_desc", ColumnDescription = "工位描述", IsNullable = true, Length = 64)]
        public string OpDesc{ get; set; }

        /// <summary>
        /// Desc:物料编号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编号", IsNullable = true, Length = 64)]
        public string MaterialCode{ get; set; }

        /// <summary>
        /// Desc:物料名称描述
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "material_des", ColumnDescription = "物料名称描述", IsNullable = true, Length = 255)]
        public string MaterialDes{ get; set; }

        /// <summary>
        /// Desc:包装数量
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "packages_num", ColumnDescription = "包装数量", IsNullable = true, Length = 11)]
        public int? PackagesNum{ get; set; }

        /// <summary>
        /// Desc:拉动箱数
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "pull_boxes", ColumnDescription = "拉动箱数", IsNullable = true, Length = 11)]
        public int? PullBoxes{ get; set; }

        /// <summary>
        /// Desc:拉动方式
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "pull_type", ColumnDescription = "拉动方式", IsNullable = true, Length = 64)]
        public string PullType{ get; set; }

    }
}