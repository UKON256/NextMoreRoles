using UnityEngine;

namespace NextMoreRoles.Modules.CustomOptions
{
    public class CustomOptions
    {
        //=====選択肢=====//
        public static string[] Rates = new string[] { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

        public static string[] Rates4 = new string[] { "0%", "25%", "50%", "75%", "100%" };

        public static string[] Presets = new string[] { "preset1", "preset2", "preset3", "preset4", "preset5"};



        //=====   メインタブ   =====//
        public static CustomOption PresetSelection;
        public static CustomOption CrewmateRolesMin;
        public static CustomOption CrewmateRolesMax;
        public static CustomOption NeutralRolesMin;
        public static CustomOption NeutralRolesMax;
        public static CustomOption ImpostorRolesMin;
        public static CustomOption ImpostorRolesMax;

        //=====クルーメイトタブ=====//

        //=====インポスタータブ=====//

        //=====  第三陣営タブ  =====//



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
            //=====   メインタブ   =====//
            PresetSelection = CustomOption.Create(1000, CustomOptionType.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"), Presets, null, true);

            //=====クルーメイトタブ=====//

            //=====インポスタータブ=====//

            //=====  第三陣営タブ  =====//
        }
    }
}
