using HarmonyLib;
using UnityEngine;

namespace NextMoreRoles.Roles.Data.Neutral
{
    [HarmonyPatch]
    public class Jackal : RoleBase<Jackal>
    {
        public static Color Color = new Color32(65, 105, 255, byte.MaxValue);

        public override void FixedUpdate() {}
        public override void OnKill(PlayerControl Target) {}
        public override void HandleDisconnect(PlayerControl Player, DisconnectReasons Reason) {}
        public override void OnMeetingEnd() {}
        public override void OnMeetingStart() {}
        public override void OnDeath(PlayerControl Killer = null) {}
    }



    class JackalFunctions
    {

    }
}
