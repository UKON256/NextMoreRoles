using HarmonyLib;
using System;
using NextMoreRoles.Modules.DatasManager;

namespace NextMoreRoles.Patches.HarmonyPatches.PlayerControls
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
    class PlayerControl_Exiled
    {
        static void Postifx(PlayerControl __instance)
        {
            //死んでるプレイヤーを追加
            DeadPlayer DeadPlayer = new(__instance, DateTime.UtcNow, DeathReason.Exile, __instance);
            DeadPlayer.DeadPlayers.Add(DeadPlayer);
        }
    }
}
