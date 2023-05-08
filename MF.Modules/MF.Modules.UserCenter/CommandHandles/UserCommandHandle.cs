using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;

using Mapster;

using Masuit.Tools;

using MDCenter.Commands.Shift;
using MDCenter.Commands.WorkStation;

using MediatR;
using MF.Cache;
using MF.Core.Check;
using MF.Core.Extensions;
using MF.Core.Security;
using MF.FluentValidation;
using MF.MediatR;
using MF.NetCore;
using MF.NetCoreApp;
using MF.Orm;
using MF.Orm.UnitOfWork;
using MF.Swagger;
using MF.Utils;

using Microsoft.Extensions.Configuration;

using SqlSugar;

using UserCenter.Commands;
using UserCenter.Dtos;
using UserCenter.Enums;
using UserCenter.Response;
using UserCenter.Response.User;

namespace UserCenter.CommandHandles
{
    public class UserCommandHandle : ICommandHandler,
        IRequestHandler<QueryUserCommand, PubResponse>,
        IRequestHandler<LoginUserCommand, PubResponse>,
        IRequestHandler<CreateUserCommand, PubResponse>,
        IRequestHandler<UpdateUserCommand, PubResponse>,
        IRequestHandler<DeleteUserCommand, PubResponse>,
        IRequestHandler<ExportUserCommand, PubResponse>,
        IRequestHandler<ImportUserCommand, PubResponse>,
        IRequestHandler<LogoutUserCommand, PubResponse>,
        IRequestHandler<IPCLoginUserCommand, PubResponse>,
        IRequestHandler<QueryAllUserCommand, PubResponse>,
        IRequestHandler<QueryPageUserCommand, PubResponse>,
        IRequestHandler<QueryByNameUserCommand, PubResponse>,
        IRequestHandler<UpdateStatusUserCommand, PubResponse>,
        IRequestHandler<ResetPasswordUserCommand, PubResponse>,
        IRequestHandler<QueryUserListByIdCommand, PubResponse>,
        IRequestHandler<AssignFavoriteUserCommand, PubResponse>,
        IRequestHandler<ChangePasswordUserCommand, PubResponse>,
        IRequestHandler<QueryUserByIdsUserCommand, PubResponse>,
        IRequestHandler<QueryUserListByNameCommand, PubResponse>,
        IRequestHandler<BatchResetPasswordUserCommand, PubResponse>,
        IRequestHandler<ImportExcelWithAreaUserCommand, PubResponse>,
        IRequestHandler<QueryFavoriteByUserIdUserCommand, PubResponse>,

        IRequestHandler<StoreCreateUserCommand, PubResponse>,
        IRequestHandler<StoreUpdateUserCommand, PubResponse>,
        IRequestHandler<StoreDeleteUserCommand, PubResponse>,
        IRequestHandler<StoreExportUserCommand, PubResponse>,
        IRequestHandler<StoreQueryPageUserCommand, PubResponse>,
        IRequestHandler<StoreUpdateStatusUserCommand, PubResponse>,
        IRequestHandler<StoreBatchResetPasswordUserCommand, PubResponse>,
        IRequestHandler<IPCCheckBindUserCommand, PubResponse>,
        IRequestHandler<IPCCheckBtnUserCommand, PubResponse>,
        IRequestHandler<FindUserTokenCommand, PubResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccesslogRepository _accesslogRepository;

        private readonly IUserFavoriteRepository _userFavoriteRepository;
        private readonly GlobalCore _globalCore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;

