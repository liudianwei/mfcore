using SqlSugar;

using MF.Orm;

namespace DAL.FactoryCenter.Entities
{
    /// <summary>
    /// 工位用户关系 表
    /// </summary>
    [SugarTable("fm_workstation_user", "工位用户关系")]
    public class WorkstationUser : BaseEntity
    {
        public WorkstationUser()
        {
        }
        
        /// <summary>
        /// Desc:用户主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户主键", IsNullable = true, Length = 36)]
        public string UserId{ get; set; }

        /// <summary>
        /// Desc:工位主键
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "workstation_id", ColumnDescription = "工位主键", IsNullable = true, Length = 36)]
        public string WorkstationId{ get; set; }

        /// <summary>
        /// Desc:租户code
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户code", IsNullable = true, Length = 64)]
        public string TenantCode{ get; set; }

    }
}