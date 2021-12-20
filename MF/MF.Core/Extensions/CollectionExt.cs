using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

using MF.Core.Reflection;

namespace MF.Core.Extensions
{
    public static class CollectionExt
    {
        public static List<T> EmitToList<T>(this DataTable dt)
        {
            if (dt is null)
                return null;

            List<T> list = new List<T>();
            var objBuilder = EmitUtil.CreateBuilder(typeof(T));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T _t = (T)objBuilder();
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t, dt.Rows[i][j], null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> list = new List<T>();

            if (dt is null)
                return list;
            else if (dt.Rows.Count == 0)
                return list;

            Dictionary<string, string> dicField = new Dictionary<string, string>();
            Dictionary<string, string> dicProperty = new Dictionary<string, string>();
            Type type = typeof(T);

            type.GetFields().ForEach(aFiled =>
            {
                dicField.Add(aFiled.Name.ToLower(), aFiled.Name);
            });

            type.GetProperties().ForEach(aProperty =>
            {
                dicProperty.Add(aProperty.Name.ToLower(), aProperty.Name);
            });

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T _t = Activator.CreateInstance<T>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string memberKey = dt.Columns[j].ColumnName.ToLower();

                    if (dicField.ContainsKey(memberKey))
                    {
                        FieldInfo theField = type.GetField(dicField[memberKey]);
                        var dbValue = dt.Rows[i][j];
                        if (dbValue.GetType() == typeof(DBNull))
                            dbValue = null;
                        if (dbValue != null)
                        {
                            Type memberType = theField.FieldType;
                            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                NullableConverter newNullableConverter = new NullableConverter(memberType);
                                dbValue = newNullableConverter.ConvertFrom(dbValue);
                            }
                            else
                            {
                                dbValue = Convert.ChangeType(dbValue, memberType);
                            }
                        }
                        theField.SetValue(_t, dbValue);
                    }
                    if (dicProperty.ContainsKey(memberKey))
                    {
                        PropertyInfo theProperty = type.GetProperty(dicProperty[memberKey]);
                        var dbValue = dt.Rows[i][j];
                        if (dbValue.GetType() == typeof(DBNull))
                            dbValue = null;
                        if (dbValue != null)
                        {
                            Type memberType = theProperty.PropertyType;
                            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                NullableConverter newNullableConverter = new NullableConverter(memberType);
                                dbValue = newNullableConverter.ConvertFrom(dbValue);
                            }
                            else
                            {
                                dbValue = Convert.ChangeType(dbValue, memberType);
                            }
                        }
                        theProperty.SetValue(_t, dbValue);
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        public static DataTable ToDataTable(this IEnumerable<ExpandoObject> dataList)
        {
            DataTable dt = new DataTable();
            if (dataList is null)
                return null;
            else if (dataList.Count() == 0)
                return dt;
            else
            {
                var aEntity = dataList.FirstOrDefault();
                var properties = aEntity.GetProperties();
                properties.ForEach(aProperty =>
                {
                    dt.Columns.Add(aProperty);
                });
                dataList.ForEach((aData, index) =>
                {
                    dt.Rows.Add(dt.NewRow());
                    properties.ForEach(aProperty =>
                    {
                        dt.Rows[index][aProperty] = aData.GetProperty(aProperty);
                    });
                });
            }
            return dt;
        }

        public static string ToCsvStr(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colum = dt.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 数据字典合并
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="sourceDict"></param>
        /// <param name="aimDict"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> sourceDict, IDictionary<TKey, TValue> aimDict) where TValue : class
        {
            if (sourceDict is null)
            {
                return aimDict;
            }
            if (aimDict is null)
            {
                return sourceDict;
            }
            return sourceDict.Keys.Union(aimDict.Keys).ToDictionary(k => k, k => sourceDict.ContainsKey(k) ? sourceDict[k] : aimDict[k]);
        }
    }
}