using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    class Rolemanager_SelectRoles
    {
        static void Postfix()
        {
            NextMoreRoles.Patches.GamePatches.GameStart.RoleSet.SetUpRoles();
        }
    }
}
