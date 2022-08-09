using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NextMoreRoles.Roles;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Modules.Intro
{
    public class IntroData
    {
        public static List<IntroData> GhostRoleDatas = new();
        public string Name;
        public string TitleDescription;
        public Color Color;
        public RoleId RoleId;
        public string GameDescription;
        public RoleType Team;
        //public IntroSoundType IntroSound;
        public bool IsGhostRole;
        IntroData(string Name, Color Color, RoleId RoleId, RoleType Team = RoleType.Crewmate, /*IntroSoundType IntroSound = IntroSoundType.Crewmate, */bool IsGhostRole = false)
        {
            this.Name = ModTranslation.GetString(Name);
            this.Color = Color;
            this.RoleId = RoleId;
            this.Team = Team;
            //this.IntroSound = IntroSound;
            this.IsGhostRole = IsGhostRole;
            this.TitleDescription = ModTranslation.GetString(Name + "TitleDesc");
            this.GameDescription = ModTranslation.GetString(Name + "GameDesc");

            if (IsGhostRole)
            {
                GhostRoleDatas.Add(this);
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
                if (Data == null) Data = Crewmate;
                IntroDatasCache[RoleId] = Data;
                return Data;
            }
        }
        public static CustomRoleOption GetOption(RoleId RoleId)
        {
            var Option = CustomRoleOption.RoleOptions.FirstOrDefault((_) => _.RoleId == RoleId);
            return Option;
        }



        public static List<IntroData> IntroDatas = new();                       //イントロのリスト
        public static Dictionary<RoleId, IntroData> IntroDatasCache = new();    //紐づけて受け取るとき軽くする
        //=====クルーメイト陣営=====//
        public static IntroData Crewmate = new("Crewmate", RoleBase.CrewmateBlue, RoleId.Crewmate, RoleType.Crewmate);
        //public static IntroData Sheriff = new("Sheriff", Color.yellow, RoleId.Sheriff, RoleType.Crewmate);

        //=====インポスター陣営=====//
        public static IntroData Impostor = new("Impostor", RoleBase.ImpostorRed, RoleId.Crewmate, RoleType.Impostor);
        public static IntroData Ninja = new("Ninja", RoleBase.ImpostorRed, RoleId.Ninja, RoleType.Impostor);
        public static IntroData Madmate = new("Madmate", RoleBase.ImpostorRed, RoleId.Madmate, RoleType.Impostor);

        //=====  ニュートラル  =====//
    }
}
