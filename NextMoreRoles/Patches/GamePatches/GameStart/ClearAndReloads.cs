using System.Collections.Generic;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomRPC;

namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    public static class GameStart_ClearAndReloads
    {
        //実行元:HarmonyPatches.GameStartManager.cs、   終了時:Patches.GamePatches.GameEnds.GameEnds.cs
        public static void ClearAndReloads()
        {
            NextMoreRoles.Roles.RoleClass.ClearAndReloads();                            //役職の設定などを再取得
            NextMoreRoles.Patches.GamePatches.GameEnds.AdditionalTempData.Clear();      //試合終了ステータスをリセット
            NextMoreRoles.Modules.DatasManager.Reset.ClearAndReloads();                 //データリセット
            NextMoreRoles.Modules.Role.DebugDisplayShower.Reset();                      //デバッグ用ディスプレイ情報をリセット

            //BotRPC送信
            if (AmongUsClient.Instance.AmHost)
            {
                foreach(PlayerControl Bot in BotManager.AllBots)
                {
                    RPCSender.CallRPC(CustomRPC.ShareBotData, new List<byte> {Bot.PlayerId});
                }
            }
        }
    }
}
