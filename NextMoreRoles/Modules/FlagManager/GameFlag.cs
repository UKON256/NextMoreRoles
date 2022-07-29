using System.Runtime;
using System;
using System.Collections.Generic;
using NextMoreRoles.Helpers;

//試合系のフラグ管理
namespace NextMoreRoles.Modules.FlagManager
{
    class Meeting
    {
        //現在ミーティング中かどうか。
        //変更元: TRUE:HarmonyPatches.MeetingHud.cs FALSE:GamePatches.GameStart.ClearAndReloads.cs
        public static bool IsMeeting;
    }

    public static class Players
    {
        public static List<PlayerControl> AlivePlayers;
        public static List<PlayerControl> GetAllAlivePlayer()
        {
            AlivePlayers = new();
            foreach(PlayerControl p in CachedPlayer.AllPlayers)
            {
                //生きてるプレイヤー全員をリストに追加、返り値としてだす
                if (IsAlive(p))
                {
                    AlivePlayers.Add(p);
                }
            }
            return AlivePlayers;
        }

        public static bool IsDead(this PlayerControl Target)
        {
            return Target.Data.IsDead;
        }
        public static bool IsAlive(this PlayerControl Target)
        {
            return !Target.Data.IsDead;
        }

        //BOTフラグ！
        public static bool IsBot(this PlayerControl Target)
        {
            if (Target == null || Target.Data.Disconnected) return false;
            //BOTのどれかが目標のIDが一緒ならBOT
            foreach (PlayerControl Bots in BotManager.AllBots)
            {
                if (Bots.PlayerId == Target.PlayerId) return true;
            }
            return false;
        }
        public static bool IsPlayer(this PlayerControl Target)
        {
            return !IsBot(Target);
        }
    }
}
