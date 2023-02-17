using HarmonyLib;
using NextMoreRoles.Modules.CustomRPC;

namespace NextMoreRoles.Patches.GamePatches.GameStart;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.StartGame))]
class StartGame
{
    //ClearAndReload
    static void Postfix() {
        RPCSender.CallRPC(RPCId.RPCClearAndReload, new());
        RPCProcedure.RPCClearAndReload();
    }
}
