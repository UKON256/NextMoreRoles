using System.Collections.Generic;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Roles;

public static class RoleHelper
{
    //* プレイヤー関係 *//
    public static bool IsDead(this PlayerControl target) => target.Data.IsDead;
    public static bool IsAlive(this PlayerControl target) => !target.Data.IsDead;
    public static bool IsBot(this PlayerControl target)
    {
        if (target == null || target.Data.Disconnected) return false;
        foreach (PlayerControl Bots in BotManager.AllBots)
        {
            if (Bots.PlayerId == target.PlayerId) return true;
        }
        return false;
    }
    public static bool IsPlayer(this PlayerControl target) => !target.IsBot();



    //* 役職関係 *//
    public static Dictionary<int, RoleBase> RoleCache = new();          //PlayerId, Roleのキャッシュ
    public static Dictionary<int, RoleBase> AttributeCache = new();     //PlayerId, Roleのキャッシュ
    public static Dictionary<int, RoleBase> GhostRoleCache = new();     //PlayerId, Roleのキャッシュ

    public static bool IsCrewmate(this PlayerControl target) => target != null && target.GetRole().IsCrewmateRole();
    public static bool IsImpostor(this PlayerControl target) => target != null && target.GetRole().IsImpostorRole();
    public static bool IsNeutral(this PlayerControl target) => target != null && target.GetRole().IsNeutralRole();
    public static bool IsMad(this PlayerControl target ) => target != null && target.GetRole().IsMadRole();

    public static bool IsRole(this PlayerControl target, RoleId roleId) => target != null && target.GetRole().RoleId == roleId;
    public static bool IsAttribute(this PlayerControl target, RoleId roleId) => target != null && target.GetAttribute().RoleId == roleId;
    public static bool IsGhostRole(this PlayerControl target, RoleId roleId) => target != null && target.GetGhostRole().RoleId == roleId;

    public static bool HasAttribute(this PlayerControl target) => target != null && target.GetAttribute() != null;

    public static RoleBase GetLocalPlayerRole() => RoleCache[CachedPlayer.LocalPlayer.PlayerId];
    public static RoleBase GetLocalPlayerAttribute() => AttributeCache[CachedPlayer.LocalPlayer.PlayerId];
    public static RoleBase GetLocalPlayerGhostRole() => GhostRoleCache[CachedPlayer.LocalPlayer.PlayerId];

    public static RoleBase GetRole(this PlayerControl target) => RoleCache[target.PlayerId];
    public static RoleBase GetAttribute(this PlayerControl target) => AttributeCache[target.PlayerId];
    public static RoleBase GetGhostRole(this PlayerControl target) => GhostRoleCache[target.PlayerId];
}
