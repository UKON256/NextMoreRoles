using HarmonyLib;
using UnityEngine;
using UnhollowerBaseLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.InitializeOptions))]
    class GameSettingMenu_Init
    {
        static void Prefix(GameSettingMenu __instance)
        {
            //部屋を立て直さなくてもマップ変更できるようにする
            __instance.HideForOnline = new Il2CppReferenceArray<Transform>(0);
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    class GameSettingMenu_Satrt
    {
        static void Postfix(GameOptionsMenu __instance)
        {
            GamePatches.CreateOptionTab.Open_Postfix(__instance);
        }
    }
}
