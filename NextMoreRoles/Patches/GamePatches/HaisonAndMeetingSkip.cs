using NextMoreRoles.Helpers;
using NextMoreRoles.Patches.GamePatches.GameEnds;

namespace NextMoreRoles.Patches.GamePatches
{
    //実行元:HarmonyPatches.KeyBoardOrJoyStick.cs
    class HaisonAndMeetingSkip
    {
        //廃村
        public static void Haison()
        {
            Logger.Info("=====廃村しました=====", "HaisonAndMeetingSkip");
            ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.Haison, false);
            MapUtilities.CachedShipStatus.enabled = false;
        }

        //会議をスキップ
        public static void MeetingSkip()
        {
            Logger.Info("=====会議をスキップしました=====", "HaisonAndMeetingSkip");
            MeetingHud.Instance.RpcClose();
        }
    }
}
