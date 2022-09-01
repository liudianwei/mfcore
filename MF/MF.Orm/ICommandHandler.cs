using MF.Core.Extensions;
using MF.FluentValidation;
using MF.Orm.Repository;
using MF.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MF.Orm
{
    public abstract class ICommandHandler
    {
        protected Task<PubResponse> Succeed()
        {
            return PubResponse.Succeed().ToAsync();
        }

        protected Task<PubResponse> Succeed(object data)
        {
            return PubResponse.Succeed(data).ToAsync();
        }

        protected Task<PubResponse> Failed()
        {
            return PubResponse.Failed().ToAsync();
        }

        protected Task<PubResponse> Failed(string msg, object data)
        {
            return PubResponse.Failed(msg, data).ToAsync();
        }

        protected Task<PubResponse> Failed(string msg, List<string> msgParams = null)
        {
            return PubResponse.Failed(msg, msgParams).ToAsync();
        }

        protected Task<PubResponse> SucceedOrFail(bool flag, object sData = null, string fmsg = "", List<string> fmsgParams = null)
        {
            return PubResponse.SucceedOrFail(flag, sData, fmsg, fmsgParams).ToAsync();
        }

        protected void ThrowError(string errmsg)
        {
            throw new Exception(errmsg);
        }


        protected Task<PubResponse> EnabledStateus<T>(List<string> ids, string state, IBaseRepository<T> Trepository) where T : BaseEntity, new()
        {
            if (state.IsNull())
            {
                return Failed(BaseSystemError.BATCH_STATE_NULL);
            }
            if (ids.Count == 0)
            {
                return Failed(BaseSystemError.BATCH_STATE_DATA_NULL);
            }
            List<T> list = new List<T>();
            var stateL = Trepository.Queryable().Where(i => ids.Contains(i.Id)).ToList();
            if (stateL.Count != ids.Count) return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);

            foreach (var item in stateL)
            {
                if (item.State == BaseStateConstants.DELETE)
                {
                    continue;
                }

                if (item.State == BaseStateConstants.ACTIVATE)
                {
                    item.State = BaseStateConstants.DEACTIVE;
                }
                else if (item.State == BaseStateConstants.DEACTIVE)
                {
                    item.State = BaseStateConstants.ACTIVATE;
                }
                else
                {
                    continue;
                }

                list.Add(item);
            }
            var flag = Trepository.Update(list);
            return SucceedOrFail(flag);
        }

        protected void CheckResult(Task<PubResponse> result)
        {
            if (!result.Result.Status.Equals("success"))
            {
                ThrowError(result.Result.Message);
            }
        }
    }
}