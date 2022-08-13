using System;
using System.Collections.Generic;
using NextMoreRoles.Roles;
using NextMoreRoles.Roles.Data.Crewmate;
using NextMoreRoles.Roles.Data.Impostor;
using NextMoreRoles.Roles.Data.Neutral;

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

    static class PlayerDatas
    {
        public static RoleId GetRole(this PlayerControl Player)
        {
            try
            {
                // クルー
                if (Player.IsRole(RoleId.Sheriff)) return RoleId.Sheriff;

                // インポ
                if (Player.IsRole(RoleId.Madmate)) return RoleId.Madmate;
                if (Player.IsRole(RoleId.Ninja)) return RoleId.Ninja;

                // 第三
                if (Player.IsRole(RoleId.Jackal)) return RoleId.Jackal;
            }
            catch(SystemException Error)
            {
                Logger.Error($"Roleの取得に失敗しました。エラー:{Error}", "DataManager");
            }
            return RoleId.Crewmate;
        }
    }
}
