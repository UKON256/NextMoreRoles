using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles;

class SerialKiller : RoleBase
{
    public static readonly Color RoleColor = Palette.ImpostorRed;

    public SerialKiller() : base(RoleId.SerialKiller.ToString(), RoleId.SerialKiller, RoleType.Impostor, RoleColor, RoleTypes.Impostor, false) { }

    public static CustomRoleOption SerialKillerOption;
    public static CustomOption SerialKillerKillCool;
    public static CustomOption SerialKillerSucideTime;
    public static CustomOption SerialKillerIsCountTimerOnMeeting;
    public override void SetUpRoleOptions() {
        SerialKillerOption = CustomRoleOption.Create(3001010, this);
        SerialKillerKillCool = CustomOption.Create(3001020, CustomOptionType.Impostor, "KillCool", DefaultCooldown[0], DefaultCooldown[1], DefaultCooldown[2], DefaultCooldown[3], SerialKillerOption);
        SerialKillerSucideTime = CustomOption.Create(3001030, CustomOptionType.Impostor, "SucideTime", LongCooldown[0], LongCooldown[1], LongCooldown[2], LongCooldown[3], SerialKillerOption);
        SerialKillerIsCountTimerOnMeeting = CustomOption.Create(3001040, CustomOptionType.Impostor, "IsCountOnMeeting", false, SerialKillerOption);
    }

    public static float KillCool;
    public static float SucideTime;
    public static bool IsCountOnMeeting;
    public override void ClearAndReload() {
        KillCool = SerialKillerKillCool.GetFloat();
        SucideTime = SerialKillerSucideTime.GetFloat();
        IsCountOnMeeting = SerialKillerIsCountTimerOnMeeting.GetBool();
    }
}
