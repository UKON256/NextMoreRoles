using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches.Meeting
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    static class MeetingHud_Awake
    {
        static void Postfix()
        {
            Logger.Info("=====緊急会議開始=====", "MeetingHud");
            NextMoreRoles.Modules.FlagManager.Meeting.IsMeeting = true;
        }
    }
}
