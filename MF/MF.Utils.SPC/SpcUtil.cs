using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Xml;

namespace MF.Utils.SPC
{
    public class SpcUtil
    {
        public DataTable SpcToJson(string TableName, string Dictionary, string OpName, string MeasurePosition, string MeasureItem
            , string EngineType, string Sample, string SampleNum, string StartTime, string EndTime, string SpcType, string order, string where)
        {
            //string errorMessage = "";

            #region 数据源准备

            #region 条件

            if (!string.IsNullOrEmpty(OpName))
            {
                OpName = " AND " + SqlTabCoName(Dictionary, "OpName") + "='" + OpName + "'";
            }
            if (!string.IsNullOrEmpty(MeasurePosition))
            {
                MeasurePosition = " AND " + SqlTabCoName(Dictionary, "MeasurePosition") + "='" + MeasurePosition + "'";
            }
            if (!string.IsNullOrEmpty(MeasureItem))
            {
                MeasureItem = " AND " + SqlTabCoName(Dictionary, "MeasureItem") + "='" + MeasureItem + "'";
            }
            if (!string.IsNullOrEmpty(EngineType))
            {
                EngineType = " AND " + SqlTabCoName(Dictionary, "EngineType") + "='" + EngineType + "'";
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                StartTime = " AND " + SqlTabCoName(Dictionary, "OperateTime") + ">'" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                EndTime = " AND " + SqlTabCoName(Dictionary, "OperateTime") + "<'" + EndTime + "'";
            }
            if (string.IsNullOrEmpty(Sample))
            {
                Sample = "5";
            }
            if (string.IsNullOrEmpty(SampleNum))
            {
                SampleNum = "30";
            }

            #endregion 条件

            string condition = " WHERE 0=0" + OpName + MeasurePosition + MeasureItem + EngineType + StartTime + EndTime;
            if (!string.IsNullOrWhiteSpace(where))
            {
                condition = condition + " AND " + where;
            }
            DataTable dt = null;
            string sql = "SELECT TOP " + Convert.ToInt32(Sample) * Convert.ToInt32(SampleNum) + " * FROM " + TableName + condition;
            if (!string.IsNullOrWhiteSpace(order))
            {
                sql += " order by " + order;
            }

            DataTable JsonStr;
            //执行SQL语句
            //var db = YL.Utils.Env.GlobalCore.GetRequiredService<SqlSugar.SqlSugarClient>();
            //dt = client.Ado.GetDataTable(sql);
            //SQLCommon.ExecuteDataTable(SQL, connectionString, out dt, out errorMessage);

            #endregion 数据源准备

            #region 选择SPC图形类型

            switch (SpcType)
            {
                case "1"://基本趋势图
                    JsonStr = BasicTrend(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "UpperLimit"), SqlTabCoName(Dictionary, "LowerLimit"), SqlTabCoName(Dictionary, "Measurements"), SqlTabCoName(Dictionary, "ComponentSerialNum"));
                    break;

                case "2"://样本趋势图
                    JsonStr = SampleTrend(dt, SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "3"://直方图
                    JsonStr = Histogram(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "4"://工序能力分析
                    JsonStr = NormalDistribution(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "UpperLimit"), SqlTabCoName(Dictionary, "LowerLimit"), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "5"://排列图
                    JsonStr = Pareto(dt, Convert.ToInt32(Sample), Convert.ToInt32(SampleNum), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "6"://均值正太分布图(XR/XS)
                    JsonStr = NormalDistribution_Mean(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "UpperLimit"), SqlTabCoName(Dictionary, "LowerLimit"), SqlTabCoName(Dictionary, "Measurements"), SqlTabCoName(Dictionary, "Type"));
                    break;

                case "7"://均值极差-均值图
                    JsonStr = Meanpoor_Mean(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "8"://均值极差-极差图
                    JsonStr = Meanpoor_Poor(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "9"://均值标准差-均值图
                    JsonStr = MeanStandardpoor_Mean(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "10"://均值标准差-标准差图
                    JsonStr = MeanStandardpoor_Standardpoor(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "11"://均值极差-数据图

                    JsonStr = MeanpoorTablePic(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                case "12"://均值标准差-数据图
                    JsonStr = MeanStandardpoorTablePic(dt, Convert.ToInt32(Sample), SqlTabCoName(Dictionary, "Measurements"));
                    break;

                default:
                    JsonStr = null;
                    break;
            }

            #endregion 选择SPC图形类型

            return JsonStr;
        }

        private string SqlTabCoName(string Dictionary, string itemName)
        {
            string CoName = "";
            string[] List = Dictionary.Split(',');
            for (int i = 0; i < List.Length; i++)
            {
                if (List[i].Split(':')[1] == itemName)
                {
                    CoName = List[i].Split(':')?[0];
                    break;
                }
            }
            return CoName;
        }

        /// <summary>
        /// 基本趋势图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="UpperLimitName"></param>
        /// <param name="LowerLimitName"></param>
        /// <param name="MeasurementsName"></param>
        /// <param name="ComponentSerialNum"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable BasicTrend(DataTable dt, int Sample, string UpperLimitName, string LowerLimitName, string MeasurementsName, string ComponentSerialNum, string Type = "XS")
        {
            DataTable table = new DataTable();
            double USL = 0;
            double x = 0;
            double s = 0;
            double QU = 0;
            double QL = 0;
            double LSL = 0;
            double Cp = 0;
            double Cpk = 0;
            double[] sx;
            double[] sy;
            double yMin = 0;
            double yMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region Y_Data

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion Y_Data

                #region USL LSL

                try
                {
                    USL = Convert.ToDouble(dt.Rows[0][UpperLimitName]);
                    LSL = Convert.ToDouble(dt.Rows[0][LowerLimitName]);
                }
                catch
                {
                }
                //规格上下合理性判断，超过90%样本，就算不合理
                if ((X.Where(x => x > USL).Count() * 1.0 / X.Count()) > 0.9 || (X.Where(x => x < LSL).Count() * 1.0 / X.Count()) > 0.9)
                {
                    USL = Type == "XR" ? Spc_Data.XR(X, Sample).UCL_X : Spc_Data.XS(X, Sample).UCL_X;
                    LSL = Type == "XR" ? Spc_Data.XR(X, Sample).LCL_X : Spc_Data.XS(X, Sample).LCL_X;
                }

                #endregion USL LSL

                #region QU SL QL

                //计算SPC参数
                SpcCaculator.SpcValue(X, ref USL, ref LSL, out x, out s, out QU, out QL, out Cp, out Cpk, out sx, out sy);
                CSpc_Data_Cpk Spc_Data_Cpk = Spc_Data.Cpk(X, Sample, USL, LSL, Type);

                #endregion QU SL QL

                DataRow dr;
                table.Columns.Add("X_Data", typeof(string));
                table.Columns.Add("X_Data_Description", typeof(string));
                table.Columns.Add("Y_Data", typeof(string));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region yMin,yMax

                yMin = Math.Min(Math.Min(Math.Min(Math.Min(SpcCaculator.Min(sx), USL), QU), QL), LSL) - (USL + LSL) / 2;
                yMax = Math.Max(Math.Max(Math.Max(Math.Max(SpcCaculator.Max(sx), USL), QU), QL), LSL) + (USL + LSL) / 2;

                #endregion yMin,yMax

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = i + 1;
                    dr["X_Data_Description"] = dt.Rows[i][ComponentSerialNum];
                    //dr["X_Data_Description"] = dt.Rows[i]["ComponentSerialNum"];
                    //dr["Y_Data"] = dt.Rows[i]["Measurements"];
                    dr["Y_Data"] = dt.Rows[i][MeasurementsName];
                    dr["SPC_KeyValue"] = USL.ToString("F3") + "," + QU.ToString("F3") + "," + sx[5].ToString("F3") + "," + QL.ToString("F3") + "," + LSL.ToString("F3") + ","
                        + Spc_Data_Cpk.Cp.ToString("F3") + "," + Spc_Data_Cpk.Cpk.ToString("F3") + "," + yMin.ToString("F3") + "," + yMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 样本趋势图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable SampleTrend(DataTable dt, string MeasurementsName)
        {
            DataTable table = new DataTable();
            if (dt.Rows.Count > 0)
            {
                DataRow dr;
                table.Columns.Add("X_Data", typeof(string));
                table.Columns.Add("Y_Data", typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = i + 1;
                    dr["Y_Data"] = dt.Rows[i][MeasurementsName];
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 直方图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable Histogram(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region 公值

                double max = 0;//最大值
                double min = 0;// 最小值

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }
                max = SpcCaculator.Max(X);//最大值 max
                min = SpcCaculator.Min(X);//最小值 min

                double mid = (max - min) / Sample;

                #endregion 公值

                #region X_Data

                string last = min.ToString() + "~" + (min + mid).ToString();

                if (mid != 0)
                {
                    last = min.ToString() + "~" + (min + mid).ToString();
                    for (double i = min + mid; i < max; i = i + mid)
                    {
                        last = last + "," + i.ToString("f2") + "~" + (i + mid).ToString("f2");
                    }
                }

                string X_Data = last;

                #endregion X_Data

                #region Y_Data

                int[] arr1 = new int[Sample];

                for (int i = 0; i < Sample; i++)
                {
                    for (int r = 0; r < X.Length; r++)
                    {
                        if (X[r] >= (min + mid * (i)) && X[r] < (min + mid * (i + 1)))
                        {
                            arr1[i]++;
                        }
                        else if ((min + mid * (i + 1)) == max && X[r] >= max)
                        {
                            arr1[i]++;
                        }
                    }
                }
                string str = arr1[0].ToString();
                for (int d = 1; d < Sample; d++)
                {
                    string str1 = arr1[d].ToString();
                    str = str + "," + str1;
                }
                string Y_Data = str;

                #endregion Y_Data

                DataRow dr;
                table.Columns.Add("X_Data", typeof(string));
                table.Columns.Add("Y_Data", typeof(int));

                int Length_min = Math.Min(X_Data.Split(',').Length, Y_Data.Split(',').Length);//最小值 Length

                for (int j = 0; j < Length_min; j++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = X_Data.Split(',')[j];
                    dr["Y_Data"] = Y_Data.Split(',')[j];
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 正太分布图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="UpperLimitName"></param>
        /// <param name="LowerLimitName"></param>
        /// <param name="MeasurementsName"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable NormalDistribution(DataTable dt, int Sample, string UpperLimitName = "upper_limit", string LowerLimitName = "lower_limit",
            string MeasurementsName = "measure_value", string Type = "XS")
        {
            DataTable table = new DataTable();

            double USL = 0;
            double x = 0;
            double s = 0;
            double QU = 0;
            double QL = 0;
            double LSL = 0;
            double Cp = 0;
            double Cpk = 0;
            double[] sx;
            double[] sy;
            double xMin = 0;
            double xMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region USL LSL

                try
                {
                    USL = Convert.ToDouble(dt.Rows[0][UpperLimitName]);
                    LSL = Convert.ToDouble(dt.Rows[0][LowerLimitName]);
                }
                catch
                {
                }
                //规格上下合理性判断，超过90%样本，就算不合理
                if ((X.Where(x => x > USL).Count() * 1.0 / X.Count()) > 0.9 || (X.Where(x => x < LSL).Count() * 1.0 / X.Count()) > 0.9)
                {
                    USL = Type == "XR" ? Spc_Data.XR(X, Sample).UCL_X : Spc_Data.XS(X, Sample).UCL_X;
                    LSL = Type == "XR" ? Spc_Data.XR(X, Sample).LCL_X : Spc_Data.XS(X, Sample).LCL_X;
                }

                #endregion USL LSL

                #region QU SL QL

                //计算SPC参数
                SpcCaculator.SpcValue(X, ref USL, ref LSL, out x, out s, out QU, out QL, out Cp, out Cpk, out sx, out sy);
                CSpc_Data_Cpk Spc_Data_Cpk = Spc_Data.Cpk(X, Sample, USL, LSL, Type);

                #endregion QU SL QL

                DataRow dr;
                table.Columns.Add("X_Data", typeof(double));
                table.Columns.Add("Y_Data", typeof(double));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region xMin,xMax

                xMin = Math.Min(Math.Min(Math.Min(Math.Min(SpcCaculator.Min(sx), USL), QU), QL), LSL) - (USL + LSL) / 2;
                xMax = Math.Max(Math.Max(Math.Max(Math.Max(SpcCaculator.Max(sx), USL), QU), QL), LSL) + (USL + LSL) / 2;

                #endregion xMin,xMax

                for (int i = 0; i < sx.Length; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = sx[i];
                    dr["Y_Data"] = sy[i];
                    dr["SPC_KeyValue"] = USL.ToString("F3") + "," + QU.ToString("F3") + "," + sx[5].ToString("F3") + "," + QL.ToString("F3") + "," + LSL.ToString("F3") + ","
                        + Spc_Data_Cpk.Cp.ToString("F3") + "," + Spc_Data_Cpk.Cpk.ToString("F3") + "," + xMin.ToString("F3") + "," + xMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 排列图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="SampleNum"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable Pareto(DataTable dt, int Sample, int SampleNum, string MeasurementsName)
        {
            DataTable table = new DataTable();

            double[] X;
            if (dt.Rows.Count > 0)
            {
                #region 公值

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                double minV = SpcCaculator.Min(X);
                double maxV = SpcCaculator.Max(X);
                double Dv = SpcCaculator.RoundInterval((maxV - minV) / Sample);
                minV = Math.Floor(minV / Dv) * Dv;
                maxV = Math.Ceiling(maxV / Dv) * Dv;

                string Y_Data_1 = "";
                string Y_Data_2 = "";
                string X_Data = "";
                double acv = 0.0;
                double average;
                double bottom = minV;

                for (; bottom < maxV; bottom += Dv)
                {
                    int number = 0;
                    double top = bottom + Dv;
                    foreach (double p in X)
                    {
                        if (top >= maxV)
                        {
                            if (p >= bottom && p <= top)
                                ++number;
                        }
                        else
                        {
                            if (p >= bottom && p < top)
                                ++number;
                        }
                    }
                    acv += Convert.ToDouble(number);
                    average = acv / (Sample * SampleNum);
                    X_Data += bottom.ToString() + "~" + top.ToString();
                    Y_Data_1 += number;
                    Y_Data_2 += average.ToString("F3");
                    if (top < maxV)
                    {
                        Y_Data_1 += ",";
                        Y_Data_2 += ",";
                        X_Data += ",";
                    }
                }

                #endregion 公值

                DataRow dr;
                table.Columns.Add("X_Data", typeof(string));
                table.Columns.Add("Y_Data_1", typeof(string));
                table.Columns.Add("Y_Data_2", typeof(string));

                #region 最小值 Length

                int Length_min = Math.Min(X_Data.Split(',').Length, Y_Data_1.Split(',').Length);

                #endregion 最小值 Length

                for (int j = 0; j < Length_min; j++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = X_Data.Split(',')[j];
                    dr["Y_Data_1"] = Y_Data_1.Split(',')[j];
                    dr["Y_Data_2"] = Y_Data_2.Split(',')[j];
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 均值-正太分布
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable NormalDistribution_Mean(DataTable dt, int Sample, string UpperLimitName = "upper_limit", string LowerLimitName = "lower_limit", string MeasurementsName = "measure_value", string Type = "XR")
        {
            DataTable table = new DataTable();

            double USL = 0;
            double SL = 0;
            double LSL = 0;
            double Cp = 0;
            double Cpk = 0;
            double Cpl = 0;
            double Cpu = 0;
            double xMin = 0;
            double xMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                //USL = Spc_Data_XR.UCL_X;
                //LSL = Spc_Data_XR.LCL_X;
                //double ValueUp = SpcCaculator.Max(X);
                //double ValueDown = SpcCaculator.Min(X);
                try
                {
                    USL = Convert.ToDouble(dt.Rows[0][UpperLimitName]);
                    LSL = Convert.ToDouble(dt.Rows[0][LowerLimitName]);
                }
                catch
                {
                }
                //规格上下合理性判断，超过90%样本，就算不合理
                if ((X.Where(x => x > USL).Count() * 1.0 / X.Count()) > 0.9 || (X.Where(x => x < LSL).Count() * 1.0 / X.Count()) > 0.9)
                {
                    USL = Type == "XR" ? Spc_Data.XR(X, Sample).UCL_X : Spc_Data.XS(X, Sample).UCL_X;
                    LSL = Type == "XR" ? Spc_Data.XR(X, Sample).LCL_X : Spc_Data.XS(X, Sample).LCL_X;
                }
                CSpc_Data_Cpk Spc_Data_Cpk = Spc_Data.Cpk(X, Sample, USL, LSL, Type);
                SL = Spc_Data_Cpk.X2;
                Cp = Spc_Data_Cpk.Cp;
                Cpk = Spc_Data_Cpk.Cpk;
                Cpl = Spc_Data_Cpk.Cpl;
                Cpu = Spc_Data_Cpk.Cpu;

                #endregion 计算SPC参数

                DataRow dr;
                table.Columns.Add("X_Data", typeof(double));
                table.Columns.Add("Y_Data", typeof(double));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region xMin,xMax

                xMin = Math.Min(Math.Min(SpcCaculator.Min(Spc_Data_Cpk.NormalDistributionX), USL), LSL) - (USL + LSL) / 2;
                xMax = Math.Max(Math.Max(SpcCaculator.Max(Spc_Data_Cpk.NormalDistributionX), USL), LSL) + (USL + LSL) / 2;

                #endregion xMin,xMax

                for (int i = 0; i < Spc_Data_Cpk.NormalDistributionX.Length; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = Spc_Data_Cpk.NormalDistributionX[i];
                    dr["Y_Data"] = Spc_Data_Cpk.NormalDistributionY[i];
                    dr["SPC_KeyValue"] = USL.ToString("F3") + "," + SL.ToString("F3") + "," + LSL.ToString("F3") + ","
                        + Cp.ToString("F3") + "," + Cpk.ToString("F3") + "," + Cpl.ToString("F3") + "," + Cpu.ToString("F3") + "," + xMin.ToString("F3") + "," + xMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        #region 均值极差

        /// <summary>
        /// 均值极差-均值图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable Meanpoor_Mean(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();
            double UCL = 0;
            double CL = 0;
            double LCL = 0;
            double yMin = 0;
            double yMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                CSpc_Data_XR Spc_Data_XR = Spc_Data.XR(X, Sample);
                CL = Spc_Data_XR.CL_X;
                UCL = Spc_Data_XR.UCL_X;
                LCL = Spc_Data_XR.LCL_X;

                #endregion 计算SPC参数

                DataRow dr;
                table.Columns.Add("X_Data", typeof(double));
                table.Columns.Add("Y_Data", typeof(double));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region yMin,yMax

                yMin = Math.Min(Math.Min(SpcCaculator.Min(Spc_Data_XR.CL_Xk), UCL), LCL) - (UCL + LCL) / 2;
                yMax = Math.Max(Math.Max(SpcCaculator.Max(Spc_Data_XR.CL_Xk), UCL), LCL) + (UCL + LCL) / 2;

                #endregion yMin,yMax

                for (int i = 0; i < Spc_Data_XR.CL_Xk.Length; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = i + 1;
                    dr["Y_Data"] = Spc_Data_XR.CL_Xk[i];
                    dr["SPC_KeyValue"] = UCL.ToString("F3") + "," + CL.ToString("F3") + "," + LCL.ToString("F3") + "," + yMin.ToString("F3") + "," + yMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 均值极差-极差图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable Meanpoor_Poor(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();
            double UCL = 0;
            double CL = 0;
            double LCL = 0;
            double yMin = 0;
            double yMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                CSpc_Data_XR Spc_Data_XR = Spc_Data.XR(X, Sample);
                CL = Spc_Data_XR.CL_R;
                UCL = Spc_Data_XR.UCL_R;
                LCL = Spc_Data_XR.LCL_R;

                #endregion 计算SPC参数

                DataRow dr;
                table.Columns.Add("X_Data", typeof(double));
                table.Columns.Add("Y_Data", typeof(double));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region yMin,yMax

                yMin = Math.Min(Math.Min(SpcCaculator.Min(Spc_Data_XR.CL_Rk), UCL), LCL) - (UCL + LCL) / 2;
                yMax = Math.Max(Math.Max(SpcCaculator.Max(Spc_Data_XR.CL_Rk), UCL), LCL) + (UCL + LCL) / 2;

                #endregion yMin,yMax

                for (int i = 0; i < Spc_Data_XR.CL_Rk.Length; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = i + 1;
                    dr["Y_Data"] = Spc_Data_XR.CL_Rk[i];
                    dr["SPC_KeyValue"] = UCL.ToString("F3") + "," + CL.ToString("F3") + "," + LCL.ToString("F3") + "," + yMin.ToString("F3") + "," + yMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 均值极差-数据图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable MeanpoorTablePic(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();

            double[] X;
            double USL = 0;
            //double CL = 0;
            double LSL = 0;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                try
                {
                    USL = Convert.ToDouble(dt.Rows[0][1]);
                    LSL = Convert.ToDouble(dt.Rows[0][2]);
                    //规格上下合理性判断，超过90%样本，就算不合理
                    if ((X.Where(x => x > USL).Count() * 1.0 / X.Count()) > 0.9 || (X.Where(x => x < LSL).Count() * 1.0 / X.Count()) > 0.9)
                    {
                        USL = 0;
                        LSL = 0;
                    }
                }
                catch
                {
                }
                //CSpc_Data_XR Spc_Data_XR = Spc_Data.XR(X, Sample);
                //CL = Spc_Data_XR.CL_X;
                //UCL = Spc_Data_XR.UCL_X;
                //LCL = Spc_Data_XR.LCL_X;
                //USL= dt.Rows[0]

                #endregion 计算SPC参数

                #region 生成图片Image

                string ImageCode = "";

                Bitmap bp = DrawXRChart_X_2018(X, Sample, LSL, USL);
                IntPtr pr = bp.GetHbitmap();
                Image _Image = Image.FromHbitmap(pr);
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();
                _Image.Save(_MemoryStream, ImageFormat.Jpeg);
                byte[] _ImageBytes = _MemoryStream.ToArray();

                ImageCode = Convert.ToBase64String(_ImageBytes);

                #endregion 生成图片Image

                DataRow dr;
                table.Columns.Add("ImageCode", typeof(string));
                dr = table.NewRow();
                dr["ImageCode"] = ImageCode;
                table.Rows.Add(dr);
            }
            return table;
        }

        public static Bitmap DrawXRChart_X_2018(double[] X, int n, double LSL, double USL)
        {
            Bitmap mybBitmap = new Bitmap(1000, 330);
            Graphics mybGraphics = Graphics.FromImage(mybBitmap); ;
            Spc_Table_XR(X, n, LSL, USL, mybGraphics);
            return mybBitmap;
        }

        /// <summary>
        /// 画XR图的表
        /// </summary>
        /// <param name="X">数据</param>
        /// <param name="n">没个样本的数据个数</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="USL">规格上限</param>
        /// <returns></returns>
        public static void Spc_Table_XR(double[] X, int n, double LSL, double USL, Graphics g)
        {
            CSpc_Data_XR Spc_Data_XR = Spc_Data.XR(X, n);
            USL = USL == 0 ? Convert.ToDouble(Spc_Data_XR.UCL_X) : USL;
            LSL = LSL == 0 ? Convert.ToDouble(Spc_Data_XR.LCL_X) : LSL;

            int x0 = 5;
            int y0 = 5;
            int k = X.Length / n;
            Pen BlackPen = new Pen(Color.Black, 1);
            //Bitmap tableBitmap = new Bitmap(1000, (n + 10) * 15 + 5);
            //Graphics g = Graphics.FromImage(tableBitmap);
            Font font1 = new Font("Tahoma", 7);
            Font font2 = new Font("Tahoma", 8, FontStyle.Bold);
            Font font3 = new Font("Tahoma", 8);
            g.Clear(Color.White);
            int width = 990 / (k + 1);
            int height = 15;
            string ExceptionalAnalysis1 = "异常情况分析：(1)出现超出控制限的点；(2)连续七个点全在控制限之上或之下；";
            string ExceptionalAnalysis2 = "(3)任何其他明显非随机的图形；";
            string ReasonAnalysis = "内容(原因分析、对策和整改情况)：";
            //float width = g.MeasureString(data, new Font("Tahoma", 10)).Width;
            //float height = g.MeasureString(data, new Font("Tahoma", 10)).Height;

            //反锯齿算法
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            g.DrawString("组号", font1, Brushes.Black, 0 + x0, 4 + y0);

            //画水平线并写出表头
            for (int i = 0; i <= n + 4; i++)
            {
                //画水平的表格线
                g.DrawLine(BlackPen, 0 + x0, i * height + y0, (k + 1) * width + x0, i * height + y0);

                //写出各组的表头（竖）
                if (i > 0 && i <= n)
                {
                    g.DrawString(i.ToString(), font1, Brushes.Black, 10 + x0, 4 + i * height + y0);
                }

                if (i == n + 1)
                {
                    g.DrawString("求和", font1, Brushes.Black, 3 + x0, 4 + i * height + y0);
                }

                if (i == n + 2)
                {
                    g.DrawString("X", font1, Brushes.Black, 10 + x0, 4 + i * height + y0);
                    g.DrawLine(BlackPen, 11 + x0, 4 + i * height + y0, 16 + x0, 4 + i * height + y0);
                }

                if (i == n + 3)
                {
                    g.DrawString("R", font1, Brushes.Black, 10 + x0, 4 + i * height + y0);
                }
            }

            //画垂直线并写出表头
            for (int j = 0; j <= k + 1; j++)
            {
                //画垂直的表格线
                g.DrawLine(BlackPen, j * width + x0, 0 + y0, j * width + x0, (n + 4) * height + y0);
                //写出各组的表头（横）
                if (j > 0 && j <= k)
                {
                    g.DrawString(j.ToString(), font1, Brushes.Black, 10 + j * width + x0, 4 + y0);
                }
            }

            //画出表最下面的横线
            g.DrawLine(BlackPen, 0 + x0, (n + 10) * height + y0, (k + 1) * width + x0, (n + 10) * height + y0);

            //画出竖直的中心线
            g.DrawLine(BlackPen, ((k + 1) * width) / 2 + x0, (n + 4) * height + y0, ((k + 1) * width) / 2 + x0, (n + 10) * height + y0);

            //画出表的倒数第二根横线
            g.DrawLine(BlackPen, ((k + 1) * width) / 2 + x0, (n + 6) * height + y0, (k + 1) * width + x0, (n + 6) * height + y0);

            //写出“异常情况分析”
            g.DrawString(ExceptionalAnalysis1, font3, Brushes.Black, 3 + ((k + 1) * width) / 2 + x0, 3 + (n + 4) * height + y0);
            g.DrawString(ExceptionalAnalysis2, font3, Brushes.Black, 87 + ((k + 1) * width) / 2 + x0, 18 + (n + 4) * height + y0);

            //写出“原因分析”
            g.DrawString(ReasonAnalysis, font3, Brushes.Black, 3 + ((k + 1) * width) / 2 + x0, 3 + (n + 6) * height + y0);

            //把从数据库中取出的数据写到表中
            for (int x = 1; x <= k; x++)
            {
                for (int y = 1; y <= n; y++)
                {
                    g.DrawString(X[(x - 1) * n + (y - 1)].ToString("0.000").ToString().TrimEnd('0'), font1, Brushes.Black, 1 + x * width + x0, 4 + y * height + y0);
                }
            }

            //计算各组数的和、平均值和极差
            double[] R = new double[X.Length / n];
            double[] Xbar = new double[X.Length / n];
            double[] sum = new double[X.Length / n];

            SPC_Table_Data.CalcSumXbarR(X, n, out R, out Xbar, out sum);
            //向表中写入各组数据的和、均值和极差
            for (int a = 0; a < X.Length / n; a++)
            {
                g.DrawString(sum[a].ToString("0.000").ToString().TrimEnd('0'), font1, Brushes.Black, 2 + (a + 1) * width + x0, 4 + (n + 1) * height + y0);
                g.DrawString(Xbar[a].ToString("0.00").ToString().TrimEnd('0'), font1, Brushes.Black, 2 + (a + 1) * width + x0, 4 + (n + 2) * height + y0);
                g.DrawString(R[a].ToString("0.000").ToString().TrimEnd('0'), font1, Brushes.Black, 2 + (a + 1) * width + x0, 4 + (n + 3) * height + y0);
            }

            //计算各组平均值的均值
            double Xbarbar = SPC_Table_Data.CalcXbarbar(X, n);
            //把计算的均值写到表中
            g.DrawString("X=", font2, Brushes.Black, 10 + x0, 4 + (n + 5) * height + y0);
            g.DrawLine(BlackPen, 11 + x0, 3 + (n + 5) * height + y0, 18 + x0, 3 + (n + 5) * height + y0);
            g.DrawLine(BlackPen, 11 + x0, 1 + (n + 5) * height + y0, 18 + x0, 1 + (n + 5) * height + y0);
            g.DrawString(Xbarbar.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 30 + x0, 4 + (n + 5) * height + y0);

            //计算各组极差的均值
            double Rbar = SPC_Table_Data.CalcRbar(X, n);
            //把计算的极差均值写到表中
            g.DrawString("R=", font2, Brushes.Black, 10 + x0, 4 + (n + 6) * height + y0);
            g.DrawLine(BlackPen, 11 + x0, 3 + (n + 6) * height + y0, 18 + x0, 3 + (n + 6) * height + y0);
            g.DrawString(Rbar.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 30 + x0, 4 + (n + 6) * height + y0);

            //写出USL和LSL
            SolidBrush brush = new SolidBrush(Color.GreenYellow);
            //g.FillRectangle(brush, 10+x0, 4 + (n + 7) * height+y0, 65+x0, 34+y0);
            g.DrawString("USL=", font2, Brushes.Black, 10 + x0, 4 + (n + 7) * height + y0);
            g.DrawString(USL.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 40 + x0, 4 + (n + 7) * height + y0);
            g.DrawString("LSL=", font2, Brushes.Black, 10 + x0, 4 + (n + 8) * height + y0);
            g.DrawString(LSL.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 40 + x0, 4 + (n + 8) * height + y0);

            if (USL != 0 && LSL != 0)
            {
                //计算无偏稳态过程能力指数
                double Cp = SPC_Table_Data.CalcCp(n, X, LSL, USL, "XR");
                //把计算的能力指数写到表中
                g.DrawString("Cp=", font2, Brushes.Black, 120 + x0, 4 + (n + 5) * height + y0);
                g.DrawString(Cp.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 5) * height + y0);

                ////计算有偏稳态过程能力指数
                //double Cpu = SPC_Table_Data.CalcCpu(X, n, USL);
                //double Cpl = SPC_Table_Data.CalcCpl(X, n, LSL);
                //double Cpk = SPC_Table_Data.CalcCpk(Cpu, Cpl);

                //double ValueUp = Max(X);
                //double ValueDown = Min(X);
                //绘图数据
                CSpc_Data_Cpk Spc_Data_Cpk = Spc_Data.Cpk(X, n, Spc_Data_XR.UCL_X, Spc_Data_XR.LCL_X, "XR");

                //把计算的能力指数写到表中
                g.DrawString("Cpk=", font2, Brushes.Black, 120 + x0, 4 + (n + 6) * height + y0);
                g.DrawString(Spc_Data_Cpk.Cpk.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 6) * height + y0);
                //把计算的Cpu写到表中
                g.DrawString("Cpu=", font2, Brushes.Black, 120 + x0, 4 + (n + 7) * height + y0);
                g.DrawString(Spc_Data_Cpk.Cpu.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 7) * height + y0);
                //把计算的Cpl写到表中
                g.DrawString("Cpl=", font2, Brushes.Black, 120 + x0, 4 + (n + 8) * height + y0);
                g.DrawString(Spc_Data_Cpk.Cpl.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 8) * height + y0);

                //计算无偏过程性能指数
                double Pp = SPC_Table_Data.CalcPp(n, X, LSL, USL);
                //把计算的性能指数写到表中
                g.DrawString("Pp=", font2, Brushes.Black, 230 + x0, 4 + (n + 5) * height + y0);
                g.DrawString(Pp.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 5) * height + y0);

                //计算有偏过程性能指数
                double Ppu = SPC_Table_Data.CalcPpu(X, n, USL);
                double Ppl = SPC_Table_Data.CalcPpl(X, n, LSL);
                double Ppk = SPC_Table_Data.CalcPpk(Ppu, Ppl);
                //把计算的性能指数写到表中
                g.DrawString("Ppk=", font2, Brushes.Black, 230 + x0, 4 + (n + 6) * height + y0);
                g.DrawString(Ppk.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 6) * height + y0);
                //把计算的Ppu写到表中
                g.DrawString("Ppu=", font2, Brushes.Black, 230 + x0, 4 + (n + 7) * height + y0);
                g.DrawString(Ppu.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 7) * height + y0);
                //把计算的Ppl写到表中
                g.DrawString("Ppl=", font2, Brushes.Black, 230 + x0, 4 + (n + 8) * height + y0);
                g.DrawString(Ppl.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 8) * height + y0);

                //计算无偏过程能力比值Cr并写到表中
                double Cr = 1 / Cp;
                g.DrawString("Cr=", font2, Brushes.Black, 330 + x0, 4 + (n + 5) * height + y0);
                g.DrawString(Cr.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 360 + x0, 4 + (n + 5) * height + y0);

                //计算无偏过程性能比值Pr并写到表中
                double Pr = 1 / Pp;
                g.DrawString("Pr=", font2, Brushes.Black, 330 + x0, 4 + (n + 6) * height + y0);
                g.DrawString(Pr.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 360 + x0, 4 + (n + 6) * height + y0);
            }
            //g.Dispose();
            BlackPen.Dispose();
            font1.Dispose();
            font2.Dispose();
            font3.Dispose();
            //return tableBitmap;
        }

        #endregion 均值极差

        #region 均值标准差

        /// <summary>
        /// 均值标准差-均值图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable MeanStandardpoor_Mean(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();
            double UCL = 0;
            double CL = 0;
            double LCL = 0;
            double yMin = 0;
            double yMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                CSpc_Data_XS Spc_Data_XS = Spc_Data.XS(X, Sample);
                CL = Spc_Data_XS.CL_X;
                UCL = Spc_Data_XS.UCL_X;
                LCL = Spc_Data_XS.LCL_X;

                #endregion 计算SPC参数

                DataRow dr;
                table.Columns.Add("X_Data", typeof(double));
                table.Columns.Add("Y_Data", typeof(double));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region yMin,yMax

                yMin = Math.Min(Math.Min(SpcCaculator.Min(Spc_Data_XS.CL_Xk), UCL), LCL) - (UCL + LCL) / 2;
                yMax = Math.Max(Math.Max(SpcCaculator.Max(Spc_Data_XS.CL_Xk), UCL), LCL) + (UCL + LCL) / 2;

                #endregion yMin,yMax

                for (int i = 0; i < Spc_Data_XS.CL_Xk.Length; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = i + 1;
                    dr["Y_Data"] = Spc_Data_XS.CL_Xk[i];
                    dr["SPC_KeyValue"] = UCL.ToString("F3") + "," + CL.ToString("F3") + "," + LCL.ToString("F3") + "," + yMin.ToString("F3") + "," + yMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 均值标准差-标准差图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable MeanStandardpoor_Standardpoor(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();
            double UCL = 0;
            double CL = 0;
            double LCL = 0;
            double yMin = 0;
            double yMax = 0;

            double[] X;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                CSpc_Data_XS Spc_Data_XS = Spc_Data.XS(X, Sample);
                CL = Spc_Data_XS.CL_S;
                UCL = Spc_Data_XS.UCL_S;
                LCL = Spc_Data_XS.LCL_S;

                #endregion 计算SPC参数

                DataRow dr;
                table.Columns.Add("X_Data", typeof(double));
                table.Columns.Add("Y_Data", typeof(double));
                table.Columns.Add("SPC_KeyValue", typeof(string));

                #region yMin,yMax

                yMin = Math.Min(Math.Min(SpcCaculator.Min(Spc_Data_XS.CL_Sk), UCL), LCL) - (UCL + LCL) / 2;
                yMax = Math.Max(Math.Max(SpcCaculator.Max(Spc_Data_XS.CL_Sk), UCL), LCL) + (UCL + LCL) / 2;

                #endregion yMin,yMax

                for (int i = 0; i < Spc_Data_XS.CL_Sk.Length; i++)
                {
                    dr = table.NewRow();
                    dr["X_Data"] = i + 1;
                    dr["Y_Data"] = Spc_Data_XS.CL_Sk[i];
                    dr["SPC_KeyValue"] = UCL.ToString("F3") + "," + CL.ToString("F3") + "," + LCL.ToString("F3") + "," + yMin.ToString("F3") + "," + yMax.ToString("F3");
                    table.Rows.Add(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// 均值标准差-数据图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Sample"></param>
        /// <param name="MeasurementsName"></param>
        /// <returns></returns>
        public DataTable MeanStandardpoorTablePic(DataTable dt, int Sample, string MeasurementsName)
        {
            DataTable table = new DataTable();

            double[] X;
            double USL = 0;
            //double CL = 0;
            double LSL = 0;

            if (dt.Rows.Count > 0)
            {
                #region X

                X = new double[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    X[i] = Convert.ToDouble(dt.Rows[i][MeasurementsName]);
                }

                #endregion X

                #region 计算SPC参数

                //CSpc_Data_XS Spc_Data_XS = Spc_Data.XS(X, Sample);
                //CL = Spc_Data_XS.CL_X;
                //UCL = Spc_Data_XS.UCL_X;
                //LCL = Spc_Data_XS.LCL_X;
                try
                {
                    USL = Convert.ToDouble(dt.Rows[0][1]);
                    LSL = Convert.ToDouble(dt.Rows[0][2]);
                    //规格上下合理性判断，超过90%样本，就算不合理
                    if ((X.Where(x => x > USL).Count() * 1.0 / X.Count()) > 0.9 || (X.Where(x => x < LSL).Count() * 1.0 / X.Count()) > 0.9)
                    {
                        USL = 0;
                        LSL = 0;
                    }
                }
                catch
                {
                }

                #endregion 计算SPC参数

                #region 生成图片Image

                string ImageCode = "";

                Bitmap bp = DrawXSChart_X_2018(X, Sample, LSL, USL);
                IntPtr pr = bp.GetHbitmap();
                Image _Image = Image.FromHbitmap(pr);
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();
                _Image.Save(_MemoryStream, ImageFormat.Jpeg);
                byte[] _ImageBytes = _MemoryStream.ToArray();

                ImageCode = Convert.ToBase64String(_ImageBytes);

                #endregion 生成图片Image

                DataRow dr;
                table.Columns.Add("ImageCode", typeof(string));
                dr = table.NewRow();
                dr["ImageCode"] = ImageCode;
                table.Rows.Add(dr);
            }
            return table;
        }

        /// <summary>
        /// 均值-标准差控制图无返回值
        /// </summary>
        public static Bitmap DrawXSChart_X_2018(double[] X, int n, double LSL, double USL)
        {
            Bitmap mybBitmap = new Bitmap(1000, 330);
            Graphics mybGraphics = Graphics.FromImage(mybBitmap); ;
            Spc_Table_XS(X, n, LSL, USL, mybGraphics);
            return mybBitmap;
        }

        /// <summary>
        /// 画XS图的表
        /// </summary>
        /// <param name="X">数据</param>
        /// <param name="n">没个样本的数据个数</param>
        /// <param name="LSL">规格下限</param>
        /// <param name="USL">规格上限</param>
        /// <returns></returns>
        public static void Spc_Table_XS(double[] X, int n, double LSL, double USL, Graphics g)
        {
            CSpc_Data_XS Spc_Data_XS = Spc_Data.XS(X, n);
            USL = USL == 0 ? Convert.ToDouble(Spc_Data_XS.UCL_X) : USL;
            LSL = LSL == 0 ? Convert.ToDouble(Spc_Data_XS.LCL_X) : LSL;

            int x0 = 5;
            int y0 = 5;

            int k = X.Length / n;
            Pen BlackPen = new Pen(Color.Black, 1);
            //Bitmap tableBitmap = new Bitmap(1000, (n + 10) * 15 + 5);
            //Graphics g = Graphics.FromImage(tableBitmap);
            Font font1 = new Font("Tahoma", 7);
            Font font2 = new Font("Tahoma", 8, FontStyle.Bold);
            Font font3 = new Font("Tahoma", 8);
            g.Clear(Color.White);
            int width = 990 / (k + 1);
            int height = 15;
            string ExceptionalAnalysis1 = "异常情况分析：(1)出现超出控制限的点；(2)连续七个点全在控制限之上或之下；";
            string ExceptionalAnalysis2 = "(3)任何其他明显非随机的图形；";
            string ReasonAnalysis = "内容(原因分析、对策和整改情况)：";
            //float width = g.MeasureString(data, new Font("Tahoma", 10)).Width;
            //float height = g.MeasureString(data, new Font("Tahoma", 10)).Height;

            //反锯齿算法
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            g.DrawString("组号", font1, Brushes.Black, 0 + x0, 4 + y0);

            //画水平线并写出表头
            for (int i = 0; i <= n + 4; i++)
            {
                //画水平的表格线
                g.DrawLine(BlackPen, 0 + x0, i * height + y0, (k + 1) * width + x0, i * height + y0);

                //写出各组的表头（竖）
                if (i > 0 && i <= n)
                {
                    g.DrawString(i.ToString(), font1, Brushes.Black, 10 + x0, 4 + i * height + y0);
                }

                if (i == n + 1)
                {
                    g.DrawString("求和", font1, Brushes.Black, 3 + x0, 4 + i * height + y0);
                }

                if (i == n + 2)
                {
                    g.DrawString("X", font1, Brushes.Black, 10 + x0, 4 + i * height + y0);
                    g.DrawLine(BlackPen, 11 + x0, 4 + i * height + y0, 16 + x0, 4 + i * height + y0);
                }

                if (i == n + 3)
                {
                    g.DrawString("S", font1, Brushes.Black, 10 + x0, 4 + i * height + y0);
                }
            }

            //画垂直线并写出表头
            for (int j = 0; j <= k + 1; j++)
            {
                //画垂直的表格线
                g.DrawLine(BlackPen, j * width + x0, 0 + y0, j * width + x0, (n + 4) * height + y0);

                //写出各组的表头（横）
                if (j > 0 && j <= k)
                {
                    g.DrawString(j.ToString(), font1, Brushes.Black, 10 + j * width + x0, 4 + y0);
                }
            }

            //画出表最下面的横线
            g.DrawLine(BlackPen, 0 + x0, (n + 10) * height + y0, (k + 1) * width + x0, (n + 10) * height + y0);

            //画出竖直的中心线
            g.DrawLine(BlackPen, ((k + 1) * width) / 2 + x0, (n + 4) * height + y0, ((k + 1) * width) / 2 + x0, (n + 10) * height + y0);

            //画出表的倒数第二根横线
            g.DrawLine(BlackPen, ((k + 1) * width) / 2 + x0, (n + 6) * height + y0, (k + 1) * width + x0, (n + 6) * height + y0);

            //写出“异常情况分析”
            g.DrawString(ExceptionalAnalysis1, font3, Brushes.Black, 3 + ((k + 1) * width) / 2 + x0, 3 + (n + 4) * height + y0);
            g.DrawString(ExceptionalAnalysis2, font3, Brushes.Black, 87 + ((k + 1) * width) / 2 + x0, 18 + (n + 4) * height + y0);

            //写出“原因分析”
            g.DrawString(ReasonAnalysis, font3, Brushes.Black, 3 + ((k + 1) * width) / 2 + x0, 3 + (n + 6) * height + y0);

            //把从数据库中取出的数据写到表中
            for (int x = 1; x <= k; x++)
            {
                for (int y = 1; y <= n; y++)
                {
                    g.DrawString(X[(x - 1) * n + (y - 1)].ToString("0.000").ToString().TrimEnd('0'), font1, Brushes.Black, 1 + x * width + x0, 4 + y * height + y0);
                }
            }

            //计算各组数的和、平均值和标准差
            double[] R = new double[X.Length / n];
            double[] Xbar = new double[X.Length / n];
            double[] sum = new double[X.Length / n];
            double[] S = new double[X.Length / n];

            SPC_Table_Data.CalcSumXbarR(X, n, out R, out Xbar, out sum);
            SPC_Table_Data.CaluS(X, n, out S);
            //向表中写入各组数据的和、均值和标准差
            for (int a = 0; a < X.Length / n; a++)
            {
                g.DrawString(sum[a].ToString("0.000").ToString().TrimEnd('0'), font1, Brushes.Black, 2 + (a + 1) * width + x0, 4 + (n + 1) * height + y0);
                g.DrawString(Xbar[a].ToString("0.00").ToString().TrimEnd('0'), font1, Brushes.Black, 2 + (a + 1) * width + x0, 4 + (n + 2) * height + y0);
                g.DrawString(S[a].ToString("0.000").ToString().TrimEnd('0'), font1, Brushes.Black, 2 + (a + 1) * width + x0, 4 + (n + 3) * height + y0);
            }

            //计算各组平均值的均值
            double Xbarbar = SPC_Table_Data.CalcXbarbar(X, n);
            //把计算的均值写到表中
            g.DrawString("X=", font2, Brushes.Black, 10 + x0, 4 + (n + 5) * height + y0);
            g.DrawLine(BlackPen, 11 + x0, 3 + (n + 5) * height + y0, 18 + x0, 3 + (n + 5) * height + y0);
            g.DrawLine(BlackPen, 11 + x0, 1 + (n + 5) * height + y0, 18 + x0, 1 + (n + 5) * height + y0);
            g.DrawString(Xbarbar.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 30 + x0, 4 + (n + 5) * height + y0);

            //计算各组极差的均值
            double Sbar = SPC_Table_Data.CalcSbar(X, n);
            //把计算的极差均值写到表中
            g.DrawString("S=", font2, Brushes.Black, 10 + x0, 4 + (n + 6) * height + y0);
            g.DrawLine(BlackPen, 11 + x0, 3 + (n + 6) * height + y0, 18 + x0, 3 + (n + 6) * height + y0);
            g.DrawString(Sbar.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 30 + x0, 4 + (n + 6) * height + y0);

            //写出USL和LSL
            SolidBrush brush = new SolidBrush(Color.GreenYellow);
            //g.FillRectangle(brush, 10+x0, 4 + (n + 7) * height+y0, 65+x0, 34+y0);
            g.DrawString("USL=", font2, Brushes.Black, 10 + x0, 4 + (n + 7) * height + y0);
            g.DrawString(USL.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 40 + x0, 4 + (n + 7) * height + y0);
            g.DrawString("LSL=", font2, Brushes.Black, 10 + x0, 4 + (n + 8) * height + y0);
            g.DrawString(LSL.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 40 + x0, 4 + (n + 8) * height + y0);

            if (USL != 0 && LSL != 0)
            {
                //计算无偏稳态过程能力指数
                double Cp = SPC_Table_Data.CalcCp(n, X, LSL, USL, "XS");
                //把计算的能力指数写到表中
                g.DrawString("Cp=", font2, Brushes.Black, 120 + x0, 4 + (n + 5) * height + y0);
                g.DrawString(Cp.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 5) * height + y0);

                ////计算有偏稳态过程能力指数
                //double Cpu = SPC_Table_Data.CalcCpu(X, n, USL);
                //double Cpl = SPC_Table_Data.CalcCpl(X, n, LSL);
                //double Cpk = SPC_Table_Data.CalcCpk(Cpu, Cpl);

                //double ValueUp = Max(X);
                //double ValueDown = Min(X);
                //绘图数据
                CSpc_Data_Cpk Spc_Data_Cpk = Spc_Data.Cpk(X, n, Spc_Data_XS.UCL_X, Spc_Data_XS.LCL_X, "XS");

                //把计算的能力指数写到表中
                g.DrawString("Cpk=", font2, Brushes.Black, 120 + x0, 4 + (n + 6) * height + y0);
                g.DrawString(Spc_Data_Cpk.Cpk.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 6) * height + y0);
                //把计算的Cpu写到表中
                g.DrawString("Cpu=", font2, Brushes.Black, 120 + x0, 4 + (n + 7) * height + y0);
                g.DrawString(Spc_Data_Cpk.Cpu.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 7) * height + y0);
                //把计算的Cpl写到表中
                g.DrawString("Cpl=", font2, Brushes.Black, 120 + x0, 4 + (n + 8) * height + y0);
                g.DrawString(Spc_Data_Cpk.Cpl.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 150 + x0, 4 + (n + 8) * height + y0);

                //计算无偏过程性能指数
                double Pp = SPC_Table_Data.CalcPp(n, X, LSL, USL);
                //把计算的性能指数写到表中
                g.DrawString("Pp=", font2, Brushes.Black, 230 + x0, 4 + (n + 5) * height + y0);
                g.DrawString(Pp.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 5) * height + y0);

                //计算有偏过程性能指数
                double Ppu = SPC_Table_Data.CalcPpu(X, n, USL);
                double Ppl = SPC_Table_Data.CalcPpl(X, n, LSL);
                double Ppk = SPC_Table_Data.CalcPpk(Ppu, Ppl);
                //把计算的性能指数写到表中
                g.DrawString("Ppk=", font2, Brushes.Black, 230 + x0, 4 + (n + 6) * height + y0);
                g.DrawString(Ppk.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 6) * height + y0);
                //把计算的Ppu写到表中
                g.DrawString("Ppu=", font2, Brushes.Black, 230 + x0, 4 + (n + 7) * height + y0);
                g.DrawString(Ppu.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 7) * height + y0);
                //把计算的Ppl写到表中
                g.DrawString("Ppl=", font2, Brushes.Black, 230 + x0, 4 + (n + 8) * height + y0);
                g.DrawString(Ppl.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 260 + x0, 4 + (n + 8) * height + y0);

                //计算无偏过程能力比值Cr并写到表中
                double Cr = 1 / Cp;
                g.DrawString("Cr=", font2, Brushes.Black, 330 + x0, 4 + (n + 5) * height + y0);
                g.DrawString(Cr.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 360 + x0, 4 + (n + 5) * height + y0);

                //计算无偏过程性能比值Pr并写到表中
                double Pr = 1 / Pp;
                g.DrawString("Pr=", font2, Brushes.Black, 330 + x0, 4 + (n + 6) * height + y0);
                g.DrawString(Pr.ToString("0.000").ToString().TrimEnd('0'), font3, Brushes.Black, 360 + x0, 4 + (n + 6) * height + y0);
            }
            //g.Dispose();
            BlackPen.Dispose();
            font1.Dispose();
            font2.Dispose();
            font3.Dispose();

            //return tableBitmap;
        }

        #endregion 均值标准差

        /// <summary>
        /// 观测值最大值
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static private double Max(double[] Xn)
        {
            double temp;
            temp = Xn[0];
            for (int i = 0; i < Xn.Length; i++)
            {
                temp = (Xn[i] > temp) ? Xn[i] : temp;
            }
            return temp;
        }

        /// <summary>
        /// 观测值最小值
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static private double Min(double[] Xn)
        {
            double temp;
            temp = Xn[0];
            for (int i = 0; i < Xn.Length; i++)
            {
                temp = (Xn[i] < temp) ? Xn[i] : temp;
            }
            return temp;
        }
    }
}