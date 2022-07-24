using HarmonyLib;

namespace NextMoreRoles.Modules.GameEnds
{
    //カスタム勝利
    enum CustomGameEndReason
    {
        Haison,
    }

    [HarmonyPatch(typeof(ShipStatus))]
    public class ShipStatusPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.IsGameOverDueToDeath))]
        public static void Postfix2(ShipStatus __instance, ref bool __result)
        {
            __result = false;
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    class GameEndsSetUp
    {
        public static bool IsHaison = false;
    }
}
