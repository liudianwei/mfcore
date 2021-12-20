using System;

namespace MF.Utils.SPC
{
    public class SPC_Table_Data
    {
        private static readonly Xml_Spc_Param Spc_Param = new Xml_Spc_Param();

        /// <summary>
        /// 计算各组数的和、均值和极差
        /// </summary>
        /// <param name="X">存所有数据的数据</param>
        /// <param name="n">每组样本个数</param>
        /// <param name="R">极差</param>
        /// <param name="Xbar">各组样本平均值</param>
        /// <param name="sum">各组样本和</param>
        static public void CalcSumXbarR(double[] X, int n, out double[] R, out double[] Xbar, out double[] sum)
        {
            R = new double[X.Length / n];
            Xbar = new double[X.Length / n];
            sum = new double[X.Length / n];
            float a = 0;
            float b = 0;
            for (int j = 0; j < X.Length / n; j++)
            {
                sum[j] = 0.0;
                Xbar[j] = 0.0;
                for (int i = 0; i < n; i++)
                {
                    if (i == 0)
                    {
                        a = (float)X[j * n + i];
                        b = (float)X[j * n + i];
                    }
                    if (i != 0)
                    {
                        if (a <= (float)X[j * n + i])
                        {
                            a = (float)X[j * n + i];
                        }

                        //if (a > (float)X[j * n + i])
                        //{
                        //    a = a;
                        //}
                        //else
                        //{
                        //    a = (float)X[j * n + i];
                        //}
                        if (b >= (float)X[j * n + i])
                        {
                            b = (float)X[j * n + i];
                        }
                        //if (b < (float)X[j * n + i])
                        //{
                        //    b = b;
                        //}
                        //else
                        //{
                        //    b = (float)X[j * n + i];
                        //}
                    }
                    sum[j] = X[j * n + i] + sum[j];
                    Xbar[j] = sum[j] / n;
                }
                R[j] = (double)(a - b);
            }
        }

        /// <summary>
        /// 计算各组平均值的均值
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public double CalcXbarbar(double[] X, int n)
        {
            double Xbarbar;
            double[] sum = new double[X.Length / n];
            double[] R = new double[X.Length / n];
            double[] Xbar = new double[X.Length / n];
            double Allsum = 0;

            CalcSumXbarR(X, n, out R, out Xbar, out sum);
            for (int i = 0; i < X.Length / n; i++)
            {
                if (i == 0)
                {
                    Allsum = Xbar[i];
                }
                if (i != 0)
                {
                    Allsum += Xbar[i];
                }
            }
            Xbarbar = Allsum / (X.Length / n);
            return Xbarbar;
        }

        /// <summary>
        /// 计算各组方差均值
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public double CalcRbar(double[] X, int n)
        {
            double Rbar;
            double[] sum = new double[X.Length / n];
            double[] R = new double[X.Length / n];
            double[] Xbar = new double[X.Length / n];
            double Rsum = 0;

            CalcSumXbarR(X, n, out R, out Xbar, out sum);
            for (int i = 0; i < X.Length / n; i++)
            {
                if (i == 0)
                {
                    Rsum = R[i];
                }
                if (i != 0)
                {
                    Rsum += R[i];
                }
            }
            Rbar = Rsum / (X.Length / n);
            return Rbar;
        }

        /// <summary>
        /// 计算无偏稳态过程能力指数Cp
        /// </summary>
        /// <param name="n"></param>
        /// <param name="X"></param>
        /// <param name="LSL"></param>
        /// <param name="USL"></param>
        /// <returns></returns>
        static public double CalcCp(int n, double[] X, double LSL, double USL)
        {
            double Sigma;
            double Rbar = CalcRbar(X, n);
            double d2 = (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_L_D1];
            Sigma = Rbar / d2;
            double Cp = (USL - LSL) / (6 * Sigma);
            return Cp;
        }

        /// <summary>
        /// 计算有偏稳态过程能力指数Cpk
        /// </summary>
        /// <param name="Cpu"></param>
        /// <param name="Cpl"></param>
        /// <returns></returns>
        static public double CalcCpk(double Cpu, double Cpl)
        {
            double Cpk = Math.Min(Cpu, Cpl);
            return Cpk;
        }

