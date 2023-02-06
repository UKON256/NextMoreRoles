using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AmongUs.GameOptions;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles;

public class IntroData
{
    public static List<IntroData> IntroDatas = new();
    public static List<IntroData> GhostIntroDatas = new();
    public string Name;
    public string TitleDescription;
    public Color Color;
    public RoleId RoleId;
    public string GameDescription;
    public RoleType Team;
    public RoleTypes IntroSound;
    public bool IsGhostRole;
    IntroData(string Name, Color Color, RoleId RoleId, RoleType Team = RoleType.Crewmate, RoleTypes IntroSound = RoleTypes.Crewmate, bool IsGhostRole = false)
    {
        this.Name = Translator.GetString(Name);
        this.Color = Color;
        this.RoleId = RoleId;
        this.Team = Team;
        this.IntroSound = IntroSound;
        this.IsGhostRole = IsGhostRole;
        this.TitleDescription = Translator.GetString(Name + "TitleDesc");
        this.GameDescription = Translator.GetString(Name + "GameDesc");

        if (IsGhostRole)
        {
            GhostIntroDatas.Add(this);
        }
        IntroDatas.Add(this);
    }

    public static IntroData GetIntroData(RoleId RoleId, PlayerControl p = null)
    {
        try
        {
            return IntroDatasCache[RoleId];
        }
        catch
        {
            var Data = IntroDatas.FirstOrDefault((_) => _.RoleId == RoleId);
            /*if (Data == null) Data = Crewmate;
            IntroDatasCache[RoleId] = Data;*/
            return Data;
        }
    }
    public static CustomRoleOption GetOption(RoleId RoleId)
    {
        var Option = CustomRoleOption.RoleOptions.FirstOrDefault((_) => _.RoleId == RoleId);
        return Option;
    }
    public static AudioClip GetIntroSound(RoleTypes RoleType)
    {
        return RoleManager.Instance.AllRoles.Where((role) => role.Role == RoleType).FirstOrDefault().IntroSound;
    }
    public static Dictionary<RoleId, IntroData> IntroDatasCache = new();    //紐づけて受け取るとき軽くする

    //* ファイル作成がめんどいのでここで宣言 *//
    public static IntroData Crewmate = new("Crewmate", Palette.CrewmateBlue, RoleId.Crewmate, RoleType.Crewmate, RoleTypes.Crewmate);
    public static IntroData Impostor = new("Impostor", Palette.ImpostorRed, RoleId.Impostor, RoleType.Impostor, RoleTypes.Impostor);
}
