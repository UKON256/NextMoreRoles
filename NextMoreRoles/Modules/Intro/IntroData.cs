using System;
using UnityEngine;
using System.Collections.Generic;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.Intro
{
    public class IntroData
    {
        public static List<IntroData> IntroDatas = new();
        public static Dictionary<RoleId, IntroData> IntroDatasCache = new();
        public static List<IntroData> GhostRoleDatas = new();
        public string NameKey;
        public string Name;
        public Int16 TitleNum;
        public string TitleDesc;
        public Color Color;
        public RoleId RoleId;
        public string Description;
        public RoleType Team;
        public bool IsGhostRole;
        IntroData(string NameKey, Color Color, Int16 TitleNum, RoleId RoleId, RoleType Team = RoleType.Crewmate, bool IsGhostRole = false)
        {
            this.Color = Color;
            this.NameKey = NameKey;
            this.Name = ModTranslation.GetString(NameKey);
            this.RoleId = RoleId;
            this.TitleNum = TitleNum;
            this.TitleDesc = GetTitle(NameKey, TitleNum);
            this.Description = ModTranslation.GetString(NameKey + "Description");
            this.Team = Team;
            this.IsGhostRole = IsGhostRole;

            if (IsGhostRole)
            {
                GhostRoleDatas.Add(this);
            }
            IntroDatas.Add(this);
        }
    }
}
