using MF.Utils;
using SqlSugar;

using System;

namespace Common.DBUtils
{
    public class BaseEntity
    {
        public BaseEntity()
        {
        }

        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "id", ColumnDescription = "主键", Length = 36)]
        public virtual string Id { get; set; }

        /// <summary>
        /// Desc:乐观锁 版本号
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "inner_version", IsEnableUpdateVersionValidation = true, ColumnDataType = ("bigint"), Length = 20, ColumnDescription = "内部版本 乐观锁 标识字段,同一时间不能同时操作", DefaultValue = "0")]
        public virtual int InnerVersion { get; set; }

        /// <summary>
        /// Desc:状态：0启用 1禁用 2删除
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "state", ColumnDescription = "状态：0启用 1禁用 2删除", DefaultValue = "0", Length = 10)]
        public virtual string State { get; set; }

        /// <summary>
        /// Desc:更新人
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "updator", ColumnDescription = "更新人", Length = 64, IsNullable = true)]
        public virtual string Updator { get; set; }

        /// <summary>
        /// Desc:更新时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "update_time", ColumnDescription = "更新时间", IsNullable = true)]
        public virtual DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Desc:创建人
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "creator", IsOnlyIgnoreUpdate = true, ColumnDescription = "创建人", Length = 64, IsNullable = true)]
        public virtual string Creator { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:NULL
        /// Nullable:True
        /// </summary>
        [SugarColumn(ColumnName = "create_time", IsOnlyIgnoreUpdate = true, ColumnDescription = "创建时间", IsNullable = true)]
        public virtual DateTime? CreateTime { get; set; }

        ///// <summary>
        ///// Desc:租户code
        ///// Default:NULL
        ///// Nullable:True
        ///// </summary>
        //[SugarColumn(ColumnName = "tenant_code")]
        //public virtual string TenantCode { get; set; }

        /// <summary>
        /// 创建、新增
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="isUPdateTime">是否初始化实体时,更新时间</param>
        public virtual void InitEntiy(string nickName, bool isUPdateTime = false)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = PubId.SnowflakeId.ToString();
                State = "0";
                Creator = nickName;
                CreateTime = DateTime.Now;
                if (isUPdateTime)
                {
                    Updator = nickName;
                    UpdateTime = DateTime.Now;
                }
            }
            else
            {
                Updator = nickName;
                UpdateTime = DateTime.Now;
            }
        }

        public void Copy(BaseEntity oldEntity)
        {
            Id = oldEntity.Id;
            Creator = oldEntity.Creator;
            CreateTime = oldEntity.CreateTime;
            Updator = oldEntity.Updator;
            UpdateTime = oldEntity.UpdateTime;
            InnerVersion = oldEntity.InnerVersion;
            State = oldEntity.State;
        }
    }
}