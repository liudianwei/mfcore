using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;
using Mapster;
using MediatR;
using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCore;
using MF.NetCoreApp;
using MF.Orm;
using MF.Orm.UnitOfWork;
using MF.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserCenter.Commands;
using UserCenter.Dtos;
using UserCenter.Enums;

namespace UserCenter.CommandHandles
{
    public class PermissionCommandHandle : ICommandHandler,
        IRequestHandler<CreatePermissionCommand, PubResponse>,
        IRequestHandler<UpdatePermissionCommand, PubResponse>,
        IRequestHandler<QueryPermissionCommand, PubResponse>,
        IRequestHandler<DeletePermissionCommand, PubResponse>,
        IRequestHandler<BatchPermissionCommand, PubResponse>,
        IRequestHandler<QueryButtonByPermissionGroupNameCommand, PubResponse>,
        IRequestHandler<QueryMenuByPermissionGroupNameCommand, PubResponse>,
        IRequestHandler<QueryPermissionTreeCommand, PubResponse>,
        IRequestHandler<QueryPermissionTreeByRoleCommand, PubResponse>,
        IRequestHandler<QueryListPermissionCommand, PubResponse>
    {
        private readonly IUcPermissionRepository _permissionRepository;
        private readonly IPermissionPermissiongroupRepository _permissionPermissiongroupRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly GlobalCore _globalCore;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionCommandHandle(GlobalCore globalCore
            , IUcPermissionRepository permissionRepository
            , IPermissionPermissiongroupRepository permissionPermissiongroupRepository
            , IRolePermissionRepository rolePermissionRepository
            , IUnitOfWork unitOfWork)
        {
            _globalCore = globalCore;
            _permissionRepository = permissionRepository;
            _permissionPermissiongroupRepository = permissionPermissiongroupRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(CreatePermissionCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Dto.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_CANNOT_BE_NULL);
            }
            PermissionDto dto = cmd.Dto;
            var permission = _permissionRepository.Queryable().First(p => p.Name == dto.Name && p.Type != PermissionType.BUTTON);

            // 判断权限是否存在
            if (permission.NotNull())
            {
                return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
            }

            // 如果父id不为空 查询父权限
            if (dto.ParentId.NotNull())
            {
                var parent = _permissionRepository.QueryableToEntity(p => p.Id == dto.ParentId);
                if (parent.IsNull())
                {
                    return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }

            // 权限组不能为空
            if (dto.PermissionGroupIdList.IsNull() && dto.PermissionGroupIdList.IsNullT())
            {
                return Failed(BaseSystemError.OBJECT_CANNOT_BE_NULL);
            }

            // 创建权限对象
            permission = new Permission
            {
                Name = dto.Name,
                Code = dto.Code,
                ParentId = dto.ParentId,
                Url = dto.Url,
                OrderNum = dto.OrderNum,
                Perms = dto.Perms,
                Type = dto.Type,
                Icon = dto.Icon,
                ApiUrl = dto.ApiUrl,
                IframeUrl = dto.IframeUrl,
                Remark = dto.Remark,
                Updator = dto.Updator
            };

            // 保存权限
            permission = _permissionRepository.InsertReturnEntity(permission);

            // 分配权限和权限组的关系
            var flag = AssignPermissionGroup(permission, dto.PermissionGroupIdList, dto.Updator);
            return SucceedOrFail(flag, permission.Id, UserCenterError.ASSIGN_PERMISSIO_ERROR);
        }

        /// <summary>
        /// 批量新增权限
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="allpermissions"></param>
        /// <returns></returns>
        private Task<PubResponse> Add(List<PermissionDto> dtos, List<Permission> allpermissions)
        {
            if (dtos.IsNullT())
            {
                return Failed(BaseSystemError.OBJECT_CANNOT_BE_NULL);
            }

            var parentId = dtos[0].ParentId;
            // 根据父id和名称查询权限
            var names = dtos.Select(i => i.Name).ToList();
            var permissions = allpermissions.Where(p => p.ParentId == parentId && names.Contains(p.Name)).ToList();

            // 判断权限是否存在
            if (permissions.NotNullT())
            {
                return Failed(UserCenterError.BUTTON_NAME_ALREADY_EXIST);
            }

            // 如果父id不为空 查询父权限
            if (parentId.NotNull())
            {
                var parent = allpermissions.Where(p => p.Id == parentId).ToList();
                if (parent.IsNull())
                {
                    return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }

            // 权限组不能为空
            foreach (var dto in dtos)
            {
                if (dto.PermissionGroupIdList.IsNull() && dto.PermissionGroupIdList.IsNullT())
                {
                    return Failed(BaseSystemError.OBJECT_CANNOT_BE_NULL);
                }
            }

            bool flag = false;
            List<Permission> adds = new List<Permission>();
            var dict = new Dictionary<string, PermissionDto>();
            foreach (var dto in dtos)
            {
                // 创建权限对象
                adds.Add(new Permission
                {
                    Name = dto.Name,
                    Code = dto.Code,
                    ParentId = dto.ParentId,
                    Url = dto.Url,
                    IframeUrl = dto.IframeUrl,
                    OrderNum = dto.OrderNum,
                    Perms = dto.Perms,
                    Type = dto.Type,
                    Icon = dto.Icon,
                    ApiUrl = dto.ApiUrl,
                    Remark = dto.Remark,
                    Updator = dto.Updator
                });
                dict.Add(dto.Name + ":" + dto.Code, dto);
            }
            // 批量插入权限
            _permissionRepository.Insert(adds);

            // 分配权限和权限组的关系
            foreach (var permission in adds)
            {
                if (dict.TryGetValue(permission.Name + ":" + permission.Code, out var a))
                {
                    a.Id = permission.Id;
                }
            }

            flag = AssignPermissionGroup(dtos);

            return SucceedOrFail(flag, "", UserCenterError.ASSIGN_PERMISSIO_ERROR);
        }

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="permissionGroupIds"></param>
        /// <param name="updator"></param>
        /// <returns></returns>
        private bool AssignPermissionGroup(Permission permission, List<string> permissionGroupIds, string updator)
        {
            //删除和权限组的关联记录
            _permissionPermissiongroupRepository.Delete(pgp => pgp.PermissionId == permission.Id, false);

            List<PermissionPermissiongroup> list = new List<PermissionPermissiongroup>();
            foreach (var permissionGroupId in permissionGroupIds)
            {
                PermissionPermissiongroup permissionPermissiongroup = new PermissionPermissiongroup
                {
                    PermissiongroupId = permissionGroupId,
                    PermissionId = permission.Id,
                    Updator = updator
                };
                list.Add(permissionPermissiongroup);
            }

            //插入关联记录
            return _permissionPermissiongroupRepository.Insert(list);
        }

        /// <summary>
        /// 批量分配权限
        /// </summary>
        /// <param name="listdto"></param>
        /// <returns></returns>
        private bool AssignPermissionGroup(List<PermissionDto> listdto)
        {
            var ids = listdto.Select(i => i.Id).ToList();
            //批量删除和权限组的关联记录
            _permissionPermissiongroupRepository.Delete(pgp => ids.Contains(pgp.PermissionId), false);

            List<PermissionPermissiongroup> list = new List<PermissionPermissiongroup>();
            foreach (var item in listdto)
            {
                foreach (var permissionGroupId in item.PermissionGroupIdList)
                {
                    PermissionPermissiongroup permissionPermissiongroup = new PermissionPermissiongroup
                    {
                        PermissiongroupId = permissionGroupId,
                        PermissionId = item.Id,
                        Updator = item.Updator
                    };
                    list.Add(permissionPermissiongroup);
                }
            }
            //批量插入关联记录
            return _permissionPermissiongroupRepository.Insert(list);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(UpdatePermissionCommand cmd, CancellationToken cancellationToken)
        {
            PermissionDto dto = cmd.Dto;
            Task<PubResponse> _task = GetExistPermission(cmd.Id);
            if (_task.Result.Status != ResultStatusConstants.SUCCESS)
            {
                return Failed(_task.Result.Message);
            }
            Permission permission = (Permission)_task.Result.Data;

            if (dto.PermissionGroupIdList.IsNullT())
            {
                return Failed(BaseSystemError.OBJECT_CANNOT_BE_NULL);
            }

            //父id不能是自己
            if (dto.ParentId.Equals(permission.Id))
            {
                return Failed(BaseSystemError.PARENT_ID_IS_SELF);
            }
            //权限名称不能空
            if (dto.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }
            //名称发生修改 不能和别人重名
            if (!dto.Name.Equals(permission.Name))
            {
                var permissionIndb = _permissionRepository.Queryable().First(p => p.Id != dto.Id && p.Name == dto.Name && p.Type != PermissionType.BUTTON);
                if (permissionIndb.NotNull())
                {
                    return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
                }
            }

            if (!permission.ParentId.Equals(dto.ParentId))
            {
                if (dto.ParentId.NotNull())
                {
                    Task<PubResponse> _task1 = GetExistPermission(dto.ParentId);
                    if (_task1.Result.Status != ResultStatusConstants.SUCCESS)
                    {
                        return Failed(_task.Result.Message);
                    }
                }
                else
                {
                    //权限如果已经被分配给角色 就不允许修改权限的上级菜单
                    if (_rolePermissionRepository.Queryable().Any(p => p.PermissionId == dto.Id))
                    {
                        return Failed(UserCenterError.PERMISSION_ALREADY_ASSIGN);
                    }
                }
            }

            //List<Permission> children = new List<Permission>();
            //var allpermission = _permissionRepository.QueryAll();
            //FindAllChildNode(children, permission.Id, allpermission);

            //foreach (var p in children)
            //{
            //    if (p.Id.Equals(dto.ParentId))
            //    {
            //        return Failed(BaseSystemError.PARENT_ID_IS_CHILDREN);
            //    }
            //}

            permission.Name = dto.Name;
            permission.Code = dto.Code;

            if (permission.ParentId.IsNull() && dto.ParentId.NotNull())
            {
                permission.Type = PermissionType.MENU;
            }
            else if (permission.ParentId.NotNull() && dto.ParentId.IsNull())
            {
                permission.Type = PermissionType.CATALOG;
            }
            else
            {
                permission.Type = dto.Type;
            }

            permission.ParentId = dto.ParentId;
            permission.Url = dto.Url;
            permission.IframeUrl = dto.IframeUrl;
            permission.OrderNum = dto.OrderNum;
            permission.Perms = dto.Perms;

            permission.Icon = dto.Icon;
            permission.ApiUrl = dto.ApiUrl;
            permission.Remark = dto.Remark;
            permission.Updator = dto.Updator;

            if (_permissionRepository.UpdateEntity(permission))
            {
                if (!AssignPermissionGroup(permission, dto.PermissionGroupIdList, dto.Updator))
                {
                    return Failed(UserCenterError.ASSIGN_PERMISSIO_ERROR);
                }
            }

            //TODO 重新设置缓存
            return Succeed(permission.Id);
        }

        /// <summary>
        /// 批量更新权限
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="allpermissions"></param>
        /// <returns></returns>
        private Task<PubResponse> Update(List<PermissionDto> dtos, List<Permission> allpermissions)
        {
            foreach (var dto in dtos)
            {
                if (dto.PermissionGroupIdList.IsNullT())
                {
                    return Failed(BaseSystemError.OBJECT_CANNOT_BE_NULL);
                }
                //权限名称不能空
                if (dto.Name.IsNull())
                {
                    return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
                }
                //父id不能是自己
                if (dto.ParentId.Equals(dto.Id))
                {
                    return Failed(BaseSystemError.PARENT_ID_IS_SELF);
                }
            }

            var ids = dtos.Select(i => i.Id).ToList();
            var editList = allpermissions.Where(i => ids.Contains(i.Id)).ToList();//获取所有需要编辑的权限

            if (editList.Count != ids.Count)
            {
                return Failed(UserCenterError.PERMISSION_NOT_FOUND);//肯定有没查询到的
            }
            var id2objdict = editList.ToDictionary(i => i.Id, i => i);//转字典

            var modifyParentIdPermissions = new List<string>();//修改过上级的权限id列表
            foreach (var dto in dtos)
            {
                if (id2objdict.TryGetValue(dto.Id, out var permission))
                {
                    //如果父级id改变
                    if (!dto.ParentId.Equals(permission.ParentId))
                    {
                        if (dto.ParentId.NotNull())//父级菜单存在
                        {
                            if (!allpermissions.Any(i => i.Id== dto.ParentId))
                            {
                                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                            }
                        }
                        else
                        {
                            modifyParentIdPermissions.Add(dto.Id);
                        }
                    }
                }
            }

            //权限如果已经被分配给角色 就不允许修改权限的上级菜单
            if (_rolePermissionRepository.Queryable().Any(p => modifyParentIdPermissions.Contains(p.PermissionId)))
            {
                return Failed(UserCenterError.PERMISSION_ALREADY_ASSIGN);
            }

            List<Permission> children = new List<Permission>();
            foreach (var dto in dtos)
            {
                if (id2objdict.TryGetValue(dto.Id, out var permission))
                {
                    // 如果名称发生改变 需要判断和兄弟节点是否重名
                    if (!dto.Name.Equals(permission.Name))
                    {
                        if (allpermissions.Any(p => p.Id != dto.Id && p.ParentId == dto.ParentId && p.Name == dto.Name))
                        {
                            return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);//同名的兄弟权限已经存在
                        }
                    }
                }
                else
                {
                    return Failed(UserCenterError.PERMISSION_NOT_FOUND);
                }

                //FindAllChildNode(children, dto.Id, allpermissions);//查询子权限
                //foreach (var p in children)
                //{
                //    if (p.Id.Equals(dto.ParentId))// 如果子孙节点的id等于父id 提示错误信息
                //    {
                //        return Failed(BaseSystemError.PARENT_ID_IS_CHILDREN);
                //    }
                //}

                permission.Name = dto.Name;
                permission.Code = dto.Code;

                if (permission.ParentId.IsNull() && dto.ParentId.NotNull())
                {
                    //如果原来节点是顶级 编辑后不是 该权限就改成菜单类型
                    permission.Type = PermissionType.MENU;
                }
                else if (permission.ParentId.NotNull() && dto.ParentId.IsNull())
                {
                    //如果原来节点不是是顶级 编辑后是 该强行就改成分类类型
                    permission.Type = PermissionType.CATALOG;
                }
                else
                {
                    permission.Type = dto.Type;
                }

                permission.ParentId = dto.ParentId;
                permission.Url = dto.Url;
                permission.IframeUrl = dto.IframeUrl;
                permission.OrderNum = dto.OrderNum;
                permission.Perms = dto.Perms;

                permission.Icon = dto.Icon;
                permission.ApiUrl = dto.ApiUrl;
                permission.Remark = dto.Remark;
                permission.Updator = dto.Updator;
            }

            if (_permissionRepository.Update(editList))
            {
                if (!AssignPermissionGroup(dtos))
                {
                    return Failed(UserCenterError.ASSIGN_PERMISSIO_ERROR);
                }
            }

            return Succeed();
        }

        /// <summary>
        /// 根据id获取所有子孙权限列表
        /// </summary>
        /// <param name="children"></param>
        /// <param name="parentId"></param>
        /// <param name="allpermission"></param>
        private void FindAllChildNode(List<Permission> children, string parentId, List<Permission> allpermission)
        {
            List<Permission> permissionList = allpermission.Where(p => p.ParentId == parentId).ToList();
            if (permissionList.NotNullT())
            {
                children.AddRange(permissionList);
                permissionList.ForEach(permission => FindAllChildNode(children, permission.Id, allpermission));
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(DeletePermissionCommand cmd, CancellationToken cancellationToken)
        {
            Task<PubResponse> _task = GetExistPermission(cmd.Id);
            if (_task.Result.Status != ResultStatusConstants.SUCCESS)
            {
                return Failed(_task.Result.Message);
            }

            Permission permission = (Permission)_task.Result.Data;
            List<RolePermission> linkList = _rolePermissionRepository.QueryableToList(rp => rp.PermissionId == permission.Id);

            if (linkList.NotNullT())
            {
                return Failed(UserCenterError.PERMISSION_ALREADY_ASSIGN);
            }

            List<Permission> permissions = _permissionRepository.QueryableToList(p => p.ParentId == cmd.Id);

            if (permissions.NotNullT())
            {
                return Failed(UserCenterError.SUB_PERMISSION_NOT_EMPTY);
            }

            bool flag = _permissionRepository.Delete(p => p.Id == cmd.Id);

            return SucceedOrFail(flag, cmd.Id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="allpermissions"></param>
        /// <returns></returns>
        private Task<PubResponse> Delete(List<string> ids, List<Permission> allpermissions)
        {
            ids = ids.Distinct().ToList();
            if (ids.Count <= 0)
            {
                return Succeed();
            }
            bool flag = false;

            flag = _rolePermissionRepository.IsAny(rp => ids.Contains(rp.PermissionId));
            if (flag)
            {
                return Failed(UserCenterError.PERMISSION_ALREADY_ASSIGN);//有关联关系
            }

            var count = allpermissions.Count(i => ids.Contains(i.Id));
            if (count != ids.Count)
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);//数量不一致
            }

            flag = allpermissions.Any(i => ids.Contains(i.ParentId));
            if (flag)
            {
                return Failed(UserCenterError.SUB_PERMISSION_NOT_EMPTY);//有子权限 不能删除
            }

            flag = _permissionRepository.Delete(p => ids.Contains(p.Id));
            flag = flag ? _permissionPermissiongroupRepository.Delete(p => ids.Contains(p.PermissionId)) : false;

            return SucceedOrFail(flag, ids);
        }

        /// <summary>
        /// 根据id获取权限记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        private Task<PubResponse> GetExistPermission(string id)
        {
            if (id.IsNull())
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }

            var permission = _permissionRepository.Queryable().InSingle(id);

            return SucceedOrFail(permission.NotNull(), permission, BaseSystemError.OBJECT_DOES_NOT_EXIST);
        }

        /// <summary>
        /// 根据id查询权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPermissionCommand cmd, CancellationToken cancellationToken)
        {
            return GetExistPermission(cmd.Id);
        }

        /// <summary>
        /// 按钮 批量操作权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        public Task<PubResponse> Handle(BatchPermissionCommand cmd, CancellationToken cancellationToken)
        {
            cmd.List.ForEach(p =>
            {
                p.ParentId = cmd.Id;
            });

            var uid = _globalCore.UserId;
            var db = _unitOfWork.GetDbClient();
            string[] oldArr = new string[] { };
            var subpermissions = _permissionRepository.Queryable().Where(i => i.ParentId == cmd.Id).ToList();
            if (!uid.Equals(SystemConstants.superId))
            {
                var ss1 = db.Queryable<RolePermission, RoleUser>((a, b) => new JoinQueryInfos(
                        JoinType.Left, a.RoleId == b.RoleId
                    ))
                    .Where((a, b) => b.UserId == uid && a.State != BaseStateConstants.DELETE && b.State != BaseStateConstants.DELETE)
                    .Select(a => a.PermissionId)
                    .ToArray();

                oldArr = subpermissions.Where(it => ss1.Contains(it.Id) || it.Creator == _globalCore.UserConcatName).Select(it => it.Id).ToArray();
            }
            else
            {
                oldArr = subpermissions.Select(i => i.Id).ToArray();// 数据库里的id
            }

            //var allpermissions = _permissionRepository.Queryable().WhereIF(!uid.Equals(SystemConstants.superId), it =>
            //    it.Id == SqlFunc.Subqueryable<RolePermission>().Where(s1 => s1.RoleId.Equals(SqlFunc.Subqueryable<RoleUser>().Where(s => s.UserId.Equals(uid)).Select(s => s.RoleId))).Select(s2 => s2.Id)
            //   ).Where(i => i.ParentId.Equals(cmd.Id)).ToList();



            //var oldArr = allpermissions.Select(i => i.Id).ToArray();// 数据库里的id
            var newArr = cmd.List.Select(i => i.Id).ToArray();// 接口传来的id

            var operas = ArrayExt.GetOperationArray(oldArr, newArr);// 获取删除，新增，修改的id

            //// 新增权限操作
            var createIds = operas.Item2.ToList();
            var createList = cmd.List.Where(i => createIds.Contains(i.Id)).ToList();//过滤出
            if (createList.NotNullLt())
            {
                var createResult = Add(createList, subpermissions);
                CheckResult(createResult);
            }

            //// 修改操作
            var updateIds = operas.Item3.ToList();
            var updateList = cmd.List.Where(i => updateIds.Contains(i.Id)).ToList();
            if (updateList.NotNullLt())
            {
                var updateResult = Update(updateList, subpermissions);
                CheckResult(updateResult);
            }

            // 删除操作
            var deleteList = operas.Item1.ToList();
            if (deleteList.NotNullLt())
            {
                var deleteResult = Delete(deleteList, subpermissions);
                CheckResult(deleteResult);
            }
            return Succeed();
        }

        /// <summary>
        /// 根据权限组名称查询按钮权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryButtonByPermissionGroupNameCommand cmd, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db
                .Queryable<Permission, RolePermission, Role, RoleUser, User, PermissionPermissiongroup, Permissiongroup>
                ((p, rp, r, ru, u, pgp, gp) => new JoinQueryInfos(
                    JoinType.Left, rp.PermissionId == p.Id,
                    JoinType.Left, r.Id == rp.RoleId,
                    JoinType.Left, ru.RoleId == r.Id,
                    JoinType.Left, u.Id == ru.UserId,
                    JoinType.Left, pgp.PermissionId == p.Id,
                    JoinType.Left, gp.Id == pgp.PermissiongroupId
                ))
                .Where((p, rp, r, ru, u, pgp, gp) => p.State != BaseStateConstants.DELETE)
                .WhereIF(_globalCore.UserId != SystemConstants.superId, (p, rp, r, ru, u, pgp, gp) => u.Id == _globalCore.UserId && rp.State == BaseStateConstants.ACTIVATE)
                .Where((p, rp, r, ru, u, pgp, gp) => p.Type == PermissionType.BUTTON)
                .Where((p, rp, r, ru, u, pgp, gp) => gp.Name == cmd.PermissionGroupName)
                .OrderBy((p, rp, r, ru, u, pgp, gp) => p.CreateTime, OrderByType.Desc)
                .Select((p, rp, r, ru, u, pgp, gp) => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    ParentId = p.ParentId,
                    ApiUrl = p.ApiUrl,
                    OrderNum = p.OrderNum,
                    Url = p.Url,
                    IframeUrl = p.IframeUrl,
                    Type = p.Type,
                    Icon = p.Icon,
                    CreateTime = p.CreateTime,
                    Perms = p.Perms
                });
            var list = query.ToList();
            list = list.Distinct(new PermissionDtoComparer<PermissionDto>()).ToList();
            return Succeed(list);
        }

        /// <summary>
        /// 根据权限组名称查询菜单权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryMenuByPermissionGroupNameCommand cmd, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db
            .Queryable<Permission, RolePermission, Role, RoleUser, User, PermissionPermissiongroup, Permissiongroup>
            ((p, rp, r, ru, u, pgp, gp) => new JoinQueryInfos(
                JoinType.Left, rp.PermissionId == p.Id,
                JoinType.Left, r.Id == rp.RoleId && rp.State == BaseStateConstants.ACTIVATE,
                JoinType.Left, ru.RoleId == r.Id,
                JoinType.Left, u.Id == ru.UserId,
                JoinType.Left, pgp.PermissionId == p.Id,
                JoinType.Left, gp.Id == pgp.PermissiongroupId
            ))
            .WhereIF(_globalCore.UserId != SystemConstants.superId, (p, rp, r, ru, u, pgp, gp) => u.Id == _globalCore.UserId)
            .Where((p, rp, r, ru, u, pgp, gp) => p.Type == PermissionType.MENU || p.Type == PermissionType.CATALOG)
            .Where((p, rp, r, ru, u, pgp, gp) => gp.Name == cmd.PermissionGroupName)
            .Where((p, rp, r, ru, u, pgp, gp) => p.State == BaseStateConstants.ACTIVATE)
            .OrderBy((p, rp, r, ru, u, pgp, gp) => p.CreateTime, OrderByType.Desc)
            .Select((p, rp, r, ru, u, pgp, gp) => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
                ApiUrl = p.ApiUrl,
                OrderNum = p.OrderNum,
                Url = p.Url,
                IframeUrl = p.IframeUrl,
                Type = p.Type,
                Icon = p.Icon,
                CreateTime = p.CreateTime,
                Perms = p.Perms,
                State = p.State
            });
            var list = query.ToList();
            list = list.Distinct(new PermissionDtoComparer<PermissionDto>()).ToList();

