using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;
using MediatR;
using MF.Cache;
using MF.Core.Extensions;
using MF.FluentValidation;
using MF.NetCoreApp;
using MF.Orm;
using MF.Orm.UnitOfWork;
using MF.Utils;

using SqlSugar;

using UserCenter.Commands;
using UserCenter.Dtos;
using UserCenter.Enums;
using UserCenter.Response;

namespace UserCenter.CommandHandles
{
    public class RoleCommandHandle : ICommandHandler,
        IRequestHandler<CreateRoleCommand, PubResponse>,
        IRequestHandler<UpdateRoleCommand, PubResponse>,
        IRequestHandler<QueryRoleCommand, PubResponse>,
        IRequestHandler<DeleteRoleCommand, PubResponse>,
        IRequestHandler<AssignPermissionToRoleRoleCommand, PubResponse>,
        IRequestHandler<AssignUserToRoleRoleCommand, PubResponse>,
        IRequestHandler<QueryAllTreeRoleCommand, PubResponse>,
        IRequestHandler<QueryAllRoleCommand, PubResponse>,
        IRequestHandler<QueryByNameRoleCommand, PubResponse>, 
        IRequestHandler<QueryByUserNameRoleCommand, PubResponse>,
        IRequestHandler<QueryByRoleNameCommand, PubResponse>,
        IRequestHandler<QueryPageRoleCommand, PubResponse>,
        IRequestHandler<QueryPermissionsByRoleIdRoleCommand, PubResponse>,
        IRequestHandler<QueryUsersByRoleIdRoleRoleCommand, PubResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleUserRepository _roleUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUcPermissionRepository _permissionRepository;
        private readonly GlobalCore _globalCore;
        private readonly IUnitOfWork _unitOfWork;

        public RoleCommandHandle(GlobalCore globalCore
            , IRoleRepository roleRepository
            , IRolePermissionRepository rolePermissionRepository
            , IRoleUserRepository roleUserRepository
            , IUserRepository userRepository
            , IUcPermissionRepository permissionRepository
            , IUnitOfWork unitOfWork)
        {
            _globalCore = globalCore;
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleUserRepository = roleUserRepository;
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(CreateRoleCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }
            var roleByName = _roleRepository.QueryableToEntity(u => u.Name.Equals(cmd.Name));
            if (roleByName.NotNull())
            {
                return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
            }

            Role role = new Role
            {
                Name = cmd.Name,
                Remark = cmd.Remark,
                Updator = cmd.Updator
            };
            role = _roleRepository.InsertReturnEntity(role);

            bool flag = role.NotNull();
            return SucceedOrFail(flag, role.Id);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(UpdateRoleCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.IsNull())
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }

            if (cmd.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }

            var roleById = _roleRepository.Queryable().InSingle(cmd.Id);
            if (roleById.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            if (!roleById.Name.Equals(cmd.Name))
            {
                var roleByName = _roleRepository.QueryableToEntity(u => u.Name.Equals(cmd.Name));
                if (roleByName.NotNull())
                {
                    return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
                }
            }
            roleById.Name = cmd.Name;
            roleById.Remark = cmd.Remark;
            roleById.Updator = cmd.Updator;

            bool flag = _roleRepository.UpdateEntity(roleById);
            return SucceedOrFail(flag, roleById.Id);
        }

