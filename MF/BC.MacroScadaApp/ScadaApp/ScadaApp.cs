using System;
using SystemFramework;
using Mes.Exe.Driver.Redis;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using BasicData;
using EvetnArgData;
using System.Collections.Concurrent;
using RestSharp;

namespace ScadaAppCore
{
    /// <summary>
    /// 核心类库，注意保密
    /// </summary>
    public partial class ScadaApp
    {
        #region Fields

        /// <summary>
        /// 连接主服务器客户端
        /// </summary>
        public static MesRedisClient mesRedisClient { set; get; } = null;

        /// <summary>
        /// 是否允许工作
        /// </summary>
        public static bool IsCanWork { set; get; } = false;

        #endregion Fields

        /// <summary>
        /// 对象
        /// </summary>
        public ScadaApp()
        {
        }

        /// <summary>
        /// 初始化Redis客户端
        /// </summary>
        /// <returns></returns>
        public bool InitRedisClient(string Product)
        {
            bool flag = false;
            try
            {
                #region 初始化Redis客户端

                string errorMsg = "";
                mesRedisClient = new MesRedisClient(RedisIp, RedisPort);
                if (!mesRedisClient.Open(out errorMsg))
                {
                    ApplicationLog.WriteLog("打开Redis失败:" + errorMsg);
                    return flag;
                }
                mesRedisClient.RedisSubMessage += MesRedisClient_RedisSubMessage;

                #endregion 初始化Redis客户端

                #region 注册监控事件，监控TAG写入与变化

                if (!mesRedisClient.Sub(ChangeChannelName, out errorMsg))
                {
                    errorMsg = "注册" + ChangeChannelName + "主题失败|" + errorMsg;
                    return flag;
                }

                #endregion 注册监控事件，监控TAG写入与变化

                #region 与配置信号程序之间的心跳

                Thread hb = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            mesRedisClient.Pub($"{Product}Heartbeat", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out errorMsg);
                            Thread.Sleep(3000);
                        }
                        catch (Exception ex)
                        {
                            ApplicationLog.WriteLog($"{Product}Heartbeat:{ex.ToString()}");
                        }
                    }
                });
                hb.IsBackground = true;
                hb.Start();

                #endregion 与配置信号程序之间的心跳

                CreateQueue();

                flag = true;
            }
            catch (Exception ex)
            {
                CloseRedisClient();
                ApplicationLog.WriteLog(ex.ToString());
            }
            return flag;
        }

        /// <summary>
        /// 关闭Redis连接
        /// </summary>
        public void CloseRedisClient()
        {
            try
            {
                #region 关闭redis服务

                if (mesRedisClient != null)
                {
                    try
                    {
                        mesRedisClient.RedisSubMessage -= MesRedisClient_RedisSubMessage;
                    }
                    catch { }
                    mesRedisClient.Close();
                    mesRedisClient = null;
                }

                #endregion 关闭redis服务
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err.ToString());
            }
        }

        /// <summary>
        /// 记录跳变的Tag名称
        /// </summary>
        public List<string> ChangeTagNames { get; set; } = new List<string>();

        /// <summary>
        /// 接受到Redis请求
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="Message"></param>
        private void MesRedisClient_RedisSubMessage(string Topic, string Message)
        {
            List<TagMsg> tagMsgs = new List<TagMsg>();

            #region Json字符串转实体

            try
            {
                tagMsgs = SimpleJson.DeserializeObject<List<TagMsg>>(Message);
            }
            catch (Exception e)
            {
                ApplicationLog.WriteLog($"字符串{Message}转换实体失败:" + e.Message);
                return;
            }

            #endregion Json字符串转实体

            try
            {
                //开始工作时
                if (IsCanWork)
                {
                    //变量回调
                    if (Topic.Equals(ChangeChannelName))
                    {
                        foreach (var tagMsg in tagMsgs)
                        {
                            var tag = TagList.Find(it => it.TagID.ToString().Equals(tagMsg.TagID));
                            if (tag == null)
                            {
                                continue;
                            }

                            tag.TagValue = tagMsg.TagValue;
                            tag.TagValue = ConvertStringToTagType(tag);

                            CustomeEvetnArgs e = new CustomeEvetnArgs()
                            {
                                TagID = tag.TagID,
                                OpName = tag.OpName,
                                TagClassType = tag.TagClassType,
                                TagTypeID = tag.TagTypeID,
                                TagType = tag.TagType,
                                TagName = tag.TagName,
                                TagValue = tag.TagValue,
                                TagQuality = tagMsg.TagQuality == "good" ? 192 : 0,
                                TimeStamp = DateTime.Now
                            };
                            OPCTagData_TagDataOnChange(this, e);
                            OPCTagData.InvokeTagData(this, e);

                            #region 逻辑日志

                            if (tag.TagName != "HeartBeatPLC" && tag.TagName != "HeartBeatMIS")
                            {
                                if (ChangeTagNames != null)
                                {
                                    if (ChangeTagNames.Contains(tag.TagName))
                                    {
                                        ApplicationLog.BusinessLog(tag.OpName, "ValueChange:" + tag.TagDescription + "|" + tag.TagValue.ToString());
                                    }
                                }
                                else
                                {
                                    ApplicationLog.BusinessLog(tag.OpName, "ValueChange:" + tag.TagDescription + "|" + tag.TagValue.ToString());
                                }
                            }

                            #endregion 逻辑日志
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// 通知队列
        /// </summary>
        protected static ConcurrentQueue<TagMsg> concurrentQueue = new ConcurrentQueue<TagMsg>();

        /// <summary>
        /// 通知队列 信号量
        /// </summary>
        protected static AutoResetEvent autoResetQueue = new AutoResetEvent(false);

        /// <summary>
        ///写入推送队列
        /// </summary>
        /// <param name="tagMsg"></param>
        public static void Enqueue(TagMsg tagMsg)
        {
            if (IsCanWork)
            {
                concurrentQueue.Enqueue(tagMsg);
                autoResetQueue.Set(); //通知task队列里有内容了  可以开始循环读取
            }
        }

        /// <summary>
        /// 创建接收推送消息队列
        /// </summary>
        private void CreateQueue(int rowCount = 30)
        {
            new Thread(() =>
            {
                while (true)
                {
                    autoResetQueue.WaitOne();
                    while (concurrentQueue.Any())
                    {
                        List<TagMsg> tagMsgs = new List<TagMsg>();
                        if (concurrentQueue.Count >= rowCount)
                        {
                            for (int i = 0; i < rowCount; i++)
                            {
                                if (concurrentQueue.TryDequeue(out TagMsg tagMsg))
                                {
                                    tagMsgs.Add(tagMsg);
                                }
                            }
                        }
                        else
                        {
                            int crowCount = concurrentQueue.Count;
                            for (int i = 0; i < crowCount; i++)
                            {
                                if (concurrentQueue.TryDequeue(out TagMsg tagMsg))
                                {
                                    tagMsgs.Add(tagMsg);
                                }
                            }
                        }
                        if (tagMsgs.Count > 0)
                        {
                            mesRedisClient.Pub(WriteTagNodeValue, SimpleJson.SerializeObject(tagMsgs), out string errorMsg);
                            //if (tagMsgs.Count > 1)
                            //{
                            //    ApplicationLog.WriteLog($"发送条数{tagMsgs.Count} 明细:" + JsonConvert.SerializeObject(tagMsgs));
                            //}
                        }
                    }
                    autoResetQueue.Reset();
                }
            }).Start();
        }

        /// <summary>
        /// 数据类型转换字符串类型
        /// </summary>
        /// <param name="e"></param>
        public static QualityDataType ConvertTagTypeToString(QualityDataType e)
        {
            try
            {
                switch (e.TagType)
                {
                    case "shortArray":
                        if (e.TagValue is short[])
                        {
                            e.TagValue = string.Join(",", (short[])e.TagValue);
                        }
                        break;

                    case "intArray":
                        if (e.TagValue is int[])
                        {
                            e.TagValue = string.Join(",", (int[])e.TagValue);
                        }
                        break;

                    case "floatArray":
                        if (e.TagValue is float[])
                        {
                            e.TagValue = string.Join(",", (float[])e.TagValue);
                        }
                        break;

                    case "datetimeArray":
                        if (e.TagValue is DateTime[])
                        {
                            e.TagValue = string.Join(",", (DateTime[])e.TagValue);
                        }
                        break;

                    case "boolArray":
                        if (e.TagValue is bool[])
                        {
                            e.TagValue = string.Join(",", (bool[])e.TagValue);
                        }
                        break;

                    case "sbyteArray":
                        if (e.TagValue is sbyte[])
                        {
                            e.TagValue = string.Join(",", (sbyte[])e.TagValue);
                        }
                        break;

                    case "byteArray":
                        if (e.TagValue is byte[])
                        {
                            e.TagValue = string.Join(",", (byte[])e.TagValue);
                        }
                        break;

                    case "ushortArray":
                        if (e.TagValue is ushort[])
                        {
                            e.TagValue = string.Join(",", (ushort[])e.TagValue);
                        }
                        break;

                    case "uintArray":
                        if (e.TagValue is uint[])
                        {
                            e.TagValue = string.Join(",", (uint[])e.TagValue);
                        }
                        break;

                    default:
                        e.TagValue = Convert.ToString(e.TagValue ?? "");
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteLog(SimpleJson.SerializeObject(e) + " " + ex.ToString());
            }
            return e;
        }

        /// <summary>
        /// 字符串类型转换数据类型
        /// </summary>
        /// <param name="e"></param>
        public static object ConvertStringToTagType(QualityDataType e)
        {
            object TagValue = null;
            try
            {
                switch (e.TagType)
                {
                    case "short":
                        TagValue = Convert.ToInt16(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "shortArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                short[] vaule = Enumerable.Repeat(short.Parse("0"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = short.Parse(strvaule[i]);
                                }
                                TagValue = vaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "int":
                        TagValue = Convert.ToInt32(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "intArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                int[] vaule = Enumerable.Repeat(int.Parse("0"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = int.Parse(strvaule[i]);
                                }
                                TagValue = vaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "float":
                        TagValue = float.Parse(Convert.ToString(e.TagValue == null || e.TagValue.ToString() == "" ? "-1" : e.TagValue));
                        break;

                    case "floatArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                float[] vaule = Enumerable.Repeat(float.Parse("0"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = float.Parse(strvaule[i]);
                                }
                                TagValue = vaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "datetime":
                        TagValue = DateTime.Parse(Convert.ToString(e.TagValue ?? DateTime.Parse("0000/00/00 00:00:00")));
                        break;

                    case "datetimeArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                DateTime[] vaule = Enumerable.Repeat(DateTime.Parse("0000/00/00 00:00:00"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = DateTime.Parse(strvaule[i]);
                                }
                                TagValue = vaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "string":
                        TagValue = Convert.ToString(e.TagValue ?? "");
                        break;

                    case "bool":
                        TagValue = bool.Parse(e.TagValue == null || e.TagValue.ToString() == "" ? "False" : e.TagValue.ToString());
                        break;

                    case "boolArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                bool[] bytevaule = Enumerable.Repeat(bool.Parse("False"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = bool.Parse(strvaule[i]);
                                }
                                TagValue = bytevaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "sbyte":
                        TagValue = Convert.ToSByte(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "sbyteArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                sbyte[] bytevaule = Enumerable.Repeat((sbyte)0x00, e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = sbyte.Parse(strvaule[i]);
                                }
                                TagValue = bytevaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "byte":
                        TagValue = Convert.ToByte(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "byteArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                byte[] bytevaule = Enumerable.Repeat((byte)0x00, e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = byte.Parse(strvaule[i]);
                                }
                                TagValue = bytevaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "ushort":
                        TagValue = Convert.ToUInt16(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "ushortArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                ushort[] bytevaule = Enumerable.Repeat(ushort.Parse("0"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = ushort.Parse(strvaule[i]);
                                }
                                TagValue = bytevaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "uint":
                        TagValue = Convert.ToUInt32(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "uintArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                uint[] bytevaule = Enumerable.Repeat(uint.Parse("0"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = uint.Parse(strvaule[i]);
                                }
                                TagValue = bytevaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;

                    case "tod":
                        TagValue = Convert.ToInt32(e.TagValue == null || e.TagValue.ToString() == "" ? 0 : e.TagValue);
                        break;

                    case "todArray":
                        if (e.TagValue != null && e.TagValue.ToString() != "")
                        {
                            string[] strvaule = e.TagValue.ToString().Split(',');
                            if (strvaule.Length <= e.TagTypeLength)
                            {
                                int[] bytevaule = Enumerable.Repeat(int.Parse("0"), e.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = int.Parse(strvaule[i]);
                                }
                                TagValue = bytevaule;
                            }
                            else
                            {
                                TagValue = e.TagValue;
                            }
                        }
                        else
                        {
                            TagValue = e.TagValue;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteLog(SimpleJson.SerializeObject(e) + " " + ex.ToString());
            }
            return TagValue;
        }

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void GetDataHandler(object sender, CustomeEvetnArgs e);

        /// <summary>
        /// 委托（指针函数）
        /// </summary>
        public event GetDataHandler TagDataOnChange;

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OPCTagData_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            TagDataOnChange(sender, e);
        }

        /// <summary>
        /// 数据包
        /// </summary>
        public TagData OPCTagData = new TagData();

        /// <summary>
        /// 接收与发送消息
        /// </summary>
        public class TagMsg
        {
            /// <summary>
            /// 设备唯一标识
            /// </summary>
            public string UniqueId { get; set; }

            /// <summary>
            /// Tag唯一ID
            /// </summary>

            public string TagID { get; set; }

            /// <summary>
            /// Tag名称
            /// </summary>
            public string TagName { get; set; }

            /// <summary>
            /// Tag值
            /// </summary>
            public string TagValue { get; set; }

            /// <summary>
            /// Tag状态
            /// </summary>
            public string TagQuality { get; set; }
        }
    }
}