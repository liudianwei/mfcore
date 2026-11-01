using System.Collections.Generic;
using DAL.UserCenter.Entities;
using MF.Utils;
using SqlSugar;

namespace UserCenter.CommandHandles
{
    internal static class PermissionAssignmentScope
    {
        public static List<string> GetOwnedPermissionIds(SqlSugarClient db, string userId)
        {
            return db.Queryable<Permission, RolePermission, Role, RoleUser>(
                (p, rp, r, ru) => new JoinQueryInfos(
                    JoinType.Inner, rp.PermissionId == p.Id,
                    JoinType.Inner, r.Id == rp.RoleId,
                    JoinType.Inner, ru.RoleId == r.Id))
                .Where((p, rp, r, ru) => ru.UserId == userId
                    && p.State == BaseStateConstants.ACTIVATE
                    && rp.State == BaseStateConstants.ACTIVATE
                    && r.State == BaseStateConstants.ACTIVATE
                    && ru.State == BaseStateConstants.ACTIVATE)
                .Select((p, rp, r, ru) => p.Id)
                .Distinct()
                .ToList();
        }
    }
}
