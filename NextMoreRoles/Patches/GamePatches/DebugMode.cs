using UnityEngine;

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

        //BOT召喚！
        public static void BotSpawn()
        {
            if (!Configs.DebugMode.Value || !AmongUsClient.Instance.AmHost) return;

            if (Input.GetKeyDown(KeyCode.G))
            {
                NextMoreRoles.Modules.BotManager.Spawn();
            }
        }
    }
}
