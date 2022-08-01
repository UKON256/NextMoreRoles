using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches.Meeting
{
    [HarmonyPatch(typeof(MeetingIntroAnimation), nameof(MeetingIntroAnimation.Start))]
    class MeetingIntroAnimation_Start
    {
        //ミーティング召集アニメーションが始まったとき実行
        static void Postfix()
        {

        }
    }
}
