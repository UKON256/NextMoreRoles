using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles
{
    public class IntroData
    {
        public static List<IntroData> IntroDatas = new();                       //イントロのリスト
        public static Dictionary<RoleId, IntroData> IntroDatasCache = new();    //紐づけて受け取るとき軽くする
        public static List<IntroData> GhostRoleDatas = new();                   //幽霊役職のイントロ
        public static List<IntroData> AttributeRoleDatas = new();               //重複のイントロ
        public string Name;
        public string IntroDescription;
        public Color Color;
        public RoleId RoleId;
        public string GameDescription;
        public RoleType Team;
        public RoleTypes IntroSound;
        public bool IsGhostRole;
        public bool IsAttributeRole;
        IntroData(string Name, Color Color, RoleId RoleId, RoleType Team = RoleType.Crewmate, RoleTypes IntroSound = RoleTypes.Crewmate, bool IsGhostRole = false, bool IsAttributeRole = false)
        {
            this.Name = ModTranslation.GetString(Name);
            this.Color = Color;
            this.RoleId = RoleId;
            this.Team = Team;
            this.IntroSound = IntroSound;
            this.IsGhostRole = IsGhostRole;
            this.IsAttributeRole = IsAttributeRole;
            this.IntroDescription = ModTranslation.GetString(Name + "TitleDesc");
            this.GameDescription = ModTranslation.GetString(Name + "GameDesc");

            if (IsGhostRole)
            {
                GhostRoleDatas.Add(this);
            }
            if (IsAttributeRole)
            {
                AttributeRoleDatas.Add(this);
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
                //キャッシュ化させる
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
        public static void PlayIntroSound(RoleId RoleId)
        {
            var Info = GetIntroData(RoleId, PlayerControl.LocalPlayer);
            PlayerControl.LocalPlayer.Data.Role.IntroSound = RoleManager.Instance.AllRoles.Where((role) => role.Role == Info.IntroSound).FirstOrDefault().IntroSound;
            SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.Data.Role.IntroSound, false, 1);
        }



        //=====クルーメイト陣営=====//
        public static IntroData Crewmate = new("Crewmate", Palette.CrewmateBlue, RoleId.Crewmate, RoleType.Crewmate, RoleTypes.Crewmate);
        public static IntroData Sheriff = new("Sheriff", RoleClass.Sheriff.Color, RoleId.Sheriff, RoleType.Crewmate, RoleTypes.Engineer);

        //=====インポスター陣営=====//
        public static IntroData Impostor = new("Impostor", Palette.ImpostorRed, RoleId.Impostor, RoleType.Impostor, RoleTypes.Impostor);
        public static IntroData SerialKiller = new("SerialKiller", RoleClass.SerialKiller.Color, RoleId.SerialKiller, RoleType.Impostor, RoleTypes.Shapeshifter);
        public static IntroData Madmate = new("Madmate", RoleClass.Madmate.Color, RoleId.Madmate, RoleType.Impostor, RoleTypes.Impostor);

        //=====  ニュートラル  =====//
        public static IntroData Jackal = new("Jackal", RoleClass.Jackal.Color, RoleId.Jackal, RoleType.Neutral);
        public static IntroData SideKick = new("SideKick", RoleClass.SideKick.Color, RoleId.SideKick, RoleType.Neutral);


        //=====コンビネーション=====//

        //=====重複陣営=====//
        public static IntroData Debugger = new("Debugger", RoleClass.Debugger.Color, RoleId.Debugger, RoleType.Attribute, IsAttributeRole:true);
    }
}