            Dictionary<string, PermissionTreeResp> dict = list.ToDictionary(i => i.Id, i => i.Adapt<PermissionTreeResp>());
            var tree = ToTree(dict);
            return Succeed(tree);
        }

        /// <summary>
        /// 根据权限id列表查询菜单权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryListPermissionCommand cmd, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db
                .Queryable<Permission>()
                .Where(p => p.Type == PermissionType.MENU || p.Type == PermissionType.CATALOG)
                .Where(p => p.State != BaseStateConstants.DELETE)
                .Where(p => cmd.List.Contains(p.Id))
                .OrderBy(p => p.OrderNum, OrderByType.Asc)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    ParentId = p.ParentId,
                    ApiUrl = p.ApiUrl,
                    OrderNum = p.OrderNum,
                    Url = p.Url,
                    IframeUrl = p.IframeUrl,
                    Type = p.Type,
                    Icon = p.Icon,
                    CreateTime = p.CreateTime,
                    Perms = p.Perms
                });
            var list = query.ToList();
            list = list.Distinct(new PermissionDtoComparer<PermissionDto>()).ToList();
            var menuids = list.Select(i => i.Id).ToList();
            //查询所有菜单的按钮
            //GetChilders(menuids, list);

            var dtoList = list.Adapt<List<PermissionTreeResp>>();
            //Dictionary<string, PermissionTreeResp> dict = list.ToDictionary(i => i.Id, i => i.Adapt<PermissionTreeResp>());
            //var tree = ToTree(dict);
            return Succeed(dtoList);
        }

        private void GetChilders(List<string> menuids, List<Permission> list)
        {
            var allpermission = _permissionRepository.QueryAll();
            foreach (var id in menuids)
            {
                FindAllChildNode(list, id, allpermission);
            }
        }

        private List<PermissionDto> GetPermissionByUserId(string uid)
        {
            //根据用户id 查询所有权限带分组信息
            var db = _unitOfWork.GetDbClient();
            var list1 = db.Queryable<Permission, RolePermission, Role, RoleUser, User, PermissionPermissiongroup, Permissiongroup>
            ((p, rp, r, ru, u, pgp, gp) => new JoinQueryInfos(
                JoinType.Left, rp.PermissionId == p.Id,
                JoinType.Left, r.Id == rp.RoleId,
                JoinType.Left, ru.RoleId == r.Id,
                JoinType.Left, u.Id == ru.UserId,
                JoinType.Left, pgp.PermissionId == p.Id,
                JoinType.Left, gp.Id == pgp.PermissiongroupId
            ))
            .Where((p, rp, r, ru, u, pgp, gp) => u.Id == uid && p.State == BaseStateConstants.ACTIVATE)
            //.OrderBy((p, rp, r, ru, u, pgp, gp) => p.CreateTime, OrderByType.Desc)
            .Select((p, rp, r, ru, u, pgp, gp) => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
                ApiUrl = p.ApiUrl,
                OrderNum = p.OrderNum,
                Url = p.Url,
                IframeUrl = p.IframeUrl,
                Type = p.Type,
                Icon = p.Icon,
                CreateTime = p.CreateTime,
                PermissionGroupId = gp.Id,
                PermissionGroupName = gp.Name,
                Perms = p.Perms
            })
            .Distinct();

            var list2 = db.Queryable<Permission, PermissionPermissiongroup, Permissiongroup>
            ((p, pgp, gp) => new JoinQueryInfos(
                    JoinType.Left, pgp.PermissionId == p.Id,
                    JoinType.Left, gp.Id == pgp.PermissiongroupId
            ))
            .Where((p, pgp, gp) => p.Creator == _globalCore.UserConcatName && p.State == BaseStateConstants.ACTIVATE)
            //.OrderBy((p, pgp, gp) => p.CreateTime, OrderByType.Desc)
            .Select((p, pgp, gp) => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
                ApiUrl = p.ApiUrl,
                OrderNum = p.OrderNum,
                Url = p.Url,
                IframeUrl = p.IframeUrl,
                Type = p.Type,
                Icon = p.Icon,
                CreateTime = p.CreateTime,
                PermissionGroupId = gp.Id,
                PermissionGroupName = gp.Name,
                Perms = p.Perms
            })
            .Distinct();
            var list = db.UnionAll(list1, list2).OrderBy(it => it.CreateTime, OrderByType.Desc).Distinct().ToList();
            return list;
        }

        private List<PermissionDto> GetAllPermissionDtos()
        {
            //查询所有权限带分组信息
            var db = _unitOfWork.GetDbClient();
            var list = db.Queryable<Permission, PermissionPermissiongroup, Permissiongroup>
            ((p, pgp, pg) => new JoinQueryInfos(
                JoinType.Left, pgp.PermissionId == p.Id,
                JoinType.Left, pg.Id == pgp.PermissiongroupId
            ))
            .Where((p, pgp, pg) => p.State != BaseStateConstants.DELETE && pgp.State != BaseStateConstants.DELETE && pg.State != BaseStateConstants.DELETE)
            .OrderBy((p, pgp, pg) => p.CreateTime, OrderByType.Desc)
            .Select((p, pgp, pg) => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
                ApiUrl = p.ApiUrl,
                OrderNum = p.OrderNum,
                Url = p.Url,
                IframeUrl = p.IframeUrl,
                Type = p.Type,
                Icon = p.Icon,
                CreateTime = p.CreateTime,
                PermissionGroupId = pg.Id,
                PermissionGroupName = pg.Name,
                Perms = p.Perms
            })
            //.Distinct()
            .ToList();

            return list;
        }

        /// <summary>
        /// 根据角色查询权限树
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPermissionTreeByRoleCommand cmd, CancellationToken cancellationToken)
        {
            var uid = _globalCore.UserId;

            List<PermissionDto> list;

            if (uid.IsNull())
            {
                return Failed(UserCenterError.USER_NOT_FOUND);
            }

            if (!uid.Equals(SystemConstants.superId))
            {
                list = GetPermissionByUserId(uid);
            }
            else
            {
                list = GetAllPermissionDtos();
            }

            Dictionary<string, PermissionTreeResp> dict = new Dictionary<string, PermissionTreeResp>();
            foreach (var item in list)
            {
                if (dict.TryGetValue(item.Id, out var p))
                {
                    p.PermissionGroupName += ("," + item.PermissionGroupName);
                    p.PermissionGroupId += ("," + item.PermissionGroupId);
                }
                else
                {
                    dict.Add(item.Id, item.Adapt<PermissionTreeResp>());
                }
            }

            var tree = ToTree(dict);
            return Succeed(tree);
        }
        /// <summary>
        /// 查询所有权限
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPermissionTreeCommand cmd, CancellationToken cancellationToken)
        {
            var uid = _globalCore.UserId;

            List<PermissionDto> list;

            if (uid.IsNull())
            {
                return Failed(UserCenterError.USER_NOT_FOUND);
            }

            list = GetAllPermissionDtos();

            Dictionary<string, PermissionTreeResp> dict = new Dictionary<string, PermissionTreeResp>();
            foreach (var item in list)
            {
                if (dict.TryGetValue(item.Id, out var p))
                {
                    p.PermissionGroupName += ("," + item.PermissionGroupName);
                    p.PermissionGroupId += ("," + item.PermissionGroupId);
                }
                else
                {
                    dict.Add(item.Id, item.Adapt<PermissionTreeResp>());
                }
            }

            var tree = ToTree(dict);
            return Succeed(tree);
        }

        /// <summary>
        /// 字典转树结构
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public List<PermissionTreeResp> ToTree(Dictionary<string, PermissionTreeResp> dict)
        {
            foreach (var item in dict.Values)
            {
                if (!item.ParentId.IsEmpty())
                {
                    if (dict.TryGetValue(item.ParentId, out var parent))
                    {
                        item.ParentName = parent.Name;
                        parent.Children.Add(item);
                    }
                }
            }

            var noOrderList = dict.Values.Where(p => p.ParentId.Trim().IsNull()).ToList();
            var orderList = OrderTree(noOrderList);
            return orderList;
        }

        private List<PermissionTreeResp> OrderTree(List<PermissionTreeResp> noOrderList)
        {
            var order = noOrderList.OrderBy(p => p.OrderNum).ToList();
            foreach (var item in noOrderList)
            {
                item.Children = OrderTree(item.Children);
            }
            return order;
        }

        public void AddPermissionGroupId(PermissionDto unGroupDto, string id)
        {
            if (id.IsNull()) return;
            if (unGroupDto.PermissionGroupId.IsNull())
            {
                unGroupDto.PermissionGroupId = id;
            }
            else
            {
                if (!unGroupDto.PermissionGroupId.Contains(id ?? string.Empty))
                {
                    unGroupDto.PermissionGroupId += "," + id;
                }
            }
        }

        public void AddPermissionGroupName(PermissionDto unGroupDto, string name)
        {
            if (name.IsNull()) return;
            if (unGroupDto.Name.IsNull())
            {
                unGroupDto.PermissionGroupName = name;
            }
            else
            {
                if (!unGroupDto.PermissionGroupName.Contains(name ?? string.Empty))
                {
                    unGroupDto.PermissionGroupName += "," + name;
                }
            }
        }
    }
}