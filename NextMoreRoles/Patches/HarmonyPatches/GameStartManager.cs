using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    class GameStartManager_Start
    {
        static void Postfix()
        {
            //RoleClass.ClearAndReloadRoles();
        }
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    public static class PlayerCountChange
    {
        public static void Prefix(GameStartManager __instance)
        {
            if (Configs.IsDebugMode.Value) {NextMoreRoles.Patches.GamePatches.DebugModePatch.SetRoomMinPlayer(__instance);}
        }
    }
}
