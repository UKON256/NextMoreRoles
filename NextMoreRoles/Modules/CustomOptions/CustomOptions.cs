using System.Diagnostics;
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
        public static CustomOption ImpostorRolesMin;
        public static CustomOption ImpostorRolesMax;
        public static CustomOption NeutralRolesMin;
        public static CustomOption NeutralRolesMax;



        //=====クルーメイトタブ=====//
        public static CustomRoleOption MadmateOption;
        public static CustomOption MadmateCanVent;
        public static CustomOption MadmateIsImpostorVision;                     //インポの視界
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
        public static CustomOption SerialKillerSucideTime;                      //自殺時間
        public static CustomOption SerialKillerIsMeetingReset;                  //会議でリセット



        //=====  第三陣営タブ  =====//
        public static CustomRoleOption JackalOption;
        public static CustomOption JackalKillCool;
        public static CustomOption JackalCanVent;
        public static CustomOption JackalIsImpostorVision;                      //インポと同じ視界
        public static CustomOption JackalCanMakeSideKick;
        public static CustomOption SideKickMakeCool;                            //指名クール

        public static CustomOption SideKickCanPromotion;                        //昇格可能か
        public static CustomOption SideKickCanVent;
        public static CustomOption SideKickCanMakeSideKick;                     //サイドキックがサイドキックを作れる
        public static CustomOption SideKickCanKill;



        //=====コンビネーションタブ=====//



        //=====重複陣営タブ=====//
        public static CustomOption DebuggerOption;



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

            Color32 PresetYellow = new Color(204f / 255f, 204f / 255f, 0, 1f);
            //=====   メインタブ   =====//
            PresetSelection = CustomOption.Create(1100, CustomOptionType.General, cs(PresetYellow, "Preset"), Presets, null, true);
            CrewmateRolesMin = CustomOption.Create(1200, CustomOptionType.General, cs(PresetYellow, "CrewmateRolesMin"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            CrewmateRolesMax = CustomOption.Create(1300, CustomOptionType.General, cs(PresetYellow, "CrewmateRolesMax"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            ImpostorRolesMin = CustomOption.Create(1400, CustomOptionType.General, cs(PresetYellow, "ImpostorRolesMin"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            ImpostorRolesMax = CustomOption.Create(1500, CustomOptionType.General, cs(PresetYellow, "ImpostorRolesMax"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            NeutralRolesMin = CustomOption.Create(1600, CustomOptionType.General, cs(PresetYellow, "NeutralRolesMin"), 0f, 0f, 15f, 1f, Format:"NoTranslate");
            NeutralRolesMax = CustomOption.Create(1700, CustomOptionType.General, cs(PresetYellow, "NeutralRolesMax"), 0f, 0f, 15f, 1f, Format:"NoTranslate");



            //=====クルーメイトタブ=====//
            MadmateOption = new(2001010, CustomOptionType.Crewmate, "Madmate", RoleClass.Madmate.Color, (int)CrewmatePlayers[2]);
            MadmateCanVent = CustomOption.Create(2001020, CustomOptionType.Crewmate, "CanVent", true, MadmateOption);
            MadmateIsImpostorVision = CustomOption.Create(2001030, CustomOptionType.Crewmate, "IsImpostorVision", true, MadmateOption);
            MadmateCanKnowImpostor = CustomOption.Create(2001040, CustomOptionType.Crewmate, "CanKnowImpostor", Rates4, MadmateOption, Format:"NoTranslate");
            MadmateTask = new(2001040, CustomOptionType.Crewmate, 1, 1, 3, MadmateCanKnowImpostor);

            SheriffOption = new(2002010, CustomOptionType.Crewmate, "Sheriff", RoleClass.Sheriff.Color);
            SheriffFireCool = CustomOption.Create(2002020, CustomOptionType.Crewmate, "FireCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], SheriffOption, Format:"NoTranslate");
            SheriffCanFireLimit = CustomOption.Create(2002030, CustomOptionType.Crewmate, "AbilityLimit", AbilityLimit[0], AbilityLimit[1], AbilityLimit[2], AbilityLimit[3], SheriffOption, Format:"NoTranslate");
            SheriffCanFireMadmate = CustomOption.Create(2002040, CustomOptionType.Crewmate, "CanFireMadmate", true, SheriffOption);
            SheriffCanFireNeutral = CustomOption.Create(2002050, CustomOptionType.Crewmate, "CanFireNeutral", true, SheriffOption);



            //=====インポスタータブ=====//
            SerialKillerOption = new(3001010, CustomOptionType.Impostor, "SerialKiller", RoleClass.SerialKiller.Color, (int)ImpostorPlayers[2]);
            SerialKillerKillCool = CustomOption.Create(3001020, CustomOptionType.Impostor, "KillCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], SerialKillerOption, Format:"NoTranslate");
            SerialKillerSucideTime = CustomOption.Create(3001030, CustomOptionType.Impostor, "SucideTime", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], SerialKillerOption, Format:"NoTranslate");
            SerialKillerIsMeetingReset = CustomOption.Create(3001040, CustomOptionType.Impostor, "IsMeetingReset", true, SerialKillerOption);



            //=====  第三陣営タブ  =====//
            JackalOption = new(4001010, CustomOptionType.Neutral, "Jackal", RoleClass.Jackal.Color);
            JackalKillCool = CustomOption.Create(4001020, CustomOptionType.Neutral, "KillCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], JackalOption, Format:"NoTranslate");
            JackalCanVent = CustomOption.Create(4001030, CustomOptionType.Neutral, "CanVent", true, JackalOption);
            JackalIsImpostorVision = CustomOption.Create(4001040, CustomOptionType.Neutral, "IsImpostorVision", true, JackalOption);
            JackalCanMakeSideKick = CustomOption.Create(4001050, CustomOptionType.Neutral, "CanMakeSideKick", true, JackalOption);
            SideKickMakeCool = CustomOption.Create(4001060, CustomOptionType.Neutral, "MakeCool", Cooldown[0], Cooldown[1], Cooldown[2], Cooldown[3], JackalCanMakeSideKick, true);

            SideKickCanPromotion = CustomOption.Create(4001070, CustomOptionType.Neutral, "CanPromotion", true, JackalCanMakeSideKick);
            SideKickCanVent = CustomOption.Create(4001080, CustomOptionType.Neutral, "CanVent", true, JackalCanMakeSideKick);
            SideKickCanMakeSideKick = CustomOption.Create(4001090, CustomOptionType.Neutral, "SideKickCanMakeSideKick", false, JackalCanMakeSideKick);
            SideKickCanKill = CustomOption.Create(4001100, CustomOptionType.Neutral, "CanKill", false, JackalCanMakeSideKick);



            //=====コンビネーションタブ=====//



            //=====重複陣営タブ=====//
            if(Configs.IsDebugMode.Value) DebuggerOption = CustomOption.Create(6999010, CustomOptionType.Attribute, cs(RoleClass.Debugger.Color, "Debugger"), true, null, true);
        }
    }
}