        public UserCommandHandle(
              GlobalCore globalCore
            , IUserRepository userRepository
            , IAccesslogRepository accesslogRepository
            , IUserFavoriteRepository userFavoriteRepository
            , IUnitOfWork unitOfWork
            , IConfiguration configuration
            , IBus bus)
        {
            _userRepository = userRepository;
            _accesslogRepository = accesslogRepository;
            _userFavoriteRepository = userFavoriteRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _globalCore = globalCore;
            _bus = bus;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(CreateUserCommand cmd, CancellationToken cancellationToken)
        {
            UserDto dto = cmd.Dto;
            if (dto.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }

            if (dto.FullName.IsNull())
            {
                return Failed(BaseSystemError.FULLNAME_CANNOT_BE_EMPTY);
            }

            if (_userRepository.Queryable().Where(u => u.Name == dto.Name).Any())
            {
                return Failed(UserCenterError.NAME_ALREADY_EXIST);
            }

            if (dto.Email.NotNull() && !dto.Email.MatchEmail().isMatch)
            {
                return Failed(UserCenterError.EMAIL_ERROR);
            }

            if (dto.Tel.NotNull() && !dto.Tel.MatchPhoneNumber())
            {
                return Failed(UserCenterError.TEL_ERROR);
            }

            User user = new User
            {
                Name = dto.Name,
                FullName = dto.FullName,
                Email = dto.Email,
                Tel = dto.Tel,
                Remark = "普通用户",
                UserType = "SYS_USER"
            };

            ChangePassword(user, SystemConstants.defaultPassword);

            user.State = BaseStateConstants.ACTIVATE;
            var dbuser = _userRepository.InsertReturnEntity(user);
            string userId = dbuser?.Id;
            return SucceedOrFail(userId.NotNull(), userId, BaseSystemError.OBJECT_IS_INACTIVE);
        }

        /// <summary>
        /// 创建用户 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(StoreCreateUserCommand cmd, CancellationToken cancellationToken)
        {
            UserDto dto = cmd.Dto;
            if (dto.Name.IsNull())
            {
                return Failed(BaseSystemError.NAME_CANNOT_BE_EMPTY);
            }

            if (dto.FullName.IsNull())
            {
                return Failed(BaseSystemError.FULLNAME_CANNOT_BE_EMPTY);
            }

            if (_userRepository.Queryable().Where(u => u.Name == dto.Name).Any())
            {
                return Failed(UserCenterError.NAME_ALREADY_EXIST);
            }

            if (dto.Email.NotNull() && !dto.Email.MatchEmail().isMatch)
            {
                return Failed(UserCenterError.EMAIL_ERROR);
            }

            if (dto.Tel.NotNull() && !dto.Tel.MatchPhoneNumber())
            {
                return Failed(UserCenterError.TEL_ERROR);
            }

            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                FullName = dto.FullName,
                Email = dto.Email,
                Tel = dto.Tel,
                Remark = "普通用户",
                UserType = "SYS_USER"
            };

            ChangePassword(user, SystemConstants.defaultPassword);

            var list = new List<SugarParameter>
            {
                new SugarParameter("@operation", 1, false),
                new SugarParameter("@id", user.Id, false),
                new SugarParameter("@name", user.Name, false),
                new SugarParameter("@remark", user.Remark, false),
                new SugarParameter("@full_name", user.FullName, false),
                new SugarParameter("@password", user.Password, false),
                new SugarParameter("@salt", user.Salt, false),
                new SugarParameter("@email", user.Email, false),
                new SugarParameter("@tel", user.Tel, false),
                new SugarParameter("@type", user.UserType, false),
                new SugarParameter("@creator", _globalCore.UserConcatName, false)
            };

            //var list = _unitOfWork.GetParameter(new StoreCreateUser()
            //{
            //    operation = 1,
            //    id = user.Id,
            //    name = user.Name,
            //    remark = user.Remark,
            //    full_name = user.FullName,
            //    password = user.Password,
            //    salt = user.Salt,
            //    email = user.Email,
            //    tel = user.Tel,
            //    type = user.UserType,
            //    creator = _globalCore.UserConcatName
            //});

            try
            {
                var dt = _userRepository.UseStoredProcedureToDataTable("pro_uc_user_submit", list);
                return Succeed();
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        private class StoreCreateUser
        {
            [StoreParamDirect]
            public int operation { get; set; }

            [StoreParamDirect]
            public string id { get; set; }

            [StoreParamDirect]
            public string name { get; set; }

            [StoreParamDirect]
            public string remark { get; set; }

            [StoreParamDirect]
            public string full_name { get; set; }

            [StoreParamDirect]
            public string password { get; set; }

            [StoreParamDirect]
            public string salt { get; set; }

            [StoreParamDirect]
            public string email { get; set; }

            [StoreParamDirect]
            public string tel { get; set; }

            [StoreParamDirect]
            public string type { get; set; }

            [StoreParamDirect]
            public string creator { get; set; }
        }

        private void RecordLoginSuccess(User user)
        {
            RecordLogin(user, LoginRecordEnum.loginIn);
        }

        private void RecordLoginFailed(User user)
        {
            RecordLogin(user, LoginRecordEnum.loginFailed);
        }

        private void RecordLogin(User user, LoginRecordEnum type)
        {
            // 这里可以在缓存里 判断今天用户失败了几次 并且禁止登录
            if (user is null) return;

            //记录AccessLog日志
            Accesslog accesslog = new Accesslog
            {
                AccessType = type.Name,
                UserName = user.Name,
                Ip = _globalCore.GetIp(),
                ClientName = _globalCore.GetBrowser(),
                Creator = string.Concat(user.Name, "|", user.FullName),
                Updator = string.Concat(user.Name, "|", user.FullName)
            };

            _accesslogRepository.Insert(accesslog);
        }

        /// <summary>
        /// 用户登入
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(LoginUserCommand cmd, CancellationToken cancellationToken)
        {
            // 查询用户是否存在
            var user = _userRepository.QueryableToEntity(u => u.Name == cmd.Name);
            if (user.IsNull())
            {
                RecordLoginFailed(user);
                return Failed(BaseSystemError.USER_NOT_FOUND);
            }

            // 校验状态
            if (user.State == BaseStateConstants.DEACTIVE)
            {
                RecordLoginFailed(user);
                return Failed(BaseSystemError.USER_DEACTIVE);
            }

            // 校验密码
            if (!VerifyPassword(user, cmd.Password))
            {
                RecordLoginFailed(user);
                return Failed(BaseSystemError.USERNAME_OR_PASSWORD_ERROR);
            }

            // 生成jwt
            JwtConfig jwt = _configuration?.GetSection("Jwt")?.Get<JwtConfig>();
            var claims = new List<Claim>()
                        {
                           new Claim("loginType","WEB"),
                           new Claim("userFullName",user.FullName),
                           new Claim("userName",user.Name),
                           new Claim("userId",user.Id),
                           new Claim("jti",Guid.NewGuid().ToString()),
                           new Claim("enabled",user.State.Equals(BaseStateConstants.ACTIVATE) ? "true" : "false")
                        };
            var jwtobj = jwt.GenToken(claims);

            UserLoginResp data = new UserLoginResp
            {
                Token = jwtobj.Item1,
                ExpireTime = jwtobj.Item2,
                Username = user.Name,
                UserFullName = user.FullName,
                UserId = user.Id,
                LoginType = cmd.LoginType,
                Enable = user.State.Equals(BaseStateConstants.ACTIVATE),
                AdditionalInformation = new Additionalinformation()
            };
            if (!cmd.LoginFalse)
            {
                RecordLoginSuccess(user);//记录访问日志
            }
            return Succeed(data);
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(LogoutUserCommand cmd, CancellationToken cancellationToken)
        {
            Accesslog accesslog = new Accesslog
            {
                AccessType = LoginRecordEnum.loginOut.Name,
                UserName = _globalCore.UserName,
                Ip = _globalCore.GetIp(),
                ClientName = _globalCore.GetBrowser()
            };
            _accesslogRepository.Insert(accesslog);

            return Succeed();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(UpdateUserCommand cmd, CancellationToken cancellationToken)
        {
            UserDto dto = cmd.Dto;
            User user = _userRepository.Queryable().InSingle(dto.Id);
            if (user.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            if (user.State != BaseStateConstants.ACTIVATE)
            {
                return Failed(BaseSystemError.OBJECT_IS_INACTIVE);
            }

            if (dto.Name.NotNull() && !dto.Name.Equals(user.Name))
            {
                if (_userRepository.Queryable().Any(u => u.Name == dto.Name && u.Id != dto.Id))
                {
                    return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
                }
            }

            if (dto.Email.NotNull() && !dto.Email.MatchEmail().isMatch)
            {
                return Failed(UserCenterError.EMAIL_ERROR);
            }

            if (dto.Tel.NotNull() && !dto.Tel.MatchPhoneNumber())
            {
                return Failed(UserCenterError.TEL_ERROR);
            }

            user.Name = dto.Name;
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Tel = dto.Tel;

            bool flag = _userRepository.UpdateEntity(user);

            return SucceedOrFail(flag);
        }

        /// <summary>
        /// 修改用户 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(StoreUpdateUserCommand cmd, CancellationToken cancellationToken)
        {
            var dto = cmd.Dto;
            User user = _userRepository.Queryable().InSingle(dto.Id);
            if (user.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            if (user.State != BaseStateConstants.ACTIVATE)
            {
                return Failed(BaseSystemError.OBJECT_IS_INACTIVE);
            }

            if (dto.Name.NotNull() && !dto.Name.Equals(user.Name))
            {
                if (_userRepository.Queryable().Any(u => u.Name == dto.Name && u.Id != dto.Id))
                {
                    return Failed(BaseSystemError.OBJECT_ALREADY_EXIST);
                }
            }

            if (dto.Email.NotNull() && !dto.Email.MatchEmail().isMatch)
            {
                return Failed(UserCenterError.EMAIL_ERROR);
            }

            if (dto.Tel.NotNull() && !dto.Tel.MatchPhoneNumber())
            {
                return Failed(UserCenterError.TEL_ERROR);
            }

            user.Name = dto.Name;
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Tel = dto.Tel;

            var list = new List<SugarParameter>
            {
                new SugarParameter("@operation", 2, false),//2 表示编辑
                new SugarParameter("@id", user.Id, false),
                new SugarParameter("@name", user.Name, false),
                new SugarParameter("@remark", "", false),
                new SugarParameter("@full_name", user.FullName, false),
                new SugarParameter("@password", "", false),
                new SugarParameter("@salt", "", false),
                new SugarParameter("@email", user.Email, false),
                new SugarParameter("@tel", user.Tel, false),
                new SugarParameter("@type", "", false),
                new SugarParameter("@creator", _globalCore.UserConcatName, false)
            };

            //var list = _unitOfWork.GetParameter(new StoreUpdateUser()
            //{
            //    operation = 2,
            //    id = user.Id,
            //    name = user.Name,
            //    remark = "",
            //    full_name = user.FullName,
            //    password = "",
            //    salt = "",
            //    email = user.Email,
            //    tel = user.Tel,
            //    type = "",
            //    creator = _globalCore.UserConcatName
            //});

            try
            {
                var dt = _userRepository.UseStoredProcedureToDataTable("pro_uc_user_submit", list);
                return Succeed();
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        public class StoreUpdateUser
        {
            [StoreParamDirect]
            public int operation { get; set; }

            [StoreParamDirect]
            public string id { get; set; }

            [StoreParamDirect]
            public string name { get; set; }

            [StoreParamDirect]
            public string remark { get; set; }

            [StoreParamDirect]
            public string full_name { get; set; }

            [StoreParamDirect]
            public string password { get; set; }

            [StoreParamDirect]
            public string salt { get; set; }

            [StoreParamDirect]
            public string email { get; set; }

            [StoreParamDirect]
            public string tel { get; set; }

            [StoreParamDirect]
            public string type { get; set; }

            [StoreParamDirect]
            public string creator { get; set; }
        }

        /// <summary>
        /// 根据id查询用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryUserCommand cmd, CancellationToken cancellationToken)
        {
            User user = _userRepository.Queryable().InSingle(cmd.Id);
            bool flag = user.NotNull();

            return SucceedOrFail(flag, user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        [CacheRemove("uc:userole")]
        public Task<PubResponse> Handle(DeleteUserCommand cmd, CancellationToken cancellationToken)
        {
            //不能删除超级管理员
            cmd.List.Remove(SystemConstants.superId);
            CheckNull.BusinessException(!_userRepository.Delete(u => cmd.List.Contains(u.Id)), BaseSystemError.FAILED);

            return Succeed();
        }

        /// <summary>
        /// 删除用户 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(StoreDeleteUserCommand cmd, CancellationToken cancellationToken)
        {
            //不能删除超级管理员
            cmd.List.Remove(SystemConstants.superId);
            string ids = cmd.List.Parse2StoreString();

            var list = new List<SugarParameter>();
            list.Add(new SugarParameter("@ids", ids, false));

            try
            {
                var dt = _userRepository.UseStoredProcedureToDataTable("pro_uc_user_delete", list);
                return Succeed();
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        /// <summary>
        /// 修改用户自定义图标
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        public Task<PubResponse> Handle(AssignFavoriteUserCommand cmd, CancellationToken cancellationToken)
        {
            //User user = _userRepository.Queryable().InSingle(cmd.OwnerId);
            List<UserFavorite> list = _userFavoriteRepository.QueryableToList(uf => uf.OwnerId == _globalCore.UserId);

            _userFavoriteRepository.Delete(i => i.OwnerId == _globalCore.UserId, false);

            var newArr = cmd.List.Select(i => i.Id).ToArray();
            var oldArr = list.Select(i => i.Id).ToArray();

            var operas = ArrayExt.GetOperationArray(oldArr, newArr);

            var deleteList = operas.Item1.ToList();

            var db = _unitOfWork.GetDbClient();

            _userFavoriteRepository.Delete(uf => deleteList.Contains(uf.OwnerId));

            var updateids = operas.Item3.ToList();
            var updateList = list.Where(i => updateids.Contains(i.Id)).ToList();
            foreach (var update in updateList)
            {
                update.OwnerId = _globalCore.UserId;
            }
            _userFavoriteRepository.Update(updateList);

            var createids = operas.Item2.ToList();
            var creates = cmd.List.Where(i => createids.Contains(i.Id)).ToList();
            List<UserFavorite> addList = new List<UserFavorite>();
            foreach (var dto in creates)
            {
                var e = dto.Adapt<UserFavorite>();
                e.OwnerId = _globalCore.UserId;
                addList.Add(e);
            }
            if (addList.Count > 0)
            {
                CheckNull.BusinessException(!_userFavoriteRepository.Insert(addList), BaseSystemError.FAILED);
            }
            return Succeed(cmd.List.Count);
        }

        /// <summary>
        /// 获得用户自定义图标
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryFavoriteByUserIdUserCommand cmd, CancellationToken cancellationToken)
        {
            //User user = _userRepository.Queryable().InSingle(_globalCore.UserId);
            List<UserFavorite> list = new List<UserFavorite>();
            //if (user.NotNull())
            //{
            list = _userFavoriteRepository.QueryableToList(uf => uf.OwnerId == _globalCore.UserId);
            if (list.NotNullT())
            {
                list = list.OrderBy(x => x.DisplayIndex).ToList();
            }
            //}
            return Succeed(list);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(ResetPasswordUserCommand cmd, CancellationToken cancellationToken)
        {
            User user = _userRepository.Queryable().InSingle(cmd.Id);
            if (user.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            user.Updator = cmd.Updator;
            ChangePassword(user, SystemConstants.defaultPassword);
            bool flag = _userRepository.UpdateEntity(user);
            return SucceedOrFail(flag, user.Id);
        }

        /// <summary>
        /// 批量重置密码
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(BatchResetPasswordUserCommand cmd, CancellationToken cancellationToken)
        {
            List<User> userList = _userRepository.Queryable().In(cmd.List).ToList();
            foreach (var user in userList)
            {
                user.Updator = _globalCore.UserName;
                ChangePassword(user, SystemConstants.defaultPassword);
            }
            bool flag = _userRepository.Update(userList);
            return SucceedOrFail(flag);
        }

        /// <summary>
        /// 批量重置密码 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(StoreBatchResetPasswordUserCommand cmd, CancellationToken cancellationToken)
        {
            var users = _userRepository.Queryable().In(cmd.List).ToList();
            if (users.IsNullLt())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }
            var user = users.First();
            ChangePassword(user, SystemConstants.defaultPassword);

            var list = _unitOfWork.GetSugarParameters(new StoreBatchResetPasswordUser
            {
                ids = cmd.List.Parse2StoreString(),
                pwd = user.Password,
                salt = user.Salt,
                updator = _globalCore.UserConcatName
            });

            try
            {
                var dt = _userRepository.UseStoredProcedureToDataTable("pro_uc_user_reset_pwd", list);
                return Succeed();
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        private class StoreBatchResetPasswordUser
        {
            [StoreParamDirect]
            public string ids { get; set; }

            [StoreParamDirect]
            public string pwd { get; set; }

            [StoreParamDirect]
            public string salt { get; set; }

            [StoreParamDirect]
            public string updator { get; set; }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(ChangePasswordUserCommand cmd, CancellationToken cancellationToken)
        {
            User user = _userRepository.Queryable().InSingle(cmd.Id);
            if (user.NotNull())
            {
                if (VerifyPassword(user, cmd.OldPassword))
                {
                    ChangePassword(user, cmd.NewPassword);
                    var flag = _userRepository.UpdateEntity(user);
                    return SucceedOrFail(flag, user.Id);
                }
                else
                {
                    return Failed(UserCenterError.OLD_PASSWORD_ERROR);
                }
            }

            return Failed(UserCenterError.USER_NOT_FOUND);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        private void ChangePassword(User user, string newPassword)
        {
            user.Salt = SecurityUtil.GetSalt();
            user.Password = SecurityUtil.ToMd5(newPassword + user.Salt);
            user.PasswordChangeTime = DateTime.Now;
        }

        /// <summary>
        /// 校验密码正确性
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        private bool VerifyPassword(User user, string oldPassword)
        {
            return user.Password.Equals(SecurityUtil.ToMd5(oldPassword + user.Salt));
        }

        /// <summary>
        /// 查询所有的用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryAllUserCommand cmd, CancellationToken cancellationToken)
        {
            List<User> users = _userRepository.QueryableToList(u => u.Id != SystemConstants.superId);//过滤超级管理员

            return Succeed(users);
        }

        /// <summary>
        /// 根据user name查询用户
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryByNameUserCommand cmd, CancellationToken cancellationToken)
        {
            User user = null;
            if (!cmd.Name.Equals(SystemConstants.superName))
            {
                user = _userRepository.QueryableToEntity(u => u.Name == cmd.Name);
            }

            return Succeed(user);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryPageUserCommand cmd, CancellationToken cancellationToken)
        {
            var userList = GetQueryList(cmd.Condition, cmd.Order);

            int totalCount = userList.ToList().Count;
            var userList1 = userList.ToPageList(cmd.PageNum, cmd.PageSize);
            List<UserDto> dtos = userList1.Adapt<List<UserDto>>();
            AddOrgAndRoleInfo(dtos);

            return Succeed(new { list = dtos, total = totalCount });
        }

        /// <summary>
        /// 分页查询 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(StoreQueryPageUserCommand cmd, CancellationToken cancellationToken)
        {
            var list = new List<SugarParameter>
            {
                new SugarParameter("@prarms", cmd.Condition, false),
                new SugarParameter("@pageSize", cmd.PageSize, false),
                new SugarParameter("@pageNum", cmd.PageNum, false),
                new SugarParameter("@totalSize", 0, true),
                new SugarParameter("@totalPages", 0, true)
            };

            //var list = _unitOfWork.GetSugarParameters(new StoreQueryPageUser()
            //{
            //    prarms = cmd.Condition,
            //    pageSize = cmd.PageSize,
            //    pageNum = cmd.PageNum,
            //    totalSize = 0,
            //    totalPages = 0
            //});

            try
            {
                //带out参数的方法
                var tuple = _userRepository.UseStoredProcedureToTuple("pro_uc_user_query", list);
                var dt = tuple.Item1;
                List<UserDto> dtos = dt.ToList<UserDto>();
                AddOrgAndRoleInfo(dtos);
                var totalpage = int.Parse(tuple.Item2[3].Value.ToString());
                return Succeed(new { list = dtos, total = totalpage });
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        private class StoreQueryPageUser
        {
            [StoreParamDirect]
            public string prarms { set; get; }

            [StoreParamDirect]
            public int pageSize { set; get; }

            [StoreParamDirect]
            public int pageNum { set; get; }

            [StoreParamDirect(ParameterDirection.Output)]
            public int totalSize { set; get; } = 0;

            [StoreParamDirect(ParameterDirection.Output)]
            public int totalPages { set; get; } = 0;
        }

        private ISugarQueryable<UserDto> GetQueryList(string strCondition, string strOrder)
        {
            var db = _unitOfWork.GetDbClient();
            //return db.Queryable<User, RoleUser, Role>
            //    ((u, ru, r) => new JoinQueryInfos(
            //        JoinType.Left, ru.UserId.Equals(u.Id),
            //        JoinType.Left, r.Id.Equals(ru.RoleId)
            //    ))
            //    .Where(u => u.State != BaseStateConstants.DELETE && !u.Id.Equals(SystemConstants.superId))
            //    .WhereIF(strCondition.NotNull(), strCondition)
            //    .OrderByIF(strOrder.NotNull(), strOrder)
            //    .OrderBy(u => u.CreateTime, OrderByType.Desc)
            //    .Select((u, ru, r) => new UserDto
            //    {
            //        Id = u.Id,
            //        Name = u.Name,
            //        FullName = u.FullName,
            //        Email = u.Email,
            //        Tel = u.Tel,
            //        Theme = u.Theme,
            //        State = u.State,
            //        CreateTime = u.CreateTime
            //    })
            //    .Distinct();

            //切换成sql server之后，不能使用 text字段

            return db.Queryable<User>("u")
                .Where(u => u.State != BaseStateConstants.DELETE && u.Id != SystemConstants.superId)
                .WhereIF(strCondition.NotNull(), strCondition)
                .OrderByIF(strOrder.NotNull(), strOrder)
                .OrderBy(u => u.CreateTime, OrderByType.Desc)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    FullName = u.FullName,
                    Email = u.Email,
                    Tel = u.Tel,
                    Theme = u.Theme,
                    State = u.State,
                    CreateTime = u.CreateTime,
                    UpdateTime = u.UpdateTime,
                    Creator = u.Creator,
                    Updator = u.Updator
                });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userList"></param>
        private void AddOrgAndRoleInfo(List<UserDto> userList)
        {
            var ids = userList.Select(u => u.Id).ToList();
            var userMoreList = FindUsersAllRole(ids);

            foreach (var userDto in userList)
            {
                foreach (var userMoredto in userMoreList)
                {
                    if (userDto.Id.Equals(userMoredto.UserId))
                    {
                        if (userMoredto.RoleName.NotNull())
                        {
                            AddRoleIdAndName(userDto, userMoredto.RoleId, userMoredto.RoleName);
                        }
                    }
                }
            }
        }

        private void AddRoleIdAndName(UserDto userDto, string roleId, string roleName)
        {
            if (userDto.RoleName.IsEmptyZero())
            {
                userDto.RoleId = roleId;
                userDto.RoleName = roleName;
            }
            else
            {
                if (!userDto.RoleId.Contains(roleId))
                {
                    userDto.RoleId += "," + roleId;
                    userDto.RoleName += "," + roleName;
                }
            }
        }

        private List<RoleDto> FindUsersAllRole(List<string> ids)
        {
            var db = _unitOfWork.GetDbClient();
            var list = db
                .Queryable<RoleUser, Role>
                ((ru, r) => new JoinQueryInfos(
                    JoinType.Left, r.Id == ru.RoleId
                ))
                .Where((ru, r) => ids.Contains(ru.UserId) && r.State != BaseStateConstants.DELETE)
                .Select((ru, r) => new RoleDto
                {
                    UserId = ru.UserId,
                    RoleId = r.Id,
                    RoleName = r.Name
                })
                .Distinct()
                .ToList();

            return list;
        }

        #region 导入导出

        [Transaction]
        public Task<PubResponse> Handle(ImportUserCommand cmd, CancellationToken cancellationToken)
        {
            var dt = cmd.dataTable;
            int count = dt.Rows.Count;
            if (count <= 0)
            {
                return Failed(UserCenterError.EXCEL_IS_NULL);
            }

            List<User> addUsers = new List<User>();
            List<User> updateUsers = new List<User>();
            var userType = "SYS_USER";//UserTypeEnum.SYS_USER.Name;

            List<string> names = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var r = dt.Rows[i];
                var name = r["账号"].ToString();
                if (name.NotNull())
                {
                    if (!names.Contains(name))
                    {
                        names.Add(name);
                    }
                    else
                    {
                        return Failed(UserCenterError.USER_IS_REPEAT_EXCEL);
                    }
                }
            }

            var userlist = _userRepository.QueryableToList(u => names.Contains(u.Name));
            var udict = userlist.ToDictionary(i => i.Name, i => i);

            List<User> addList = new List<User>();
            List<User> updateList = new List<User>();
            for (int i = 0; i < count; i++)
            {
                var r = dt.Rows[i];
                var name = r["账号"].ToString();

                //超级管理员 不许修改
                if (name.Equals(SystemConstants.superName))
                {
                    continue;
                }
                string fullName = r["姓名"].ToString();
                //string account = r["工号"].ToString();
                string email = r["邮箱"].ToString();
                string tel = r["手机"].ToString();
                //string remark = r["备注"].ToString();
                if (udict.TryGetValue(name, out var user))
                {
                    user.FullName = fullName;
                    user.Email = email;
                    user.Tel = tel;
                    user.UserType = userType;
                    updateList.Add(user);
                }
                else
                {
                    user = new User
                    {
                        Name = name,
                        FullName = fullName,
                        Email = email,
                        Tel = tel,
                        UserType = userType
                    };

                    ChangePassword(user, SystemConstants.defaultPassword);
                    addList.Add(user);
                }
            }
            if (addList.NotNullT())
            {
                _userRepository.Insert(addList);
            }

            if (updateList.NotNullT())
            {
                _userRepository.Update(updateList);
            }

            return Succeed();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(ExportUserCommand cmd, CancellationToken cancellationToken)
        {
            var userList = GetQueryList(cmd.Condition, cmd.Order).ToList();

            List<UserDto> listResult = userList.Adapt<List<UserDto>>();
            AddOrgAndRoleInfo(listResult);

            List<UserOutExcelResp> outExcelDtoList = new List<UserOutExcelResp>();
            foreach (var u in listResult)
            {
                var r = u.Adapt<UserOutExcelResp>();
                r.State = EnumUtils.GetEnumDescription<EnableState>(u.State);
                outExcelDtoList.Add(r);
            }

            return Succeed(outExcelDtoList);
        }

        /// <summary>
        /// 导出 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(StoreExportUserCommand cmd, CancellationToken cancellationToken)
        {
            var list = new List<SugarParameter>
            {
                new SugarParameter("@prarms", cmd.Condition, false)
            };

            try
            {
                var dt = _userRepository.UseStoredProcedureToDataTable("pro_uc_user_export", list);

                List<UserDto> listResult = dt.ToList<UserDto>();
                if (listResult.Count == 0)
                {
                    return Succeed(null);
                }
                AddOrgAndRoleInfo(listResult);

                List<UserOutExcelResp> outExcelDtoList = new List<UserOutExcelResp>();
                foreach (var u in listResult)
                {
                    var r = u.Adapt<UserOutExcelResp>();
                    r.State = EnumUtils.GetEnumDescription<EnableState>(u.State);
                    outExcelDtoList.Add(r);
                }

                return Succeed(outExcelDtoList);
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(ImportExcelWithAreaUserCommand cmd, CancellationToken cancellationToken)
        {
            return Succeed(new List<string>());
        }

        #endregion 导入导出

        /// <summary>
        /// 根据用户id列表查询用户列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryUserByIdsUserCommand cmd, CancellationToken cancellationToken)
        {
            cmd.List.Remove(SystemConstants.superId);//过滤超级管理员

            List<User> users = _userRepository.Queryable().In(cmd.List).ToList();
            var dtolist = users.Adapt<List<UserDto>>();
            return Succeed(dtolist);
        }

        /// <summary>
        /// 根据 user Id 获取用户组list
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryUserListByIdCommand cmd, CancellationToken cancellationToken)
        {
            User user = null;
            if (!cmd.Id.Equals(SystemConstants.superId))//过滤超级管理员
            {
                user = _userRepository.Queryable().InSingle(cmd.Id);
            }

            return GetUserGroupByUser(user);
        }

        /// <summary>
        /// 根据 user Name 获取用户组list
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(QueryUserListByNameCommand cmd, CancellationToken cancellationToken)
        {
            User user = _userRepository.QueryableToEntity(u => u.Name == cmd.Name);
            return GetUserGroupByUser(user);
        }

        /// <summary>
        /// 根据 user info 查询用户组list
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Task<PubResponse> GetUserGroupByUser(User user)
        {
            if (user.IsNull())
            {
                return Failed(BaseSystemError.OBJECT_DOES_NOT_EXIST);
            }

            //List<UsergroupUser> list = _usergroupUserRepository.QueryableToList(ugu => ugu.UserId.Equals(user.Id));

            //List<string> groupIds = list.Select(ugu => ugu.UsergroupId).ToList();
            //List<Usergroup> groupList = _userGroupRepository.Queryable().In(groupIds).ToList();

            //return SucceedOrFail(groupList.Count == groupIds.Count, groupList, BaseSystemError.OBJECT_DOES_NOT_EXIST);
            return null;
        }

        /// <summary>
        /// 根据user id修改user status
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        public Task<PubResponse> Handle(UpdateStatusUserCommand cmd, CancellationToken cancellationToken)
        {
            return EnabledStateus(cmd.List, cmd.State, _userRepository);
        }

        /// <summary>
        /// 根据user id修改user status 存储过程方式
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Transaction]
        public Task<PubResponse> Handle(StoreUpdateStatusUserCommand cmd, CancellationToken cancellationToken)
        {
            var list = new List<SugarParameter>();
            string ids = cmd.List.Parse2StoreString();
            list.Add(new SugarParameter("@ids", ids, false));
            list.Add(new SugarParameter("@state", cmd.State, false));
            try
            {
                var dt = _userRepository.UseStoredProcedureToDataTable("pro_uc_user_update_state", list);
                return Succeed();
            }
            catch (Exception e)
            {
                return Failed(e.Message);
            }
        }

        /// <summary>
        /// 工位登录
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(IPCLoginUserCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd.ShiftCode.IsNull())
            {
                return Failed(UserCenterError.SHIFT_IS_NULL);
            }

            if (cmd.OpName.IsNull())
            {
                return Failed(UserCenterError.WORKSTATION_NAME_IS_NULL);
            }

            // 查询班次是否存在
            Task<PubResponse> _task1 = _bus.SendAsync(new QueryExistByCodeShiftCommand() { Code = cmd.ShiftCode });
            if (_task1.Result.Status == ResultStatusConstants.SUCCESS)
            {
                if (!(bool)_task1.Result.Data)
                {
                    return Failed(UserCenterError.SHIFT_DOES_NOT_EXIST);
                }
            }
            else
            {
                return Failed(UserCenterError.SHIFT_DOES_NOT_EXIST);
            }

            // 查询工位是否存在
            Task<PubResponse> _task2 = _bus.SendAsync(new QueryExistByNameWorkStationCommand() { Name = cmd.OpName, LineCode = cmd.LineCode });
            if (_task2.Result.Status == ResultStatusConstants.SUCCESS)
            {
                if (!(bool)_task2.Result.Data)
                {
                    return Failed(UserCenterError.WORKSTATION_DOES_NOT_EXIST);
                }
            }
            else
            {
                return Failed(UserCenterError.WORKSTATION_DOES_NOT_EXIST);
            }

            // 查询工位下时候存在用户
            Task<PubResponse> _task3 = _bus.SendAsync(new QueryUserExistWorkStationCommand() { OpName = cmd.OpName, WorkStationId = cmd.WorkStationId, Name = cmd.Name, LineCode = cmd.LineCode });
            if (_task3.Result.Status == ResultStatusConstants.SUCCESS)
            {
                if (!(bool)_task3.Result.Data)
                {
                    return Failed(UserCenterError.USER_NOT_IN_WORKSTATION);
                }
            }
            else
            {
                return Failed(UserCenterError.USER_NOT_IN_WORKSTATION);
            }

            return Handle(new LoginUserCommand()
            {
                Name = cmd.Name,
                Password = cmd.Password,
                LoginType = cmd.LoginType
            },
            cancellationToken);
        }

        /// <summary>
        /// IPC的物料解绑用户权限校验
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(IPCCheckBindUserCommand cmd, CancellationToken cancellationToken)
        {
            // 查询用户是否存在
            var user = _userRepository.QueryableToEntity(u => u.Name == cmd.Name);
            if (user.IsNull())
            {
                //RecordLoginFailed(user);
                return Failed(BaseSystemError.USERNAME_OR_PASSWORD_ERROR);
            }

            // 校验密码
            if (!VerifyPassword(user, cmd.Password))
            {
                //RecordLoginFailed(user);
                return Failed(BaseSystemError.USERNAME_OR_PASSWORD_ERROR);
            }

            // 校验状态
            if (user.State == BaseStateConstants.DEACTIVE || user.State == BaseStateConstants.DELETE)
            {
                //RecordLoginFailed(user);
                return Failed(BaseSystemError.USER_DEACTIVE);
            }
            //校验角色
            var db = _unitOfWork.GetDbClient();
            var list = db.Queryable<RoleUser, Role>((ru, r) => new JoinQueryInfos(JoinType.Left, r.Id == ru.RoleId))
                .Where((ru, r) => ru.UserId == user.Id && r.State != BaseStateConstants.DELETE && r.Name == UserConstants.BIND_ROLE)
                .ToList();
            if (list.Count > 0)
            {
                return Succeed();
            }
            else
            {
                return Failed(UserCenterError.PERMISSION_NOT_FOUND);
            }
        }
        /// <summary>
        /// IPC的按钮校验
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PubResponse> Handle(IPCCheckBtnUserCommand cmd, CancellationToken cancellationToken)
        {
            // 查询用户是否存在
            var user = _userRepository.QueryableToEntity(u => u.Name == cmd.Name);
            if (user.IsNull())
            {
                //RecordLoginFailed(user);
                return Failed(BaseSystemError.USERNAME_OR_PASSWORD_ERROR);
            }

            // 校验密码
            if (!VerifyPassword(user, cmd.Password))
            {
                //RecordLoginFailed(user);
                return Failed(BaseSystemError.USERNAME_OR_PASSWORD_ERROR);
            }

            // 校验状态
            if (user.State == BaseStateConstants.DEACTIVE || user.State == BaseStateConstants.DELETE)
            {
                //RecordLoginFailed(user);
                return Failed(BaseSystemError.USER_DEACTIVE);
            }
            //查询角色
            List<string> roles = new List<string>();
            Task<PubResponse> _task = _bus.SendAsync(new MDCenter.Commands.Globalsetting.QueryByNameGlobalsettingCommand() { Name = "systemConfig" });
            if (_task.Result.Status == ResultStatusConstants.SUCCESS)
            {
                var listBtn = (MDCenter.Commands.Globalsetting.QueryByNameGlobalsettingCommand)_task.Result.Data;
                if (!listBtn.IsNullT())
                {
                    var system = Newtonsoft.Json.JsonConvert.DeserializeObject<btnGloabl>(listBtn.Value); //listBtn.Value;
                    roles = system.roles;
                }
            }
            else
            {
                return Failed(UserCenterError.SHIFT_DOES_NOT_EXIST);
            }
            if (roles.IsNullT())
            {
                return Failed(UserCenterError.PERMISSION_NOT_FOUND);
            }

            //校验角色
            var db = _unitOfWork.GetDbClient();
            var list = db.Queryable<RoleUser, Role>((ru, r) => new JoinQueryInfos(JoinType.Left, r.Id == ru.RoleId))
                .Where((ru, r) => ru.UserId == user.Id && r.State != BaseStateConstants.DELETE && roles.Contains(r.Id))
                .ToList();
            if (list.Count > 0)
            {
                return Succeed();
            }
            else
            {
                return Failed(UserCenterError.PERMISSION_NOT_FOUND);
            }
        }
        public class btnGloabl
        {
            public List<string> roles { get; set; }
        }
        public Task<PubResponse> Handle(FindUserTokenCommand cmd, CancellationToken cancellationToken)
        {
            var ex = DateTime.Now.AddDays(1);
            var token = getToken(ex);
            UserLoginResp data = new UserLoginResp();
            data.Token = token;
            data.ExpireTime = ex;
            return Succeed(data);
        }

        private string getToken(DateTime exTime)
        {
            // 生成jwt
            JwtConfig jwt = _configuration?.GetSection("Jwt")?.Get<JwtConfig>();
            var claims = new List<Claim>()
                        {
                           new Claim("loginType","WEB"),
                           new Claim("userFullName","system"),
                           new Claim("userName","sysytem"),
                           new Claim("userId","00000000-0000-0000-0000-000000000000"),
                           new Claim("jti",Guid.NewGuid().ToString()),
                           new Claim("enabled","true")
                        };

            return jwt.CreateToken(claims, exTime);
        }
    }
}