using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Common.Helper
{
    public class ImgHelper
    {
        /// <summary>
        /// 切边
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap Cutedge(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            return Cutedge(bmp);
        }

        /// <summary>
        /// 切边
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap Cutedge(Bitmap bmp)
        {
            int Width = bmp.Width;
            int height = bmp.Height;
            int left = 0;
            int right = 0;
            Color c;
            int y = height / 2;
            for (int i = 0; i < Width; i++)
            {
                c = bmp.GetPixel(i, 0);
                if (c.R == 0 && c.G == 0 && c.B == 0)
                {
                    left = i;
                    break;
                }
            }

            for (int i = 0; i < Width; i++)
            {
                c = bmp.GetPixel(Width - 1 - i, y);
                if (c.R == 0 && c.G == 0 && c.B == 0)
                {
                    right = i;
                    break;
                }
            }

            Bitmap imgCropped = new Bitmap((Width - left - right) * 10, height * 10);
            Graphics objGraphics = Graphics.FromImage(imgCropped);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;
            objGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            objGraphics.CompositingQuality = CompositingQuality.HighQuality;
            objGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            objGraphics.PageUnit = GraphicsUnit.Pixel;
            var bm = new Matrix();
            bm.Scale(10, 10);
            objGraphics.Transform = bm;
            objGraphics.Clear(Color.White);
            objGraphics.DrawImage(bmp, (0 - left) * 10, 0);
            bmp.Dispose();
            objGraphics.Dispose();
            return imgCropped;
        }

        /// <summary>
        /// 根据base64字符串返回一个封装好的GDI+位图。
        /// </summary>
        /// <param name="base64string">可转换成位图的base64字符串。</param>
        /// <returns>Bitmap对象。</returns>
        public static Bitmap FromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            var bp = new Bitmap(ms);
            return bp;
        }

        /// <summary>
        /// 将图片转换成base64字符串。
        /// </summary>
        /// <param name="img">需要转换的图片对象</param>
        /// <returns>base64字符串。</returns>
        public static string FromImage(Image img)
        {
            string strbaser64 = "";

            try
            {
                Bitmap bmp = new Bitmap(img);
                strbaser64 = FromImage(bmp);
            }
            catch (Exception e)
            {
                throw e;
            }

            return strbaser64;
        }

        /// <summary>
        /// 将图片转换成base64字符串。
        /// </summary>
        /// <param name="img">需要转换的图片对象</param>
        /// <returns>base64字符串。</returns>
        public static string FromImage(Bitmap bmp)
        {
            string strbaser64 = "";

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();

                    strbaser64 = Convert.ToBase64String(arr);
                }
            }
            catch (Exception)
            {
                throw new Exception("模板图片转换失败");
            }

            return strbaser64;
        }

        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="img1"></param>
        public static Bitmap Thresholding(Image img)
        {
            Bitmap img1 = new Bitmap(img);
            int[] histogram = new int[256];
            int minGrayValue = 255, maxGrayValue = 0;
            //求取直方图
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    histogram[pixelColor.R]++;
                    if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
                    if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
                }
            }
            //迭代计算阀值
            int threshold = -1;
            int newThreshold = (minGrayValue + maxGrayValue) / 2;
            for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
            {
                threshold = newThreshold;
                int lP1 = 0;
                int lP2 = 0;
                int lS1 = 0;
                int lS2 = 0;
                //求两个区域的灰度的平均值
                for (int i = minGrayValue; i < threshold; i++)
                {
                    lP1 += histogram[i] * i;
                    lS1 += histogram[i];
                }
                int mean1GrayValue = 0;
                if (lP1 != 0 && lS1 != 0)
                {
                    mean1GrayValue = (lP1 / lS1);
                }
                for (int i = threshold + 1; i < maxGrayValue; i++)
                {
                    lP2 += histogram[i] * i;
                    lS2 += histogram[i];
                }
                int mean2GrayValue = 0;
                if (lP2 != 0 && lS2 != 0)
                {
                    mean2GrayValue = (lP1 / lS1);
                }
                if (mean1GrayValue != 0 && mean2GrayValue != 0)
                {
                    newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
                }
            }
            //计算二值化
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    if (pixelColor.R > threshold) img1.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else img1.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }

            return img1;
        }

        /// <summary>
        /// 图像灰度化
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ToGray(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    //利用公式计算灰度值
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        /// <summary>
        /// 图像灰度反转
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap Reverse2(Bitmap b)
        {
            Bitmap bmp = new Bitmap(b);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    Color newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        public static Bitmap Reverse(Bitmap a)
        {
            int w = a.Width;
            int h = a.Height;
            Bitmap dstBitmap = new Bitmap(a.Width, a.Height, PixelFormat.Format24bppRgb);
            BitmapData srcData = a.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = dstBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* pIn = (byte*)srcData.Scan0.ToPointer();
                byte* pOut = (byte*)dstData.Scan0.ToPointer();
                byte* p;
                int stride = srcData.Stride;
                int r, g, b;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        p = pIn;
                        r = p[2];
                        g = p[1];
                        b = p[0];
                        pOut[2] = (byte)(255 - r);
                        pOut[1] = (byte)(255 - g);
                        pOut[0] = (byte)(255 - b);
                        pIn += 3;
                        pOut += 3;
                    }
                    pIn += srcData.Stride - w * 3;
                    pOut += srcData.Stride - w * 3;
                }
                a.UnlockBits(srcData);
                dstBitmap.UnlockBits(dstData);
                return dstBitmap;
            }
        }

        /// <summary>
        /// 图像二值化1：取图片的平均灰度作为阈值，低于该值的全都为0，高于该值的全都为255
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp(Bitmap bmp)
        {
            int average = 0;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color color = bmp.GetPixel(i, j);
                    average += color.B;
                }
            }
            average = (int)average / (bmp.Width * bmp.Height);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    int value = 255 - color.B;
                    Color newColor = value > average ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        /// <summary>
        /// 图像二值化2
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp2(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int y = 0; y < h; y++)
            {
                byte[] scan = new byte[(w + 7) / 8];
                for (int x = 0; x < w; x++)
                {
                    Color c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5) scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                Marshal.Copy(scan, 0, (IntPtr)((int)data.Scan0 + data.Stride * y), scan.Length);
            }
            return bmp;
        }
    }
}