        /// <summary>
        /// 根据id查询角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryRoleCommand cmd, CancellationToken cancellationToken)
        {
            var role = _roleRepository.Queryable().InSingle(cmd.Id);
            bool flag = role.NotNull();
            return SucceedOrFail(flag, role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [CacheRemove("uc:userole")]
        public Task<PubResponse> Handle(DeleteRoleCommand cmd, CancellationToken cancellationToken)
        {
            List<string> undellist = new List<string>();
            var roles = _roleRepository.Queryable().In(cmd.List).ToList();
            if (roles.IsNullT())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            foreach (var role in roles)
            {
                // 查询角色权限关联
                var rpLink = _rolePermissionRepository.QueryableToList(rp => rp.RoleId.Equals(role.Id));
                if (rpLink.NotNullT())
                {
                    undellist.Add(role.Id);
                    continue;
                }

                // 角色用户关联
                var ruLink = _roleUserRepository.QueryableToList(ru => ru.RoleId.Equals(role.Id));
                if (ruLink.NotNullT())
                {
                    undellist.Add(role.Id);
                    continue;
                }
            }
            if (undellist.NotNullT())
            {
                return Failed(BaseSystemError.RELATION_EXIST);
            }
            bool _flag = _roleRepository.Delete(r => cmd.List.Contains(r.Id));
            return SucceedOrFail(_flag);
        }

        /// <summary>
        /// 分配权限给角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(AssignPermissionToRoleRoleCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.IsNull())
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }
            if (cmd.List.IsNullT())
            {
                _rolePermissionRepository.Delete(rp => rp.RoleId == cmd.Id, false);
                return Succeed();
            }
            var roleObj = _roleRepository.Queryable().InSingle(cmd.Id);
            if (roleObj.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            if (cmd.List.NotNullT())
            {
                List<Permission> permissionsList = _permissionRepository.Queryable().In(cmd.List).ToList();//查询权限列表
                if (permissionsList.Count != cmd.List.Count)//权限不全
                {
                    return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
                }
            }
            var db = _unitOfWork.GetDbClient();
            var flag = db.UseTran(() =>
            {
                var uid = _globalCore.UserId;
                if (uid.Equals(SystemConstants.superId))//注意：system 先删除所有，再重新创建 
                {
                    db.Deleteable<RolePermission>().Where(rp => rp.RoleId == cmd.Id).ExecuteCommand();
                    List<RolePermission> addItemList = new List<RolePermission>();
                    foreach (var pid in cmd.List)
                    {
                        RolePermission rp = new RolePermission
                        {
                            PermissionId = pid,
                            RoleId = cmd.Id,
                            InnerVersion = 0
                        };
                        rp.ChangeBaseInfo(_globalCore.UserName);
                        addItemList.Add(rp);
                    }
                    db.Insertable(addItemList).ExecuteCommand();
                }
                //除system分配权限之外的其它角色 都是禁用、启用操作 而非删除再插入  防止去掉勾选项后 就看不到之前system分配的权限了
                else
                {
                    //针对已分配给权限操作
                    var list = db.Queryable<RolePermission>().Where(i => i.RoleId.Equals(cmd.Id)).ToList();
                    //禁用
                    var disList = list.Where(i => !cmd.List.Contains(i.PermissionId)).Select(i => i.Id).ToArray();
                    //更新
                    var updateList = list.Where(i => cmd.List.Contains(i.PermissionId)).Select(i => i.Id).ToArray();

                    db.Updateable<RolePermission>()
                    .SetColumns(it => it.State == BaseStateConstants.DEACTIVE)
                    .Where(it => disList.Contains(it.Id))
                    .ExecuteCommand();

                    db.Updateable<RolePermission>()
                    .SetColumns(it => it.State == BaseStateConstants.ACTIVATE)
                    .Where(it => updateList.Contains(it.Id))
                    .ExecuteCommand();

                    //未分配 但属于自己创建的按钮、菜单
                    var allPermList=list.Select(i => i.PermissionId).ToArray();
                    var selfList = cmd.List.Except(allPermList).ToList();

                    if (selfList.Count > 0)
                    {
                        List<RolePermission> addItemList = new List<RolePermission>();
                        foreach (var pid in selfList)
                        {
                            RolePermission rp = new RolePermission
                            {
                                PermissionId = pid,
                                RoleId = cmd.Id,
                                InnerVersion = 0
                            };
                            rp.ChangeBaseInfo(_globalCore.UserName);
                            addItemList.Add(rp);
                        }
                        db.Insertable(addItemList).ExecuteCommand();
                    }
                }
            });

            return SucceedOrFail(flag.IsSuccess);
        }

        /// <summary>
        /// 分配角色给用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [CacheRemove("uc:userole")]
        public Task<PubResponse> Handle(AssignUserToRoleRoleCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.IsNull())
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }
            if (cmd.List.IsNullT())
            {
                _roleUserRepository.Delete(rp => rp.RoleId.Equals(cmd.Id), false);
                return Succeed();
            }

