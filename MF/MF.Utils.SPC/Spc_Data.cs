using System;
using System.Data;

namespace MF.Utils.SPC
{
    /// <summary>
    /// Spc_Data 的摘要说明。
    /// n 	子组大小。单个子组观测值的个数
    /// k	子组个数
    /// </summary>
    public class Spc_Data
    {
        private static readonly Xml_Spc_Param Spc_Param = new Xml_Spc_Param();

        /// <summary>
        /// 观测值和
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static private double Sum(double[] Xn)
        {
            double temp;
            temp = 0;
            for (int i = 0; i < Xn.Length; i++)
            {
                temp += Xn[i];
            }
            return temp;
        }

        /// <summary>
        /// 分别计算各个子组观测值和
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <param name="Xk">子组平均值的数组</param>
        /// <returns>全部测量数据之和</returns>
        static private double Sum(double[] X, int n, out double[] Xk)
        {
            double temp;
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            Xk = new double[k];
            temp = 0;
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                    temp += X[j * n + i];
                }
                //子组平均值
                Xk[j] = Sum(Xi);
            }
            return temp;
        }

        /// <summary>
        /// 计算X在区间[ValueDown，ValueUp]的数量
        /// </summary>
        /// <param name="X"></param>
        /// <param name="GroupCount"></param>
        /// <param name="ValueDown"></param>
        /// <param name="ValueUp"></param>
        /// <param name="Yk"></param>
        static private void SumRang(double[] X, int GroupCount, double[] ValueDown, double[] ValueUp, out double[] Yk, out double[] YkCount)
        {
            int temp = 0;
            //总数量
            int allCount = 0;
            Yk = new double[GroupCount];
            YkCount = new double[GroupCount];
            for (int j = 0; j < GroupCount - 1; j++)
            {
                temp = 0;
                for (int i = 0; i < X.Length; i++)
                {
                    if (X[i] >= ValueDown[j] & X[i] < ValueUp[j])
                    {
                        temp++;
                        allCount++;
                    }
                }
                Yk[j] = temp;
            }

            for (int i = 0; i < X.Length; i++)
            {
                if (X[i] >= ValueDown[GroupCount - 1] & X[i] <= ValueUp[GroupCount - 1])
                {
                    temp++;
                    allCount++;
                }
            }
            Yk[GroupCount - 1] = temp;
            for (int i = 0; i < Yk.Length; i++)
            {
                YkCount[i] = Yk[i];
                Yk[i] = Yk[i] / (double)allCount * 100;
            }
        }

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

        /// <summary>
        /// 一组观测值的均值
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static private double Average(double[] Xn)
        {
            if (Xn.Length < 1) return 0;
            return Sum(Xn) / (double)Xn.Length;
        }

        /// <summary>
        /// 子组平均值的平均值
        /// n 	子组大小。单个子组观测值的个数
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <param name="Xk">子组平均值的数组</param>
        /// <returns></returns>
        static private double Average(double[] X, int n)
        {
            //k	子组个数
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            double[] Xk = new double[k];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                }
                //子组平均值
                Xk[j] = Average(Xi);
            }
            //子组平均值的平均值
            return Average(Xk);
        }

        /// <summary>
        /// 子组平均值的平均值
        /// n 	子组大小。单个子组观测值的个数
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <param name="Xk">子组平均值的数组</param>
        /// <returns></returns>
        static private double Average(double[] X, int n, out double[] Xk)
        {
            //k	子组个数
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            Xk = new double[k];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                }
                //子组平均值
                Xk[j] = Average(Xi);
            }
            //子组平均值的平均值
            return Average(Xk);
        }

        /// <summary>
        /// 不合格品百分数
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <param name="Xp">不合格品百分数</param>
        static private void Average_p(double[] X, int n, out double[] Xp)
        {
            //k	子组个数
            int k;
            k = X.Length;
            Xp = new double[k];
            for (int i = 0; i < k; i++)
            {
                //不合格品百分数
                Xp[i] = X[i] / (double)n * 100.0;
            }
        }       /// <summary>

        /// 子组极差。子组观测值中的极大值和极小值之差
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static private double Range(double[] Xn)
        {
            return Max(Xn) - Min(Xn);
        }

        /// <summary>
        /// 子组标准差
        /// </summary>
        /// <param name="Xn">子组观测值</param>
        /// <returns></returns>
        static private double StandardDeviation(double[] Xn)
        {
            if (Xn.Length < 2) return 0;
            double temp;
            double X1;
            X1 = Average(Xn);
            temp = 0;
            for (int i = 0; i < Xn.Length; i++)
            {
                temp += (Xn[i] - X1) * (Xn[i] - X1);
            }
            return Math.Sqrt(temp / (double)(Xn.Length - 1));
        }

        /// <summary>
        /// 子组极差的平均值
        /// n 	子组大小。单个子组观测值的个数
        /// k	子组个数
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <returns></returns>
        static private double Range_R2(double[] X, int n)
        {
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            double[] Rk = new double[k];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                }
                //子组极差
                Rk[j] = Range(Xi);
            }
            //子组极差的平均值
            return Average(Rk);
        }

        /// <summary>
        /// 子组极差的平均值
        /// n 	子组大小。单个子组观测值的个数
        /// k	子组个数
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <param name="Xk">子组极差数组</param>
        /// <returns></returns>
        static private double Range_R2(double[] X, int n, out double[] Rk)
        {
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            Rk = new double[k];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                }
                //子组极差
                Rk[j] = Range(Xi);
            }
            //子组极差的平均值
            return Average(Rk);
        }

        /// <summary>
        /// 子组标准差的平均值
        /// </summary>
        /// <param name="Xk">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <returns></returns>
        static private double StandardDeviation_S2(double[] X, int n)
        {
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            double[] Sk = new double[k];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                }
                //子组标准差
                Sk[j] = StandardDeviation(Xi);
            }
            //子组标准差的平均值
            return Average(Sk);
        }

        /// <summary>
        /// 子组标准差的平均值
        /// </summary>
        /// <param name="Xk">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        /// <param name="Sj">子组标准差</param>
        /// <returns></returns>
        static private double StandardDeviation_S2(double[] X, int n, out double[] Sk)
        {
            int k;
            k = X.Length / n;
            double[] Xi = new double[n];
            Sk = new double[k];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    Xi[i] = X[j * n + i];
                }
                //子组极差
                Sk[j] = StandardDeviation(Xi);
            }
            //子组极差的平均值
            return Average(Sk);
        }

        /// <summary>
        /// X-R均值极差图,计算结果保存在类Spc_Data_XR中
        /// 计算结果保存在类Spc_Data_XR中
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        static public CSpc_Data_XR XR(double[] X, int n)
        {
            CSpc_Data_XR Spc_Data_XR = new CSpc_Data_XR();
            int k = X.Length / n;
            Spc_Data_XR.n = n;
            Spc_Data_XR.k = k;
            Spc_Data_XR.CL_X = Average(X, n, out Spc_Data_XR.CL_Xk);
            Spc_Data_XR.CL_R = Range_R2(X, n, out Spc_Data_XR.CL_Rk);
            Spc_Data_XR.UCL_X = Spc_Data_XR.CL_X + Spc_Data_XR.CL_R * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_A2];
            Spc_Data_XR.LCL_X = Spc_Data_XR.CL_X - Spc_Data_XR.CL_R * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_A2];
            Spc_Data_XR.UCL_R = Spc_Data_XR.CL_R * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_D4];
            Spc_Data_XR.LCL_R = Spc_Data_XR.CL_R * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_D3];

            return Spc_Data_XR;
        }

        /// <summary>
        /// X-S 均值标准差图
        /// 计算结果保存在类Spc_Data_XS中
        /// </summary>
        /// <param name="X">全部测量数据数组</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        static public CSpc_Data_XS XS(double[] X, int n)
        {
            CSpc_Data_XS Spc_Data_XS = new CSpc_Data_XS();
            int k = X.Length / n;
            Spc_Data_XS.n = n;
            Spc_Data_XS.k = k;
            //double CL_R;

            //CL_R = Range_R2(X, n, out double[] CL_Rk);

            Spc_Data_XS.CL_X = Average(X, n, out Spc_Data_XS.CL_Xk);
            Spc_Data_XS.CL_S = StandardDeviation_S2(X, n, out Spc_Data_XS.CL_Sk);
            Spc_Data_XS.UCL_X = Spc_Data_XS.CL_X + Spc_Data_XS.CL_S * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_A3];
            Spc_Data_XS.LCL_X = Spc_Data_XS.CL_X - Spc_Data_XS.CL_S * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_A3];
            Spc_Data_XS.UCL_S = Spc_Data_XS.CL_S * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_B4];
            Spc_Data_XS.LCL_S = Spc_Data_XS.CL_S * (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_B3];

            return Spc_Data_XS;
        }

        /// <summary>
        /// p不合格品率控制图,np不合格数控制图。
        /// 计算结果保存在类Spc_Data_p中
        /// </summary>
        /// <param name="X">全部测量数据数组,不合格数</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        static public CSpc_Data_p p(double[] X, int n, int k)
        {
            double p;
            CSpc_Data_p Spc_Data_p = new CSpc_Data_p
            {
                n = n,
                k = k,
                p = (Sum(X) / (n * k)) * 100
            };
            p = Spc_Data_p.p / 100.0;
            Spc_Data_p.UCL_p = (p + 3 * Math.Sqrt(p * (1 - p) / n)) * 100;
            Spc_Data_p.LCL_p = (p - 3 * Math.Sqrt(p * (1 - p) / n)) * 100;
            Average_p(X, n, out Spc_Data_p.CL_pk);
            Spc_Data_p.CL_npk = new int[k];
            for (int i = 0; i < k; i++)
            {
                Spc_Data_p.CL_npk[i] = (int)X[i];
            }
            Spc_Data_p.np = Sum(X) / (double)k;
            double np;
            np = Spc_Data_p.np;
            Spc_Data_p.UCL_np = np + 3 * Math.Sqrt(np * (1 - p));
            Spc_Data_p.LCL_np = np - 3 * Math.Sqrt(np * (1 - p));

            return Spc_Data_p;
        }

        /// <summary>
        /// 计算能力指数。计算结果保存在类Spc_Data_Cpk中
        /// </summary>
        /// <param name="X">全部测量数据数组,不合格数</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        static public CSpc_Data_Cpk Cpk(double[] X, int n, double USL, double LSL, string Type = "XR")
        {
            //子组平均值的平均值
            double X2;
            //子组极差的平均值
            double R2;
            //子组标准差的平均值
            double S2;

            CSpc_Data_Cpk Spc_Data_Cpk = new CSpc_Data_Cpk();
            int k = X.Length / n;
            X2 = Average(X, n);
            R2 = Range_R2(X, n);
            S2 = StandardDeviation_S2(X, n);
            Spc_Data_Cpk.X2 = X2;
            Spc_Data_Cpk.n = n;
            Spc_Data_Cpk.k = k;
            Spc_Data_Cpk.Singma = (Type == "XR" ? R2 : S2) / (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_L_D1];
            Spc_Data_Cpk.SingmaS = S2;
            //如果没给出测量值的上限和下限
            if ((USL - LSL) == 0 || LSL > USL)
            {
                USL = X2 + 3 * S2;
                LSL = X2 - 3 * S2;
            }
            Spc_Data_Cpk.SL = (USL + LSL) / 2.0;
            Spc_Data_Cpk.LSL = LSL;
            Spc_Data_Cpk.USL = USL;
            Spc_Data_Cpk.Cp = (USL - LSL) / (6 * Spc_Data_Cpk.Singma);
            Spc_Data_Cpk.Cpu = (USL - X2) / (3 * Spc_Data_Cpk.Singma);
            Spc_Data_Cpk.Cpl = (X2 - LSL) / (3 * Spc_Data_Cpk.Singma);

            Spc_Data_Cpk.Pp = (USL - LSL) / (6 * Spc_Data_Cpk.SingmaS);
            Spc_Data_Cpk.Ppu = (USL - X2) / (3 * Spc_Data_Cpk.SingmaS);
            Spc_Data_Cpk.Ppl = (X2 - LSL) / (3 * Spc_Data_Cpk.SingmaS);
            if ((USL - LSL) != 0)
            {
                Spc_Data_Cpk.Ca = Math.Abs((USL + LSL) / 2 - X2) / ((USL - LSL) / 2);
                Spc_Data_Cpk.Cr = 6 * Spc_Data_Cpk.Singma / (USL - LSL);
                Spc_Data_Cpk.Pr = 6 * Spc_Data_Cpk.SingmaS / (USL - LSL);
            }

            Spc_Data_Cpk.Cpk = (1 - Math.Abs(Spc_Data_Cpk.Ca)) * Spc_Data_Cpk.Cp;
            Spc_Data_Cpk.Ppk = (1 - Math.Abs(Spc_Data_Cpk.Ca)) * Spc_Data_Cpk.Pp;

            Spc_Data_Cpk.GroupCount = 1 + (int)(3.32 * Math.Log10(k * n));
            Spc_Data_Cpk.ValueMax = Max(X);
            Spc_Data_Cpk.ValueMin = Min(X);
            Spc_Data_Cpk.ProcessSpread = Spc_Data_Cpk.ValueMax - Spc_Data_Cpk.ValueMin;
            Spc_Data_Cpk.GroupWidth = Spc_Data_Cpk.ProcessSpread / (double)Spc_Data_Cpk.GroupCount;
            Spc_Data_Cpk.Xk = new double[Spc_Data_Cpk.GroupCount];
            Spc_Data_Cpk.XkDown = new double[Spc_Data_Cpk.GroupCount];
            Spc_Data_Cpk.XkUp = new double[Spc_Data_Cpk.GroupCount];

            Spc_Data_Cpk.Xk[0] = Spc_Data_Cpk.ValueMin + Spc_Data_Cpk.GroupWidth / 2;
            Spc_Data_Cpk.XkDown[0] = Spc_Data_Cpk.ValueMin;
            Spc_Data_Cpk.XkUp[0] = Spc_Data_Cpk.ValueMin + Spc_Data_Cpk.GroupWidth;
            for (int i = 1; i < Spc_Data_Cpk.GroupCount; i++)
            {
                Spc_Data_Cpk.Xk[i] = Spc_Data_Cpk.Xk[i - 1] + Spc_Data_Cpk.GroupWidth;
                Spc_Data_Cpk.XkDown[i] = Spc_Data_Cpk.Xk[i] - Spc_Data_Cpk.GroupWidth / 2;
                Spc_Data_Cpk.XkUp[i] = Spc_Data_Cpk.Xk[i] + Spc_Data_Cpk.GroupWidth / 2;
            }

            SumRang(X, Spc_Data_Cpk.GroupCount, Spc_Data_Cpk.XkDown, Spc_Data_Cpk.XkUp, out Spc_Data_Cpk.Yk, out Spc_Data_Cpk.YkCount);
            Spc_Data_Cpk.NormalDistributionX = new double[9];
            Spc_Data_Cpk.NormalDistributionY = new double[9];

            Spc_Data_Cpk.NormalDistributionX[0] = X2 - 3.0 * S2;
            Spc_Data_Cpk.NormalDistributionX[1] = X2 - 2.0 * S2;
            Spc_Data_Cpk.NormalDistributionX[2] = X2 - 1.0 * S2;
            Spc_Data_Cpk.NormalDistributionX[3] = X2 - 0.5 * S2;
            Spc_Data_Cpk.NormalDistributionX[4] = X2;
            Spc_Data_Cpk.NormalDistributionX[5] = X2 + 0.5 * S2;
            Spc_Data_Cpk.NormalDistributionX[6] = X2 + 1.0 * S2;
            Spc_Data_Cpk.NormalDistributionX[7] = X2 + 2.0 * S2;
            Spc_Data_Cpk.NormalDistributionX[8] = X2 + 3.0 * S2;

            double tempmaxdouble = Max(Spc_Data_Cpk.Yk);

            Spc_Data_Cpk.NormalDistributionY[0] = tempmaxdouble * Math.Exp(-9.0F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[1] = tempmaxdouble * Math.Exp(-4.0F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[2] = tempmaxdouble * Math.Exp(-1.0F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[3] = tempmaxdouble * Math.Exp(-0.25F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[4] = tempmaxdouble;
            Spc_Data_Cpk.NormalDistributionY[5] = tempmaxdouble * Math.Exp(-0.25F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[6] = tempmaxdouble * Math.Exp(-1.0F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[7] = tempmaxdouble * Math.Exp(-4.0F / 2.0F);
            Spc_Data_Cpk.NormalDistributionY[8] = tempmaxdouble * Math.Exp(-9.0F / 2.0F);

            return Spc_Data_Cpk;
        }

        /// <summary>
        /// 计算能力指数。计算结果保存在类Spc_Data_Cpk中
        /// </summary>
        /// <param name="X">全部测量数据数组,不合格数</param>
        /// <param name="n">子组大小。单个子组观测值的个数</param>
        static public CSpc_Data_BarCpk BarCpk(double[] X)
        {
            CSpc_Data_BarCpk Spc_Data_BarCpk = new CSpc_Data_BarCpk
            {
                GroupCount = 1 + (int)(3.32 * Math.Log10(X.Length)),
                ValueMax = Max(X),
                ValueMin = Min(X)
            };
            Spc_Data_BarCpk.ProcessSpread = Spc_Data_BarCpk.ValueMax - Spc_Data_BarCpk.ValueMin;
            Spc_Data_BarCpk.GroupWidth = Spc_Data_BarCpk.ProcessSpread / (double)Spc_Data_BarCpk.GroupCount;
            Spc_Data_BarCpk.Xk = new double[Spc_Data_BarCpk.GroupCount];
            Spc_Data_BarCpk.XkDown = new double[Spc_Data_BarCpk.GroupCount];
            Spc_Data_BarCpk.XkUp = new double[Spc_Data_BarCpk.GroupCount];

            Spc_Data_BarCpk.Xk[0] = Spc_Data_BarCpk.ValueMin + Spc_Data_BarCpk.GroupWidth / 2;
            Spc_Data_BarCpk.XkDown[0] = Spc_Data_BarCpk.ValueMin;
            Spc_Data_BarCpk.XkUp[0] = Spc_Data_BarCpk.ValueMin + Spc_Data_BarCpk.GroupWidth;
            for (int i = 1; i < Spc_Data_BarCpk.GroupCount; i++)
            {
                Spc_Data_BarCpk.Xk[i] = Spc_Data_BarCpk.Xk[i - 1] + Spc_Data_BarCpk.GroupWidth;
                Spc_Data_BarCpk.XkDown[i] = Spc_Data_BarCpk.Xk[i] - Spc_Data_BarCpk.GroupWidth / 2;
                Spc_Data_BarCpk.XkUp[i] = Spc_Data_BarCpk.Xk[i] + Spc_Data_BarCpk.GroupWidth / 2;
            }

            SumRang(X, Spc_Data_BarCpk.GroupCount, Spc_Data_BarCpk.XkDown, Spc_Data_BarCpk.XkUp, out Spc_Data_BarCpk.Yk, out Spc_Data_BarCpk.YkCount);

            return Spc_Data_BarCpk;
        }
    }

    /// <summary>
    /// X-R均值极差图
    /// </summary>
    public class CSpc_Data_XR
    {
        /// <summary>
        /// 子组大小。单个子组观测值的个数
        /// </summary>
        public int n;

        /// <summary>
        /// 子组个数
        /// </summary>
        public int k;

        /// <summary>
        /// 均值中心线
        /// </summary>
        public double CL_X;

        /// <summary>
        /// 均值上控制线
        /// </summary>
        public double UCL_X;

        /// <summary>
        /// 均值下控制线
        /// </summary>
        public double LCL_X;

        /// <summary>
        /// 极差中心线
        /// </summary>
        public double CL_R;

        /// <summary>
        /// 极差上控制线
        /// </summary>
        public double UCL_R;

        /// <summary>
        /// 极差下控制线
        /// </summary>
        public double LCL_R;

        /// <summary>
        /// 均值(子组个数k)
        /// </summary>
        public double[] CL_Xk;

        /// <summary>
        /// 极差(子组个数k)
        /// </summary>
        public double[] CL_Rk;
    }

    /// <summary>
    /// X-S 均值标准差图
    /// </summary>
    public class CSpc_Data_XS
    {
        /// <summary>
        /// 子组大小。单个子组观测值的个数
        /// </summary>
        public int n;

        /// <summary>
        /// 子组个数
        /// </summary>
        public int k;

        /// <summary>
        /// 均值中心线
        /// </summary>
        public double CL_X;

        /// <summary>
        /// 均值上控制线
        /// </summary>
        public double UCL_X;

        /// <summary>
        /// 均值下控制线
        /// </summary>
        public double LCL_X;

        /// <summary>
        /// 标准差中心线
        /// </summary>
        public double CL_S;

        /// <summary>
        /// 标准差上控制线
        /// </summary>
        public double UCL_S;

        /// <summary>
        /// 标准差下控制线
        /// </summary>
        public double LCL_S;

        /// <summary>
        /// 均值(子组个数k)
        /// </summary>
        public double[] CL_Xk;

        /// <summary>
        /// 标准差(子组个数k)
        /// </summary>
        public double[] CL_Sk;
    }

    /// <summary>
    /// p不合格品率控制图
    /// </summary>
    public class CSpc_Data_p
    {
        /// <summary>
        /// 子组大小。单个子组观测值的个数
        /// </summary>
        public int n;

        /// <summary>
        /// 子组个数
        /// </summary>
        public int k;

        /// <summary>
        /// p不合格率控制图图中心线。所有子组不合格品率的平均值
        /// </summary>
        public double p;

        /// <summary>
        /// 不合格率上控制线
        /// </summary>
        public double UCL_p;

        /// <summary>
        /// 不合格率下控制线
        /// </summary>
        public double LCL_p;

        /// <summary>
        /// np不合格数控制图中心线。 =所有子组不合格品数/子组个数
        /// </summary>
        public double np;

        /// <summary>
        /// np不合格数上控制线
        /// </summary>
        public double UCL_np;

        /// <summary>
        /// np不合格数下控制线
        /// </summary>
        public double LCL_np;

        /// <summary>
        /// 子组不合格品率(子组个数k)
        /// </summary>
        public double[] CL_pk;

        /// <summary>
        /// 子组不合格品数(子组个数k)
        /// </summary>
        public int[] CL_npk;
    }

    /// <summary>
    /// 能力指数计算
    /// 直方图。将收集的测定值或数据之全距分为几个相等区间作为横轴，
    /// 并将各区间内之测定值
    /// 所出现次数累积而成的面积以条状方式排列起来所产生的图形，
    /// 称之为直方图。
    /// </summary>
    public class CSpc_Data_Cpk
    {
        /// <summary>
        /// 子组大小。单个子组观测值的个数
        /// </summary>
        public int n;

        /// <summary>
        /// 子组个数
        /// </summary>
        public int k;

        /// <summary>
        /// 特征值中值
        /// </summary>
        public double SL;

        /// <summary>
        /// 特征值上限
        /// </summary>
        public double USL;

        /// <summary>
        /// 特征值下限
        /// </summary>
        public double LSL;

        /// <summary>
        /// 组内过程标准差的估计值
        /// </summary>
        public double Singma;

        /// <summary>
        /// 子组标准差的平均值
        /// </summary>
        public double SingmaS;

        /// <summary>
        /// 过程准确度，表示过程特性中心位置的偏移度
        /// </summary>
        public double Ca;

        /// <summary>
        /// 过程精密度，表示过程特性的一致性程度
        /// </summary>
        public double Cp;

        /// <summary>
        /// 能力指数上限
        /// </summary>
        public double Cpu;

        /// <summary>
        /// 能力指数下限
        /// </summary>
        public double Cpl;

        /// <summary>
        /// 稳定过程的能力比值
        /// </summary>
        public double Cr;

        /// <summary>
        /// 过程能力指数。过程准确度和过程精密度综合考虑， 越大越好
        /// </summary>
        public double Cpk;

        /// <summary>
        /// 性能指数
        /// </summary>
        public double Pp;

        /// <summary>
        /// 能力指数上限
        /// </summary>
        public double Ppu;

        /// <summary>
        /// 能力指数下限
        /// </summary>
        public double Ppl;

        /// <summary>
        /// 性能比率
        /// </summary>
        public double Pr;

        /// <summary>
        /// 性能指数
        /// </summary>
        public double Ppk;

        /// <summary>
        /// 全距。由全体数据中找出最大值与最小值之差。
        /// 过程分布宽度。一个过程特性单值的分布变化程度。
        /// 通常用过程平均值加减几倍的标准差来表示（例如：X±3σ）。
        /// </summary>
        public double ProcessSpread;

        /// <summary>
        /// 组距
        /// </summary>
        public double GroupWidth;

        /// <summary>
        /// 分组数
        /// </summary>
        public int GroupCount;

        /// <summary>
        /// 全部观测值最大值
        /// </summary>
        public double ValueMax;

        /// <summary>
        /// 全部观测值最小值
        /// </summary>
        public double ValueMin;

        /// <summary>
        /// 组中点
        /// </summary>
        public double[] Xk;

        /// <summary>
        /// 组观测值上界
        /// </summary>
        public double[] XkUp;

        /// <summary>
        /// 组观测值下界
        /// </summary>
        public double[] XkDown;

        /// <summary>
        /// 组中测定次数与总的测定次数的比值*100
        /// </summary>
        public double[] Yk;

        /// <summary>
        /// 组中测定次数
        /// </summary>
        public double[] YkCount;

        /// <summary>
        /// 正态分布图X坐标
        /// </summary>
        public double[] NormalDistributionX;

        /// <summary>
        /// 正态分布图Y坐标
        /// </summary>
        public double[] NormalDistributionY;

        /// <summary>
        /// Xbarbar
        /// </summary>
        public double X2;
    }

    /// <summary>
    /// 利用能力指数计算方法，绘制直方图。
    /// 将收集的测定值或数据之全距分为几个相等区间作为横轴，
    /// 并将各区间内之测定值
    /// 所出现次数累积而成的面积以条状方式排列起来所产生的图形，
    /// 称之为直方图。
    /// 不计算上下限等其他参数。
    /// </summary>
    public class CSpc_Data_BarCpk
    {
        /// <summary>
        /// 全距。由全体数据中找出最大值与最小值之差。
        /// 过程分布宽度。一个过程特性单值的分布变化程度。
        /// 通常用过程平均值加减几倍的标准差来表示（例如：X±3σ）。
        /// </summary>
        public double ProcessSpread;

        /// <summary>
        /// 组距
        /// </summary>
        public double GroupWidth;

        /// <summary>
        /// 分组数
        /// </summary>
        public int GroupCount;

        /// <summary>
        /// 全部观测值最大值
        /// </summary>
        public double ValueMax;

        /// <summary>
        /// 全部观测值最小值
        /// </summary>
        public double ValueMin;

        /// <summary>
        /// 组中点
        /// </summary>
        public double[] Xk;

        /// <summary>
        /// 组观测值上界
        /// </summary>
        public double[] XkUp;

        /// <summary>
        /// 组观测值下界
        /// </summary>
        public double[] XkDown;

        /// <summary>
        /// 组中测定次数与总的测定次数的比值*100
        /// </summary>
        public double[] Yk;

        /// <summary>
        /// 组中测定次数
        /// </summary>
        public double[] YkCount;

        /// <summary>
        /// 正态分布图X坐标
        /// </summary>
        public double[] NormalDistributionX;

        /// <summary>
        /// 正态分布图Y坐标
        /// </summary>
        public double[] NormalDistributionY;
    }

    /// <summary>
    /// 测试数据
    /// p不合格率控制图
    /// 不合格数控制图
    /// </summary>
    public class Spc_Data_TestData_P
    {
        /// <summary>
        /// 子组大小。单个子组观测值的个数
        /// </summary>
        static public int n = 4000;

        /// <summary>
        /// 子组个数
        /// </summary>
        static public int k = 25;

        static public double[] X =
            {
                8,14,10,4,13,//1
				9,7,11,15,13,//2
				5,14,12,8,15,//3
				11,9,18,6,12,//4
				6,12,8,15,14 //5
			};
    }

    /// <summary>
    /// 排列图测试数据
    /// </summary>
    public class Spc_Data_TestData_PL
    {
        static public double[] Y =
            {
                100,50,400,200,250
            };

        static public string[] legend =
            {
                "项目1","项目2","项目3","项目4","项目5"
            };
    }

    /// <summary>
    /// 基本直方图
    /// </summary>
    public class Spc_Data_TestData_BarBase
    {
        static public double[] Y =
            {
                100,50,400,200,250,100,50,400,200,250
            };

        static public string[] legend =
            {
                "项目1","项目2","项目3","项目4","项目5","项目5","项目6","项目7","项目8","项目9","项目10"
            };
    }

    public class Spc_Data_TestData_Trend
    {
        public static DataTable[] Get_Trend_Data()
        {
            //趋势图应该对应着多个数据数据表（不能要求这些数据表行数是相同的，但列数应该是相同的 ）
            DataTable newTable = new DataTable
            {
                TableName = "ly"
            };
            DataColumn myDataColumn1 = new DataColumn("X", typeof(int));
            DataColumn myDataColumn2 = new DataColumn("Y", typeof(string));
            DataColumn myDataColumn3 = new DataColumn("legend", typeof(double));

            newTable.Columns.Add(myDataColumn1);
            newTable.Columns.Add(myDataColumn2);
            newTable.Columns.Add(myDataColumn3);
            DataRow myDataRow;

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 1;
            myDataRow["legend"] = "一月";
            myDataRow["Y"] = 5.36;
            newTable.Rows.Add(myDataRow);

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 2;
            myDataRow["legend"] = "二月";
            myDataRow["Y"] = 15.23;
            newTable.Rows.Add(myDataRow);

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 3;
            myDataRow["legend"] = "三月";
            myDataRow["Y"] = 120;
            newTable.Rows.Add(myDataRow);

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 4;
            myDataRow["legend"] = "四月";
            myDataRow["Y"] = 22.05;
            newTable.Rows.Add(myDataRow);

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 5;
            myDataRow["legend"] = "五月";
            myDataRow["Y"] = 4;
            newTable.Rows.Add(myDataRow);

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 6;
            myDataRow["legend"] = "六月";
            myDataRow["Y"] = 22.05;
            newTable.Rows.Add(myDataRow);

            myDataRow = newTable.NewRow();
            myDataRow["X"] = 7;
            myDataRow["legend"] = "七月";
            myDataRow["Y"] = 4;
            newTable.Rows.Add(myDataRow);

            newTable.AcceptChanges();
            DataTable newTable1 = new DataTable
            {
                TableName = "ly1"
            };
            myDataColumn1 = new DataColumn("X", typeof(int));
            myDataColumn2 = new DataColumn("legend", typeof(string));
            myDataColumn3 = new DataColumn("Y", typeof(double));

            newTable1.Columns.Add(myDataColumn1);
            newTable1.Columns.Add(myDataColumn2);
            newTable1.Columns.Add(myDataColumn3);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 1;
            myDataRow["legend"] = "一月";
            myDataRow["Y"] = 11.2;
            newTable1.Rows.Add(myDataRow);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 2;
            myDataRow["legend"] = "二月";
            myDataRow["Y"] = 26.3;
            newTable1.Rows.Add(myDataRow);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 3;
            myDataRow["legend"] = "三月";
            myDataRow["Y"] = 24;
            newTable1.Rows.Add(myDataRow);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 4;
            myDataRow["legend"] = "四月";
            myDataRow["Y"] = 25;
            newTable1.Rows.Add(myDataRow);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 5;
            myDataRow["legend"] = "五月";
            myDataRow["Y"] = 3.6;
            newTable1.Rows.Add(myDataRow);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 6;
            myDataRow["legend"] = "六月";
            myDataRow["Y"] = 25;
            newTable1.Rows.Add(myDataRow);

            myDataRow = newTable1.NewRow();
            myDataRow["X"] = 7;
            myDataRow["legend"] = "七月";
            myDataRow["Y"] = 3.6;
            newTable1.Rows.Add(myDataRow);

            newTable1.AcceptChanges();

            DataTable newTable2 = new DataTable
            {
                TableName = "ly2"
            };
            myDataColumn1 = new DataColumn("X", typeof(int));
            myDataColumn2 = new DataColumn("legend", typeof(string));
            myDataColumn3 = new DataColumn("Y", typeof(double));

            newTable2.Columns.Add(myDataColumn1);
            newTable2.Columns.Add(myDataColumn2);
            newTable2.Columns.Add(myDataColumn3);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 1;
            myDataRow["legend"] = "一月";
            myDataRow["Y"] = 15;
            newTable2.Rows.Add(myDataRow);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 2;
            myDataRow["legend"] = "二月";
            myDataRow["Y"] = 25;
            newTable2.Rows.Add(myDataRow);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 3;
            myDataRow["legend"] = "三月";
            myDataRow["Y"] = 35;
            newTable2.Rows.Add(myDataRow);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 4;
            myDataRow["legend"] = "四月";
            myDataRow["Y"] = 20;
            newTable2.Rows.Add(myDataRow);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 5;
            myDataRow["legend"] = "五月";
            myDataRow["Y"] = 24;
            newTable2.Rows.Add(myDataRow);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 6;
            myDataRow["legend"] = "六月";
            myDataRow["Y"] = 56;
            newTable2.Rows.Add(myDataRow);

            myDataRow = newTable2.NewRow();
            myDataRow["X"] = 7;
            myDataRow["legend"] = "七月";
            myDataRow["Y"] = 17.5;
            newTable2.Rows.Add(myDataRow);

            newTable2.AcceptChanges();
            DataTable[] mytable = new DataTable[3];

            mytable[0] = newTable;
            mytable[1] = newTable1;
            mytable[2] = newTable2;
            return mytable;
        }
    }
}