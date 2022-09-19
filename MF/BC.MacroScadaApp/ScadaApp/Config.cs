using BasicData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using SystemFramework;

namespace ScadaAppCore
{
    /// <summary>
    ///
    /// </summary>
    public partial class ScadaApp
    {
        /// <summary>
        /// redis Master服务ip
        /// </summary>
        private static string redisIp { set; get; } = "127.0.0.1";

        /// <summary>
        /// redis服务ip
        /// </summary>
        public static string RedisIp
        {
            get
            {
                return redisIp;
            }
            set
            {
                redisIp = value;
            }
        }

        /// <summary>
        /// redis服务端口
        /// </summary>
        private static int redisPort { set; get; } = 18222;

        /// <summary>
        /// redis服务端口
        /// </summary>
        public static int RedisPort
        {
            get
            {
                return redisPort;
            }
            set
            {
                redisPort = value;
            }
        }

        /// <summary>
        /// http服务端口
        /// </summary>
        private static int httpPort { set; get; } = 18555;

        /// <summary>
        /// http服务端口
        /// </summary>
        public static int HttpPort
        {
            get
            {
                return httpPort;
            }
            set
            {
                httpPort = value;
            }
        }

        /// <summary>
        /// http服务请求地址
        /// </summary>
        public static string HttpUri
        {
            get
            {
                return $"http://{redisIp}:{HttpPort}";
            }
        }

        /// <summary>
        /// Redis订阅通道
        /// </summary>
        private static string changeChannelName { set; get; } = "ChangeChannelName";

        /// <summary>
        /// Redis订阅通道
        /// </summary>
        public static string ChangeChannelName
        {
            get
            {
                return changeChannelName;
            }
            set
            {
                changeChannelName = value;
            }
        }

        /// <summary>
        /// Redis发布通道
        /// </summary>
        private static string writeTagNodeValue { set; get; } = "WriteTagNodeValue";

        /// <summary>
        /// Redis发布通道
        /// </summary>
        public static string WriteTagNodeValue
        {
            get
            {
                return writeTagNodeValue;
            }
            set
            {
                writeTagNodeValue = value;
            }
        }

        /// <summary>
        /// 允许MES监控读写变量类型
        /// </summary>
        private static string[] allowRWCode { set; get; }

        /// <summary>
        /// 允许MES监控读写变量类型
        /// </summary>
        public static string[] AllowRWCode
        {
            get
            {
                return allowRWCode;
            }
            set
            {
                allowRWCode = value;
            }
        }

