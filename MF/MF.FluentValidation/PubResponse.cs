using System;
using System.Collections.Generic;
using System.Linq;

using FluentValidation.Results;

namespace MF.FluentValidation
{
    public class PubResponse
    {
        private readonly List<ValidationFailure> _validations = new List<ValidationFailure>();
        public object Data { get; set; }
        public string Message { get; set; }
        public List<string> MessageParams { get; set; }
        public object StackInfo { get; set; }
        public string Status { get; set; }
        public bool HasValidation { get; set; }
        public IEnumerable<ValidationFailure> Validations => this._validations;
        public bool HasError { get; set; }
        public ValidationFailure Error { get; set; }

        public PubResponse()
        {
            this.Data = null;
            this.HasValidation = false;
            this.HasError = false;
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static PubResponse Succeed()
        {
            return new PubResponse("");
        }

        /// <summary>
        /// 成功 有数据
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static PubResponse Succeed(object data)
        {
            return new PubResponse(null, data, "success");
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static PubResponse Failed()
        {
            return new PubResponse(null, null, "error");
        }

        /// <summary>
        /// 失败 有消息 有数据
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static PubResponse Failed(string msg, object data)
        {
            return new PubResponse(msg, data, "error");
        }

        /// <summary>
        /// 失败 有消息 有消息占位符
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="msgParams">信息参数列表 默认无（信息中带有占位符{0}）</param>
        /// <returns></returns>
        public static PubResponse Failed(string msg, List<string> msgParams = null)
        {
            return new PubResponse(null, msg, "error", msgParams);
        }

        /// <summary>
        /// 成功或失败
        /// </summary>
        /// <param name="flag">业务操作 返回状态</param>
        /// <param name="sData">成功返回的数据</param>
        /// <param name="fmsg">失败返回的消息</param>
        /// <returns></returns>
        public static PubResponse SucceedOrFail(bool flag, object sData = null, string fmsg = "", List<string> fmsgParams = null)
        {
            return flag ?
                (sData is null ? Succeed()
                :
                Succeed(sData))
                    :
                   (fmsgParams == null ? Failed(fmsg, sData)
                        : Failed(fmsg, fmsgParams)
                     );
        }

        #region 抛弃

        //public static PubResponse Success(string msg)
        //{
        //    return new PubResponse(msg);
        //}

        //public static PubResponse Success(string msg, object data)
        //{
        //    return new PubResponse(msg, data, "success");
        //}

        //public static PubResponse Failed(string propertyName, string errorMessage)
        //{
        //    return new PubResponse(propertyName, errorMessage, "error");
        //}

        //public static PubResponse Failed(string errorMessage)
        //{
        //    return new PubResponse("error", errorMessage, "error");
        //}

        //public static PubResponse Ok(object data)
        //{
        //    return new PubResponse(data);
        //}

        #endregion 抛弃

        public PubResponse(string msg)
        {
            this.Message = msg;
            this.HasValidation = false;
            this.HasError = false;
            this.Status = "success";
        }

        public PubResponse(object data)
        {
            this.Data = data;
            this.HasValidation = false;
            this.HasError = false;
        }

        public PubResponse(string msg, object data)
        {
            this.Message = msg;
            this.Data = data;
            this.HasValidation = false;
            this.HasError = false;
        }

        public PubResponse(string msg, object data, string status)
        {
            this.Message = msg;
            this.Status = status;
            this.Data = data;
            if (status == "success")
            {
                this.HasValidation = false;
                this.HasError = false;
            }
            else
            {
                this.HasValidation = true;
                this.HasError = true;
            }
        }

        public PubResponse(Exception ex)
        {
            this.Error = new ValidationFailure("$error", ex.Message) { ErrorCode = "100" };
            this.HasValidation = true;
            this.HasError = true;
        }

        public PubResponse(IEnumerable<ValidationFailure> validations)
        {
            this._validations = validations.ToList();
            this.HasValidation = true;
            this.HasError = true;
        }

        public PubResponse(ValidationFailure validation) : this(new[] { validation })
        {
            this.Error = validation;
            this.HasValidation = true;
            this.HasError = true;
        }

        /// <summary>
        /// error方法调用
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errorMessage"></param>
        /// <param name="status"></param>
        public PubResponse(string propertyName, string errorMessage, string status, List<string> msgParams) : this(new ValidationFailure(propertyName, errorMessage))
        {
            this.Error = new ValidationFailure(propertyName, errorMessage) { ErrorCode = "100" };
            this.Message = errorMessage;
            this.Status = status;
            this._validations = ValidationFailures(this.Error).ToList();
            this.HasValidation = true;
            this.HasError = true;
            this.MessageParams = msgParams;
        }

        private IEnumerable<ValidationFailure> ValidationFailures(ValidationFailure validationFailure)
        {
            yield return validationFailure;
        }
    }
}