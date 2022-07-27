using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    class VersionShowerPatch
    {
        static void Postfix(VersionShower __instance)
        {
            NextMoreRoles.Patches.TitlePatches.WrapUpPatch.ChangeAmongUsLogo(__instance);
        }
    }
}
