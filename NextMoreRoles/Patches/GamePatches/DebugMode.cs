using System;

namespace NextMoreRoles.Patches.GamePatches
{
    class DebugModePatch
    {
        public static void SetRoomMinPlayer(GameStartManager __instance)
        {
            __instance.MinPlayers = 1;
        }
    }
}
