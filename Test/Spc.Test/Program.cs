using MF.Utils.SPC;
using System;
using System.Data;

namespace Spc.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DataTable dt = new DataTable();
            string coln = "data";
            dt.Columns.Add(coln);
            string data = "22.05\r\n49.96\r\n80.35\r\n9.8\r\n78.3\r\n92.36\r\n35.64\r\n47.85\r\n6.14\r\n99.17\r\n32.74\r\n90.3\r\n8.45\r\n26.24\r\n29.93\r\n17.78\r\n57.53\r\n73.65\r\n94.31\r\n98.7\r\n86.19\r\n24.49\r\n53.05\r\n91.5\r\n9.24\r\n55.76\r\n3.79\r\n18.36\r\n19.88\r\n9.36\r\n63.45\r\n43.37\r\n97.27\r\n2.7\r\n64.95\r\n79.8\r\n39.47\r\n70.87\r\n82.22\r\n34.64\r\n29.18\r\n97.1\r\n90.92\r\n22.84\r\n82.12\r\n50.09\r\n87.99\r\n58.54\r\n56.22\r\n84.16";
            foreach (string d in data.Split("\r\n"))
            {
                DataRow newRow = dt.NewRow();
                newRow["data"] = d;
                dt.Rows.Add(newRow);
            }

            SpcUtil spcUtil = new SpcUtil();
            var result1 = spcUtil.BasicTrend(dt, 5, "upper_limit", "lower_limit", coln, "");
            var result2 = spcUtil.SampleTrend(dt, coln);
            var result3 = spcUtil.Histogram(dt, 5, coln);
            var result4 = spcUtil.NormalDistribution(dt, 5, "upper_limit", "lower_limit", coln);
            var result5 = spcUtil.Pareto(dt, 5, 10, coln);
            var result6 = spcUtil.NormalDistribution_Mean(dt, 5, "upper_limit", "lower_limit", coln);
            var result7 = spcUtil.Meanpoor_Mean(dt, 5, coln);
            var result8 = spcUtil.Meanpoor_Poor(dt, 5, coln);
            var result9 = spcUtil.MeanStandardpoor_Mean(dt, 5, coln);
            var result10 = spcUtil.MeanStandardpoor_Standardpoor(dt, 5, coln);
            var result11 = spcUtil.MeanpoorTablePic(dt, 5, coln);
            var result12 = spcUtil.MeanStandardpoorTablePic(dt, 5, coln);
            Console.ReadLine();
        }
    }
}