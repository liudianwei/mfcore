using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MF.FluentValidation
{
    public abstract class PubResponseResult
    {
        protected virtual Task<PubResponse> Result(bool flag)
        {
            return null;
            //return flag ? Task.FromResult(PubResponse.Success()) : Task.FromResult(new PubResponse("error", "操作失败"));
        }

        protected virtual Task<PubResponse> Result(bool flag, string success, string error)
        {
            return null;
            //return flag ? Task.FromResult(PubResponse.Success(success)) : Task.FromResult(new PubResponse("error", error));
        }
    }
}