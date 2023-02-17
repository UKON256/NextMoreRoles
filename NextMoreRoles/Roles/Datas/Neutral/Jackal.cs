using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles;

class Jackal : RoleBase
{
    public static readonly Color RoleColor = new Color32(65, 105, 255, byte.MaxValue);

    public Jackal() : base(RoleId.Jackal.ToString(), RoleId.Jackal, RoleType.Neutral, RoleColor, RoleTypes.Shapeshifter) { }

    public static CustomRoleOption JackalOption;
    public static CustomOption JackalKillCool;
    public static CustomOption JackalCanVent;
    public static CustomOption JackalCanSabotage;
    public static CustomOption JackalCanCreateSidekick;
    public override void SetUpRoleOptions() {
        JackalOption = CustomRoleOption.Create(2001010, this);
        JackalKillCool = CustomOption.Create(2001020, CustomOptionType.Neutral, "KillCool", DefaultCooldown[0], DefaultCooldown[1], DefaultCooldown[2], DefaultCooldown[3], JackalOption);
        JackalCanVent = CustomOption.Create(2001030, CustomOptionType.Neutral, "CanVent", false, JackalOption);
        JackalCanSabotage = CustomOption.Create(2001040, CustomOptionType.Neutral, "CanSabotage", false, JackalOption);
        JackalCanCreateSidekick = CustomOption.Create(2001050, CustomOptionType.Neutral, "CanCreateSidekick", false, JackalOption);
    }

    public static float KillCool;
    public static bool CanVent;
    public static bool CanSabotage;
    public static bool CanCreateSidekick;
    public override void ClearAndReload() {
        KillCool = JackalKillCool.GetFloat();
        CanVent = JackalCanVent.GetBool();
        CanSabotage = JackalCanSabotage.GetBool();
        CanCreateSidekick = JackalCanCreateSidekick.GetBool();
    }
}
