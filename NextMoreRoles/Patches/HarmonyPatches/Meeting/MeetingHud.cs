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
            NextMoreRoles.Modules.FlagManager.Meeting.IsMeeting = true;                                         //ミーティング中かどうかのフラグを変える
            if (BotManager.AllBots != null) new LateTask(()=> {BotManager.VotingBot(__instance);}, 2.5f);       //BOTに投票させる
        }
    }
}
