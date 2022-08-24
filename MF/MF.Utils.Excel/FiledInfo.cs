using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Utils.Excel
{
    public class FiledInfo
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Rule { get; set; }
        public string Type { get; set; }
    }

    public class ResultQueryListData
    {
        public List<ShiftInfoClass> ShiftInfo { get; set; }
        public List<ResultItemListClass> ResultItem { get; set; }
        public List<ResultUserClass> ResultUser { get; set; }
    }
    public class ResultItemListClass
    {
        public string CheckItem { get; set; }
        public string JudgeStandard { get; set; }
        public List<ValueDate> DateList { get; set; }

    }
    public class ResultUserClass
    {
        public string CheckDate { get; set; }
        public string UserName { get; set; }

    }
    public class ValueDate
    {
        public string CheckDate { get; set; }
        public string CheckValue { get; set; }
    }
    public class ShiftInfoClass
    {
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }

    }
    public class CheckValueClass
    {
        public string Day { get; set; }
        public string Value { get; set; }
        public string User { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }

    }
}
