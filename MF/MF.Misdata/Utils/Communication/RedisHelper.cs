using Common.Model;
using Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common.Communication
{
    /// <summary>
    /// Redis客户端
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        /// Redis 客户端
        /// </summary>
        public static RedisClient redisClient { get; set; }

        /// <summary>
        /// Redis 地址
        /// </summary>
        public static string RedisAddress { get; set; } = "127.0.0.1:18222";

        /// <summary>
        /// Redis 访问密码
        /// </summary>
        public static string RedisPassword { get; set; } = "";

        /// <summary>
        /// Redis 写通道名称
        /// </summary>
        public static string WriteChannel { get; set; } = "WriteTagNodeValue";

        /// <summary>
        /// Redis 触发通道名称
        /// </summary>
        public static string OnChangeChannel { get; set; } = "ChangeTagNodeValue";

        /// <summary>
        /// scada请求Url
        /// </summary>
        public static string HttpServerUrl { get; set; } = "http://127.0.0.1:18555";

        /// <summary>
        /// 初始化 Redis 客户端
        /// </summary>
        public static bool InitRedisClient()
        {
            bool flag = false;
            string errorMsg = "";
            try
            {
                RedisAddress = ConfigHelper.GetAppseting("Redis:Address") ?? "127.0.0.1:18222";
                RedisPassword = ConfigHelper.GetAppseting("Redis:Password") ?? "";
                WriteChannel = ConfigHelper.GetAppseting("Redis:PlcWriteChannel") ?? "WriteTagNodeValue";
                OnChangeChannel = ConfigHelper.GetAppseting("Redis:PlcOnChangeChannel") ?? "ChangeTagNodeValue";
                HttpServerUrl = ConfigHelper.GetAppseting("HttpServer:Url") ?? "http://127.0.0.1:18555";
                //初始化redis
                redisClient = new RedisClient(RedisAddress.Split(':')[0], Convert.ToInt32(RedisAddress.Split(':')[1]), RedisPassword);
                if (!redisClient.Open(out errorMsg))
                {
                    SystemLog.Fatal($"打开Redis失败:{errorMsg}");
                    return flag;
                }
                //订阅消息
                if (!redisClient.Sub(OnChangeChannel, out errorMsg))
                {
                    SystemLog.Fatal($"注册{OnChangeChannel}主题失败|{errorMsg}");
                    return flag;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                CloseRedisClient();
                SystemLog.Fatal("InitRedisClient", ex);
            }
            return flag;
        }

        /// <summary>
        /// 开启心跳
        /// </summary>
        public static void StartHearbeat()
        {
            Thread hb = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        redisClient.Pub("MisdataHeartbeat", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out string errorMsg);
                        Thread.Sleep(3000);
                    }
                    catch (Exception ex)
                    {
                        SystemLog.Exception("StartHearbeat", ex);
                    }
                }
            })
            {
                IsBackground = true
            };
            hb.Start();
        }

        /// <summary>
        /// 关闭Redis连接
        /// </summary>
        public static void CloseRedisClient()
        {
            try
            {
                #region 关闭redis服务

                if (redisClient != null)
                {
                    redisClient.Close();
                    redisClient = null;
                }

                #endregion 关闭redis服务
            }
            catch (Exception ex)
            {
                SystemLog.Fatal("CloseRedisClient", ex);
            }
        }

        #region 读

        /// <summary>
        /// 按 TagID 读 PLC
        /// </summary>
        /// <param name="TagID">标签ID</param>
        /// <returns>标签值</returns>
        public static object Read(string TagID)
        {
            object TagValue;
            string errorMsg = string.Empty;
            string MesVal = string.Empty;
            if (string.IsNullOrEmpty(TagID.Trim())) throw new ArgumentNullException(nameof(TagID));
            try
            {
                if (BaseData.AllTagsMap.TryGetValue(TagID, out var tag))
                {
                    if (tag.IsEnable)
                    {
                        #region 监控数据

                        if (tag.IsMonitor)
                        {
                            if (redisClient.Read(tag.OpCode + ":" + tag.TagID, out MesVal, out errorMsg))
                            {
                                tag.TagValue = MesVal;
                                ConvertStringToTagType(ref tag);
                                TagValue = tag.TagValue;
                                SystemLog.Debug($"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{MesVal}", tag.OpName);
                            }
                            else
                            {
                                TagValue = null;
                                SystemLog.Debug($"ReadError:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|缓存读取失败|{errorMsg}", tag.OpName);
                            }
                        }

                        #endregion 监控数据

                        #region 非监控数据

                        else
                        {
                            var result = RestHelper.Get(HttpServerUrl, $"/{tag.OpCode}/DeviceRead?TagId={tag.TagID}");
                            if (result.IsSuccess)
                            {
                                tag.TagValue = result.Value;
                                ConvertStringToTagType(ref tag);
                                TagValue = tag.TagValue;
                                SystemLog.Debug($"ReadApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{result.Value}", tag.OpName);
                            }
                            else
                            {
                                TagValue = null;
                                SystemLog.Debug($"ReadApiError:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|Api读取失败|{result.Message}", tag.OpName);
                            }
                        }

                        #endregion 非监控数据
                    }
                    else
                    {
                        TagValue = null;
                        SystemLog.Debug($"读取操作:TagID未启用:{TagID}", tag.OpName);
                    }
                }
                else
                {
                    TagValue = null;
                    SystemLog.Error($"读取操作:未找到TagID:{TagID}");
                }
            }
            catch (Exception e)
            {
                TagValue = null;
                SystemLog.Exception($"Read(TagID)出错:{TagID}", e);
            }

            return TagValue;
        }

        /// <summary>
        /// 批量读取 PLC 指定数据
        /// </summary>
        /// <param name="tags"></param>
        public static void ReadPLC_Sync_DataList(ref List<QualityDataType> tags)
        {
            string errorMsg = string.Empty;
            try
            {
                var needReadFromRedis = tags.Where(i => i.IsEnable).ToList();

                #region 间接缓存读取

                var FromRedis = needReadFromRedis.Where(i => i.IsMonitor).ToList();
                string[] keys = FromRedis.Select(i => i.OpCode + ":" + i.TagID).ToArray();
                string[] vals = new string[keys.Length];
                if (redisClient.Read(keys, out vals, out errorMsg))
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        var tag = FromRedis.Where(x => (x.OpCode + ":" + x.TagID).Equals(keys[i])).FirstOrDefault();
                        tag.TagValue = vals[i];
                        ConvertStringToTagType(ref tag);
                    }
                    if (keys.Length > 0)
                    {
                        SystemLog.Debug($"Read:{string.Join("$", keys)}|{string.Join("$", vals)}", tags.FirstOrDefault().OpName);
                    }
                }
                else
                {
                    if (keys.Length > 0)
                    {
                        SystemLog.Debug($"ReadError:{string.Join(",", keys)}|缓存读取失败|{errorMsg}", tags.FirstOrDefault().OpName);
                    }
                }

                #endregion 间接缓存读取

                #region 直接PLC读取

                var FromPLC = needReadFromRedis.Where(i => !i.IsMonitor).ToList();
                FromPLC.ForEach(tag =>
                {
                    var result = RestHelper.Get(HttpServerUrl, $"/{tag.OpCode}/DeviceRead?TagId={tag.TagID}");
                    if (result.IsSuccess)
                    {
                        tag.TagValue = result.Value;
                        ConvertStringToTagType(ref tag);
                        SystemLog.Debug($"ReadApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{result.Value}", tag.OpName);
                    }
                    else
                    {
                        tag.TagQuality = "bad";
                        SystemLog.Debug($"ReadApiError:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|Api读取失败|{result.Message}", tag.OpName);
                    }
                });

                #endregion 直接PLC读取
            }
            catch (Exception e)
            {
                SystemLog.Exception($"ReadPLC_Sync_DataList(List<QualityDataType>)出错:{JsonConvert.SerializeObject(tags)}", e);
            }
        }

        /// <summary>
        /// 批量读取 PLC 质量数据
        /// </summary>
        /// <param name="OpName"></param>
        /// <param name="TagClassType"></param>
        /// <returns></returns>
        public static List<QualityDataType> ReadPLC_Sync_DataList_MesRead(string OpName, string TagClassType = "Quality")
        {
            List<QualityDataType> valueList = new List<QualityDataType>();
            string errorMsg = string.Empty;
            try
            {
                List<QualityDataType> tags = new List<QualityDataType>();
                tags = BaseData.OPName2TagsMap[OpName].Where(x => x.TagClassType.Contains(TagClassType)).ToList();

                var needReadFromRedis = tags.Where(i => i.IsEnable).ToList();

                #region 间接缓存读取

                var FromRedis = needReadFromRedis.Where(i => i.IsMonitor).ToList();
                string[] keys = FromRedis.Select(i => i.OpCode + ":" + i.TagID).ToArray();
                string[] vals = new string[keys.Length];
                if (redisClient.Read(keys, out vals, out errorMsg))
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        var tagv = FromRedis.Where(x => (x.OpCode + ":" + x.TagID).Equals(keys[i])).FirstOrDefault();
                        tagv.TagValue = vals[i];
                        ConvertStringToTagType(ref tagv);
                        valueList.Add(tagv);
                    }
                    if (keys.Length > 0)
                    {
                        SystemLog.Debug($"Read:{string.Join("$", keys)}|{string.Join("$", vals)}", tags.FirstOrDefault().OpName);
                    }
                }
                else
                {
                    if (keys.Length > 0)
                    {
                        SystemLog.Debug($"ReadError:{string.Join(",", keys)}|缓存读取失败|{errorMsg}", tags.FirstOrDefault().OpName);
                    }
                }

                #endregion 间接缓存读取

                #region 直接PLC读取

                var FromPLC = needReadFromRedis.Where(i => !i.IsMonitor).ToList();
                foreach (var tag in FromPLC)
                {
                    var tagv = tag;
                    var result = RestHelper.Get(HttpServerUrl, $"/{tag.OpCode}/DeviceRead?TagId={tag.TagID}");
                    if (result.IsSuccess)
                    {
                        tagv.TagValue = result.Value;
                        ConvertStringToTagType(ref tagv);
                        SystemLog.Debug($"ReadApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{result.Value}", tag.OpName);
                    }
                    else
                    {
                        tagv.TagQuality = "bad";
                        SystemLog.Debug($"ReadApiError:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|Api读取失败|{result.Message}", tag.OpName);
                    }
                    valueList.Add(tagv);
                }

                #endregion 直接PLC读取
            }
            catch (Exception e)
            {
                SystemLog.Exception($"ReadPLC_Sync_DataList_MesRead出错:{OpName} {TagClassType}", e);
            }
            return valueList;
        }

        #endregion 读

        #region 写

        /// <summary>
        /// 根据TagID写数值
        /// </summary>
        /// <param name="TagID">标签ID</param>
        /// <param name="TagValue">标签值</param>
        /// <returns></returns>
        public static bool Write(string TagID, object TagValue)
        {
            bool flag = false;
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(TagID.Trim())) throw new ArgumentNullException(nameof(TagID));
            try
            {
                if (BaseData.AllTagsMap.TryGetValue(TagID, out var tag))
                {
                    if (tag.IsEnable)
                    {
                        tag.TagValue = TagValue;
                        tag = ConvertTagTypeToString(tag);
                        List<TagMsg> list = new List<TagMsg>
                        {
                            new TagMsg
                            {
                                 UniqueId = tag.OpCode,
                                 TagID = tag.TagID,
                                 TagName = tag.TagName,
                                 TagQuality = "good",
                                 TagValue = tag.TagValue.ToString()
                            }
                        };

                        #region 监控数据

                        if (tag.IsMonitor)
                        {
                            if (redisClient.Pub(WriteChannel, JsonConvert.SerializeObject(list), out errorMsg))
                            {
                                SystemLog.Debug($"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue}", tag.OpName);
                            }
                            else
                            {
                                SystemLog.Debug($"WriteError:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue}|到服务端写入失败|{errorMsg}", tag.OpName);
                                return flag;
                            }
                        }

                        #endregion 监控数据

                        #region 非监控数据

                        else
                        {
                            var result = RestHelper.Post(HttpServerUrl, $"/{tag.OpCode}/DeviceWrite", new OperateWriteValue() { TagId = tag.TagID.ToString(), Value = tag.TagValue.ToString() });
                            if (result.IsSuccess)
                            {
                                SystemLog.Debug($"WriteApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue}", tag.OpName);
                            }
                            else
                            {
                                SystemLog.Debug($"WriteApiError:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue}|到服务端写入失败|{errorMsg}", tag.OpName);
                                return flag;
                            }
                        }

                        #endregion 非监控数据
                    }
                    else
                    {
                        SystemLog.Debug($"写入操作:TagID未启用:{TagID}", tag.OpName);
                    }
                }
                else
                {
                    SystemLog.Error($"写入操作:未找到TagID:{TagID}");
                }
            }
            catch (Exception e)
            {
                SystemLog.Exception($"Write(TagID)出错:{TagID}", e);
            }
            return flag;
        }

        /// <summary>
        /// 批量写 PLC
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public static bool WritePLC_Sync_DataList(List<QualityDataType> List)
        {
            bool flag = false;
            try
            {
                string errorMsg = string.Empty;
                List<TagMsg> list = new List<TagMsg>();
                foreach (QualityDataType item in List)
                {
                    if (BaseData.AllTagsMap.TryGetValue(item.TagID, out var tag))
                    {
                        if (tag.IsEnable)
                        {
                            tag.TagValue = item.TagValue;
                            tag = ConvertTagTypeToString(tag);
                            list.Add(new TagMsg()
                            {
                                UniqueId = tag.OpCode,
                                TagID = tag.TagID,
                                TagName = tag.TagName,
                                TagQuality = "good",
                                TagValue = tag.TagValue.ToString()
                            });
                        }
                        else
                        {
                            SystemLog.Debug($"批量写入操作:TagID未启用: {item.TagID}", tag.OpName);
                        }
                    }
                    else
                    {
                        SystemLog.Error($"批量写入操作:未找到TagID: {item.TagID}");
                    }
                }
                if (redisClient.Pub(WriteChannel, JsonConvert.SerializeObject(list), out errorMsg))
                {
                    SystemLog.Debug($"Write:{JsonConvert.SerializeObject(list)}", List.FirstOrDefault().OpName);
                }
                else
                {
                    SystemLog.Debug($"Write:{JsonConvert.SerializeObject(list)}|到服务端写入失败|{errorMsg}", List.FirstOrDefault().OpName);
                    return flag;
                }
                flag = true;
            }
            catch (Exception e)
            {
                SystemLog.Exception($"WritePLC_Sync_DataList出错:{JsonConvert.SerializeObject(List)}", e);
            }
            return flag;
        }

        #endregion 写

        /// <summary>
        /// 数据类型转换字符串类型
        /// </summary>
        /// <param name="q"></param>
        public static QualityDataType ConvertTagTypeToString(QualityDataType q)
        {
            try
            {
                switch (q.TagType.ToString())
                {
                    case "shortArray":
                        if (q.TagValue is short[])
                        {
                            q.TagValue = string.Join(",", (short[])q.TagValue);
                        }
                        break;

                    case "intArray":
                        if (q.TagValue is int[])
                        {
                            q.TagValue = string.Join(",", (int[])q.TagValue);
                        }
                        break;

                    case "floatArray":
                        if (q.TagValue is float[])
                        {
                            q.TagValue = string.Join(",", (float[])q.TagValue);
                        }
                        break;

                    case "datetimeArray":
                        if (q.TagValue is DateTime[])
                        {
                            q.TagValue = string.Join(",", (DateTime[])q.TagValue);
                        }
                        break;

                    case "boolArray":
                        if (q.TagValue is bool[])
                        {
                            q.TagValue = string.Join(",", (bool[])q.TagValue);
                        }
                        break;

                    case "sbyteArray":
                        if (q.TagValue is sbyte[])
                        {
                            q.TagValue = string.Join(",", (sbyte[])q.TagValue);
                        }
                        break;

                    case "byteArray":
                        if (q.TagValue is byte[])
                        {
                            q.TagValue = string.Join(",", (byte[])q.TagValue);
                        }
                        break;

                    case "ushortArray":
                        if (q.TagValue is ushort[])
                        {
                            q.TagValue = string.Join(",", (ushort[])q.TagValue);
                        }
                        break;

                    case "uintArray":
                        if (q.TagValue is uint[])
                        {
                            q.TagValue = string.Join(",", (uint[])q.TagValue);
                        }
                        break;

                    default:
                        q.TagValue = Convert.ToString(q.TagValue ?? "");
                        break;
                }
            }
            catch (Exception e)
            {
                SystemLog.Exception($"ConvertTagTypeToString:{JsonConvert.SerializeObject(q)}", e);
            }
            return q;
        }

        /// <summary>
        /// 字符串类型转换数据类型
        /// </summary>
        /// <param name="q"></param>
        public static void ConvertStringToTagType(ref QualityDataType q)
        {
            try
            {
                switch (q.TagType.ToString())
                {
                    case "short":
                        q.TagValue = Convert.ToInt16(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "shortArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                short[] vaule = Enumerable.Repeat(short.Parse("0"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = short.Parse(strvaule[i]);
                                }
                                q.TagValue = vaule;
                            }
                        }
                        break;

                    case "int":
                        q.TagValue = Convert.ToInt32(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "intArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                int[] vaule = Enumerable.Repeat(int.Parse("0"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = int.Parse(strvaule[i]);
                                }
                                q.TagValue = vaule;
                            }
                        }
                        break;

                    case "float":
                        q.TagValue = float.Parse(Convert.ToString(q.TagValue == null || q.TagValue.ToString() == "" ? "-1" : q.TagValue));
                        break;

                    case "floatArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                float[] vaule = Enumerable.Repeat(float.Parse("0"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = float.Parse(strvaule[i]);
                                }
                                q.TagValue = vaule;
                            }
                        }
                        break;

                    case "datetime":
                        q.TagValue = DateTime.Parse(Convert.ToString(q.TagValue ?? DateTime.Parse("0000/00/00 00:00:00")));
                        break;

                    case "datetimeArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                DateTime[] vaule = Enumerable.Repeat(DateTime.Parse("0000/00/00 00:00:00"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    vaule[i] = DateTime.Parse(strvaule[i]);
                                }
                                q.TagValue = vaule;
                            }
                        }
                        break;

                    case "string":
                        q.TagValue = Convert.ToString(q.TagValue ?? "");
                        break;

                    case "bool":
                        q.TagValue = bool.Parse(q.TagValue == null || q.TagValue.ToString() == "" ? "False" : q.TagValue.ToString());
                        break;

                    case "boolArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                bool[] bytevaule = Enumerable.Repeat(bool.Parse("False"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = bool.Parse(strvaule[i]);
                                }
                                q.TagValue = bytevaule;
                            }
                        }
                        break;

                    case "sbyte":
                        q.TagValue = Convert.ToSByte(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "sbyteArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                sbyte[] bytevaule = Enumerable.Repeat((sbyte)0x00, q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = sbyte.Parse(strvaule[i]);
                                }
                                q.TagValue = bytevaule;
                            }
                        }
                        break;

                    case "byte":
                        q.TagValue = Convert.ToByte(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "byteArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                byte[] bytevaule = Enumerable.Repeat((byte)0x00, q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = byte.Parse(strvaule[i]);
                                }
                                q.TagValue = bytevaule;
                            }
                        }
                        break;

                    case "ushort":
                        q.TagValue = Convert.ToUInt16(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "ushortArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                ushort[] bytevaule = Enumerable.Repeat(ushort.Parse("0"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = ushort.Parse(strvaule[i]);
                                }
                                q.TagValue = bytevaule;
                            }
                        }
                        break;

                    case "uint":
                        q.TagValue = Convert.ToUInt32(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "uintArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                uint[] bytevaule = Enumerable.Repeat(uint.Parse("0"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = uint.Parse(strvaule[i]);
                                }
                                q.TagValue = bytevaule;
                            }
                        }
                        break;

                    case "tod":
                        q.TagValue = Convert.ToInt32(q.TagValue == null || q.TagValue.ToString() == "" ? 0 : q.TagValue);
                        break;

                    case "todArray":
                        if (q.TagValue != null && q.TagValue.ToString() != "")
                        {
                            string[] strvaule = q.TagValue.ToString().Split(',');
                            if (strvaule.Length <= q.TagTypeLength)
                            {
                                int[] bytevaule = Enumerable.Repeat(int.Parse("0"), q.TagTypeLength).ToArray();
                                for (int i = 0; i < strvaule.Length; i++)
                                {
                                    bytevaule[i] = int.Parse(strvaule[i]);
                                }
                                q.TagValue = bytevaule;
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                SystemLog.Exception($"ConvertStringToTagType:{JsonConvert.SerializeObject(q)}", e);
            }
        }
    }
}