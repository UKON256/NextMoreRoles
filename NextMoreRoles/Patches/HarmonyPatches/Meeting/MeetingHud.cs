using HarmonyLib;
using NextMoreRoles.Modules;
using System.Collections.Generic;

namespace NextMoreRoles.Patches.HarmonyPatches.Meeting
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    class MeetingHud_Awake
    {
        //ボタンが押されたイントロとかが始まった時
        void Postfix(MeetingHud __instance)
        {
            Logger.Info("=====緊急会議開始=====", "MeetingHud");
            NextMoreRoles.Modules.FlagManager.Meeting.IsMeeting = true;                                         //ミーティング中かどうかのフラグを変える
            if (BotManager.AllBots != null) new LateTask(()=> {BotManager.VotingBot(__instance);}, 2.5f);       //BOTに投票させる
        }
    }



    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
    class MeetingHud_CheckForEndVoting
    {
        //東甫ゆが終わった時実行
        static void Postfix()
        {
            NextMoreRoles.Modules.FlagManager.Meeting.IsMeeting = false;                                        //ミーティング中かどうかのフラグを変える
        }
    }



    //イントロが始まる時
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CoIntro))]
    public class MeetingHud_CoIntro
    {
        public static GameData.PlayerInfo Reporter;
        public static GameData.PlayerInfo ReportedBody;
        public static List<GameData.PlayerInfo> ReportedDeadBodys;
        //Reporter:通報した人、 ReportedBody:通報された死体、 DeadBodys:今回の死体
        void Prefix(MeetingHud __instance, [HarmonyArgument(0)]GameData.PlayerInfo MeetingReporter, [HarmonyArgument(1)]GameData.PlayerInfo MeetingReportedBody, [HarmonyArgument(3)]List<GameData.PlayerInfo> MeetingDeadBodys)
        {
            Reporter = MeetingReporter;
            ReportedBody = MeetingReportedBody;
            ReportedDeadBodys = MeetingDeadBodys;
        }
    }
}
