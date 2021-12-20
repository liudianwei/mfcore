using Newtonsoft.Json;

using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Communication
{
    /// <summary>
    /// web service 调用客戶端
    /// </summary>
    public partial class HttpHelper
    {
        /// <summary>
        /// 需要WebService支持Post调用
        /// </summary>
        public static string PostWebServiceByJson(string URL, string MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{URL}/{MethodName}");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Pars));
            request.ContentLength = data.Length;
            using (var writer = request.GetRequestStream())
            {
                writer.Write(data, 0, data.Length);
            }
            var retXml = "";
            using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8))
            {
                retXml = sr.ReadToEnd();
            }
            return retXml;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="MethodName"></param>
        /// <param name="Pars"></param>
        /// <returns></returns>
        public static string WebServiceGet(string URL, string MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{URL}/{MethodName}?{HashtableToPostData(Pars)}");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            // 凭证
            request.Credentials = CredentialCache.DefaultCredentials;
            //超时时间
            request.Timeout = 10000;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var retXml = "";
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                retXml = sr.ReadToEnd();
            }
            return retXml;
        }

        /// <summary>
        /// 需要WebService支持Post调用
        /// </summary>
        public static string PostWebService(string URL, string MethodName, Hashtable ht)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{URL}/{MethodName}");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            // 凭证
            request.Credentials = CredentialCache.DefaultCredentials;
            //超时时间
            request.Timeout = 10000;
            var PostStr = HashtableToPostData(ht);
            byte[] data = Encoding.UTF8.GetBytes(PostStr);
            request.ContentLength = data.Length;
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(data, 0, data.Length);
            }
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var retXml = "";
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                retXml = sr.ReadToEnd();
            }
            return retXml;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        private static string HashtableToPostData(Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in ht.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }
                sb.Append($"{HttpUtility.UrlEncode(k)}={HttpUtility.UrlEncode(ht[k].ToString())}");
            }
            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="MethodName"></param>
        /// <param name="Pars"></param>
        /// <param name="XmlNs"></param>
        /// <returns></returns>
        public static string SoapV1_1WebService(string URL,
                                                string MethodName,
                                                Hashtable Pars,
                                                string XmlNs)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", $"\"{XmlNs}{(XmlNs.EndsWith("/") ? "" : "/")}{MethodName}\"");
            // 凭证
            request.Credentials = CredentialCache.DefaultCredentials;
            //超时时间
            request.Timeout = 10000;
            byte[] data = HashtableToSoap(Pars, XmlNs, MethodName);
            request.ContentLength = data.Length;
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(data, 0, data.Length);
            }
            var response = request.GetResponse();
            XmlDocument doc = new XmlDocument();
            string retXml = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                retXml = sr.ReadToEnd();
            }
            doc.LoadXml(retXml);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            return doc.SelectSingleNode("//soap:Body/*/*", mgr).InnerXml;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="XmlNs"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        private static byte[] HashtableToSoap(Hashtable ht, string XmlNs, string MethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
            XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");

            XmlElement soapMethod = doc.CreateElement(MethodName);
            soapMethod.SetAttribute("xmlns", XmlNs);
            foreach (var (k, soapPar) in from string k in ht.Keys
                                         let soapPar = doc.CreateElement(k)
                                         select (k, soapPar))
            {
                soapPar.InnerXml = ObjectToSoapXml(ht[k]);
                soapMethod.AppendChild(soapPar);
            }

            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="MethodName"></param>
        /// <param name="Pars"></param>
        /// <param name="XmlNs"></param>
        /// <returns></returns>
        public static string SoapV1_2WebService(string URL,
                                                string MethodName,
                                                Hashtable Pars,
                                                string XmlNs)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/soap+xml; charset=utf-8";

            // 凭证
            request.Credentials = CredentialCache.DefaultCredentials;
            //超时时间
            request.Timeout = 10000;
            byte[] data = HashtableToSoap12(Pars, XmlNs, MethodName);
            request.ContentLength = data.Length;
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(data, 0, data.Length);
            }
            var response = request.GetResponse();
            XmlDocument doc = new XmlDocument();
            var retXml = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                retXml = sr.ReadToEnd();
            }
            doc.LoadXml(retXml);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
            return doc.SelectSingleNode("//soap12:Body/*/*", mgr).InnerXml;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="XmlNs"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        private static byte[] HashtableToSoap12(Hashtable ht, string XmlNs, string MethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"></soap12:Envelope>");
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
            XmlElement soapBody = doc.CreateElement("soap12", "Body", "http://www.w3.org/2003/05/soap-envelope");

            XmlElement soapMethod = doc.CreateElement(MethodName);
            soapMethod.SetAttribute("xmlns", XmlNs);
            foreach (string k in ht.Keys)
            {
                XmlElement soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(ht[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }
    }
}