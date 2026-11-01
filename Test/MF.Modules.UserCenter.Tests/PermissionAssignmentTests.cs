using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DAL.UserCenter.Entities;
using DAL.UserCenter.IRepository;
using MF.NetCoreApp;
using MF.Orm.UnitOfWork;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using UserCenter.CommandHandles;
using UserCenter.Commands;
using UserCenter.Enums;
using UserCenter.Dtos;
using Xunit;

namespace UserCenter.Tests
{
    public class PermissionAssignmentTests : IDisposable
    {
        private readonly string path = Path.Combine(Path.GetTempPath(), $"uc-permissions-{Guid.NewGuid():N}.db");
        private readonly SqlSugarClient db;

        public PermissionAssignmentTests()
        {
            db = new SqlSugarClient(new ConnectionConfig {
                DbType = DbType.Sqlite, ConnectionString = $"Data Source={path};Pooling=False",
                IsAutoCloseConnection = true, InitKeyType = InitKeyType.Attribute
            });
            db.CodeFirst.InitTables<Permission, RolePermission, Role, RoleUser>();
            db.CodeFirst.InitTables<User, PermissionPermissiongroup, Permissiongroup>();
            db.Insertable(new User { Id = "admin", Name = "admin", State = "0" }).ExecuteCommand();
            db.Insertable(new[] {
                new Role { Id = "owner", Name = "owner", State = "0" },
                new Role { Id = "target", Name = "target", State = "0" }
            }).ExecuteCommand();
            db.Insertable(new RoleUser { Id = "membership", RoleId = "owner", UserId = "admin", State = "0" }).ExecuteCommand();
            db.Insertable(new[] {
                new Permission { Id = "owned", Name = "owned", ParentId = "", State = "0" },
                new Permission { Id = "foreign", Name = "foreign", ParentId = "", State = "0", Creator = "admin|Admin" }
            }).ExecuteCommand();
            db.Insertable(new[] {
                new RolePermission { Id = "owner-owned", RoleId = "owner", PermissionId = "owned", State = "0" },
                new RolePermission { Id = "owner-foreign", RoleId = "owner", PermissionId = "foreign", State = "1" },
                new RolePermission { Id = "target-owned", RoleId = "target", PermissionId = "owned", State = "1" },
                new RolePermission { Id = "target-foreign", RoleId = "target", PermissionId = "foreign", State = "0" }
            }).ExecuteCommand();
        }

        [Fact]
        public async Task AssignOwnedPermission_PreservesPermissionsOutsideScope()
        {
            var result = await Assign("admin", new List<string> { "owned", "owned" });
            Assert.Equal("success", result.Status);
            Assert.Equal("0", State("target-owned"));
            Assert.Equal("0", State("target-foreign"));
        }

        [Fact]
        public async Task AssignUnownedPermission_RejectsEntireRequest()
        {
            var result = await Assign("admin", new List<string> { "owned", "foreign" });
            Assert.Equal("error", result.Status);
            Assert.Equal("DO_NOT_HAVE_PERMISSION", result.Message);
            Assert.Equal("1", State("target-owned"));
            Assert.Equal("0", State("target-foreign"));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ClearSelection_OnlyRevokesOwnedPermissions(bool nullList)
        {
            db.Updateable<RolePermission>().SetColumns(x => x.State == "0").Where(x => x.Id == "target-owned").ExecuteCommand();
            var result = await Assign("admin", nullList ? null : new List<string>());
            Assert.Equal("success", result.Status);
            Assert.Equal("1", State("target-owned"));
            Assert.Equal("0", State("target-foreign"));
        }

        [Fact]
        public async Task SuperUser_CanAssignUnownedPermissionAndClearAll()
        {
            Assert.Equal("success", (await Assign(SystemConstants.superId, new List<string> { "foreign" })).Status);
            Assert.Single(db.Queryable<RolePermission>().Where(x => x.RoleId == "target").ToList());
            Assert.Equal("success", (await Assign(SystemConstants.superId, new List<string>())).Status);
            Assert.Empty(db.Queryable<RolePermission>().Where(x => x.RoleId == "target").ToList());
        }

        [Theory]
        [InlineData("uc_permission")]
        [InlineData("uc_role_permission")]
        [InlineData("uc_role")]
        [InlineData("uc_role_user")]
        public async Task DisabledOwnershipLink_CannotGrantPermission(string table)
        {
            // Table names are fixed test cases, never external input.
            db.Ado.ExecuteCommand($"UPDATE {table} SET state = '1'");
            Assert.Equal("error", (await Assign("admin", new List<string> { "owned" })).Status);
        }

        [Fact]
        public async Task AssignNewOwnedPermission_InsertsRelationship()
        {
            db.Deleteable<RolePermission>().Where(x => x.Id == "target-owned").ExecuteCommand();
            Assert.Equal("success", (await Assign("admin", new List<string> { "owned" })).Status);
            Assert.Equal("0", db.Queryable<RolePermission>().Where(x => x.RoleId == "target" && x.PermissionId == "owned").Single().State);
        }

        [Fact]
        public async Task PermissionTree_ExcludesDisabledAndCreatorOnlyPermissions()
        {
            var core = CreateCore("admin");
            var handler = new PermissionCommandHandle(core, null, null, null,
                QueryProxy.Create<IUnitOfWork>(() => db));
            var result = await handler.Handle(new QueryPermissionTreeByRoleCommand(), CancellationToken.None);
            Assert.Equal("success", result.Status);
            var tree = Assert.IsType<List<PermissionTreeResp>>(result.Data);
            Assert.Equal("owned", Assert.Single(tree).Id);
        }

        [Fact]
        public async Task UserWithoutPermissions_CannotClearOtherPermissions()
        {
            var result = await Assign("unassigned", new List<string>());
            Assert.Equal("success", result.Status);
            Assert.Equal("0", State("target-foreign"));
            Assert.Equal("1", State("target-owned"));
        }

        private string State(string id) => db.Queryable<RolePermission>().InSingle(id).State;

        private Task<MF.FluentValidation.PubResponse> Assign(string userId, List<string> permissions)
        {
            var core = CreateCore(userId);
            var roles = QueryProxy.Create<IRoleRepository>(() => db.Queryable<Role>());
            var perms = QueryProxy.Create<IUcPermissionRepository>(() => db.Queryable<Permission>());
            var unit = QueryProxy.Create<IUnitOfWork>(() => db);
            var handler = new RoleCommandHandle(core, roles, null, null, null, perms, unit);
            return handler.Handle(new AssignPermissionToRoleRoleCommand { Id = "target", List = permissions }, CancellationToken.None);
        }

        private static GlobalCore CreateCore(string userId)
        {
            var context = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim("userId", userId), new Claim("userName", "admin"), new Claim("userFullName", "Admin")
            }, "test")) };
            return new GlobalCore(new HttpContextAccessor { HttpContext = context });
        }

        public void Dispose()
        {
            db.Dispose();
            File.Delete(path);
        }
    }

    public class QueryProxy : DispatchProxy
    {
        public Func<object> Result { get; set; }
        public static T Create<T>(Func<object> result) where T : class
        {
            var proxy = DispatchProxy.Create<T, QueryProxy>();
            ((QueryProxy)(object)proxy).Result = result;
            return proxy;
        }
        protected override object Invoke(MethodInfo method, object[] args)
        {
            if (method.Name == "Queryable" || method.Name == "GetDbClient") return Result();
            throw new NotSupportedException(method.Name);
        }
    }
}
