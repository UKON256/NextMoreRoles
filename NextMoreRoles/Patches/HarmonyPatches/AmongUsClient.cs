using HarmonyLib;
using NextMoreRoles.Patches;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    class AmongUsClientOnPlayerJoinedPatch
    {
        static void Postfix()
        {
            //プレイヤーが入ったとき実行
            LobbyPatches.ShareGameVersion.AmongUsClientOnPlayerJoinedPatch.Postfix();
        }
    }
}
