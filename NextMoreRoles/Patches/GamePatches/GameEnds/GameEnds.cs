using HarmonyLib;

namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    //試合終了対策
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.IsGameOverDueToDeath))]
    static class ShipStatus_IsGameOverDueToDeath
    {
        static void Postfix(ShipStatus __instance, ref bool __result)
        {
            __result = false;
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    class GameEndsSetUp
    {
        public static void Postfix(EndGameManager __instance)
        {
            SetPlayerNameAndRole(__instance);
            SetWinBonusText(__instance);
            SetRoleSummary(__instance);
        }
        public static bool IsHaison = false;

        //役職とプレイヤー名表示
        private static void SetPlayerNameAndRole(EndGameManager Manager)
        {
            foreach (PoolablePlayer pb in Manager.transform.GetComponentsInChildren<PoolablePlayer>())
            {
                UnityEngine.Object.Destroy(pb.gameObject);
            }
        }
    }
}
