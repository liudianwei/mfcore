using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mes.Exe.Driver.Redis
{
    /// <summary>
    /// 操作Redis类
    /// </summary>
    public class MesRedisClient
    {
        //读取与写入Redis客户端
        private IConnectionMultiplexer redisMulti_RW = null;

        private IDatabase redisDb_RW = null;

        //发布与订阅Redis客户端
        private IConnectionMultiplexer redisMulti_SP = null;

        private IDatabase redisDb_SP = null;
        private ISubscriber redisSub_SP = null;

        //订阅主题集合，防止同一个实例订阅重复主题
        private List<string> subList = new List<string>();

        private object objLock = new object();

        /// <summary>
        /// 请求订阅
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="Message"></param>
        public delegate void RedisSubMessageHandle(string Topic, string Message);

        /// <summary>
        ///
        /// </summary>
        public event RedisSubMessageHandle RedisSubMessage;

        /// <summary>
        /// redis服务端ip
        /// </summary>
        private static string _ip = "127.0.0.1";

        /// <summary>
        /// redis服务端port
        /// </summary>
        private static int _port = 18222;

        /// <summary>
        /// redis服务端password
        /// </summary>
        private static string _password = "";

        /// <summary>
        /// redis服务端是否连接成功
        /// </summary>
        public bool IsConnected => redisMulti_SP.IsConnected;

        /// <summary>
        /// Redis客户端
        /// </summary>
        public MesRedisClient(string ip = "127.0.0.1", int port = 18222, string password = "")
        {
            _ip = ip;
            _port = port;
            _password = password;
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Open(out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            try
            {
                ConfigurationOptions config = new ConfigurationOptions()
                {
                    EndPoints = { { _ip, _port } },
                    Password = _password,
                    AllowAdmin = true,
                    AbortOnConnectFail = false
                };
                //读取与写入
                redisMulti_RW = ConnectionMultiplexer.Connect(config);
                redisDb_RW = redisMulti_RW.GetDatabase();
                //发布与订阅
                redisMulti_SP = ConnectionMultiplexer.Connect(config);
                redisDb_SP = redisMulti_SP.GetDatabase();
                redisSub_SP = redisMulti_SP.GetSubscriber();
                flag = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Close();
            }
            return flag;
        }

        /// <summary>
        /// 释放
        /// </summary>
        ~MesRedisClient()
        {
            Close();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                subList = new List<string>();
                if (redisSub_SP != null)
                {
                    redisSub_SP.UnsubscribeAll();
                    redisSub_SP = null;
                }
                if (redisMulti_SP != null)
                {
                    redisMulti_SP.Close();
                    redisMulti_SP = null;
                }
                if (redisMulti_RW != null)
                {
                    redisMulti_RW.Close();
                    redisMulti_RW = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// 删除db=0,指定MesKey
        /// </summary>
        /// <param name="mesKey"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool DeleteMesKey(string mesKey, out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                flag = redisDb_RW.KeyDelete(mesKey);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 清除Redis中所有数据
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool DeleteAllMesKey(out string errorMsg, int db = 0)
        {
            bool flag = false;
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                redisMulti_RW.GetServer(_ip, _port).FlushDatabase(db);
                flag = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 批量读取MesVal
        /// </summary>
        /// <param name="mesKey"></param>
        /// <param name="MesVal"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Read(string mesKey, out string MesVal, out string errorMsg)
        {
            bool flag = false;
            MesVal = "";
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                MesVal = redisDb_RW.StringGet(mesKey);
                flag = true;
            }
            catch
            {
                try
                {
                    MesVal = redisDb_RW.StringGet(mesKey);
                    flag = true;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
            }
            MesVal = string.IsNullOrEmpty(MesVal) ? "" : MesVal;
            return flag;
        }

        /// <summary>
        /// 读取MesVal
        /// </summary>
        /// <param name="mesKey"></param>
        /// <param name="MesVal"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Read(string[] mesKey, out string[] MesVal, out string errorMsg)
        {
            bool flag = false;
            MesVal = new string[mesKey.Length];
            errorMsg = string.Empty;
            RedisKey[] redisKeys = new RedisKey[mesKey.Length];
            RedisValue[] redisValues = new RedisValue[mesKey.Length];
            for (int i = 0; i < mesKey.Length; i++)
            {
                redisKeys[i] = mesKey[i];
            }
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                redisValues = redisDb_RW.StringGet(redisKeys);
                flag = true;
            }
            catch
            {
                try
                {
                    redisValues = redisDb_RW.StringGet(redisKeys);
                    flag = true;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
            }
            for (int i = 0; i < redisValues.Length; i++)
            {
                MesVal[i] = string.IsNullOrEmpty(redisValues[i]) ? "" : redisValues[i].ToString();
            }
            return flag;
        }

        /// <summary>
        /// 读取配置文件
        /// 配置文件存储在DB=1中
        /// </summary>
        /// <param name="MesVal"></param>
        /// <param name="errorMsg"></param>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ReadConfig(out string MesVal, out string errorMsg, int db = 1, string key = "Settings.xml")
        {
            bool flag = false;
            MesVal = "";
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                MesVal = redisMulti_RW.GetDatabase(db).StringGet(key);
                flag = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            MesVal = string.IsNullOrEmpty(MesVal) ? "" : MesVal;
            return flag;
        }

        /// <summary>
        /// 读取是否允许工作
        /// </summary>
        /// <param name="MesVal"></param>
        /// <param name="errorMsg"></param>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ReadAllowWork(out string MesVal, out string errorMsg, int db = 1, string key = "MES_ALLOWWORK")
        {
            bool flag = false;
            MesVal = "";
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                MesVal = redisMulti_RW.GetDatabase(db).StringGet(key);
                flag = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            MesVal = string.IsNullOrEmpty(MesVal) ? "" : MesVal;
            return flag;
        }

        /// <summary>
        /// 写入MesVal
        /// </summary>
        /// <param name="mesKey"></param>
        /// <param name="mesVal"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Write(string mesKey, string mesVal, out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                flag = redisDb_RW.StringSet(mesKey, mesVal);
                if (!flag)
                {
                    flag = redisDb_RW.StringSet(mesKey, mesVal);
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 批量写入MesVal
        /// </summary>
        /// <param name="mesKey"></param>
        /// <param name="mesVal"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Write(string[] mesKey, string[] mesVal, out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            if (redisMulti_RW == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                List<KeyValuePair<RedisKey, RedisValue>> keyValuePair = new List<KeyValuePair<RedisKey, RedisValue>>();
                for (int i = 0; i < mesKey.Length; i++)
                {
                    keyValuePair.Add(new KeyValuePair<RedisKey, RedisValue>(mesKey[i], mesVal[i]));
                }
                flag = redisDb_RW.StringSet(keyValuePair.ToArray());
                if (!flag)
                {
                    flag = redisDb_RW.StringSet(keyValuePair.ToArray());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Pub(string topic, string message, out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            if (redisMulti_SP == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                redisSub_SP.Publish(topic, message);
                flag = true;
            }
            catch
            {
                try
                {
                    redisSub_SP.Publish(topic, message);
                    flag = true;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
            }
            return flag;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Sub(string topic, out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            if (redisMulti_SP == null)
            {
                errorMsg = "未将对象实例化";
                return flag;
            }
            try
            {
                lock (objLock)
                {
                    if (subList.Contains(topic))
                    {
                        return true;
                    }
                    redisSub_SP.Subscribe(topic, (chl, msg) =>
                    {
                        Task.Run(() => subMessage(chl, msg));
                    });
                    if (!subList.Contains(topic))
                    {
                        subList.Add(topic);
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// subMessage
        /// </summary>
        /// <param name="chl"></param>
        /// <param name="msg"></param>
        private void subMessage(string chl, string msg)
        {
            try
            {
                RedisSubMessage?.BeginInvoke(chl, msg, null, null);
            }
            catch { }
        }
    }
}