using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Modules.Role.CustomButtons;

namespace NextMoreRoles.Roles.Data.Crewmate
{
    class SheriffFunctions
    {
        public static void OnMeetingEndEvent()
        {
            CustomButtons.SheriffFireButton.MaxTimer = RoleClass.Sheriff.FireCool;
            CustomButtons.SheriffFireButton.Timer = RoleClass.Sheriff.FireCool;
        }
    }
}
