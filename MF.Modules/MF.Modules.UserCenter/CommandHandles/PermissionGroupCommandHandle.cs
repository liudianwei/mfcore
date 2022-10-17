using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using UserCenter.Commands;
using UserCenter.Dtos;
using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCoreApp;
using MF.Orm;
using MF.Utils;
using UserCenter.Enums;

namespace UserCenter.CommandHandles
{
    public class PermissionGroupCommandHandle : ICommandHandler,
        IRequestHandler<CreatePermissionGroupCommand, PubResponse>,
        IRequestHandler<UpdatePermissionGroupCommand, PubResponse>,
        IRequestHandler<QueryPermissionGroupCommand, PubResponse>,
        IRequestHandler<DeletePermissionGroupCommand, PubResponse>,
        IRequestHandler<AssignPermissionPermissionGroupCommand, PubResponse>,
        IRequestHandler<QueryAllPermissionGroupCommand, PubResponse>,
        IRequestHandler<QueryByNamePermissionGroupCommand, PubResponse>,
        IRequestHandler<QueryPagePermissionGroupCommand, PubResponse>,
        IRequestHandler<QueryPermissionByIdPermissionGroupCommand, PubResponse>
    {
        private readonly IPermissiongroupRepository _permissiongroupRepository;
        private readonly IUcPermissionRepository _permissionRepository;
        private readonly IPermissionPermissiongroupRepository _permissionPermissiongroupRepository;
        private readonly GlobalCore _globalCore;

        public PermissionGroupCommandHandle(GlobalCore globalCore
            , IPermissiongroupRepository permissiongroupRepository
            , IUcPermissionRepository permissionRepository
            , IPermissionPermissiongroupRepository permissionPermissiongroupRepository)
        {
            _globalCore = globalCore;
            _permissiongroupRepository = permissiongroupRepository;
            _permissionRepository = permissionRepository;
            _permissionPermissiongroupRepository = permissionPermissiongroupRepository;
        }

        /// <summary>
        /// 创建权限组
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(CreatePermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            PermissionGroupDto dto = cmd.Dto;

            if (dto.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }
            Permissiongroup permissionGroup = FindModelByName(dto.Name);
            if (permissionGroup.NotNull())
            {
                return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
            }
            permissionGroup = new Permissiongroup
            {
                Name = dto.Name,
                Code = dto.Code,
                Remark = dto.Remark,
                Updator = dto.Updator
            };

            permissionGroup = _permissiongroupRepository.InsertReturnEntity(permissionGroup);
            return Succeed(permissionGroup.Id);
        }

        private Permissiongroup FindModelByName(string name)
        {
            Permissiongroup permissionGroup = _permissiongroupRepository.Queryable().First(pg => pg.Name.Equals(name));
            if (permissionGroup != null)
            {
                return permissionGroup;
            }
            return null;
        }

        /// <summary>
        /// 更新权限组
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(UpdatePermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.IsNull())
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }
            if (cmd.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }
            Permissiongroup permissionGroup = GetExistPermissionGroup(cmd.Id);
            if (permissionGroup.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            if (!permissionGroup.Name.Equals(cmd.Name))
            {
                Permissiongroup permission = FindModelByName(cmd.Name);
                if (permission.NotNull())
                {
                    return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
                }
            }
            permissionGroup.Name = cmd.Name;
            permissionGroup.Code = cmd.Code;
            permissionGroup.Remark = cmd.Remark;
            permissionGroup.Updator = cmd.Updator;

            var flag = _permissiongroupRepository.UpdateEntity(permissionGroup);

            return SucceedOrFail(flag, permissionGroup.Id);
        }

        /// <summary>
        /// 根据id获取权限组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Permissiongroup GetExistPermissionGroup(string id)
        {
            if (id.IsNull())
            {
                return null;
            }
            Permissiongroup permissionGroup = _permissiongroupRepository.QueryableToEntity(pg => pg.Id.Equals(id));
            return permissionGroup;
        }

