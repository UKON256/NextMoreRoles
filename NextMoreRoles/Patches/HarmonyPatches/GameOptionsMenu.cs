using System;
using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenu_Start
    {
        static void Postfix(GameOptionsMenu __instance)
        {
            try
            {
                LobbyPatches.CreateOptionTab.Open_Postfix(__instance);
            }
            catch(SystemException Error)
            {
                Logger.Error($"部屋設定画面の構築に失敗しました。エラー:{Error}", "GameOptionsMenu");
            }
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    class GameOptionsMenu_Update
    {
        static void Postfix(GameOptionsMenu __instance)
        {
            NextMoreRoles.Patches.LobbyPatches.UpdateCustomOptions.Postifx(__instance);
        }
    }

    [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.GetAdjustedNumImpostors))]
    public static class GameOptionsGetAdjustedNumImpostorsPatch
    {
        public static bool Prefix(GameOptionsData __instance, ref int __result)
        {
            __result = PlayerControl.GameOptions.NumImpostors;
            return false;
        }
    }

    [HarmonyPatch(typeof(SaveManager), "GameHostOptions", MethodType.Getter)]
    public static class SaveManagerGameHostOptionsPatch
    {
        private static int numImpostors;
        public static void Prefix()
        {
            if (SaveManager.hostOptionsData == null)
            {
                SaveManager.hostOptionsData = SaveManager.LoadGameOptions("gameHostOptions");
            }

            numImpostors = SaveManager.hostOptionsData.NumImpostors;
        }

        public static void Postfix(ref GameOptionsData __result)
        {
            __result.NumImpostors = numImpostors;
        }
    }
}
