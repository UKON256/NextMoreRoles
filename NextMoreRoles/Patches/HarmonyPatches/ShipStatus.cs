using HarmonyLib;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Awake))]
    class ShipStatus_Awake
    {
        [HarmonyPostfix, HarmonyPriority(Priority.Last)]
        public static void Postfix(ShipStatus __instance)
        {
            MapUtilities.CachedShipStatus = __instance;
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.OnDestroy))]
    public static class ShipStatus_OnDestroy
    {
        [HarmonyPostfix, HarmonyPriority(Priority.Last)]
        public static void Postfix()
        {
            MapUtilities.CachedShipStatus = null;
            MapUtilities.MapDestroyed();
        }
    }
}
