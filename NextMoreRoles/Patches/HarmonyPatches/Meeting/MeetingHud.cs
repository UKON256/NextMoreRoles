using HarmonyLib;
using NextMoreRoles.Modules.FlagManager;

namespace NextMoreRoles.Patches.HarmonyPatches.Meeting
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    static class MeetingHud_Awake
    {
        static void Postfix()
        {
            NextMoreRoles.Modules.FlagManager.Meeting.IsMeeting = true;
        }
    }
}
