using HarmonyLib;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.GamePatches
{
    class DebugModePatch
    {
        //ロビーの最少人数設定
        //実行元:HarmonyPatches.GameStartManager.cs
        public static void SetRoomMinPlayer(GameStartManager __instance)
        {
            __instance.MinPlayers = 1;
        }



        //BOT召喚！ 実行元:HarmonyPatches.KeyBoardOrJoyStick.cs
        public static void BotSpawn()
        {
            Logger.Info("BOTがスポーンしました", "BotManager");
            NextMoreRoles.Modules.BotManager.Spawn();
        }



        //デバッグモードONをPingメッセージに追記
        //実行元:GamePatches.PingMessages.cs
        public static void PingSetDebugMode(PingTracker __instance)
        {
            __instance.text.text += "\n" + $"<color=#a4ebf0>デバッグモード:ON</color>";
        }
    }

    //デバッグモードの時に最初のいんなーすろすのロゴをスキップ
    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
    class SplashLogoAnimatorPatch
    {
        public static void Prefix(SplashManager __instance)
        {
            if (Configs.IsDebugMode.Value)
            {
                __instance.sceneChanger.AllowFinishLoadingScene();
                __instance.startedSceneLoad = true;
            }
        }
    }
}