        /// <summary>
        /// Tag标签
        /// </summary>
        public static List<QualityDataType> TagList { set; get; } = new List<QualityDataType>();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public bool InitConfig()
        {
            try
            {
                try
                {
                    redisIp = Convert.ToString(ConfigurationManager.AppSettings.GetValues("RedisIp")[0].Split('|')[0]);
                }
                catch
                {
                    redisIp = "127.0.0.1";
                    ApplicationLog.SystemLog("common", $"未适配Redis服务IP:配置<RedisIp>将以默认值({redisIp})启动", "INFO");
                }

                try
                {
                    redisPort = Convert.ToInt32(ConfigurationManager.AppSettings.GetValues("RedisPort")[0].Split('|')[0]);
                }
                catch
                {
                    redisPort = 18222;
                    ApplicationLog.SystemLog("common", $"未适配Redis服务端口:配置<RedisPort>将以默认值({redisPort})启动", "INFO");
                }

                try
                {
                    httpPort = Convert.ToInt32(ConfigurationManager.AppSettings.GetValues("HttpPort")[0]);
                }
                catch
                {
                    httpPort = 18555;
                    ApplicationLog.SystemLog("common", $"未适配Http服务端口:配置<HttpPort>将以默认值({httpPort})启动", "INFO");
                }
                try
                {
                    changeChannelName = Convert.ToString(ConfigurationManager.AppSettings.GetValues("ChangeChannelName")[0]);
                }
                catch
                {
                    changeChannelName = "ChangeTagNodeValue";
                    ApplicationLog.SystemLog("common", $"未适配Redis订阅通道:配置<ChangeChannelName>将以默认值({changeChannelName})启动", "INFO");
                }

                try
                {
                    writeTagNodeValue = Convert.ToString(ConfigurationManager.AppSettings.GetValues("WriteTagNodeValue")[0]);
                }
                catch
                {
                    writeTagNodeValue = "WriteTagNodeValue";
                    ApplicationLog.SystemLog("common", $"未适配Redis发布通道:配置<WriteTagNodeValue>将以默认值({writeTagNodeValue})启动", "INFO");
                }

                try
                {
                    allowRWCode = Convert.ToString(ConfigurationManager.AppSettings.GetValues("AllowRWCode")[0]).Split(',');
                }
                catch
                {
                    allowRWCode = new string[] { "" };
                    ApplicationLog.SystemLog("common", $"未适配允许监控读写变量类型:配置<AllowRWCode>将以默认值({allowRWCode})启动", "INFO");
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteLog(ex, ex.Message);
            }

            return true;
        }

        /// <summary>
        /// 初始化配置标签
        /// </summary>
        public bool InitTags(List<string> OpNames = null)
        {
            bool flag = false;
            try
            {
                #region 宏软scada模式

                string settings = string.Empty;
                string errorMsg = string.Empty;
                if (!mesRedisClient.ReadConfig(out settings, out errorMsg))
                {
                    ApplicationLog.WriteLog($"请检查Tag是否配置:{errorMsg}");
                    return flag;
                }
                XElement element = XElement.Parse(settings);

                foreach (var xmlWorkStation in element.Elements("WorkStation"))
                {
                    string OpName = Convert.ToString(xmlWorkStation.Attribute("Name").Value);
                    if (OpNames != null && OpNames.Count > 0 && !OpNames.Contains(OpName)) continue;

                    string OpCode = Convert.ToString(xmlWorkStation.Attribute("UniqueId").Value);
                    //加载所有TagNode
                    foreach (XElement tagClass in xmlWorkStation.Elements("TagClass"))
                    {
                        string TagClassType = Convert.ToString(tagClass.Attribute("Name").Value);
                        foreach (var tagNode in tagClass.Elements("TagNode"))
                        {
                            try
                            {
                                int TagID = Convert.ToInt32(tagNode.Attribute("TagID").Value);
                                int TagTypeID = (int)Enum.Parse(typeof(TagType), Convert.ToString("_" + tagNode.Attribute("TagType").Value));
                                string TagType = Convert.ToString(tagNode.Attribute("TagType").Value);
                                int TagTypeLength = Convert.ToInt32(tagNode.Attribute("Length").Value);
                                string TagDescription = Convert.ToString(tagNode.Attribute("Description").Value);
                                string TagName = Convert.ToString(tagNode.Attribute("Name").Value);
                                bool IsMonitor = Convert.ToBoolean(Convert.ToInt32(tagNode.Attribute("IsMonitor").Value));
                                bool IsEnable = Convert.ToBoolean(Convert.ToInt32(tagNode.Attribute("IsEnable").Value));
                                if (TagList.FindAll(it => it.TagID == TagID).Count == 0)
                                {
                                    TagList.Add(new QualityDataType()
                                    {
                                        TagID = TagID,
                                        TagClassType = TagClassType,
                                        TagTypeID = TagTypeID,
                                        TagType = TagType,
                                        TagTypeLength = TagTypeLength,
                                        OpCode = OpCode,
                                        OpName = OpName,
                                        TagDescription = TagDescription,
                                        TagName = TagName,
                                        IsMonitor = IsMonitor,
                                        IsEnable = IsEnable
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                ApplicationLog.WriteLog(ex, $"{tagNode} 错误消息:{ex.Message}");
                            }
                        }
                    }
                }

                #endregion 宏软scada模式

                flag = true;
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteLog(ex, ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// 西门子 11：BOOL 2：INT 3：DINT 17：BYTE 8：STRING 18:WORD 4:REAL
        /// </summary>
        public enum TagType
        {
            /// <summary>
            ///
            /// </summary>
            _short = 2,

            /// <summary>
            ///
            /// </summary>
            _shortArray = 2,

            /// <summary>
            ///
            /// </summary>
            _int = 3,

            /// <summary>
            ///
            /// </summary>
            _intArray = 3,

            /// <summary>
            ///
            /// </summary>
            _float = 4,

            /// <summary>
            ///
            /// </summary>
            _floatArray = 4,

            /// <summary>
            ///
            /// </summary>
            _datetime = 7,

            /// <summary>
            ///
            /// </summary>
            _datetimeArray = 7,

            /// <summary>
            ///
            /// </summary>
            _double = 7,

            /// <summary>
            ///
            /// </summary>
            _doubleArray = 7,

            /// <summary>
            ///
            /// </summary>
            _string = 8,

            /// <summary>
            ///
            /// </summary>
            _bool = 11,

            /// <summary>
            ///
            /// </summary>
            _boolArray = 11,

            /// <summary>
            /// /
            /// </summary>
            _sbyte = 16,

            /// <summary>
            /// /
            /// </summary>
            _sbyteArray = 16,

            /// <summary>
            /// /
            /// </summary>
            _byte = 17,

            /// <summary>
            ///
            /// </summary>
            _byteArray = 17,

            /// <summary>
            ///
            /// </summary>
            _ushort = 18,

            /// <summary>
            ///
            /// </summary>
            _ushortArray = 18,

            /// <summary>
            /// /
            /// </summary>
            _uint = 19,

            /// <summary>
            /// /
            /// </summary>
            _uintArray = 19,

            /// <summary>
            ///
            /// </summary>
            _time = 2,

            /// <summary>
            ///
            /// </summary>
            _timeArray = 2,

            /// <summary>
            ///
            /// </summary>
            _tod = 19,

            /// <summary>
            ///
            /// </summary>
            _todArray = 19
        }
    }
}