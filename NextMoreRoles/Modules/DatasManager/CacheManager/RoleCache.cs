using System.Collections.Generic;
using NextMoreRoles.Roles;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules.DatasManager
{
    class ResetRoleCache
    {
        //実行元:Modules.DatasManager.Reset.cs
        public static void ClearCache()
        {
            RoleCache.RoleChache = new();
            RoleCache.AttributeCache = new();
        }

        //実行元:Modules.DatasManager.Reset.cs
        public static void ClearAndReloads()
        {
            ResetRoleChache();
            ResetAttributeCache();
        }

        //全員の役職をキャッシュ化(紐づける)
        public static void ResetRoleChache()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                RoleCache.RoleChache[p.PlayerId] = p.GetRole(false);
            }
        }

        //自身の重複をキャッシュ化(紐づける)
        public static void ResetAttributeCache()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                RoleCache.AttributeCache[p.PlayerId] = p.GetAttribute(false);
            }
        }
    }

    class RoleCache
    {
        public static Dictionary<int, RoleId> RoleChache;             //PlayerIdとRoleId
        public static Dictionary<int, RoleId> AttributeCache;         //PlayerIdとRoleId
    }
}
