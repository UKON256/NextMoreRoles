using NextMoreRoles.Modules.Role.CustomButtons;

namespace NextMoreRoles.Roles.Data.Attribute
{
    class DebuggerFunctions
    {
        public static void OnClickEvent()
        {
            NextMoreRoles.Modules.Role.DebugDisplayShower.Open();
        }

        public static void OnMeetingEndEvent()
        {
            CustomButtons.DebuggerButton.MaxTimer = 0f;
            CustomButtons.DebuggerButton.Timer = 0f;
        }
    }
}
