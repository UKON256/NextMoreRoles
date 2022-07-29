using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    class EndGameManager_SetEverythingUp
    {
        static void Prefix([HarmonyArgument(0)] ref EndGameResult EndGameResult)
        {
            NextMoreRoles.Patches.GamePatches.GameEnds.GameEndsSetUp.OnGameEnd_Prefix(EndGameResult);
        }
        static void Postfix(EndGameManager __instance)
        {
            NextMoreRoles.Patches.GamePatches.GameEnds.GameEndsSetUp.OnGameEnd_Postfix(__instance);
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    class AmongUsClient_OnGameEnd
    {
        static void Prefix(EndGameManager __instance)
        {

        }
        static void Postfix(EndGameManager __instance)
        {
            NextMoreRoles.Patches.GamePatches.GameEnds.EverythingUp.Postfix(__instance);
        }
    }
}
