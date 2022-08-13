using Hazel;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomRPC;

namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    public static class GameStart_ClearAndReloads
    {
        //実行元:HarmonyPatches.GameStartManager.cs、   終了時:Patches.GamePatches.GameEnds.GameEnds.cs
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.ResetFlags.ClearAndReloads();
            NextMoreRoles.Modules.ResetDatas.ClearAndReloads();
            NextMoreRoles.Patches.GamePatches.GameEnds.AdditionalTempData.Clear();
            NextMoreRoles.Roles.RoleClass.ClearAndReload();

            //BotRPC送信
            if (AmongUsClient.Instance.AmHost)
            {
                Logger.Info("BOTのデータを送信しました", "ClearAndReloads");
                foreach(PlayerControl Bot in BotManager.AllBots)
                {
                    MessageWriter Writer = RPCHelper.StartRPC(CustomRPC.ShareBotData);
                    Writer.Write(Bot.PlayerId);
                    new LateTask(() => Writer.EndRPC(), 0.5f);
                }
            }
        }
    }
}
