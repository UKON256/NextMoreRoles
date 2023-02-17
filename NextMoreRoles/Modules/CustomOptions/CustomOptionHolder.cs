using UnityEngine;
using HarmonyLib;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.CustomOptions
{
    public class CustomOptionHolder
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

        public static CustomOption ModeSetting;

        public static void Load()
        {
            SpecialOptions = new CustomOptionBlank(null);
            Color32 PresetYellow = new Color(204f / 255f, 204f / 255f, 0, 1f);

            //* メインタブ *//
            PresetSelection = CustomOption.Create(11000, CustomOptionType.General, ModHelpers.cs(PresetYellow, "Preset"), Presets, IsHeader:true);
            CrewmateRolesMin = CustomOption.Create(11200, CustomOptionType.General, ModHelpers.cs(PresetYellow, "CrewmateRolesMin"), 0f, 0f, 15f, 1f, IsHeader:true);
            CrewmateRolesMax = CustomOption.Create(11300, CustomOptionType.General, ModHelpers.cs(PresetYellow, "CrewmateRolesMax"), 0f, 0f, 15f, 1f);
            ImpostorRolesMin = CustomOption.Create(11400, CustomOptionType.General, ModHelpers.cs(PresetYellow, "ImpostorRolesMin"), 0f, 0f, 15f, 1f);
            ImpostorRolesMax = CustomOption.Create(11500, CustomOptionType.General, ModHelpers.cs(PresetYellow, "ImpostorRolesMax"), 0f, 0f, 15f, 1f);
            NeutralRolesMin = CustomOption.Create(11600, CustomOptionType.General, ModHelpers.cs(PresetYellow, "NeutralRolesMin"), 0f, 0f, 15f, 1f);
            NeutralRolesMax = CustomOption.Create(11700, CustomOptionType.General, ModHelpers.cs(PresetYellow, "NeutralRolesMax"), 0f, 0f, 15f, 1f);

            //ModeSetting = CustomOption.Create(12000, CustomOptionType.General, "ModeSetting", ModeBase.ModeNames.ToArray(), IsHeader: true);


            //* RoleBaseの継承クラスをすべて実行 *//
            RoleBase.Roles.Do(x => x.SetUpRoleOptions());
        }
    }
}
