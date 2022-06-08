using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeGen
{
    public class TableEntity
    {
        public TableEntity()
        {
        }

        private void XX()
        {
            //try
            //{
            //    var strMachineCode = MachineCode.GetMachineCodeString();
            //    Console.WriteLine($"机器码:{strMachineCode}");
            //    var item = new Esnecil().CheckMisdataCr(strMachineCode);
            //    if (!item.Item1)
            //    {
            //        var msg = $"授权失败,请联系管理员进行授权!Warning Message ===>{item.Item2}；机器码为===>{strMachineCode}";
            //        Console.WriteLine(msg);
            //        SystemLog.Fatal(msg);
            //        throw new Exception(msg);
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }
        /// <summary>
        ///
        /// </summary>
        private string preFix;

        public string PreFix
        {
            get { return preFix; }
        }

        /// <summary>
        ///
        /// </summary>
        private string tableName;

        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    tableName = value;

                    var names = tableName.Split('_');
                    if (names.Length > 1)
                    {
                        preFix = names[0];
                        preFix=Regex.Replace(preFix, @"^\w", t => t.Value.ToUpper());//首字母转大写
                    }

                    if ((dbPrefix?.ToLower() ?? "") == "true")
                    {
                        if (!string.IsNullOrEmpty(PreFix))
                        {
                            value = value.TrimStart(PreFix.ToCharArray());
                        }
                    }

                    //EntityName = StringUtils.ToEntityName(value);
                    EntityName = value.ToEntityName();
                    EntityName = EntityName.Replace("Base", "BASE");

                    //entityName = StringUtils.LowFirst(EntityName);
                    entityName = EntityName.ToLowerFirst();
                    ENTITYNAME = EntityName.ToUpper();
                    entityname = EntityName.ToLower();

                    //PreEntityName = StringUtils.ToEntityName(tableName);
                    PreEntityName = tableName.ToEntityName();
                    PreEntityName = PreEntityName.Replace("Base", "BASE");
                    //preEntityName = StringUtils.LowFirst(PreEntityName);
                    preEntityName = PreEntityName.ToLowerFirst();
                    PREENTITYNAME = PreEntityName.ToUpper();
                    preentityname = PreEntityName.ToLower();
                }
                else
                {
                    tableName = "";
                    EntityName = "";
                    entityname = "";
                    ENTITYNAME = "";
                    entityname = "";

                    PreEntityName = "";
                    preEntityName = "";
                    PREENTITYNAME = "";
                    preentityname = "";
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        public string TableComment { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string entityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ENTITYNAME { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string entityname { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PreEntityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string preEntityName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PREENTITYNAME { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string preentityname { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string _pkColumnName;

        public string pkColumnName
        {
            get
            {
                return _pkColumnName;
            }
            set
            {
                _pkColumnName = value;
                if (!string.IsNullOrEmpty(value))
                {
                    //PkFieldName = StringUtils.ToEntityName(_pkColumnName);
                    PkFieldName = _pkColumnName.ToEntityName();
                    //pkFieldName = StringUtils.LowFirst(PkFieldName);
                    pkFieldName = PkFieldName.ToLowerFirst();
                    PKFIELDNAME = PkFieldName.ToUpper();
                    pkfieldname = PkFieldName.ToLower();
                }
                else
                {
                    PkFieldName = "";
                    pkFieldName = "";
                    PKFIELDNAME = "";
                    pkfieldname = "";
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private string _pkColumnType;

        /// <summary>
        ///
        /// </summary>
        public string pkColumnType
        {
            get
            {
                return _pkColumnType;
            }
            set
            {
                _pkColumnType = value;
                pkFieldType = GetFieldType(_pkColumnType);
            }
        }

        /// <summary>
        /// 根据数据库类型 映射C# 类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetFieldType(string _columnType)
        {
            string reval;
            switch (_columnType.ToLower())
            {
                case "int":
                    reval = "int";
                    break;

                case "text":
                    reval = "string";
                    break;

                case "bigint":
                    reval = "int";
                    break;

                case "binary":
                    reval = "byte[]";
                    break;

                case "bit":
                    reval = "bool";
                    break;

                case "char":
                    reval = "string";
                    break;

                case "datetime":
                    reval = "System.DateTime";
                    break;

                case "decimal":
                    reval = "System.Decimal";
                    break;

                case "float":
                    reval = "System.Double";
                    break;

                case "image":
                    reval = "byte[]";
                    break;

                case "money":
                    reval = "System.Decimal";
                    break;

                case "nchar":
                    reval = "string";
                    break;

                case "ntext":
                    reval = "string";
                    break;

                case "numeric":
                    reval = "System.Decimal";
                    break;

                case "nvarchar":
                    reval = "string";
                    break;

                case "real":
                    reval = "System.Single";
                    break;

                case "smalldatetime":
                    reval = "System.DateTime";
                    break;

                case "smallint":
                    reval = "int16";
                    break;

                case "smallmoney":
                    reval = "System.Decimal";
                    break;

                case "timestamp":
                    reval = "System.DateTime";
                    break;

                case "tinyint":
                    reval = "byte";
                    break;

                case "uniqueidentifier":
                    reval = "System.Guid";
                    break;

                case "varbinary":
                    reval = "byte[]";
                    break;

                case "varchar":
                    reval = "string";
                    break;

                case "variant":
                    reval = "object";
                    break;

                default:
                    reval = "string";
                    break;
            }
            return reval;
        }

        /// <summary>
        ///
        /// </summary>
        public string pkColumnComment { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string pkFieldType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int pkColumnTypeLength { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string pkFieldName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PkFieldName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string pkfieldname { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PKFIELDNAME { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, ColumnEntity> Cols { get; set; }

        public string dbPrefix { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string F(string str)
        {
            return "{{$" + str + "}}  :  ";
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(F(nameof(PreFix)) + PreFix);
            sb.AppendLine(F(nameof(TableName)) + TableName);
            sb.AppendLine(F(nameof(TableComment)) + TableComment);
            sb.AppendLine();
            sb.AppendLine(F(nameof(EntityName)) + EntityName);
            sb.AppendLine(F(nameof(entityName)) + entityName);
            sb.AppendLine(F(nameof(ENTITYNAME)) + ENTITYNAME);
            sb.AppendLine(F(nameof(entityname)) + entityname);
            sb.AppendLine();
            sb.AppendLine(F(nameof(PreEntityName)) + PreEntityName);
            sb.AppendLine(F(nameof(preEntityName)) + preEntityName);
            sb.AppendLine(F(nameof(PREENTITYNAME)) + PREENTITYNAME);
            sb.AppendLine(F(nameof(preentityname)) + preentityname);
            sb.AppendLine();
            sb.AppendLine(F(nameof(pkColumnName)) + pkColumnName);
            sb.AppendLine(F(nameof(pkColumnComment)) + pkColumnComment);
            sb.AppendLine(F(nameof(pkColumnType)) + pkColumnType);
            sb.AppendLine(F(nameof(pkFieldType)) + pkFieldType);
            sb.AppendLine(F(nameof(pkColumnTypeLength)) + pkColumnTypeLength);
            sb.AppendLine();
            sb.AppendLine(F(nameof(pkFieldName)) + pkFieldName);
            sb.AppendLine(F(nameof(PkFieldName)) + PkFieldName);
            sb.AppendLine(F(nameof(pkfieldname)) + pkfieldname);
            sb.AppendLine(F(nameof(PKFIELDNAME)) + PKFIELDNAME);
            sb.AppendLine();
            sb.AppendLine("Cols count  :  " + Cols.Count.ToString());
            return sb.ToString();
        }

        private string entityFieldsTempStr = "";

        /// <summary>
        /// 获取实体属性列表模板
        /// </summary>
        /// <returns></returns>
        public string GetEntityFieldsTempStr(List<string> ignorlcols)
        {
            if (string.IsNullOrWhiteSpace(entityFieldsTempStr))
            {
                StringBuilder sb = new StringBuilder();

                var ColumnEntityList = Cols.Values.ToList();
                foreach (var columnEntity in ColumnEntityList)
                {
                    //if (columnEntity.ColumnName == "id"
                    //    || columnEntity.ColumnName == "inner_version"
                    //    || columnEntity.ColumnName == "state"
                    //    || columnEntity.ColumnName == "updator"
                    //    || columnEntity.ColumnName == "update_time"
                    //    || columnEntity.ColumnName == "creator"
                    //    || columnEntity.ColumnName == "create_time")
                    //{
                    //    return "";
                    //}
                    // 过滤忽略的字段
                    if (ignorlcols.Count > 0 && ignorlcols.Contains(columnEntity.ColumnName)) continue;

                    sb.AppendLine();
                    sb.AppendLine($"        /// <summary>");
                    sb.AppendLine($"        /// Desc:{columnEntity.ColumnComment}");

                    if (columnEntity.IsNull)
                    {
                        sb.AppendLine("        /// Default:NULL");
                        sb.AppendLine("        /// Nullable:True");
                    }
                    else
                    {
                        sb.AppendLine("        /// Default:");
                        sb.AppendLine("        /// Nullable:False)");
                    }

                    sb.AppendLine("        /// </summary>");

                    if (!columnEntity.IsPk)
                    {
                        sb.Append($"        [SugarColumn(");
                    }
                    else
                    {
                        sb.Append($"        [SugarColumn(IsPrimaryKey = true, ");
                    }

                    sb.Append($"ColumnName = \"{columnEntity.ColumnName}\"");
                    sb.Append($", ColumnDescription = \"{columnEntity.ColumnComment.Replace("'", "''")}\"");
                    sb.Append($", IsNullable = {(columnEntity.IsNull ? "true" : "false")}");
                    if (columnEntity.DefaultValue != "NULL")
                    {
                        sb.Append($", DefaultValue = \"{columnEntity.DefaultValue.Replace("'", "")}\"");
                    }
                    if (columnEntity.ColumnType != "int")
                    {
                        sb.Append($", Length = {columnEntity.ColumnTypeLength}");
                    }
                    if (columnEntity.ColumnType != "datetime")
                    {
                        sb.Append($", ColumnDataType = \"{columnEntity.ColumnType}\"");
                    }
                    sb.AppendLine($", DecimalDigits = {columnEntity.DecimalDigits})]");
                    sb.AppendLine($"        public {columnEntity.FieldType}{(columnEntity.IsNull && columnEntity.FieldType != "string" ? "? " : " ")}{columnEntity.FieldName}{{ get; set; }}");
                }

                entityFieldsTempStr = sb.ToString();
            }

            return entityFieldsTempStr;
        }

        private string dtoFieldsTempStr = "";

        /// <summary>
        /// 获取Dto属性列表模板
        /// </summary>
        /// <returns></returns>
        public string GetDtoFieldsTempStr()
        {
            if (string.IsNullOrWhiteSpace(dtoFieldsTempStr))
            {
                StringBuilder sb = new StringBuilder();

                var ColumnEntityList = Cols.Values.ToList();

                foreach (var columnEntity in ColumnEntityList)
                {
                    sb.AppendLine();
                    sb.AppendLine($"        public {columnEntity.FieldType} {columnEntity.FieldName} {{ get; set; }}");
                }

                dtoFieldsTempStr = sb.ToString();
            }

            return dtoFieldsTempStr;
        }

        private string respFieldsTempStr = "";

        /// <summary>
        /// 获取Excel导出属性列表模板
        /// </summary>
        /// <returns></returns>
        public string GetRespExcelFieldsTempStr()
        {
            if (string.IsNullOrWhiteSpace(respFieldsTempStr))
            {
                StringBuilder sb = new StringBuilder();

                var ColumnEntityList = Cols.Values.ToList();

                foreach (var columnEntity in ColumnEntityList)
                {
                    sb.AppendLine();
                    sb.AppendLine($"        [ExcelDescription(Name = \"{columnEntity.ColumnComment}\")]");
                    sb.AppendLine($"        [ExcelColumnName(\"{columnEntity.ColumnComment}\")]");
                    sb.AppendLine($"        public {columnEntity.FieldType} {columnEntity.FieldName} {{ get; set; }}");
                }

                respFieldsTempStr = sb.ToString();
            }

            return respFieldsTempStr;
        }
    }
}