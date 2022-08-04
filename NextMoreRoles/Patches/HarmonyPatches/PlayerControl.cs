using HarmonyLib;
using System;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    class PlayerControl_MurderPlayer
    {
        //プレイヤー誰かがキルすると実行(自分がキルしてなくても追加)
        static void Postfix(PlayerControl __instance, [HarmonyArgument(0)]PlayerControl Target)
        {
            //死んでるプレイヤーを追加
            DeadPlayer DeadPlayer = new(Target, DateTime.UtcNow, DeathReason.Kill, __instance);
            DeadPlayer.DeadPlayers.Add(DeadPlayer);
        }
    }



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
