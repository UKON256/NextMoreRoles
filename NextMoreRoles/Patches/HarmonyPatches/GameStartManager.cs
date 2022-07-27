using HarmonyLib;
using UnityEngine;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    class GameStartManager_Start
    {
        static void Postfix(GameStartManager __instance)
        {
            GamePatches.GameStart.GameStart_ClearAndReloads.ClearAndReloads();
            if (Configs.IsDebugMode.Value) NextMoreRoles.Patches.GamePatches.DebugModePatch.SetRoomMinPlayer(__instance);
        }
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    public static class PlayerCountChange
    {
        public static void Prefix(GameStartManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.F7)) NextMoreRoles.Patches.LobbyPatches.QuickGameStart.QuickStartCancel();
            if (Input.GetKeyDown(KeyCode.F8)) NextMoreRoles.Patches.LobbyPatches.QuickGameStart.QuickStart();
        }
    }
}
