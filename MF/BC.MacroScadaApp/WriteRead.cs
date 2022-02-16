using System;
using System.Linq;
using System.Collections;
using SystemFramework;
using BasicData;
using System.Collections.Generic;
using Newtonsoft.Json;
using Mes.Exe.Driver.Rest;
using System.Collections.Concurrent;
using System.Threading;

namespace ScadaAppCore
{
    public partial class BaseScadaApp
    {
        #region 读

        /// <summary>
        /// 根据TagID读取数值
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public static object ReadPLC(int TagID)
        {
            bool flag = false;
            object TagValue;
            try
            {
                var tag = ScadaApp.TagList.Find(it => it.TagID == TagID);
                if (tag.IsEnable)
                {
                    if (tag.IsMonitor)
                    {
                        string errorMsg = string.Empty;
                        string MesVal = string.Empty;
                        if (ScadaApp.mesRedisClient.Read($"{tag.OpCode}:{tag.TagID}", out MesVal, out errorMsg))
                        {
                            tag.TagValue = MesVal;
                            ScadaApp.ConvertStringToTagType(ref tag);
                            TagValue = tag.TagValue;
                            ApplicationLog.BusinessLog(tag.OpName, $"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{MesVal}");
                        }
                        else
                        {
                            TagValue = null;
                            ApplicationLog.WriteLog($"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|缓存读取失败{errorMsg}");
                        }
                    }
                    else
                    {
                        var result = MesRestClient.Get(ScadaApp.HttpUri, $"/{tag.OpCode}/DeviceRead?TagId={tag.TagID}");
                        if (result.IsSuccess)
                        {
                            tag.TagValue = result.Value;
                            ScadaApp.ConvertStringToTagType(ref tag);
                            TagValue = tag.TagValue;
                            ApplicationLog.BusinessLog(tag.OpName, $"ReadApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{result.Value}");
                        }
                        else
                        {
                            TagValue = null;
                            ApplicationLog.WriteLog($"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|Api读取失败{result.Message}");
                        }
                    }
                }
                else
                {
                    TagValue = null;
                    ApplicationLog.BusinessLog(tag.OpName, $"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|变量未启用,请确认");
                }
            }
            catch (Exception err)
            {
                TagValue = null;
                ApplicationLog.WriteLog(err, err.Message);
            }
            return TagValue;
        }

