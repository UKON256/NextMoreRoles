using UnityEngine;
using AmongUs.GameOptions;

namespace NextMoreRoles.Roles;

public class CombinationRoleBase : RoleBase
{
    public CombinationRoleBase() {}
    public CombinationRoleBase(
            string RoleNameKey,
            RoleId RoleId,
            RoleType RoleType,
            Color RoleNameColor,
            RoleTypes IntroSound,
            bool CanSeeRole = false,
            bool HasTask = true)
    : base(
            RoleNameKey,
            RoleId,
            RoleType,
            RoleNameColor,
            IntroSound,
            CanSeeRole,
            HasTask
    ) {}
}
