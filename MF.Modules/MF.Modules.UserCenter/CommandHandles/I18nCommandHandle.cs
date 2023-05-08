using DAL.MDCenter.Entities;
using DAL.MDCenter.IRepository;
using Mapster;
using MDCenter.Commands.I18n;
using MediatR;
using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCore;
using MF.NetCoreApp;
using MF.Orm;
using MF.Orm.UnitOfWork;
using MF.Utils;
using MF.Utils.Json;
using SqlSugar;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MDCenter.CommandHandles
{
    public class I18nCommandHandle : ICommandHandler,
        IRequestHandler<CreateI18nCommand, PubResponse>,
        IRequestHandler<DeleteI18nCommand, PubResponse>,
        IRequestHandler<UpdateI18nCommand, PubResponse>,
        IRequestHandler<QueryI18nCommand, PubResponse>,
        IRequestHandler<QueryListI18nCommand, PubResponse>,
        IRequestHandler<QueryPageI18nCommand, PubResponse>,
        IRequestHandler<QueryAllI18nCommand, PubResponse>,
        IRequestHandler<SetStateI18nCommand, PubResponse>,
        IRequestHandler<PublishI18nCommand, PubResponse>,
        IRequestHandler<BatchSetStateI18nCommand, PubResponse>

    {
        private readonly II18nRepository _ucI18nRepository;
        private readonly GlobalCore _globalCore;
        private readonly IUnitOfWork _unitOfWork;

        public I18nCommandHandle(
              II18nRepository ucI18nRepository
            , GlobalCore globalCore
            , IUnitOfWork unitOfWork)
        {
            _ucI18nRepository = ucI18nRepository;
            _globalCore = globalCore;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 创建 多语言配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(CreateI18nCommand cmd, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cmd.Category) || string.IsNullOrWhiteSpace(cmd.Language) || string.IsNullOrWhiteSpace(cmd.Context))
            {
                return Failed(BaseSystemError.PARAM_IS_BLANK);
            }
            var isExist = _ucI18nRepository.Queryable().Where(i => i.Category == cmd.Category && i.Language == cmd.Language).Any();
            if (isExist)
            {
                return Failed(BaseSystemError.CATEGORY_LANGUAGE_ALREADY_EXIST);
            }

            I18n ucI18n = cmd.Adapt<I18n>();
            ucI18n.Id = string.Empty;
            //var obj = cmd.Context?.ToObj<dynamic>() ?? new { };
            ////生成json路径
            //ucI18n.Path = $"{ucI18n.Category}-{ucI18n.Language}.json";
            //FileHelper.WriteJsonFile(BaseStateConstants.i18nPath, ucI18n.Path, JilH.JilToJson(obj));
            ucI18n = _ucI18nRepository.InsertReturnEntity(ucI18n);
            return Succeed(ucI18n.Id);
        }
        /// <summary>
        /// 删除 多语言配置（单个或批量）
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(DeleteI18nCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.List.Count <= 0)
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }
            //删文件
            var ucI18n = _ucI18nRepository.Queryable().Where(w => cmd.List.Contains(w.Id));
            ucI18n.ForEach(item =>
            {
                var delpath = BaseStateConstants.i18nPath + item.Path;
                if (File.Exists(delpath))
                {
                    File.Delete(delpath);
                }
            });
            return SucceedOrFail(_ucI18nRepository.Delete(w => cmd.List.Contains(w.Id)));
        }

        /// <summary>
        /// 更新 多语言配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(UpdateI18nCommand cmd, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cmd.Id))
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }
            var isExist = _ucI18nRepository.Queryable().Where(i => i.Category == cmd.Category && i.Language == cmd.Language && i.Id != cmd.Id).Any();
            if (isExist)
            {
                return Failed(BaseSystemError.CATEGORY_LANGUAGE_ALREADY_EXIST);
            }
            I18n ucI18n = _ucI18nRepository.Queryable().InSingle(cmd.Id);

            // 对象没有找到
            if (ucI18n.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            var updateObj = cmd.Adapt<I18n>();
            updateObj.Copy(ucI18n);

            var obj = cmd.Context?.ToObj<dynamic>() ?? new { };
            updateObj.Path = $"{updateObj.Category}-{updateObj.Language}.json";
            FileHelper.WriteJsonFile(BaseStateConstants.i18nPath, updateObj.Path, JilH.JilToJson(obj));

            var flag = _ucI18nRepository.UpdateEntity(updateObj);
            return SucceedOrFail(flag);
        }

        /// <summary>
        /// 发布 多语言配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(PublishI18nCommand cmd, CancellationToken cancellationToken)
        {
            var i18nList = _ucI18nRepository.Queryable()
                       .Select(i => new { i.Context, i.Category, i.Language })
                       .ToList();
            if (i18nList.IsNull() || i18nList.Count <= 0)
            {
                return Failed(BaseSystemError.NO_DATA_RELEASE);
            }
            try
            {
                i18nList.ForEach(item =>
                {
                    var obj = item.Context?.ToObj<dynamic>() ?? new { };
                    //生成json路径
                    var path = $"{item.Category}-{item.Language}.json";
                    FileHelper.WriteJsonFile(BaseStateConstants.i18nPath, path, JilH.JilToJson(obj));
                });
                //1先查找文件服务器是否存在文件
                var cdnlist = FileHelper.List_dir(cmd.NetUrl);
                //2如果存在调用api 根据md5进行删除
                if (cdnlist != null && cdnlist.Count > 0)
                {
                    cdnlist.ForEach(item =>
                    {
                        FileHelper.RemoveOnlineFile(cmd.NetUrl, item.md5);
                    });
                }
                //3将本地文件上传到服务器
                FileHelper.UploadFile(BaseStateConstants.i18nPath, cmd.NetUrl);
            }
            catch (System.Exception ex)
            {
                return Failed(ex.Message);
            }

            return Succeed();
        }

        /// <summary>
        /// 根据Id查询 多语言配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryI18nCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.NotNull())
            {
                var ucI18n = _ucI18nRepository.Queryable()
                    .Where(i => i.Id == cmd.Id)
                    .Select(i => new { i.Id, i.Context, i.Category, i.Language, i.Path, i.Code, i.Name, i.Remark })
                    .First();
                cmd = ucI18n.Adapt<QueryI18nCommand>();
                cmd.Context = ucI18n.Context.IsEmpty() ? "{}" : ucI18n.Context;
                //var json = FileHelper.GetJsonFile(SystemConstants.i18nPath + cmd.Path);
                //cmd.Context = json;
            }
            return Succeed(cmd);
        }

        /// <summary>
        /// 根据Id列表查询 多语言配置列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryListI18nCommand cmd, CancellationToken cancellationToken)
        {
            var list = new List<QueryAllI18nCommand>();
            if (cmd.List.NotNull())
            {
                var ucI18ns = _ucI18nRepository.Queryable().In(cmd.List).ToList();
                list = ucI18ns.Adapt<List<QueryAllI18nCommand>>();
            }
            return Succeed(list);
        }

        /// <summary>
        /// 分页查询 多语言配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPageI18nCommand cmd, CancellationToken cancellationToken)
        {
            var totalCount = 0;
            var list = _unitOfWork.GetDbClient().Queryable<I18n>()
                                .WhereIF(cmd.Condition.NotNull(), cmd.Condition)
                                .Where(i => i.State != BaseStateConstants.DELETE)
                                .OrderBy(i => i.CreateTime, OrderByType.Desc)
                                .ToPageList(cmd.PageNum, cmd.PageSize, ref totalCount);
            return Succeed(new { List = list, total = totalCount });
        }

        /// <summary>
        /// 查询所有 多语言配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryAllI18nCommand cmd, CancellationToken cancellationToken)
        {
            var list = _ucI18nRepository.QueryAll();
            var all = list.Adapt<List<QueryAllI18nCommand>>();
            return Succeed(all);
        }

        /// <summary>
        /// 修改 多语言配置 状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(SetStateI18nCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.NotNull())
            {
                var ucI18n = _ucI18nRepository.Queryable().InSingle(cmd.Id);

                if (ucI18n.NotNull())
                {

                    if (ucI18n.State.Equals(BaseStateConstants.ACTIVATE))
                    {
                        ucI18n.State = BaseStateConstants.DEACTIVE;
                    }
                    else if (ucI18n.State.Equals(BaseStateConstants.DEACTIVE))
                    {
                        ucI18n.State = BaseStateConstants.ACTIVATE;
                    }

                    var flag = _ucI18nRepository.UpdateEntity(ucI18n);
                    return SucceedOrFail(flag);

                }
                else
                {
                    return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }
            return Succeed();
        }

        /// <summary>
        /// 批量修改 多语言配置 状态
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        public Task<PubResponse> Handle(BatchSetStateI18nCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.List.NotNullT())
            {
                cmd.List = cmd.List.Distinct().ToList();
                var ucI18ns = _ucI18nRepository.Queryable().In(cmd.List).ToList();
                if (ucI18ns.Count != cmd.List.Count)
                {
                    ThrowError(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }

                if (ucI18ns.NotNullT())
                {
                    foreach (var item in ucI18ns)
                    {
                        if (item.State.Equals(BaseStateConstants.ACTIVATE))
                        {
                            item.State = BaseStateConstants.DEACTIVE;
                        }
                        else if (item.State.Equals(BaseStateConstants.DEACTIVE))
                        {
                            item.State = BaseStateConstants.ACTIVATE;
                        }
                    }

                    if (!_ucI18nRepository.Update(ucI18ns))
                    {
                        ThrowError(BaseSystemError.STATE_UPDATE_ERROR);
                    }
                }
                else
                {
                    ThrowError(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }
            return Succeed();
        }

    }
}