        /// <summary>
        /// 根据id查询权限组
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.IsNull())
            {
                Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }
            var permissionGroup = GetExistPermissionGroup(cmd.Id);
            return Succeed(permissionGroup);
        }

        /// <summary>
        /// 删除权限组
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(DeletePermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            List<string> ubdellist = new List<string>();
            List<Permissiongroup> list = _permissiongroupRepository.QueryableToList(pg => cmd.List.Contains(pg.Id));
            if (list.NotNullT())
            {
                List<string> ids = list.Select(pg => pg.Id).ToList();
                List<PermissionPermissiongroup> pgps = _permissionPermissiongroupRepository
                    .QueryableToList(pgp => ids.Contains(pgp.PermissiongroupId));
                var groups = pgps.GroupBy(pgp => pgp.PermissiongroupId).ToDictionary(g => g.Key, g => g.ToList());

                foreach (var id in ids)
                {
                    if (groups.TryGetValue(id, out var obj))
                    {
                        ubdellist.Add(id);
                    }
                    else
                    {
                        if (!_permissiongroupRepository.Delete(pg => pg.Id.Equals(id)))
                        {
                            ubdellist.Add(id);
                        }
                    }
                }
            }
            return Succeed(ubdellist);
        }

        /// <summary>
        /// 关联权限到权限组
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(AssignPermissionPermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            Permissiongroup permissionGroup = GetExistPermissionGroup(cmd.Id);
            if (permissionGroup.IsNull())
            {
                return Failed(UserCenterError.PERMISSION_NOT_FOUND);
            }
            if (cmd.plist.IsNullLt())
            {
                return Failed(UserCenterError.PERMISSION_ID_LIST_IS_EMPTY);
            }
            var permissions = _permissionRepository.QueryableToList(p => cmd.plist.Contains(p.Id));
            if (permissions.IsNullLt())
            {
                return Failed(UserCenterError.PERMISSION_LIST_NOT_FOUND);
            }
            if (cmd.plist.Count != permissions.Count)
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            List<PermissionPermissiongroup> links = new List<PermissionPermissiongroup>();
            if (_permissionPermissiongroupRepository.Delete(pgp => pgp.PermissiongroupId.Equals(cmd.Id)))
            {
                foreach (var pgId in cmd.plist)
                {
                    PermissionPermissiongroup pgp = new PermissionPermissiongroup
                    {
                        PermissiongroupId = cmd.Id,
                        PermissionId = pgId,
                        Updator = cmd.Updator
                    };
                    pgp = _permissionPermissiongroupRepository.InsertReturnEntity(pgp);
                    if (pgp.NotNull())
                    {
                        links.Add(pgp);
                    }
                }
            }

            bool flag = links.Count == cmd.plist.Count;
            return SucceedOrFail(flag, permissionGroup);
        }

        /// <summary>
        /// 查询所有的权限组
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryAllPermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            var list = _permissiongroupRepository.Queryable().ToList();
            return Succeed(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryByNamePermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            var permissionGroup = _permissiongroupRepository.Queryable().First(pg => pg.Name.Equals(cmd.Name));
            bool flag = permissionGroup.NotNull();
            return SucceedOrFail(flag, permissionGroup);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPagePermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            int totalNumber = 0;
            List<Permissiongroup> result = _permissiongroupRepository.Queryable()
                .WhereIF(cmd.Condition.NotNull(), cmd.Condition)
                .OrderBy(x=>x.CreateTime,SqlSugar.OrderByType.Desc)
                .ToPageList(cmd.PageNum, cmd.PageSize, ref totalNumber);
            return Succeed(new { list = result, total = totalNumber });
        }

        /// <summary>
        /// 根据权限组id获取所有关联的权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPermissionByIdPermissionGroupCommand cmd, CancellationToken cancellationToken)
        {
            var pgps = _permissionPermissiongroupRepository.QueryableToList(pgp => pgp.Id.Equals(cmd.Id));
            var pids = pgps.Select(pgp => pgp.PermissionId).ToList();
            var permissions = _permissionRepository.QueryableToList(p => pids.Contains(p.Id));
            bool flag = permissions.Count == pids.Count;
            return SucceedOrFail(flag, permissions);
        }
    }
}