            var role = _roleRepository.Queryable().InSingle(cmd.Id);
            if (role.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            List<User> users = _userRepository.Queryable().In(cmd.List).ToList();
            if (users.IsNullT())
            {
                return Failed(UserCenterError.ASSIGN_USER_NOT_FOUND);
            }
            if (users.Count != cmd.List.Count)
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            var db = _unitOfWork.GetDbClient();
            var flag = db.UseTran(() =>
            {
                //删除
                db.Deleteable<RoleUser>().Where(rp => rp.RoleId.Equals(cmd.Id)).ExecuteCommand();
                List<RoleUser> addItemList = new List<RoleUser>();
                foreach (var uid in cmd.List)
                {
                    RoleUser rp = new RoleUser
                    {
                        UserId = uid,
                        RoleId = cmd.Id,
                        InnerVersion = 0
                    };
                    rp.ChangeBaseInfo(_globalCore.UserName);
                    addItemList.Add(rp);
                }
                db.Insertable(addItemList).ExecuteCommand();
            });

            return SucceedOrFail(flag.IsSuccess);
        }

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryAllRoleCommand cmd, CancellationToken cancellationToken)
        {
            var list = _roleRepository.QueryAll();
            return Succeed(list);
        }

        /// <summary>
        /// 查询所有角色和用户的关联树
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryAllTreeRoleCommand cmd, CancellationToken cancellationToken)
        {
            var roleList = _roleRepository.QueryAll();
            if (roleList.NotNullT())
            {
                return Failed(UserCenterError.ROLE_LIST_IS_EMPTY);
            }
            List<RoleTreeResp> list = new List<RoleTreeResp>();

            foreach (var role in roleList)
            {
                // 查询关联表
                var links = _roleUserRepository.QueryableToList(ru => ru.RoleId.Equals(role.Id));
                // 获取用户id列表
                var uids = links.Select(l => l.UserId).ToList();
                // 查询用户列表
                var users = _userRepository.Queryable().In(uids).ToList();

                //查询角色用户关联表
                RoleTreeResp rtd = new RoleTreeResp
                {
                    Id = role.Id,
                    Name = role.Name
                };
                rtd.children.AddRange(users);
                list.Add(rtd);
            }
            bool flag = list.Count == roleList.Count;
            return SucceedOrFail(flag, list);
        }
        /// <summary>
        /// 用户-角色
        /// </summary>
        /// <returns></returns>
        [Cache("uc:userole")]
        private List<UserRDto> FindRoleByUser()
        {
            var list = _userRepository.Queryable()
                .Includes(x => x.RoleList
                                .Where(r => r.State == BaseStateConstants.ACTIVATE)
                                .ToList())
                .ToList(i => new UserRDto { UserId = i.Id, UserName = i.Name, FullName = i.FullName, RoleList = i.RoleList });

            //var all = list.Adapt<List<UserRoleDto>>();
            return list;
        }
        /// <summary>
        /// 角色-用户
        /// </summary>
        /// <returns></returns>
        //[Cache("uc:rolebyuser")]
        private List<RoleUDto> FindUserByRole()
        {
            var list = _roleRepository.Queryable()
                .Includes(x => x.UserList
                                .Where(r => r.State == BaseStateConstants.ACTIVATE)
                                .ToList())
                .ToList(i => new RoleUDto { RoleId = i.Id, RoleName = i.Name, UserList = i.UserList });

            //var all = list.Adapt<List<UserRoleDto>>();
            return list;
        }
        /// <summary>
        /// 根据用户名称查询角色列表 用户-角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryByUserNameRoleCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.UserName.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }

            var userRoleList = FindRoleByUser();

            var userEnty=userRoleList.Where(i => i.UserName == cmd.UserName).FirstOrDefault();
            if (userEnty.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            if (userEnty.RoleList.Count<=0)
            {
                return Failed(UserCenterError.ASSIGN_USER_NOT_FOUND);
            }

            return Succeed(userEnty.RoleList);
        }

        /// <summary>
        /// 根据用户名称查询角色列表 角色-用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryByRoleNameCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.RoleName.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }

            var userRoleList = FindUserByRole();

            var roleEnty = userRoleList.Where(i => i.RoleName == cmd.RoleName).FirstOrDefault();
            if (roleEnty.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            if (roleEnty.UserList.Count <= 0)
            {
                return Failed(UserCenterError.ASSIGN_USER_NOT_FOUND);
            }

            return Succeed(roleEnty.UserList);
        }

        /// <summary>
        /// 根据角色名称查询角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryByNameRoleCommand cmd, CancellationToken cancellationToken)
        {
            var role = _roleRepository.QueryableToEntity(r => r.Name.Equals(cmd.Name));
            bool flag = role.NotNull();
            return SucceedOrFail(flag, role);
        }

        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPageRoleCommand cmd, CancellationToken cancellationToken)
        {
            int totalNumber = 0;
            var roles = _roleRepository.Queryable()
                .WhereIF(cmd.Condition.NotNull(), cmd.Condition)
                .OrderBy(i => i.CreateTime, OrderByType.Desc)
                .ToPageList(cmd.PageNum, cmd.PageSize, ref totalNumber);

            return Succeed(new { list = roles, total = totalNumber });
        }

        /// <summary>
        /// 根据角色id查询权限列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPermissionsByRoleIdRoleCommand cmd, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.GetDbClient();
            var list = db.Queryable<Permission, RolePermission, Role>
               ((p, rolePermissionLink, r) => new JoinQueryInfos(
                   JoinType.Left, rolePermissionLink.PermissionId.Equals(p.Id),
                   JoinType.Left, r.Id.Equals(rolePermissionLink.RoleId)
               ))
               .Where((p, rolePermissionLink, r) => r.Id.Equals(cmd.Id)
                       && p.State != BaseStateConstants.DELETE
                       && rolePermissionLink.State == BaseStateConstants.ACTIVATE
                       && r.State != BaseStateConstants.DELETE)
               .OrderBy((p, rolePermissionLink, r) => p.CreateTime, OrderByType.Desc)
               .Select((p, rolePermissionLink, r) => new Permission
               {
                   Id = p.Id,
                   Name = p.Name,
                   Code = p.Code,
                   ParentId = p.ParentId,
                   ApiUrl = p.ApiUrl,
                   OrderNum = p.OrderNum,
                   Url = p.Url,
                   Type = p.Type,
                   Icon = p.Icon,
                   CreateTime = p.CreateTime
               })
               .ToList();

            return Succeed(list);
        }

        /// <summary>
        /// 根据角色id查询用户列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryUsersByRoleIdRoleRoleCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.Id.IsNull())
            {
                return Failed(BaseSystemError.ID_CANNOT_BE_EMPTY);
            }

            // 查询角色
            var role = _roleRepository.Queryable().InSingle(cmd.Id);
            if (role.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            //查询关联表
            var links = _roleUserRepository.QueryableToList(rp => rp.RoleId.Equals(cmd.Id)); 
            if (links.IsNullT())
            {
                return Failed(UserCenterError.ASSIGN_PERMISSION_NOT_FOUND);
            }

            //查询权限
            var uids = links.Select(p => p.UserId).ToList();

            var users = _userRepository.Queryable().In(uids).ToList();
            return Succeed(users);
            //return SucceedOrFail(users.Count == uids.Count, users, BaseSystemError.OBJECT_DOES_NOT_EXIST);
        }
    }
}