using HarmonyLib;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Patches.HarmonyPatches.PlayerControls
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            CustomOption.ShareOptionSelections();
        }
    }
}
