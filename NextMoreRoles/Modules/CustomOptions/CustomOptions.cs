using UnityEngine;
using System.Collections.Generic;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.CustomOptions
{
    public class CustomOptions
    {
        //=====選択肢=====//
        public static string[] Rates = new string[] { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

        public static string[] Rates4 = new string[] { "0%", "25%", "50%", "75%", "100%" };

        public static string[] Presets = new string[] { "preset1", "preset2", "preset3", "preset4", "preset5"};


        public static CustomOption SpecialOptions;
        public static CustomOption HideSettings;

        //=====   メインタブ   =====//
        public static CustomOption PresetSelection;
        public static CustomOption CrewmateRolesMin;
        public static CustomOption CrewmateRolesMax;
        public static CustomOption NeutralRolesMin;
        public static CustomOption NeutralRolesMax;
        public static CustomOption ImpostorRolesMin;
        public static CustomOption ImpostorRolesMax;



        //=====クルーメイトタブ=====//
        public static CustomRoleOption MadmateOption;
        public static CustomOption MadmateCanVent;
        public static CustomOption MadmateIsImpostorView;                       //インポの視界
        public static CustomOption MadmateCanKnowImpostor;                      //インポを知れる
        public static CustomTasksOption MadmateTask;                            //マッドのインポを知るためのタスク

        public static CustomRoleOption SheriffOption;
        public static CustomOption SheriffFireCool;
        public static CustomOption SheriffCanFireLimit;
        public static CustomOption SheriffCanFireMadmate;
        public static CustomOption SheriffCanFireNeutral;



        //=====インポスタータブ=====//
        public static CustomRoleOption SerialKillerOption;
        public static CustomOption SerialKillerKillCool;
        public static CustomOption SerialKillerSelfDeathTime;                   //自殺時間
        public static CustomOption SerialKillerIsMeetingReset;                  //会議でリセット



        //=====  第三陣営タブ  =====//
        public static CustomRoleOption JackalOption;
        public static CustomOption JackalKillCool;
        public static CustomOption JackalCanVent;
        public static CustomOption JackalCanMakeSideKick;
        public static CustomOption JackalIsImpostorView;                        //インポと同じ視界



        //=====上限変えたきゃここ変えれば大丈夫なやつら(テンプレ)=====//
        public static List<float> CrewmatePlayers = new() { 1f, 1f, 15f, 1f };  //デフォルト、最小、最大、増幅値        クルー全員の設定値テンプレ
        public static List<float> ImpostorPlayers = new() { 1f, 1f, 3f, 1f };   //デフォルト、最小、最大、増幅値        インポの数テンプレ
        public static List<float> Cooldown = new() { 20f, 2.5f, 90f, 2.5f };    //デフォルト、最小、最大、増幅値        クールダウンテンプレ
        public static List<int> AbilityLimit = new() { 1, 1, 30, 1 };           //デフォルト、最小、最大、増幅値        能力の使用制限回数テンプレ



        public static string cs(Color c, string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }



        //実行元:Main.cs
        public static void Load()
        {
            SpecialOptions = new CustomOptionBlank(null);
           // HideSettings = CustomOption.Create(2, CustomOptionType.General, cs(Color.white, "HideSettings"), false, SpecialOptions);

            //=====   メインタブ   =====//
            PresetSelection = CustomOption.Create(10000, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"), Presets, null, true);
            CrewmateRolesMin = CustomOption.Create(10010, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "CrewmateRolesMin"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            CrewmateRolesMax = CustomOption.Create(10011, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "CrewmateRolesMax"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            ImpostorRolesMin = CustomOption.Create(10012, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "ImpostorRolesMin"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            ImpostorRolesMax = CustomOption.Create(10013, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "ImpostorRolesMax"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            NeutralRolesMin = CustomOption.Create(10014, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "NeutralRolesMin"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            NeutralRolesMax = CustomOption.Create(10015, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "NeutralRolesMax"), 0f, 0f, 15f, 1f, Format:"NoTranslate");



            //=====クルーメイトタブ=====//
            MadmateOption = new(20100, CustomOptionType.Crewmate, "Madmate", RoleClass.Madmate.Color, (int)CrewmatePlayers[2]);
            MadmateCanVent = CustomOption.Create(20101, CustomOptionType.Crewmate, "CanVent", true, MadmateOption);
            MadmateIsImpostorView = CustomOption.Create(20102, CustomOptionType.Crewmate, "IsImpostorView", true, MadmateOption);
            MadmateCanKnowImpostor = CustomOption.Create(20103, CustomOptionType.Crewmate, "CanKnowImpostor", Rates4, MadmateOption, Format:"NoTranslate");
            MadmateTask = new(20104, CustomOptionType.Crewmate, 1, 1, 3, MadmateCanKnowImpostor);

            SheriffOption = new(20200, CustomOptionType.Crewmate, "Sheriff", RoleClass.Sheriff.Color);
            SheriffFireCool = CustomOption.Create(20201, CustomOptionType.Crewmate, "FireCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], SheriffOption, Format:"NoTranslate");
            SheriffCanFireLimit = CustomOption.Create(20202, CustomOptionType.Crewmate, "AbilityLimit", AbilityLimit[0], AbilityLimit[1], AbilityLimit[2], AbilityLimit[3], SheriffOption, Format:"NoTranslate");
            SheriffCanFireMadmate = CustomOption.Create(20203, CustomOptionType.Crewmate, "CanFireMadmate", true, SheriffOption);
            SheriffCanFireNeutral = CustomOption.Create(20204, CustomOptionType.Crewmate, "CanFireNeutral", true, SheriffOption);



            //=====インポスタータブ=====//
            SerialKillerOption = new(30100, CustomOptionType.Impostor, "SerialKiller", RoleClass.SerialKiller.Color, (int)ImpostorPlayers[2]);
            SerialKillerKillCool = CustomOption.Create(30101, CustomOptionType.Impostor, "KillCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], SerialKillerOption, Format:"NoTranslate");
            SerialKillerSelfDeathTime = CustomOption.Create(30102, CustomOptionType.Impostor, "SelfDeathTime", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], SerialKillerOption, Format:"NoTranslate");
            SerialKillerIsMeetingReset = CustomOption.Create(30103, CustomOptionType.Impostor, "IsMeetingReset", true, SerialKillerOption);



            //=====  第三陣営タブ  =====//
            JackalOption = new(40100, CustomOptionType.Neutral, "Jackal", RoleClass.Jackal.Color);
            JackalKillCool = CustomOption.Create(40101, CustomOptionType.Neutral, "KillCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], JackalOption, Format:"NoTranslate");
            JackalCanVent = CustomOption.Create(40102, CustomOptionType.Neutral, "CanVent", true, JackalOption);
            JackalCanMakeSideKick = CustomOption.Create(40103, CustomOptionType.Neutral, "CanMakeSideKick", true, JackalOption);
            JackalIsImpostorView = CustomOption.Create(40104, CustomOptionType.Neutral, "IsImpostorView", true, JackalOption);



            //=====   コンビタブ   =====//



            //=====    属性タブ    =====//
        }
    }
}
