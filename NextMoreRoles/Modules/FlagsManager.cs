using System;
using System.Collections.Generic;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules
{
    class ResetFlags
    {
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.MeetingFlags.IsMeeting = false;
        }
    }



    class MeetingFlags
    {
        //現在ミーティング中かどうか。
        //変更元: TRUE:HarmonyPatches.MeetingHud.cs FALSE:GamePatches.GameStart.ClearAndReloads.cs
        public static bool IsMeeting;
    }



    static class PlayerFlags
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
            try
            {
                if (Target == null || Target.Data.Disconnected) return false;
                //BOTのどれかが目標のIDが一緒ならBOT
                foreach (PlayerControl Bots in BotManager.AllBots)
                {
                    if (Bots.PlayerId == Target.PlayerId) return true;
                }
                return false;
            }
            catch(SystemException Error)
            {
                Logger.Error($"BOTのフラグ判定に失敗しました。:{Error}", "GameFlag");
                return false;
            }
        }
        public static bool IsPlayer(this PlayerControl Target)
        {
            return !IsBot(Target);
        }
    }



    static class RoleFlags
    {
        public static bool IsImpostor(this PlayerControl Target)
        {
            return Target != null && Target.Data.Role.IsImpostor;
        }
        public static bool IsCrew(this PlayerControl Target)
        {
            return !IsImpostor(Target) && !IsMad(Target) && !IsNeutral(Target) && !IsFriend(Target);
        }
        public static bool IsNeutral(this PlayerControl Target)
        {
            var IsNeutral = false;
            /*switch (Target.)
            {
                case
                    //第三か
                    IsNeutral = true;
                    break;
            }*/
            return IsNeutral;
        }

        public static bool IsMad(this PlayerControl Target)
        {
            var IsMad = false;
            /*switch (Target.GetRole)
            {
                case
            }*/
            return IsMad;
        }

        public static bool IsFriend(this PlayerControl Target)
        {
            var IsFriend = false;
            /*switch (Target.GetRole())
            {
                case
            }*/
            return IsFriend;
        }
    }



    static class ModeFlags
    {
        public static bool IsMode(Mode mode, bool IsChache = true)
        {
            return false;
        }
    }
}
