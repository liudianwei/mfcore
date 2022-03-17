using System;
using System.Text;

namespace CodeGen
{
    /// <summary>
    /// 列实体
    /// </summary>
    public class ColumnEntity
    {
        private string _columnName;

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
                if (!string.IsNullOrEmpty(value))
                {
                    //FieldName = StringUtils.ToEntityName(_columnName);

                    FieldName = _columnName.ToEntityName();

                    //fieldName = StringUtils.LowFirst(FieldName);
                    fieldName = FieldName.ToLowerFirst();

                    FIELDNAME = FieldName.ToUpper();
                    fieldname = FieldName.ToLower();
                }
                else
                {
                    FieldName = "";
                    fieldName = "";
                    FIELDNAME = "";
                    fieldname = "";
                }
            }
        }

        /// <summary>
        /// 字段名称 小写开头
        /// </summary>
        public string fieldName { get; private set; }

        /// <summary>
        /// 字段名称大写开头
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// 全小写
        /// </summary>
        public string fieldname { get; private set; }

        /// <summary>
        /// 全大写
        /// </summary>
        public string FIELDNAME { get; private set; }

        private string _columnType;

        /// <summary>
        /// 列数据库类型
        /// </summary>
        public string ColumnType
        {
            get
            {
                return _columnType;
            }
            set
            {
                _columnType = value;
                FieldType = GetFieldType(_columnType);
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
            //Console.WriteLine("coltype: " + _columnType);

            if (DbType.ToLower() == "oracle")
            {
                switch (_columnType.ToLower())
                {
                    case "datetime":
                        reval = "DateTime";
                        break;

                    case "decimal":
                    case "single":
                    case "double":
                    case "sbyte":
                    case "int16":
                    case "int32":
                    case "int64":
                    case "varnumeric":
                        reval = "Decimal";
                        break;

                    case "object":
                        reval = "TimeSpan";
                        break;

                    case "binary":
                        reval = "byte[]";
                        break;

                    case "AnsiStringFixedLength":
                    case "string":
                    case "ansistring":
                    case "stringfixedlength":
                    default:
                        reval = "string";
                        break;
                }
                return reval;
            }

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
                    reval = "bool";
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
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 列类型长度
        /// </summary>
        public int ColumnTypeLength { get; set; }

        /// <summary>
        /// 列备注
        /// </summary>
        public string ColumnComment { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPk { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        public string DbPrefix { get; set; }

        public string DbType { get; set; }

        public int DecimalDigits { get; set; }

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
            sb.AppendLine(F(nameof(ColumnName)) + ColumnName);
            sb.AppendLine(F(nameof(ColumnComment)) + ColumnComment);
            sb.AppendLine(F(nameof(ColumnType)) + ColumnType);
            sb.AppendLine(F(nameof(FieldType)) + FieldType);
            sb.AppendLine(F(nameof(ColumnTypeLength)) + ColumnTypeLength);
            sb.AppendLine();
            sb.AppendLine(F(nameof(fieldName)) + fieldName);
            sb.AppendLine(F(nameof(FieldName)) + FieldName);
            sb.AppendLine(F(nameof(fieldname)) + fieldname);
            sb.AppendLine(F(nameof(FIELDNAME)) + FIELDNAME);
            sb.AppendLine();
            sb.AppendLine(F(nameof(IsPk)) + IsPk);
            sb.AppendLine(F(nameof(IsNull)) + IsNull);
            sb.AppendLine(F(nameof(DefaultValue)) + DefaultValue);
            return sb.ToString();
        }
    }
}