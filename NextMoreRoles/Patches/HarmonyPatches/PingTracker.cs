using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    class PingTracker_Update
    {
        static void Postfix(PingTracker __instance)
        {
            NextMoreRoles.Patches.GamePatches.PingMessages.SetPingMessages(__instance);
        }
    }
}
