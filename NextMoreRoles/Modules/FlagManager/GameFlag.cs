//試合系のフラグ管理

namespace NextMoreRoles.Modules.FlagManager
{
    public class Meeting
    {
        //現在ミーティング中かどうか。
        //変更元: TRUE:HarmonyPatches.MeetingHud.cs FALSE:GamePatches.GameStart.ClearAndReloads.cs
        public static bool IsMeeting;
    }
}
