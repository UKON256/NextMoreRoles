using System;
using System.Collections.Generic;

namespace NextMoreRoles.Modules
{
    static class ResetDatas
    {
        //実行元:GamePatches.GameStart.ClearAndReloads.cs
        public static void ClearAndReloads()
        {

        }
    }





    //##========== プレイヤーのデータ ==========##//
    //死んでる人
    public class DeadPlayer
    {
        public static List<DeadPlayer> DeadPlayers = new();
        public PlayerControl Player;
        public DateTime TimeOfDeath;
        public DeathReason DeathReason;
        public PlayerControl KillerIfExisting;
        public byte KillerIfExistingId;
        public DeadPlayer(PlayerControl Player, DateTime TimeOfDeath, DeathReason DeathReason, PlayerControl KillerIfExisting)
        {
            this.Player = Player;
            this.TimeOfDeath = TimeOfDeath;
            this.DeathReason = DeathReason;
            this.KillerIfExisting = KillerIfExisting;
            if (KillerIfExisting != null) KillerIfExistingId = KillerIfExisting.PlayerId;
        }
    }

    class PlayerDatas
    {

    }
}
