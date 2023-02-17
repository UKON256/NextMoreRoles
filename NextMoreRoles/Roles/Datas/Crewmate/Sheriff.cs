using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles;

class Sheriff : RoleBase
{
    public static readonly Color RoleColor = new Color32(250, 190, 20, byte.MaxValue);

    public Sheriff() : base(RoleId.Sheriff.ToString(), RoleId.Sheriff, RoleType.Crewmate, RoleColor, RoleTypes.Crewmate) { }

    public static CustomRoleOption SheriffOption;
    public static CustomOption SheriffFireCool;
    public static CustomOption SheriffCanFireLimit;
    public static CustomOption SheriffCanFireMadmate;
    public static CustomOption SheriffCanFireNeutral;
    public override void SetUpRoleOptions() {
        SheriffOption = CustomRoleOption.Create(2001010, this);
        SheriffFireCool = CustomOption.Create(2001020, CustomOptionType.Crewmate, "FireCool", DefaultCooldown[0], DefaultCooldown[1], DefaultCooldown[2], DefaultCooldown[3], SheriffOption);
        SheriffCanFireLimit = CustomOption.Create(2001030, CustomOptionType.Crewmate, "AbilityLimit", AbilityLimit[0], AbilityLimit[1], AbilityLimit[2], AbilityLimit[3], SheriffOption);
        SheriffCanFireMadmate = CustomOption.Create(2001040, CustomOptionType.Crewmate, "CanFireMadmate", true, SheriffOption);
        SheriffCanFireNeutral = CustomOption.Create(2001050, CustomOptionType.Crewmate, "CanFireNeutral", true, SheriffOption);
    }

    public static float FireCool;
    public static int CanFireLimit;
    public static bool CanFireMadmate;
    public static bool CanFireNeutral;
    public override void ClearAndReload() {
        FireCool = SheriffFireCool.GetFloat();
        CanFireLimit = SheriffCanFireLimit.GetInt();
        CanFireMadmate = SheriffCanFireMadmate.GetBool();
        CanFireNeutral = SheriffCanFireNeutral.GetBool();
    }
}