        /// <summary>
        /// 计算Cpu
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <param name="USL"></param>
        /// <returns></returns>
        static public double CalcCpu(double[] X, int n, double USL)
        {
            double Cpu;
            double Xbarbar = CalcXbarbar(X, n);
            double Rbar = CalcRbar(X, n);
            double d2 = (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_L_D1];
            double Sigma = Rbar / d2;
            Cpu = (USL - Xbarbar) / (3 * Sigma);
            return Cpu;
        }

        /// <summary>
        /// 计算Cpl
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <param name="LSL"></param>
        /// <returns></returns>
        static public double CalcCpl(double[] X, int n, double LSL)
        {
            double Cpl;
            double Xbarbar = CalcXbarbar(X, n);
            double Rbar = CalcRbar(X, n);
            double d2 = (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_L_D1];
            double Sigma = Rbar / d2;
            Cpl = (Xbarbar - LSL) / (3 * Sigma);
            return Cpl;
        }

        /// <summary>
        /// 计算无偏过程性能指数Pp
        /// </summary>
        /// <param name="n"></param>
        /// <param name="X"></param>
        /// <param name="LSL"></param>
        /// <param name="USL"></param>
        /// <returns></returns>
        static public double CalcPp(int n, double[] X, double LSL, double USL)
        {
            double Pp;
            double Sigma;
            double Sbar = CalcSbar(X, n);
            double c4 = (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_C4];
            Sigma = Sbar / c4;
            Pp = (USL - LSL) / (6 * Sigma);
            return Pp;
        }

        /// <summary>
        /// 计算有偏过程性能指数Ppk
        /// </summary>
        /// <param name="Ppu"></param>
        /// <param name="Ppl"></param>
        /// <returns></returns>
        static public double CalcPpk(double Ppu, double Ppl)
        {
            double Ppk = Math.Min(Ppl, Ppu);
            return Ppk;
        }

        /// <summary>
        /// 计算Ppu
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <param name="USL"></param>
        /// <returns></returns>
        static public double CalcPpu(double[] X, int n, double USL)
        {
            double Ppu;
            double Sigma;
            double Sbar = CalcSbar(X, n);
            double c4 = (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_C4];
            double Xbarbar = CalcXbarbar(X, n);
            Sigma = Sbar / c4;
            Ppu = (USL - Xbarbar) / (3 * Sigma);
            return Ppu;
        }

        /// <summary>
        /// 计算Ppl
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <param name="LSL"></param>
        /// <returns></returns>
        static public double CalcPpl(double[] X, int n, double LSL)
        {
            double Ppl;
            double Sigma;
            double Sbar = CalcSbar(X, n);
            double c4 = (double)Spc_Param.Tables[Xml_Spc_Param.Table_Name].Rows[n - 2][Xml_Spc_Param.Param_C4];
            double Xbarbar = CalcXbarbar(X, n);
            Sigma = Sbar / c4;
            Ppl = (Xbarbar - LSL) / (3 * Sigma);
            return Ppl;
        }

        /// <summary>
        /// 计算每个子组的标准差
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public void CaluS(double[] X, int n, out double[] S)
        {
            double[] R;
            double[] Xbar;
            double[] sum;
            double[] Y = new double[X.Length / n];
            double[] sum_Y = new double[X.Length / n];
            S = new double[X.Length / n];
            CalcSumXbarR(X, n, out R, out Xbar, out sum);
            for (int i = 0; i < X.Length / n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Y[j] = (X[n * i + j] - Xbar[i]) * (X[n * i + j] - Xbar[i]);

                    if (j == 0)
                    {
                        sum_Y[i] = Y[j];
                    }
                    if (j != 0)
                    {
                        sum_Y[i] = sum_Y[i] + Y[j];
                    }
                }
                S[i] = Math.Sqrt(sum_Y[i] / (n - 1));
            }
        }

        /// <summary>
        /// 计算各组标准差均值
        /// </summary>
        /// <param name="X"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public double CalcSbar(double[] X, int n)
        {
            double Sbar;
            double[] S = new double[X.Length / n];
            double Ssum = 0;

            CaluS(X, n, out S);
            for (int i = 0; i < X.Length / n; i++)
            {
                if (i == 0)
                {
                    Ssum = S[i];
                }
                if (i != 0)
                {
                    Ssum += S[i];
                }
            }
            Sbar = Ssum / (X.Length / n);
            return Sbar;
        }
    }
}