        /// <summary>
        /// 根据批量TagID 同步读取数据
        /// </summary>
        /// <param name="tags"></param>
        /// <returns>Hashtable(TagID,QualityDataType)</returns>
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
                if (ScadaApp.mesRedisClient.Read(keys, out vals, out errorMsg))
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        var tag = FromRedis.Where(x => (x.OpCode + ":" + x.TagID).Equals(keys[i])).FirstOrDefault();
                        tag.TagValue = vals[i];
                        ScadaApp.ConvertStringToTagType(ref tag);
                    }
                    if (keys.Length > 0)
                    {
                        ApplicationLog.BusinessLog(tags.FirstOrDefault().OpName, $"Read:{string.Join("$", keys)}|{string.Join("$", vals)}");
                    }
                }
                else
                {
                    if (keys.Length > 0)
                    {
                        ApplicationLog.WriteLog($"Read:{string.Join(",", keys)}|缓存读取失败" + errorMsg);
                    }
                }

                #endregion 间接缓存读取

                #region 直接PLC读取

                var FromPLC = needReadFromRedis.Where(i => !i.IsMonitor).ToList();
                FromPLC.ForEach(tag =>
                {
                    var result = MesRestClient.Get(ScadaApp.HttpUri, $"/{tag.OpCode}/DeviceRead?TagId={tag.TagID}");
                    if (result.IsSuccess)
                    {
                        tag.TagValue = result.Value;
                        ScadaApp.ConvertStringToTagType(ref tag);
                        ApplicationLog.BusinessLog(tag.OpName, $"ReadApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{result.Value}");
                    }
                    else
                    {
                        tag.TagQuality = 0;
                        ApplicationLog.WriteLog($"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|Api读取失败{result.Message}");
                    }
                });

                #endregion 直接PLC读取
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
        }

        /// <summary>
        /// 读质量数据list
        /// </summary>
        /// <param name="OpName"></param>
        /// <param name="TagClassType">质量数据组:Quality</param>
        /// <param name="AllowRW">允许读取AllowRW,默认false</param>
        /// <returns></returns>
        public static List<QualityDataType> ReadPLC_Sync_DataList_MesRead(string OpName, string TagClassType = "Quality", bool AllowRW = false)
        {
            List<QualityDataType> valueList = new List<QualityDataType>();
            string errorMsg = string.Empty;
            try
            {
                List<QualityDataType> tags = new List<QualityDataType>();
                if (AllowRW)
                {
                    tags = ScadaApp.TagList.Where(u => u.OpName == OpName && (ScadaApp.AllowRWCode.Contains(u.TagName) || u.TagClassType.Contains(TagClassType))).ToList();
                }
                else
                {
                    tags = ScadaApp.TagList.Where(u => u.OpName == OpName && u.TagClassType.Contains(TagClassType)).ToList();
                }

                #region new代码

                var needReadFromRedis = tags.Where(i => i.IsEnable).ToList();

                #region 间接缓存读取

                var FromRedis = needReadFromRedis.Where(i => i.IsMonitor).ToList();
                string[] keys = FromRedis.Select(i => i.OpCode + ":" + i.TagID).ToArray();
                string[] vals = new string[keys.Length];
                if (ScadaApp.mesRedisClient.Read(keys, out vals, out errorMsg))
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        var tagv = FromRedis.Where(x => (x.OpCode + ":" + x.TagID).Equals(keys[i])).FirstOrDefault();
                        tagv.TagValue = vals[i];
                        ScadaApp.ConvertStringToTagType(ref tagv);
                        valueList.Add(tagv);
                    }
                    if (keys.Length > 0)
                    {
                        ApplicationLog.BusinessLog(tags.FirstOrDefault().OpName, $"Read:{string.Join("$", keys)}|{string.Join("$", vals)}");
                    }
                }
                else
                {
                    if (keys.Length > 0)
                    {
                        ApplicationLog.WriteLog($"Read:{string.Join(",", keys)}|缓存读取失败" + errorMsg);
                    }
                }

                #endregion 间接缓存读取

                #region 直接PLC读取

                var FromPLC = needReadFromRedis.Where(i => !i.IsMonitor).ToList();
                foreach (var tag in FromPLC)
                {
                    var tagv = tag;
                    var result = MesRestClient.Get(ScadaApp.HttpUri, $"/{tag.OpCode}/DeviceRead?TagId={tag.TagID}");
                    if (result.IsSuccess)
                    {
                        tagv.TagValue = result.Value;
                        ScadaApp.ConvertStringToTagType(ref tagv);
                        ApplicationLog.BusinessLog(tag.OpName, $"ReadApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{result.Value}");
                    }
                    else
                    {
                        tagv.TagQuality = 0;
                        ApplicationLog.WriteLog($"Read:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|Api读取失败{result.Message}");
                    }
                    valueList.Add(tagv);
                }

                #endregion 直接PLC读取

                #endregion new代码
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
            return valueList;
        }

        #endregion 读

        #region 写

        /// <summary>
        /// 根据TagID写数值
        /// </summary>
        /// <param name="TagID"></param>
        /// <param name="TagValue"></param>
        public static bool WritePLC(int TagID, object TagValue)
        {
            bool flag = false;
            try
            {
                var tag = ScadaApp.TagList.Find(it => it.TagID == TagID);
                if (tag.IsEnable)
                {
                    string errorMsg = string.Empty;
                    tag.TagValue = TagValue;
                    tag = ScadaApp.ConvertTagTypeToString(tag);

                    if (tag.IsMonitor)
                    {
                        ScadaApp.Enqueue(new ScadaApp.TagMsg()
                        {
                            UniqueId = tag.OpCode,
                            TagID = tag.TagID.ToString(),
                            TagName = tag.TagName,
                            TagQuality = "good",
                            TagValue = tag.TagValue.ToString()
                        });
                        ApplicationLog.BusinessLog(tag.OpName, $"Write队列:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue.ToString()}");
                    }
                    else
                    {
                        var result = MesRestClient.Post(ScadaApp.HttpUri, $"/{tag.OpCode}/DeviceWrite", new OperateWriteValue() { TagId = tag.TagID.ToString(), Value = tag.TagValue.ToString() });
                        if (result.IsSuccess)
                        {
                            ApplicationLog.BusinessLog(tag.OpName, $"WriteApi:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue.ToString()}");
                        }
                        else
                        {
                            ApplicationLog.WriteLog($"WriteApi {tag.TagID } 值为{tag.TagValue.ToString()}到服务端写入失败|{result.Message}");
                        }
                    }
                }
                else
                {
                    ApplicationLog.BusinessLog(tag.OpName, $"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|变量未启用,请确认");
                }

                flag = true;
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
            return flag;
        }

        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public static bool WritePLC_Sync_DataList(List<QualityDataType> List)
        {
            bool flag = false;
            try
            {
                string errorMsg = string.Empty;
                List<ScadaApp.TagMsg> tagMsgs = new List<ScadaApp.TagMsg>();
                foreach (QualityDataType item in List)
                {
                    var tag = ScadaApp.TagList.Find(it => it.TagID == item.TagID);
                    if (tag.IsEnable)
                    {
                        tag.TagValue = item.TagValue;
                        tag = ScadaApp.ConvertTagTypeToString(tag);
                        tagMsgs.Add(new ScadaApp.TagMsg()
                        {
                            UniqueId = tag.OpCode,
                            TagID = tag.TagID.ToString(),
                            TagName = tag.TagName,
                            TagQuality = "good",
                            TagValue = tag.TagValue.ToString()
                        });
                    }
                    else
                    {
                        ApplicationLog.BusinessLog(tag.OpName, $"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|变量未启用,请确认");
                    }
                }
                if (!ScadaApp.mesRedisClient.Pub(ScadaApp.WriteTagNodeValue, JsonConvert.SerializeObject(tagMsgs), out errorMsg))
                {
                    ApplicationLog.WriteLog($"发送值为{JsonConvert.SerializeObject(tagMsgs)}到服务端写入失败|{errorMsg}");
                    return flag;
                }
                else
                {
                    ApplicationLog.BusinessLog("WritePLC_Sync_DataList", $"Write:WritePLC_Sync_DataList|{JsonConvert.SerializeObject(tagMsgs)}");
                }

                flag = true;
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
            return flag;
        }

        /// <summary>
        /// 直接写redis
        /// </summary>
        /// <param name="TagID"></param>
        /// <param name="TagValue"></param>
        /// <returns></returns>
        public static bool WritePLCDirect(int TagID, object TagValue)
        {
            bool flag = false;
            try
            {
                var tag = ScadaApp.TagList.Find(it => it.TagID == TagID);
                if (tag.IsEnable)
                {
                    tag.TagValue = TagValue;
                    tag = ScadaApp.ConvertTagTypeToString(tag);

                    if (ScadaApp.mesRedisClient.Write($"{tag.OpCode}:{tag.TagID}", tag.TagValue.ToString(), out var errorMsg))
                    {
                        ApplicationLog.BusinessLog(tag.OpName, $"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue}");
                    }
                    else
                    {
                        ApplicationLog.WriteLog($"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|{tag.TagValue} 到服务端写入失败|{errorMsg}");
                        return flag;
                    }
                }
                else
                {
                    ApplicationLog.BusinessLog(tag.OpName, $"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|变量未启用,请确认");
                }

                flag = true;
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
            return flag;
        }

        /// <summary>
        /// 批量写redis
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public static bool WritePLC_Sync_DataList_Direct(List<QualityDataType> List)
        {
            bool flag = false;
            try
            {
                string errorMsg = string.Empty;
                List<string> Keys = new List<string>();
                List<string> Values = new List<string>();
                foreach (QualityDataType item in List)
                {
                    var tag = ScadaApp.TagList.Find(it => it.TagID == item.TagID);
                    if (tag.IsEnable)
                    {
                        tag.TagValue = item.TagValue;
                        tag = ScadaApp.ConvertTagTypeToString(tag);
                        Keys.Add($"{tag.OpCode}:{tag.TagID}");
                        Values.Add(tag.TagValue.ToString());
                    }
                    else
                    {
                        ApplicationLog.BusinessLog(tag.OpName, $"Write:{tag.OpName}|{tag.TagID}|{tag.TagDescription}|变量未启用,请确认");
                    }
                }
                if (!ScadaApp.mesRedisClient.Write(Keys.ToArray(), Values.ToArray(), out errorMsg))
                {
                    ApplicationLog.WriteLog($"发送值为Key:{JsonConvert.SerializeObject(Keys)},Value:{JsonConvert.SerializeObject(Values)}到服务端写入失败|{errorMsg}");
                    return flag;
                }
                else
                {
                    ApplicationLog.BusinessLog("WritePLC_Sync_DataList_Direct", $"Write:WritePLC_Sync_DataList_Direct|Key:{JsonConvert.SerializeObject(Keys)},Value:{JsonConvert.SerializeObject(Values)}");
                }

                flag = true;
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
            return flag;
        }

        #endregion 写
    }
}