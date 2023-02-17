using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles;

public enum RoleType
{
    Crewmate,
    Impostor,
    Neutral,
    Attribute,
    Madmate,
    CrewmateGhost,
    ImpostorGhost,
}

public class RoleBase
{
    public static Dictionary<RoleId, RoleBase> RoleBaseCaches = new();
    public static List<RoleBase> Roles = new();
    public static void Load()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (!typeof(RoleBase).IsAssignableFrom(type) || type.IsAbstract) continue;

            var roleId = (RoleId)Enum.Parse(typeof(RoleId), type.Name);
            var roleBase = Activator.CreateInstance(type) as RoleBase;
            RoleBaseCaches[roleId] = roleBase;
        }
    }

    //* オプションテンプレ *//
    public static List<float> CrewmatePlayers = new() { 1f, 1f, 15f, 1f };         //デフォルト、最小、最大、増幅値        クルー全員の設定値テンプレ
    public static List<float> ImpostorPlayers = new() { 1f, 1f, 3f, 1f };          //デフォルト、最小、最大、増幅値        インポの数テンプレ
    public static List<float> DefaultCooldown = new() { 30f, 2.5f, 60f, 2.5f };    //デフォルト、最小、最大、増幅値        短めクールダウンテンプレ
    public static List<float> LongCooldown = new() { 60f, 2.5f, 150f, 2.5f };      //デフォルト、最小、最大、増幅値        長めクールダウンテンプレ
    public static List<int> AbilityLimit = new() { 1, 1, 99, 1 };                  //デフォルト、最小、最大、増幅値        能力の使用制限回数テンプレ

    //* 役職の基本情報 *//
    public List<PlayerControl> HistoryBeenPlayers;       //試合中になったことのある人たち用のリスト
    public string RoleNameKey;
    public RoleId RoleId;
    public RoleType RoleType;
    public Color RoleNameColor;
    public RoleTypes IntroSound;

    //* 役職の可能なこと *//
    public bool HasTask = true;
    public bool CanSeeRole = false;

    //* デフォルトコンストラクタとコンストラクタ *//
    public RoleBase() { }
    public RoleBase(
            string RoleNameKey,
            RoleId RoleId,
            RoleType RoleType,
            Color RoleNameColor,
            RoleTypes IntroSound,
            bool HasTask = true,
            bool CanSeeRole = false
            )
    {
        this.RoleNameKey = RoleNameKey;
        this.RoleId = RoleId;
        this.RoleType = RoleType;
        this.RoleNameColor = RoleNameColor;
        this.IntroSound = IntroSound;

        this.HasTask = HasTask;
        this.CanSeeRole = CanSeeRole;

        Roles.Add(this);
    }

    //* 関数 *//
    public virtual void SetUpRoleOptions() { }
    public virtual void ClearAndReload() { }
    public virtual void WhenMeetingStart() { }
    public virtual void WhenMeetingEnds() { }
    public virtual void WhenSetRole() { }
    public virtual void WhenEraseRole() { }
    public virtual void WhenKill(PlayerControl killTarget) { }
    public virtual void WhenDead(PlayerControl killer = null, bool exiled = false) { }
    public virtual void FixedUpdate() { }
    public virtual void HudUpdate() { }

    public bool IsNormalRole() => this.RoleType is RoleType.Crewmate or RoleType.Impostor or RoleType.Neutral or RoleType.Madmate;
    public bool IsVanillaRole() => this.RoleId == RoleId.VanillaRoles;
    public bool IsCrewmateRole() => this.RoleType == RoleType.Crewmate;
    public bool IsImpostorRole() => this.RoleType == RoleType.Impostor;
    public bool IsNeutralRole() => this.RoleType == RoleType.Neutral;
    public bool IsAttributeRole() => this.RoleType == RoleType.Attribute;
    public bool IsMadRole() => this.RoleType == RoleType.Attribute;
    public bool IsGhostRole() => this.RoleType is RoleType.CrewmateGhost or RoleType.ImpostorGhost;
    public bool IsCrewmateGhostRole() => this.RoleType == RoleType.CrewmateGhost;
    public bool IsImpostorGhostRole() => this.RoleType == RoleType.ImpostorGhost;

    //* RoleBase関係の取得 *//
    public RoleBase GetRoleBase() => RoleBaseCaches[this.RoleId];
    public string GetRoleName() => Translator.GetString(this.RoleNameKey);
    public string GetColoredRoleName() => ModHelpers.cs(this.RoleNameColor, Translator.GetString(this.RoleNameKey));
    public string GetGameDescription() => Translator.GetString($"{this.RoleNameKey}GameDescription");
    public CustomRoleOption GetRoleOption() => CustomRoleOption.RoleOptions.FirstOrDefault(x => x.RoleId == this.RoleId);

    //* Intro関係の取得 *//
    public string GetIntroDescription() => Translator.GetString($"{this.RoleNameKey}Intro");
    public AudioClip GetIntroSound() => RoleManager.Instance.AllRoles.Where((role) => role.Role == this.IntroSound).FirstOrDefault().IntroSound;
}

[HarmonyPatch]
class RoleBaseHarmony
{
    //* ミーティング呼び出し時 *//
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.OpenMeetingRoom))]
    static void MeetingStart() {
        RoleHelper.GetLocalPlayerRole().WhenMeetingStart();
        RoleHelper.GetLocalPlayerAttribute().WhenMeetingStart();
    }

    //* ミーティング終了時 *//
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    static void ExileWrapUp() {
        RoleHelper.GetLocalPlayerRole().WhenMeetingEnds();
        RoleHelper.GetLocalPlayerAttribute().WhenMeetingEnds();
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
    static void AirShipExileWrapUp() {
        RoleHelper.GetLocalPlayerRole().WhenMeetingEnds();
        RoleHelper.GetLocalPlayerAttribute().WhenMeetingEnds();
    }

    //* キルした時、キルされたとき *//
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    static void DoKill(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target) {
        if (__instance.PlayerId == CachedPlayer.LocalPlayer.PlayerId) {
            RoleHelper.GetLocalPlayerRole().WhenKill(target);
            RoleHelper.GetLocalPlayerAttribute().WhenKill(target);
        } else if (target.PlayerId == CachedPlayer.LocalPlayer.PlayerId) {
            RoleHelper.GetLocalPlayerRole().WhenDead(__instance);
            RoleHelper.GetLocalPlayerAttribute().WhenDead(__instance);
        }
    }
    //* 追放時 *//
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    static void Exiled() {
        RoleHelper.GetLocalPlayerRole().WhenDead(exiled:true);
        RoleHelper.GetLocalPlayerAttribute().WhenDead(exiled:true);
    }

    //* FixedUpdate *//
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    static void FixedUpdate() {
        //RoleHelper.GetLocalPlayerRole().FixedUpdate();
        //RoleHelper.GetLocalPlayerAttribute().FixedUpdate();
    }

    //* HudUpdate *//
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    static void HudUpdate() {
        //RoleHelper.GetLocalPlayerRole().HudUpdate();
        //RoleHelper.GetLocalPlayerAttribute().HudUpdate();
    }
}
