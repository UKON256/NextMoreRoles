using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Roles;

public enum RoleType
{
    Crewmate,
    Impostor,
    Neutral,
    Attribute,
    CrewmateGhost,
    ImpostorGhost,
}

public class RoleBase
{
    public static List<RoleBase> Roles = new();

    //* オプション関係 *//
    public List<float> CrewmatePlayers = new() { 1f, 1f, 15f, 1f };         //デフォルト、最小、最大、増幅値        クルー全員の設定値テンプレ
    public List<float> ImpostorPlayers = new() { 1f, 1f, 3f, 1f };          //デフォルト、最小、最大、増幅値        インポの数テンプレ
    public List<float> OptionsCooldown = new() { 20f, 2.5f, 150f, 2.5f };   //デフォルト、最小、最大、増幅値        クールダウンテンプレ
    public List<int> OptionsAbilityLimit = new() { 1, 1, 99, 1 };           //デフォルト、最小、最大、増幅値        能力の使用制限回数テンプレ

    //* 役職の基本情報 *//
    public List<PlayerControl> Players;
    public string RoleNameKey;
    public RoleType RoleType;
    public RoleId RoleId;
    public Color32 RoleNameColor;
    public RoleTypes IntroSound;

    //* 役職の可能なこと *//
    public bool CanUse = true;
    public bool CanReport = true;
    public bool CanKill = false;
    public bool CanVent = false;
    public bool CanSabotage = false;
    public bool CanSeeRole = false;
    public bool HasTask = true;

    //* 宣言 *//
    public RoleBase(RoleType RoleType,
                    RoleId RoleId,
                    Color32 RoleNameColor,
                    RoleTypes IntroSound,
                    bool CanUse = true,
                    bool CanReport = true,
                    bool CanKill = false,
                    bool CanVent = false,
                    bool CanSabotage = false,
                    bool CanSeeRole = false,
                    bool HasTask = true)
    {
        this.RoleType = RoleType;
        this.RoleId = RoleId;
        this.RoleNameColor = RoleNameColor;
        this.IntroSound = IntroSound;

        this.CanUse = CanUse;
        this.CanReport = CanReport;
        this.CanKill = CanKill;
        this.CanVent = CanVent;
        this.CanSabotage = CanSabotage;
        this.CanSeeRole = CanSeeRole;
        this.HasTask = HasTask;

        Roles.Add(this);
    }

    //* 関数 *//
    public virtual void SetUpRoleOptions() { }
    public virtual void WhenGameStart() { }
    public virtual void WhenMeetingStart() { }
    public virtual void WhenMeetingEnds() { }
    public virtual void WhenSetRole() { }
    public virtual void WhenEraseRole() { }
    public virtual void WhenDead() { }
    public virtual void WhenRevive() { }
    public virtual void FixedUpdate() { }
    public virtual void HudUpdate() { }

    //* 戻り値ありの関数 *//
    public bool IsCrewmateRole() => this.RoleType == RoleType.Crewmate;
    public bool IsImpostorRole() => this.RoleType == RoleType.Impostor;
    public bool IsNeutralRole() => this.RoleType == RoleType.Neutral;
    public bool IsAttributeRole() => this.RoleType == RoleType.Attribute;
    public bool IsGhostRole() => this.RoleType is RoleType.CrewmateGhost or RoleType.ImpostorGhost;
    public bool IsCrewmateGhostRole() => this.RoleType == RoleType.CrewmateGhost;
    public bool IsImpostorGhostRole() => this.RoleType == RoleType.ImpostorGhost;

    public List<PlayerControl> AllThisRolePlayer { get{ return Players; } }
    public List<PlayerControl> AliveThisRolePlayer { get{ return Players.Where(x => x.Data.IsDead).ToList(); } }
    public List<PlayerControl> DeadThisRolePlayer { get{ return Players.Where(x => !x.Data.IsDead).ToList(); } }

    public virtual RoleBase GetRoleInfo(RoleId RoleId) => Roles.FirstOrDefault(x => x.RoleId == RoleId);
    public virtual string GetRoleName() => Translator.GetString(this.RoleId.ToString());
    public virtual Color32 GetNameColor() => this.RoleNameColor;
    public virtual string GetColoredRoleName() => ModHelpers.cs(this.RoleNameColor, Translator.GetString(this.RoleId.ToString()));
    public virtual string GetIntroDescription() => Translator.GetString($"{this.RoleId}IntroDescription");
    public virtual string GetGameDescription() => Translator.GetString($"{this.RoleId}GameDescription");
}
