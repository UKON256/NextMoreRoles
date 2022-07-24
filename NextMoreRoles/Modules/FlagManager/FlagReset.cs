namespace NextMoreRoles.Modules.FlagManager
{
    class FlagReset
    {
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.FlagManager.Meeting.IsMeeting = false;
        }
    }
}
