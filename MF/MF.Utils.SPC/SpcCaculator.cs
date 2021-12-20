using System;
using System.Collections;
using System.Data;
using System.Linq;

namespace MF.Utils.SPC
{
    public class SpcCaculator
    {
        private static readonly double QU_k = 0.99865;
        private static readonly double QL_k = 0.00135;

        /// <summary>
        /// 计算SPC主要参数：
        /// 输入："X"被分析数据组；Usl理论上限；Lsl理论下限。
        /// 输出：x平均值；"s"均方差；"QU"百分比上限；"QL"百分比下限；"cp"工序能力；"cpk"工序能力指数
        /// </summary>
        /// <param name="X">被分析数据组</param>
        /// <param name="Usl">理论上限</param>
        /// <param name="Lsl">理论下限</param>
        /// <param name="x">平均值</param>
        /// <param name="s">均方差</param>
        /// <param name="QU">百分比上限</param>
        /// <param name="QL">百分比下限</param>
        /// <param name="cp">工序能力</param>
        /// <param name="cpk">工序能力指数</param>
        public static void SpcValue(double[] X, ref double Usl, ref double Lsl, out double x, out double s,
            out double QU, out double QL, out double cp, out double cpk, out double[] sx, out double[] sy)
        {
            x = AVERAGE(X);
            s = STDEV(X);
            if (Usl == Lsl)
            {
                Usl = x + (4.0 * s);
                Lsl = x - (4.0 * s);
            }
            QU_QL(X, out QU, out QL);
            cp = Cp(QU, QL, Usl, Lsl);
            cpk = Cpk(x, QU, QL, Usl, Lsl);
            sxy(s, x, out sx, out sy);
        }

