using HarmonyLib;
using UnityEngine;

namespace NextMoreRoles.Roles.Data.Crewmate
{
    [HarmonyPatch]
    public class Sheriff : RoleBase<Sheriff>
    {
        public static Color Color = new Color32(248, 205, 70, byte.MaxValue);

        public override void FixedUpdate() {}
        public override void OnKill(PlayerControl Target) {}
        public override void HandleDisconnect(PlayerControl Player, DisconnectReasons Reason) {}
        public override void OnMeetingEnd() {}
        public override void OnMeetingStart() {}
        public override void OnDeath(PlayerControl Killer = null) {}
    }



    class SheriffFunctions
    {

    }
}
