using UnityEngine;
using System.Collections.Generic;
using NextMoreRoles.Roles;
using NextMoreRoles.Roles.Data.Crewmate;
using NextMoreRoles.Roles.Data.Impostor;
using NextMoreRoles.Roles.Data.Neutral;

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
        public static CustomRoleOption SheriffOption;

        //=====インポスタータブ=====//
        public static CustomRoleOption MadmateOption;

        public static CustomRoleOption NinjaOption;

        //=====  第三陣営タブ  =====//
        public static CustomRoleOption JackalOption;

        //=====上限変えたきゃここ変えれば大丈夫=====//
        public static List<float> CrewmatePlayers = new() { 1f, 1f, 15f, 1f };  //デフォルト、最小、最大、増幅値
        public static List<float> ImpostorPlayers = new() { 1f, 1f, 3f, 1f };   //デフォルト、最小、最大、増幅値



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
            CrewmateRolesMin = CustomOption.Create(10010, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "CrewmateRolesMin"), 0f, 0f, 15f, 1f);
            CrewmateRolesMax = CustomOption.Create(10011, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "CrewmateRolesMax"), 0f, 0f, 15f, 1f);
            ImpostorRolesMin = CustomOption.Create(10012, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "ImpostorRolesMin"), 0f, 0f, 15f, 1f);
            ImpostorRolesMax = CustomOption.Create(10013, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "ImpostorRolesMax"), 0f, 0f, 15f, 1f);
            NeutralRolesMin = CustomOption.Create(10014, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "NeutralRolesMin"), 0f, 0f, 15f, 1f);
            NeutralRolesMax = CustomOption.Create(10015, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "NeutralRolesMax"), 0f, 0f, 15f, 1f);

            //=====クルーメイトタブ=====//
            SheriffOption = new(20000, CustomOptionType.Crewmate, "Sheriff", Sheriff.Color);

            //=====インポスタータブ=====//
            MadmateOption = new(30000, CustomOptionType.Impostor, "Madmate", RoleBase.ImpostorRed, (int)CrewmatePlayers[2]);

            NinjaOption = new(30100, CustomOptionType.Impostor, "Ninja", RoleBase.ImpostorRed, (int)ImpostorPlayers[2]);

            //=====  第三陣営タブ  =====//
            JackalOption = new(40000, CustomOptionType.Neutral, "Jackal", Jackal.Color);

            //=====   コンビタブ   =====//

            //=====    属性タブ    =====//
        }
    }
}
