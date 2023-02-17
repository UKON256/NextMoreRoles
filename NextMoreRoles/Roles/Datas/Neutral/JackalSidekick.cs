using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles;

class JackalSidekick : RoleBase
{
    public static readonly Color RoleColor = Jackal.RoleColor;

    public JackalSidekick() : base(RoleId.JackalSidekick.ToString(), RoleId.JackalSidekick, RoleType.Neutral, RoleColor, RoleTypes.Shapeshifter) { }

    public static CustomOption JSCanCreateSidekick;
    public override void SetUpRoleOptions() {
        JSCanCreateSidekick = CustomOption.Create(2001051, CustomOptionType.Neutral, "SidekickCanCreateSidekick", false, Jackal.JackalCanCreateSidekick);
    }

    public static bool CanCreateSidekick;
    public override void ClearAndReload() {
        CanCreateSidekick = JSCanCreateSidekick.GetBool();
    }
}
