using UnityEngine;
using HarmonyLib;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.CustomOptions
{
    public class CustomOptions
    {
        public static string[] Rates = new string[] { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

        public static string[] Rates4 = new string[] { "0%", "25%", "50%", "75%", "100%" };

        public static string[] Presets = new string[] { "preset1", "preset2", "preset3", "preset4", "preset5"};


        public static CustomOption SpecialOptions;
        public static CustomOption HideSettings;

        //* メインタブ *//
        public static CustomOption PresetSelection;
        public static CustomOption CrewmateRolesMin;
        public static CustomOption CrewmateRolesMax;
        public static CustomOption ImpostorRolesMin;
        public static CustomOption ImpostorRolesMax;
        public static CustomOption NeutralRolesMin;
        public static CustomOption NeutralRolesMax;

        public static void Load()
        {
            SpecialOptions = new CustomOptionBlank(null);
            Color32 PresetYellow = new Color(204f / 255f, 204f / 255f, 0, 1f);

            //* メインタブ *//
            PresetSelection = CustomOption.Create(1100, CustomOptionType.General, ModHelpers.cs(PresetYellow, "Preset"), Presets, null, true);
            CrewmateRolesMin = CustomOption.Create(1200, CustomOptionType.General, ModHelpers.cs(PresetYellow, "CrewmateRolesMin"), 0f, 0f, 15f, 1f);
            CrewmateRolesMax = CustomOption.Create(1300, CustomOptionType.General, ModHelpers.cs(PresetYellow, "CrewmateRolesMax"), 0f, 0f, 15f, 1f);
            ImpostorRolesMin = CustomOption.Create(1400, CustomOptionType.General, ModHelpers.cs(PresetYellow, "ImpostorRolesMin"), 0f, 0f, 15f, 1f);
            ImpostorRolesMax = CustomOption.Create(1500, CustomOptionType.General, ModHelpers.cs(PresetYellow, "ImpostorRolesMax"), 0f, 0f, 15f, 1f);
            NeutralRolesMin = CustomOption.Create(1600, CustomOptionType.General, ModHelpers.cs(PresetYellow, "NeutralRolesMin"), 0f, 0f, 15f, 1f);
            NeutralRolesMax = CustomOption.Create(1700, CustomOptionType.General, ModHelpers.cs(PresetYellow, "NeutralRolesMax"), 0f, 0f, 15f, 1f);

            //* RoleBaseの継承クラスをすべて実行 *//
            RoleBase.Roles.Do(x => x.SetUpRoleOptions());
        }
    }
}