        /// <summary>
        /// 均方差
        /// </summary>
        /// <param name="X">被分析数据组</param>
        /// <returns>均方差</returns>
        public static double STDEV(double[] X)
        {
            if (X.Length < 2)
                return 0;
            double s = 0;
            //平均值
            double x;
            x = AVERAGE(X);
            for (int i = 0; i < X.Length; i++)
            {
                s += (X[i] - x) * (X[i] - x);
            }
            s = Math.Sqrt(s / (double)(X.Length - 1));
            return s;
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="X">被分析数据组</param>
        /// <returns>平均值</returns>
        public static double AVERAGE(double[] X)
        {
            return X.Average();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static double[] Sort(double[] X)
        {
            ArrayList temp = new ArrayList(X);
            temp.Sort();
            return (double[])temp.ToArray(typeof(double));
        }

        /// <summary>
        /// 已知表格行数为m，各行的数值分别记为 ，其中 表示第 行的数值，
        /// 将这 个数值由小到大排列，记排列后的数值为 ，其中 表示 个数值经排列之后第k个数值。
        /// 给定上下标线数值0.99865和0.00135。计算Q上和Q下的方法为：
        ///a = （m - 1）* 0.99865
        ///取a的整数部分为i，小数部分为j。
        ///Q上 = （1 – j）* x（i+1）+ j * x（i+2）
        ///同理：
        ///b = （m - 1）* 0.00135
        ///取b的整数部分为i，小数部分为j。
        ///Q下 = （1 – j）* x（i+1）+ j * x（i+2）
        /// </summary>
        /// <param name="X">被分析数据组</param>
        /// <param name="QU">百分比上限</param>
        /// <param name="QL">百分比下限</param>
        public static void QU_QL(double[] X, out double QU, out double QL)
        {
            double QU_n;
            int QU_n_i;
            double QU_n_j;
            double QL_n;
            int QL_n_i;
            double QL_n_j;

            X = Sort(X);

            QU_n = (double)(X.Length - 1) * QU_k;
            QU_n_i = (int)QU_n;
            QU_n_j = QU_n - QU_n_i;
            //Q上 = （1 – j）* x（i+1）+ j * x（i+2）
            try
            {
                QU = (1 - QU_n_j) * X[QU_n_i] + QU_n_j * X[QU_n_i + 1];
            }
            catch (Exception)
            {

                throw new Exception("计算失败");
            }
            

            QL_n = (double)(X.Length - 1) * QL_k;
            QL_n_i = (int)QL_n;
            QL_n_j = QL_n - QL_n_i;
            //Q下 = （1 – j）* x（i+1）+ j * x（i+2）
            QL = (1 - QL_n_j) * X[QL_n_i] + QL_n_j * X[QL_n_i + 1];

            return;
        }

        /// <summary>
        /// 工序能力
        /// </summary>
        /// <param name="X">被分析数据组</param>
        /// <param name="Usl">理论上限</param>
        /// <param name="Lsl">理论下限</param>
        /// <returns>工序能力</returns>
        public static double Cp(double[] X, double Usl, double Lsl)
        {
            QU_QL(X, out double QU, out double QL);

            double cp = (Usl - Lsl) / (QU - QL);
            return cp;
        }

        /// <summary>
        /// 工序能力
        /// </summary>
        /// <param name="QU">百分比上限</param>
        /// <param name="QL">百分比下限</param>
        /// <param name="Usl">理论上限</param>
        /// <param name="Lsl">理论下限</param>
        /// <returns></returns>
        public static double Cp(double QU, double QL, double Usl, double Lsl)
        {
            double cp = (Usl - Lsl) / (QU - QL);
            return cp;
        }

        /// <summary>
        /// 工序能力指数
        /// </summary>
        /// <param name="[] X">被分析数据组</param>
        /// <param name="Usl">理论上限</param>
        /// <param name="Lsl">理论下限</param>
        /// <returns></returns>
        public static double Cpk(double[] X, double Usl, double Lsl)
        {
            double cpk;
            double cpkU;
            double cpkL;
            //均方差
            //double s;
            //均值
            double x;
            x = AVERAGE(X);

            QU_QL(X, out double QU, out double QL);

            double cp = (Usl - Lsl) / (QU - QL);

            cpkU = (Usl - x) / (QU - x);
            cpkL = (Lsl - x) / (QL - x);
            cpk = Math.Min(cpkU, cpkL);
            return cpk;
        }

        /// <summary>
        /// 工序能力指数
        /// </summary>
        /// <param name="x">平均值</param>
        /// <param name="QU">百分比上限</param>
        /// <param name="QL">百分比下限</param>
        /// <param name="Usl">理论上限</param>
        /// <param name="Lsl">理论下限</param>
        /// <returns></returns>
        public static double Cpk(double x, double QU, double QL, double Usl, double Lsl)
        {
            double cpk;
            double cpkU;
            double cpkL;
            double cp = Cp(QU, QL, Usl, Lsl);
            cpkU = (Usl - x) / (QU - x);
            cpkL = (x - Lsl) / (x - QL);
            cpk = Math.Min(cpkU, cpkL);
            return cpk;
        }

        /// <summary>
        /// 计算正态分布的x坐标，y坐标
        /// </summary>
        /// <param name="s">均方差</param>
        /// <param name="x">平均值</param>
        public static void sxy(double s, double x, out double[] sx, out double[] sy)
        {
            double S2;
            double X2;
            X2 = x;
            S2 = s;
            sx = new double[11];
            sy = new double[11];
            double tempmaxdouble = 1.0 / (s * Math.Sqrt(2.0 * Math.PI));
            sx[0] = X2 - 4.0 * S2;
            sx[1] = X2 - 3.0 * S2;
            sx[2] = X2 - 2.0 * S2;
            sx[3] = X2 - 1.0 * S2;
            sx[4] = X2 - 0.5 * S2;
            sx[5] = X2;
            sx[6] = X2 + 0.5 * S2;
            sx[7] = X2 + 1.0 * S2;
            sx[8] = X2 + 2.0 * S2;
            sx[9] = X2 + 3.0 * S2;
            sx[10] = X2 + 4.0 * S2;

            sy[0] = tempmaxdouble * Math.Exp(-16.0F / 2.0F);
            sy[1] = tempmaxdouble * Math.Exp(-9.0F / 2.0F);
            sy[2] = tempmaxdouble * Math.Exp(-4.0F / 2.0F);
            sy[3] = tempmaxdouble * Math.Exp(-1.0F / 2.0F);
            sy[4] = tempmaxdouble * Math.Exp(-0.25F / 2.0F);
            sy[5] = tempmaxdouble;
            sy[6] = tempmaxdouble * Math.Exp(-0.25F / 2.0F);
            sy[7] = tempmaxdouble * Math.Exp(-1.0F / 2.0F);
            sy[8] = tempmaxdouble * Math.Exp(-4.0F / 2.0F);
            sy[9] = tempmaxdouble * Math.Exp(-9.0F / 2.0F);
            sy[10] = tempmaxdouble * Math.Exp(-16.0F / 2.0F);

            double sy_sum = 0;
            for (int i = 0; i < sy.Length; i++)
            {
                sy_sum += sy[i];
            }
            for (int i = 0; i < sy.Length; i++)
            {
                sy[i] = sy[i] / sy_sum;
            }
        }

        /// <summary>
        /// 根据查询的数据，获得理论上限，理论下限
        /// </summary>
        /// <param name="dl"></param>
        /// <param name="Usl">理论上限</param>
        /// <param name="Lsl">理论下限</param>
        public static void GetUslLsl(DataTable dt, out double Usl, out double Lsl)
        {
            Usl = 0;
            Lsl = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        Usl = Convert.ToDouble(dt.Rows[0]["上限值"]);
                        Lsl = Convert.ToDouble(dt.Rows[0]["下限值"]);
                    }
                    catch
                    {
                        Usl = 0;
                        Lsl = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 观测值最大值
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static public double Max(double[] Xn)
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
        static public double Min(double[] Xn)
        {
            double temp;
            temp = Xn[0];
            for (int i = 0; i < Xn.Length; i++)
            {
                temp = (Xn[i] < temp) ? Xn[i] : temp;
            }
            return temp;
        }

        public static double RealX(double[] sx, double val)
        {
            double realVal = 0.00;
            if (val < sx[0])
            {
                realVal = 0.00 - (sx[0] - val) / (2 * sx[0]);
            }
            else
            {
                var thanobj = sx[^1];
                if (val > thanobj)
                {
                    realVal = sx.Length - 0.5;
                }
                if (val == thanobj)
                {
                    realVal = sx.Length - 1;
                }
                if (val < thanobj)
                {
                    for (int i = 0; i < sx.Length - 1; i++)
                    {
                        if (val > sx[i] && val < sx[i + 1])
                        {
                            realVal = i + (val - sx[i]) / (sx[i + 1] - sx[i]);
                        }
                        if (val == sx[i])
                        {
                            realVal = i;
                        }
                    }
                }
            }
            return realVal;
        }

        static public double RoundInterval(double interval)
        {
            // If the interval is zero return error
            if (interval == 0.0)
            {
                throw (new ArgumentOutOfRangeException("interval", "Interval can not be zero."));
            }

            // If the real interval is > 1.0
            double step = -1;
            double tempValue = interval;
            while (tempValue > 1.0)
            {
                step++;
                tempValue /= 10.0;
                if (step > 1000)
                {
                    throw (new InvalidOperationException("Auto interval error due to invalid point values or axis minimum/maximum."));
                }
            }

            // If the real interval is < 1.0
            tempValue = interval;
            if (tempValue < 1.0)
            {
                step = 0;
            }

            while (tempValue < 1.0)
            {
                step--;
                tempValue *= 10.0;
                if (step < -1000)
                {
                    throw (new InvalidOperationException("Auto interval error due to invalid point values or axis minimum/maximum."));
                }
            }

            double tempDiff = interval / Math.Pow(10.0, step);
            if (tempDiff < 3.0)
            {
                tempDiff = 2.0;
            }
            else if (tempDiff < 7.0)
            {
                tempDiff = 5.0;
            }
            else
            {
                tempDiff = 10.0;
            }

            // Make a correction of the real interval
            return tempDiff * Math.Pow(10.0, step);
        }
    }

    /// <summary>
    /// 测试数据
    /// </summary>
    public class Spc_Data_TestData
    {
        /// <summary>
        /// 子组大小。单个子组观测值的个数
        /// </summary>
        static public int n = 5;

        /// <summary>
        /// 子组个数
        /// </summary>
        static public int k = 30;

        /// <summary>
        /// 特征值上限
        /// </summary>
        static public double USL = 1.7;

        /// <summary>
        /// 特征值下限
        /// </summary>
        static public double LSL = 1.5;

        /// <summary>
        /// 特征值上限
        /// </summary>
        static public double USL1 = 1.0;

        /// <summary>
        /// 特征值下限
        /// </summary>
        static public double LSL1 = 0.9;

        /// <summary>
        /// 特征值上限
        /// </summary>
        static public double USL2 = 1.15;

        /// <summary>
        /// 特征值下限
        /// </summary>
        static public double LSL2 = 1.10;

        /// <summary>
        /// 特征值上限
        /// </summary>
        static public double USL3 = 129.01;

        /// <summary>
        /// 特征值下限
        /// </summary>
        static public double LSL3 = 129;

        static public double[] X3 =
            {
            129.006,129.006,129.006,129.005,129.006,
            129.006,129.005,129.005,129.005,129.005,
            129.005,129.006,129.007,129.006,129.005,
            129.005,129.005,129.006,129.005,129.006,
            129.006,129.006,129.007,129.006,129.005,
            129.007,129.006,129.005,129.005,129.006,
            129.006,129.007,129.005,129.005,129.005,
            129.007,129.006,129.006,129.007,129.005,
            129.007,129.005,129.005,129.006,129.007,
            129.006,129.007,129.005,129.005,129.006
        };

        static public double[] X2 =
            {
            1.141,1.130,1.131,1.127,1.137,
            1.140,1.137,1.130,1.135,1.125,
            1.133,1.133,1.131,1.128,1.127,
            1.122,1.131,1.131,1.125,1.123,
            1.131,1.128,1.117,1.133,1.136,
            1.123,1.128,1.135,1.127,1.130,
            1.138,1.126,1.130,1.133,1.133,
            1.130,1.127,1.128,1.127,1.128,
            1.122,1.142,1.128,1.135,1.133,
            1.142,1.127,1.133,1.135,1.135
        };

        static public double[] X1 =
            {   0.941,0.942,0.947,0.939,0.942,0.943,
            0.942,0.950,0.943,0.948,0.941,0.953,
            0.944,0.943,0.941,0.933,0.942,0.941,
            0.942,0.942,0.942,0.947,0.938,0.941,
            0.947,0.943,0.943,0.948,0.938,0.946,
            0.946,0.943,0.945,0.946,0.942,0.951,
            0.951,0.938,0.938,0.952,0.947,0.945,
            0.951,0.948,0.946,0.947,0.947,0.947,
            0.953,0.956
            };

        static public double[] X =
                {   1.55,1.58,1.61,1.60,1.60,//1
				1.58,1.63,1.63,1.62,1.63,//2
				1.62,1.63,1.62,1.59,1.58,//3
				1.58,1.60,1.61,1.62,1.63,//4
				1.58,1.64,1.63,1.62,1.62,//5
				1.62,1.62,1.63,1.61,1.57,//6
				1.64,1.62,1.61,1.60,1.58,//7
				1.57,1.59,1.61,1.62,1.63,//8
				1.58,1.61,1.60,1.62,1.63,//9
				1.60,1.61,1.64,1.64,1.63,//10
				1.58,1.60,1.62,1.63,1.65,//11
				1.62,1.58,1.59,1.57,1.58,//12
				1.57,1.57,1.58,1.59,1.64,//13
				1.61,1.64,1.62,1.60,1.59,//14
				1.65,1.62,1.62,1.60,1.58,//15
				1.57,1.59,1.57,1.59,1.62,//16
				1.56,1.57,1.57,1.61,1.62,//17
				1.56,1.58,1.59,1.60,1.62,//18
				1.58,1.60,1.60,1.62,1.63,//19
				1.58,1.59,1.60,1.63,1.62,//20
				1.58,1.59,1.62,1.63,1.64,//21
				1.58,1.59,1.62,1.63,1.61,//22
				1.58,1.59,1.60,1.61,1.63,//23
				1.57,1.59,1.61,1.61,1.62,//24
				1.58,1.58,1.60,1.61,1.63,//25
				1.62,1.58,1.58,1.58,1.57,//26
				1.63,1.59,1.57,1.58,1.57,//27
				1.58,1.62,1.61,1.63,1.61,//28
				1.58,1.57,1.59,1.60,1.62,//29
				1.62,1.60,1.60,1.57,1.57 //30
			};
    }
}