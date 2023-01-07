using Common.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DBUtils
{
    /// <summary>
    /// 根据规则生成流水号
    /// </summary>
    public sealed class SerialnoRule
    {
        /// <summary>
        /// 序列号生成规则
        /// </summary>
        /// <param name="Rule"></param>
        /// <param name="serinalno"></param>
        /// <returns></returns>
        public static (bool flag, Serialno Rule) RuleEngine(Serialno Rule, out string serinalno)
        {
            bool flag = false;
            serinalno = "";
            try
            {
                List<dynamic> rules = JsonConvert.DeserializeObject<List<dynamic>>(Rule.SerialNoPattern).OrderBy(u => u.index).ToList();
                foreach (var rule in rules)
                {
                    try
                    {
                        string type = Convert.ToString(rule["type"].Value);
                        switch (type)
                        {
                            case "prefix":
                                serinalno += rule["value"].Value;
                                break;

                            case "param":
                                serinalno += "{" + rule["value"].Value + "}";
                                break;

                            case "date":

                                if (rule["value"]["isCode"].Value)
                                {
                                    var currentyear = DateTime.Now.Year.ToString();
                                    var currentmonth = DateTime.Now.Month.ToString();
                                    var currentday = DateTime.Now.Day.ToString();
                                    //替换对应的年月日代码
                                    switch (rule["value"]["data"].Value)
                                    {
                                        case "yyyy":
                                            serinalno += GetYearCode(rule, currentyear);
                                            break;

                                        case "MM":
                                            serinalno += GetMonthCode(rule, currentmonth);
                                            break;

                                        case "dd":
                                            serinalno += GetDayCode(rule, currentday);
                                            break;

                                        case "yyyyMMdd":
                                            serinalno += GetYearCode(rule, currentyear);
                                            serinalno += GetMonthCode(rule, currentmonth);
                                            serinalno += GetDayCode(rule, currentday);
                                            break;

                                        case "yyMMdd":
                                            currentyear = DateTime.Now.ToString("yy");
                                            serinalno += GetYearCode(rule, currentyear);
                                            serinalno += GetMonthCode(rule, currentmonth);
                                            serinalno += GetDayCode(rule, currentday);
                                            break;

                                        case "yyyy-MM-dd":
                                            serinalno += GetYearCode(rule, currentyear);
                                            serinalno += "-";
                                            serinalno += GetMonthCode(rule, currentmonth);
                                            serinalno += "-";
                                            serinalno += GetDayCode(rule, currentday);
                                            break;

                                        case "yyyy-MM":
                                            serinalno += GetYearCode(rule, currentyear);
                                            serinalno += "-";
                                            serinalno += GetMonthCode(rule, currentmonth);
                                            break;

                                        case "MM-dd":
                                            serinalno += GetMonthCode(rule, currentmonth);
                                            serinalno += "-";
                                            serinalno += GetDayCode(rule, currentday);
                                            break;

                                        default:
                                            serinalno += DateTime.Now.ToString(rule["value"]["data"].Value);
                                            break;
                                    }
                                }
                                else
                                {
                                    serinalno += DateTime.Now.ToString(rule["value"]["data"].Value);
                                }
                                break;

                            case "serial":

                                if (rule["value"]["resetType"].Value == "day")//流水号按天重置
                                {
                                    if (Rule.CurrentDay == null)
                                    {
                                        Rule.CurrentDay = DateTime.Now;
                                    }
                                    var MinDate = ((DateTime)Rule.CurrentDay).Date;
                                    var MaxDate = ((DateTime)Rule.CurrentDay).AddDays(Convert.ToInt32(rule["value"]["resetDay"].Value)).Date;
                                    if (DateTime.Now > MaxDate)
                                    {
                                        Rule.CurrentNo = 0;
                                        Rule.CurrentDay = DateTime.Now;
                                    }
                                    else if (DateTime.Now < MinDate)
                                    {
                                        Rule.CurrentDay = DateTime.Now;
                                    }
                                }
                                else if (rule["value"]["resetType"].Value == "month")//流水号按月重置
                                {
                                    var currentMonth = DateTime.Now;
                                    if (Rule.CurrentDay == null)
                                    {
                                        Rule.CurrentDay = currentMonth;
                                    }

                                    var MinMonth = new DateTime(((DateTime)Rule.CurrentDay).Year, ((DateTime)Rule.CurrentDay).Month, 1);
                                    var MaxMonth = new DateTime(((DateTime)Rule.CurrentDay).Year, ((DateTime)Rule.CurrentDay).AddMonths(Convert.ToInt32(rule["value"]["resetDay"].Value)).Month, 1);

                                    if (currentMonth > MaxMonth)
                                    {
                                        Rule.CurrentNo = 0;
                                        Rule.CurrentDay = currentMonth;
                                    }
                                    else if (currentMonth < MinMonth)
                                    {
                                        Rule.CurrentDay = currentMonth;
                                    }
                                }
                                else if (rule["value"]["resetType"].Value == "year")//流水号按年重置
                                {
                                    var currentYear = DateTime.Now;
                                    if (Rule.CurrentDay == null)
                                    {
                                        Rule.CurrentDay = DateTime.Now;
                                    }
                                    var MinYear = new DateTime(((DateTime)Rule.CurrentDay).Year, 1, 1);
                                    var MaxYear = new DateTime(((DateTime)Rule.CurrentDay).AddYears(Convert.ToInt32(rule["value"]["resetDay"].Value)).Year, 1, 1);

                                    if (currentYear > MaxYear)
                                    {
                                        Rule.CurrentNo = 0;
                                        Rule.CurrentDay = currentYear;
                                    }
                                    else if (currentYear < MinYear)
                                    {
                                        Rule.CurrentDay = currentYear;
                                    }
                                }
                                else if (rule["value"]["resetType"].Value == "maxNum") //流水号按最大值重置
                                {
                                    if ((Rule.CurrentNo + Convert.ToInt32(rule["value"]["uper"].Value) > Convert.ToInt32("1".PadRight(Convert.ToInt32(rule["value"]["length"].Value) + 1, '0'))))
                                    {
                                        Rule.CurrentNo = 0;
                                        Rule.CurrentDay = DateTime.Now;
                                    }
                                }

                                if (Rule.CurrentNo == 0)
                                {
                                    Rule.CurrentNo = Convert.ToInt32(rule["value"]["start"].Value);
                                }
                                else
                                {
                                    Rule.CurrentNo += Convert.ToInt32(rule["value"]["uper"].Value);
                                }
                                int len = Convert.ToInt32(rule["value"]["length"].Value);
                                if (Rule.CurrentNo.ToString().Length <= len)
                                {
                                    serinalno += Rule.CurrentNo.ToString().PadLeft(len, '0');
                                }
                                else
                                {
                                    serinalno = "流水号超位数上限,创建失败";
                                    return (flag, Rule);
                                }
                                break;
                        }
                        flag = true;
                    }
                    catch (Exception exx)
                    {
                        Console.WriteLine(exx.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return (flag, Rule);
        }

        private static string GetYearCode(dynamic rule, string currentyear)
        {
            var yearlist = rule["value"]["tableDataYear"];

            foreach (var item in yearlist)
            {
                if (item.key == currentyear)
                {
                    return item.value;
                }
            }

            return currentyear;
        }

        private static string GetMonthCode(dynamic rule, string currentmonth)
        {
            var monthlist = rule["value"]["tableDataMonth"];

            foreach (var item in monthlist)
            {
                if (item.key == currentmonth)
                {
                    return item.value;
                }
            }

            return currentmonth;
        }

        private static string GetDayCode(dynamic rule, string currentday)
        {
            var daycodelist = rule["value"]["tableDataDay"];

            foreach (var item in daycodelist)
            {
                if (item.key == currentday)
                {
                    return item.value;
                }
            }
            return currentday;
        }
    }
}