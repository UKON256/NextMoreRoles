using HarmonyLib;

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


    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
    class AmongUsClient_Awake
    {
        static void Prefix(AmongUsClient __instance)
        {
            NextMoreRoles.Modules.DatasManager.MapLoader.AmongUsClient_Awake.Prefix(__instance);
            NextMoreRoles.Modules.DatasManager.HudManagerLoader.AmongUsClient_Awake.Prefix(__instance);
            NextMoreRoles.Modules.DatasManager.MeetingHudLoader.AmongUsClient_Awake.Prefix(__instance);
        }
    }
}
