using NLog;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MF.MTTS.TTSDotNetLib
{
    #region winapi

    public class TTSDll
    {
        #region TTS dll import

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int MSPLogin(string user, string password, string configs);

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int MSPLogout();

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr QTTSSessionBegin(string _params, ref int errorCode);

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int QTTSTextPut(string sessionID, string textString, uint textLen, string _params);

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr QTTSAudioGet(string sessionID, ref uint audioLen, ref SynthStatus synthStatus, ref int errorCode);

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr QTTSAudioInfo(string sessionID);

        [DllImport("msc_x64.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int QTTSSessionEnd(string sessionID, string hints);

        #endregion TTS dll import
    }

    #endregion winapi
}