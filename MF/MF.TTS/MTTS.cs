using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MF.MTTS.TTSDotNetLib
{
    /// <summary>
    /// MTTS语音合成库
    /// </summary>
    public class MTTS
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///
        /// </summary>
        private IntPtr session_ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        private SynthStatus synth_status;

        /// <summary>
        ///
        /// </summary>
        private string login_configs { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string _params { get; set; }

        /// <summary>
        ///
        /// </summary>
        private int ret;

        /// <summary>
        /// 编码格式
        /// </summary>
        public Encoding encoding { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appid">调用id</param>
        /// <param name="speed">语速</param>
        /// <param name="volume">音量</param>
        public MTTS(string appid = "macroinf", int speed = 30, int volume = 60, string encoding_name = "GB2312")
        {
            if (appid == "macroinf") appid = "581d2eaf";
            synth_status = SynthStatus.MSP_TTS_FLAG_STILL_HAVE_DATA;
            login_configs = $"appid = {appid}, work_dir = . ";//581d2eaf
            _params = $"engine_type = local," +
                $"voice_name = xiaoyan," +
                $"text_encoding = {encoding_name}, " +
                $"tts_res_path = fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet," +
                $"sample_rate = 16000, " +
                $"speed = {speed}, " +
                $"volume = {volume}, " +
                $"pitch = 50," +
                $"rdn = 2";
            ret = 0;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            encoding = Encoding.GetEncoding(encoding_name);
        }

        /// <summary>
        /// 文字转换语音
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">路径</param>
        /// <param name="name">文件名</param>
        /// <param name="filePath">返回文件全路径</param>
        /// <returns>0 成功 非0 失败</returns>
        public int Convert(string content, string path, string name, out string filePath)
        {
            filePath = "";
            try
            {
                //待合成的文本
                string text = content.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    text = "请输入合成语音的内容";
                }
                string filename = name + ".wav"; //合成的语音文件
                filePath = path + filename;
                uint audio_len = 0;

                //第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
                ret = TTSDll.MSPLogin(string.Empty, string.Empty, login_configs);
                //MSPLogin方法返回失败
                if (ret != (int)ErrorCode.MSP_SUCCESS)
                {
                    return ret;
                }

                session_ID = TTSDll.QTTSSessionBegin(_params, ref ret);
                //QTTSSessionBegin方法返回失败
                if (ret != (int)ErrorCode.MSP_SUCCESS)
                {
                    return ret;
                }
                ret = TTSDll.QTTSTextPut(Ptr2Str(session_ID), text, (uint)encoding.GetByteCount(text), string.Empty);
                //QTTSTextPut方法返回失败
                if (ret != (int)ErrorCode.MSP_SUCCESS)
                {
                    return ret;
                }

                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(new byte[44], 0, 44);
                while (true)
                {
                    IntPtr source = TTSDll.QTTSAudioGet(Ptr2Str(session_ID), ref audio_len, ref synth_status, ref ret);
                    byte[] array = new byte[(int)audio_len];
                    if (audio_len > 0)
                    {
                        Marshal.Copy(source, array, 0, (int)audio_len);
                    }
                    memoryStream.Write(array, 0, array.Length);
                    if (synth_status == SynthStatus.MSP_TTS_FLAG_DATA_END || ret != (int)ErrorCode.MSP_SUCCESS)
                        break;
                    Thread.Sleep(50);
                }

                WAVE_Header wave_Header = getWave_Header((int)memoryStream.Length - 44);
                byte[] array2 = this.StructToBytes(wave_Header);
                memoryStream.Position = 0L;
                memoryStream.Write(array2, 0, array2.Length);
                memoryStream.Position = 0L;
                if (filename != null)
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    memoryStream.WriteTo(fileStream);
                    memoryStream.Close();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                TTSDll.QTTSSessionEnd(Ptr2Str(session_ID), "");
                TTSDll.MSPLogout();//退出登录
            }
            return ret;
        }

        /// <summary>
        /// 结构体转字符串
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        private byte[] StructToBytes(object structure)
        {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] result;
            try
            {
                Marshal.StructureToPtr(structure, intPtr, false);
                byte[] array = new byte[num];
                Marshal.Copy(intPtr, array, 0, num);
                result = array;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return result;
        }

        /// <summary>
        /// 结构体初始化赋值
        /// </summary>
        /// <param name="data_len"></param>
        /// <returns></returns>
        private WAVE_Header getWave_Header(int data_len)
        {
            return new WAVE_Header
            {
                RIFF_ID = 1179011410,
                File_Size = data_len + 36,
                RIFF_Type = 1163280727,
                FMT_ID = 544501094,
                FMT_Size = 16,
                FMT_Tag = 1,
                FMT_Channel = 1,
                FMT_SamplesPerSec = 16000,
                AvgBytesPerSec = 32000,
                BlockAlign = 2,
                BitsPerSample = 16,
                DATA_ID = 1635017060,
                DATA_Size = data_len
            };
        }

        /// <summary>
        /// 语音音频头
        /// </summary>
        private struct WAVE_Header
        {
            public int RIFF_ID;
            public int File_Size;
            public int RIFF_Type;
            public int FMT_ID;
            public int FMT_Size;
            public short FMT_Tag;
            public ushort FMT_Channel;
            public int FMT_SamplesPerSec;
            public int AvgBytesPerSec;
            public ushort BlockAlign;
            public ushort BitsPerSample;
            public int DATA_ID;
            public int DATA_Size;
        }

        /// <summary>
        /// 指针转字符串
        /// </summary>
        /// <param name="p">指向非托管代码字符串的指针</param>
        /// <returns>返回指针指向的字符串</returns>
        private string Ptr2Str(IntPtr p)
        {
            List<byte> lb = new List<byte>();
            while (Marshal.ReadByte(p) != 0)
            {
                lb.Add(Marshal.ReadByte(p));
                p = p + 1;
            }
            byte[] bs = lb.ToArray();
            return encoding.GetString(lb.ToArray());
        }

        /// <summary>
        /// 非必需的，只是为了更符合其他语言的规范，如C++、java
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        ~MTTS()
        {
            //必须为false
            Dispose(false);
        }

        /// <summary>
        /// 释放标记
        /// </summary>
        private bool disposed;

        /// <summary>
        /// 是否资源
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
            }
            //清理非托管资源
            //告诉自己已经被释放
            disposed = true;
        }
    }
}