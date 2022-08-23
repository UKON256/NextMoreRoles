using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Modules.Role.CustomButtons;

namespace NextMoreRoles.Roles.Data.Attribute
{
    class DebuggerFunctions
    {
        public static void OnMeetingEndEvent()
        {
            CustomButtons.DebuggerButton.MaxTimer = 0.1f;
            CustomButtons.DebuggerButton.Timer = 0.1f;
        }
    }
}
