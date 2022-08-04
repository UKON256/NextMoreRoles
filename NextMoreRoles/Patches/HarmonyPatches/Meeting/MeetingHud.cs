using HarmonyLib;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.HarmonyPatches.Meeting
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    static class MeetingHud_Awake
    {
        //ボタンが押されたイントロとかが始まった時
        static void Postfix(MeetingHud __instance)
        {
            Logger.Info("=====緊急会議開始=====", "MeetingHud");
            NextMoreRoles.Modules.MeetingFlags.IsMeeting = true;                                                //ミーティング中かどうかのフラグを変える
            if (BotManager.AllBots != null) new LateTask(()=> {BotManager.VotingBot(__instance);}, 2.5f);       //BOTに投票させる
        }
    }



    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
    class MeetingHud_CheckForEndVoting
    {
        //東甫ゆが終わった時実行
        static void Postfix()
        {
            NextMoreRoles.Modules.MeetingFlags.IsMeeting = false;                                               //ミーティング中かどうかのフラグを変える
        }
    }
}
