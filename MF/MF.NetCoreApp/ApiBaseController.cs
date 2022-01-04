using Folke.Localization.Json;

using MF.FluentValidation;
using MF.Rest;
using MF.Utils;
using MF.Utils.Json;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Newtonsoft.Json;

using NPOI.SS.UserModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MF.NetCoreApp
{
    [Route("rest/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiBaseController : ControllerBase
    {
        public IStringLocalizer _default_localizer;
        public IStringLocalizer _culture_localizer = null;

        public ApiBaseController(IStringLocalizer localizer)
        {
            _default_localizer = localizer;
        }

        public override ContentResult Content(string content)
        {
            return base.Content(content, "application/json");
        }

        [NonAction]
        public IEnumerable<Claim> GetClaims()
        {
            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            return isAuthenticated ? HttpContext.User.Claims : null;
        }

        [NonAction]
        protected virtual string GetRequestHeaders(string key)
        {
            return HttpContext.Request.Headers[key].ToString();
        }

        [NonAction]
        protected virtual void SetResponseHeaders(string key, string value)
        {
            HttpContext.Response.Headers.Add(key, value);
        }

        [NonAction]
        protected virtual string GetToken()
        {
            return GetRequestHeaders("Authorization")?.Split("Bearer ").Last();
        }

        [NonAction]
        protected ContentResult Result(PubResponse response)
        {
            var res = new HttpResult();
            var lang = GetRequestHeaders("Accept-language").Split(",")[0].ToLower();
            if (response.HasError)
            {
                res.Code = 100;
                res.Status = "error";
                res.Message = GetI18NMsg(response, lang);
            }
            else
            {
                res.Code = 200;
                res.Status = "success";
                res.Message = response.Message;
            }
            res.Data = response.Data;
            return Content(res.JilToJsonCamelCase());
        }

        /// <summary>
        /// 格式化i8n
        /// </summary>
        /// <param name="response"></param>
        /// <param name="res"></param>
        [NonAction]
        private string GetI18NMsg(PubResponse response, string lang)
        {
            if (_culture_localizer is null)
            {
                //var lang = GetRequestHeaders("Accept-language");
                //JsonStringLocalizer<Msg> jm = ((JsonStringLocalizer<Msg>)_default_localizer);
                //lang = lang.Split(",")[0].ToLower();
                _culture_localizer = new JsonStringLocalizer("lang", "MF.Utils.Msg", "errMsg", lang);
            }

            var key = response.Validations.Count() > 0 ? response.Validations.Aggregate("", (current, item) => (current + item.ErrorMessage)) : response.Message;
            var Message = string.Empty;

            try
            {
                Message = _culture_localizer[key]?.Value;//如果没有找到value 就设置为key

                //1.占位符顺序必须正确 2.占位必须有
                if (GetSubCount(Message, out int subCount))
                {
                    if (subCount > 0)
                    {
                        // 3.参数必须有 4.参数数量和占位符数量必须一致
                        if (response.MessageParams != null && response.MessageParams.Count > 0 && subCount == response.MessageParams.Count)
                        {
                            Message = string.Format(Message, response.MessageParams.ToArray());
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                else
                {
                    Message = "i8n提示消息 "
                            + Message
                            + " 进行字符串格式化时出现错误，请检查占位符顺序 或 格式化参数数量是否正确,参数为："
                            + string.Join(",", response.MessageParams.ToArray());
                }
            }
            catch (Exception)
            {
                Message = key;
            }
            return Message;
        }

        /// <summary>
        /// 获取占位符数量
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        private bool GetSubCount(string msg, out int count)
        {
            Regex regex = new Regex(@"\{(\S*?)\}");
            MatchCollection s = regex.Matches(msg);

            count = 0;
            foreach (Match item in s)
            {
                if (!count.ToString().Equals(item.Groups[1].Value))
                {
                    return false;
                }
                count++;
            }
            return true;
        }

        [NonAction]
        protected virtual string GetIp()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            ip = ip.Replace("::ffff:", "");
            return ip == "::1" ? "127.0.0.1" : ip;
        }

        [NonAction]
        protected string GetUrl()
        {
            var req = HttpContext.Request;
            return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        }

        [NonAction]
        protected string GetDomain()
        {
            var req = HttpContext.Request;
            return $"{req.Scheme}://{req.Host}";
        }

        [NonAction]
        protected string GetHostIp()
        {
            var req = HttpContext.Request;
            return req.Host.ToString().Split(':')[0];
        }

        [NonAction]
        protected string GetBrowser() => HttpContext.Request.Headers["User-Agent"].ToString();

        [NonAction]
        public string GetTraceId()
        {
            var activity = Activity.Current;
            return activity?.IdFormat switch
            {
                ActivityIdFormat.Hierarchical => activity.RootId,
                ActivityIdFormat.W3C => activity.TraceId.ToHexString(),
                _ => null ?? HttpContext.TraceIdentifier,
            };
        }

        /// <summary>
        /// 导出Excel 用 NPOI 组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <param name="outpath"></param>
        /// <param name="rowstart"></param>
        /// <param name="imgs"></param>
        /// <param name="ExportRecord"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [NonAction]
        protected async Task<IActionResult> ExportExcel<T>(PubResponse resp, string outpath, int rowstart = 0, List<Utils.Excel.ImgAttribute> imgs = null, bool ExportRecord = true)
        {
            IWorkbook workbook;
            if (ExportRecord)
            {
                var list = (List<T>)resp.Data;
                if (list == null || list.Count == 0)
                {
                    throw new Exception("没有记录，不能导出");
                }

                workbook = Utils.Excel.NpoiUtil.ExportToExcel(list, rowstart);
            }
            else
            {
                workbook = Utils.Excel.NpoiUtil.ExportToExcel(new List<T>(), rowstart);
            }

            if (imgs?.Count > 0)
            {
                foreach (var item in imgs)
                {
                    if (item.ImgBase64 != "")
                    {
                        string base64 = item.ImgBase64;
                        var arr = base64.Split(',');
                        if (arr.Length > 1)
                        {
                            base64 = arr[1];
                            Utils.Excel.NpoiUtil.AddPic2Excel(workbook, Convert.FromBase64String(base64), item.ImgPosition);
                        }
                    }
                }
            }
            if (false == Directory.Exists(outpath))
            {
                Directory.CreateDirectory(outpath);
            }
            var OutFullPath = $"{outpath}{Guid.NewGuid()}.xlsx";
            using (var fs = System.IO.File.OpenWrite(OutFullPath))
            {
                workbook.Write(fs);//流会自动关闭
            }

            MemoryStream memoryStream = new MemoryStream();
            using (var stream = new FileStream(OutFullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            _ = memoryStream.Seek(0, SeekOrigin.Begin);

            // 文件名必须编码，否则会有特殊字符(如中文)无法在此下载。
            var fileName = DateTime.Now.ToString("yyyyMdHms");
            string encodeFilename = HttpUtility.UrlEncode($"{fileName}.xlsx", Encoding.GetEncoding("UTF-8"));
            Response.Headers.Add("Content-Disposition", $"attachment; filename={encodeFilename}");

            // 完成后删除文件
            Response.OnCompleted(() =>
            {
                return Task.Run(() =>
                {
                    System.IO.File.Delete(OutFullPath);
                });
            });
            return GetOctetStream(memoryStream);
        }

        /// <summary>
        /// 创建下载任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cmd"></param>
        [NonAction]
        protected void SubmitExportTask(string url, dynamic cmd)
        {
            var token1 = GetToken();
            var domain1 = GetDomain();
            Task.Run(() =>
            {
                var token = token1;
                var domain = domain1;
                var startTime = DateTime.Now;
                var reqparams = new
                {
                    cmd.ModuleName,
                    FileName = "",
                    FileSize = "-1",
                    Progress = "100",
                    QueryItem = JsonConvert.SerializeObject(cmd),
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = "-1",
                    DownloadPath = "",
                    Status = "0",
                    Remark = "",
                    Url = url
                };
                //创建导出任务
                var result = MRestClient.Post(domain, "rest/productcenter/v1/exportlog", JsonConvert.SerializeObject(reqparams), $"Bearer {token}");
            });
        }

        /// <summary>
        /// 启动下载任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <param name="cmd"></param>
        [NonAction]
        protected void AsyncExport<T>(PubResponse resp, dynamic cmd, DateTime startTime1)
        {
            var token1 = GetToken();
            var domain1 = GetDomain();
            Task.Run(() =>
            {
                DateTime startTime = startTime1;
                var token = token1;
                var domain = domain1;
                var exportCount = 0;
                var list = (List<T>)resp.Data;
                if (list != null && list.Count >= 0)
                {
                    exportCount = list.Count;
                }

                //修改状态为开始下载状态 导出正在导出
                MRestClient.Post(domain, $"rest/productcenter/v1/exportlog/modify-export-status-1", JsonConvert.SerializeObject(new { id = cmd.TaskId, exportCount, startTime }), $"Bearer {token}");

                //开始导出操作
                var items = ExportExcelLocal<T>(resp, BaseStateConstants.excelOutPath, new Action<int>((process) =>
                {
                    MRestClient.Post(domain, $"rest/productcenter/v1/exportlog/modify-export-status-1", JsonConvert.SerializeObject(new { id = cmd.TaskId, exportCount, startTime }), $"Bearer {token}");
                }), cmd.ModuleName);
                var fileName = items.Item2;
                var fileSize = FormatSize(items.Item3);
                var endTime = DateTime.Now;
                var duration = FormatDuration((endTime - startTime).TotalMilliseconds);

                //修改状态为 导出完成
                MRestClient.Post(domain, $"rest/productcenter/v1/exportlog/modify-export-status-2",
                    JsonConvert.SerializeObject(new { id = cmd.TaskId, fileName, fileSize, endTime, duration, downloadPath = items.Item1 }),
                    $"Bearer {token}");

                ClearMemory();
            });
        }

        /// <summary>
        /// 导出Excel 用 MiniExcel 组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <param name="outpath"></param>
        /// <param name="fileName"></param>
        /// <param name="rowstart"></param>
        /// <param name="imgs"></param>
        /// <param name="ExportRecord"></param>
        /// <returns></returns>
        private (string, string, long) ExportExcelLocal<T>(PubResponse resp, string outpath, Action<int> callback, string fileName = "")
        {
            var list = (IEnumerable<T>)resp.Data;
            if (list == null || list.Count() == 0)
            {
                throw new Exception("没有记录，不能导出");
            }

            fileName = MakeValidFileName(fileName);
            fileName = $"{(fileName == "" ? Guid.NewGuid().ToString() : fileName)}_{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
            var OutFullPath = $"{outpath}{fileName}";
            if (!Directory.Exists(outpath))
            {
                Directory.CreateDirectory(outpath);
            }

            try
            {
                var pp = Path.Combine(Environment.CurrentDirectory, OutFullPath);
                Utils.Excel.MiniExcelUtil.ExportToExcel(list, pp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            FileInfo fi = new FileInfo(OutFullPath);
            var length = fi.Length;
            resp.Data = null;
            return (OutFullPath, fileName, length);
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        private static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 从磁盘下载Excel到客户端
        /// </summary>
        /// <param name="OutFullPath"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        [NonAction]
        protected async Task<IActionResult> DownloadLocal(string OutFullPath, string strFileName)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (var stream = new FileStream(OutFullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            _ = memoryStream.Seek(0, SeekOrigin.Begin);

            var fileName = strFileName;
            // 文件名必须编码，否则会有特殊字符(如中文)无法在此下载。
            string encodeFilename = HttpUtility.UrlEncode($"{fileName}", Encoding.GetEncoding("UTF-8"));
            Response.Headers.Add("Content-Disposition", $"attachment; filename={encodeFilename}");

            return GetOctetStream(memoryStream);
        }

        [NonAction]
        private FileStreamResult GetOctetStream(Stream memoryStream)
        {
            return new FileStreamResult(memoryStream, "application/octet-stream");
        }

        /// <summary>
        /// 时长格式化
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string FormatDuration(double b)
        {
            const int S = 1000;
            const int M = 60 * S;

            if (b > 0)
            {
                if (b / M >= 1)
                {
                    return Math.Round(b / (float)M, 2) + " min";
                }

                if (b / S >= 1)
                {
                    return Math.Round(b / (float)S, 2) + " s";
                }
            }

            return Math.Floor(b) + " ms";
        }

        /// <summary>
        /// 文件尺寸格式化
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string FormatSize(long b)
        {
            const int GB = 1024 * 1024 * 1024;
            const int MB = 1024 * 1024;
            const int KB = 1024;

            if (b > 0)
            {
                if (b / GB >= 1)
                {
                    return Math.Round(b / (float)GB, 2) + " GB";
                }

                if (b / MB >= 1)
                {
                    return Math.Round(b / (float)MB, 2) + " MB";
                }

                if (b / KB >= 1)
                {
                    return Math.Round(b / (float)KB, 2) + " KB";
                }
            }

            return b + " B";
        }

        /// <summary>
        /// 去除文件名特殊字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string MakeValidFileName(string text = "")
        {
            if (text == null) return "";
            StringBuilder str = new StringBuilder();
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (var c in text)
            {
                if (!invalidFileNameChars.Contains(c))
                {
                    str.Append(c);
                }
            }

            return str.ToString();
        }
    }
}