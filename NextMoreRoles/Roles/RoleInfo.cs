using System;
using System.Collections.Generic;
using UnityEngine;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Roles
{
    public class RoleInfo
    {
        public string Name;
        public Color IntroColor;
        public string IntroDescription;         //イントロ時の説明文
        public string GameDescription;          //試合時の説明文(タスクリストにのるやつ)
        public RoleId RoleId;
        public RoleType Team;

        public RoleInfo(string RoleName, Color IntroColor, RoleId RoleId, RoleType Team)
        {
            this.Name = RoleName;
            this.IntroColor = IntroColor;
            this.IntroDescription = ModTranslation.GetString($"{RoleName}IntroDescription");
            this.GameDescription = ModTranslation.GetString($"{RoleName}GameDescription");
            this.RoleId = RoleId;
            this.Team = Team;
            IntroDatas.Add(this);
        }
        public static RoleInfo GetRoleInfo(RoleId RoleId)
        {
            try
            {
                return IntroDatasCache[RoleId];
            }
            catch(SystemException Error)
            {
                Logger.Error($"RoleInfoの取得に失敗しました。エラー:{Error}", "RoleInfo");
                return null;
            }
        }



        public static List<RoleInfo> IntroDatas = new();                       //イントロのリスト
        public static Dictionary<RoleId, RoleInfo> IntroDatasCache = new();    //紐づけて受け取るとき軽くする
        //=====クルーメイト陣営=====//
        public RoleInfo Crewmate = new("Crewmate", RoleBase.CrewmateBlue, RoleId.Crewmate, RoleType.Crewmate);

        //=====インポスター陣営=====//
        public RoleInfo Impostor = new("Impostor", RoleBase.ImpostorRed, RoleId.Impostor, RoleType.Impostor);

        //=====第三陣営=====//

        //=====重複=====//
    }



    public enum RoleType
    {
        Crewmate,
        Impostor,
        Neutral,
    }
}
