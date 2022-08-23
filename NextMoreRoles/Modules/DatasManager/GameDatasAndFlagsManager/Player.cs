using System;
using System.Collections.Generic;
using NextMoreRoles.Helpers;
using NextMoreRoles.Roles;
using NextMoreRoles.Modules.DatasManager;

namespace NextMoreRoles.Modules.DatasManager
{
    //##=====リセット=====##//
    class ResetPlayerDatas
    {
        //実行元:Modules.DatasManager.Reset.cs
        public static void ClearCache()
        {

        }

        //実行元:Modules.DatasManager.Reset.cs
        public static void ClearAndReloads()
        {

        }
    }





    //##=====役職系やプレイヤー系のデータとフラグ=====##//
    static class PlayerDatas
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


        //BOT？
        public static bool IsBot(this PlayerControl Target)
        {
            try
            {
                if (Target == null || Target.Data.Disconnected) return false;
                //BOTのどれかが目標のIDと一緒ならBOT
                foreach (PlayerControl Bots in BotManager.AllBots)
                {
                    if (Bots.PlayerId == Target.PlayerId) return true;
                }
                return false;
            }
            catch(SystemException Error)
            {
                Logger.Error($"BOTのフラグ判定に失敗しました。:{Error}", "Player");
                return false;
            }
        }


        //BOTじゃない？
        public static bool IsPlayer(this PlayerControl Target)
        {
            return !IsBot(Target);
        }


        //インポ役職？
        public static bool IsImpostor(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;
            return Target != null && Target.Data.Role.IsImpostor;
        }


        //インポ陣営役職？
        public static bool IsImpostorTeam(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;
            return Target.IsImpostor() || Target.IsMad();
        }


        //クルーとしてカウントする役職？
        public static bool IsCrew(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;
            return !IsImpostor(Target) && !IsMad(Target) && !IsNeutral(Target) && !IsFriend(Target);
        }


        //第三陣営としてカウントする役職？
        public static bool IsNeutral(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;

            RoleId Role = Target.GetRole();
            return Role switch
            {
                RoleId.Jackal => true,
                RoleId.SideKick => true,
                //RoleGenerator.py用コメント:第三か否か
                _ => false,
            };
        }


        //マッド系役職？
        public static bool IsMad(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;

            RoleId Role = Target.GetRole();
            return Role switch
            {
                RoleId.Madmate => true,
                //RoleGenerator.py用コメント:マッドか否か
                _ => false,
            };
        }


        //フレンド系役職？
        public static bool IsFriend(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;
            return false;

            /*RoleId Role = Target.GetRole();
            return Role switch
            {
                RoleId. => true,
                //RoleGenerator.py用コメント:フレンドか否か
                _ => false,
            };*/
        }


        //重複持ってる？
        public static bool HasAttribute(this PlayerControl Target)
        {
            if (Target.IsBot()) return false;
            return Target.GetAttribute() != RoleId.Crewmate;
        }


        //OOの役職？
        public static bool IsRole(this PlayerControl Target, RoleId Role, bool IsCache = true)
        {
            RoleId RoleId;
            if (IsCache)
            {
                try
                {
                    RoleId = RoleCache.RoleChache[Target.PlayerId];
                }
                catch
                {
                    RoleId = RoleId.Crewmate;
                }
            }
            else
            {
                RoleId = Target.GetRole(false);
            }
            return RoleId == Role;
        }


        //なんの役職？
        public static RoleId GetRole(this PlayerControl Target, bool IsCache = true)
        {
            if (IsCache)
            {
                try
                {
                    return RoleCache.RoleChache[Target.PlayerId];
                }
                catch
                {
                    return RoleId.Crewmate;
                }
            }

            try
            {
                // クルー
                if (RoleClass.Sheriff.SheriffPlayer.IsCheckListPlayerControl(Target)) return RoleId.Sheriff;

                // インポ
                else if (RoleClass.Madmate.MadmatePlayer.IsCheckListPlayerControl(Target)) return RoleId.Madmate;
                else if (RoleClass.SerialKiller.SerialKillerPlayer.IsCheckListPlayerControl(Target)) return RoleId.SerialKiller;

                // 第三
                else if (RoleClass.Jackal.JackalPlayer.IsCheckListPlayerControl(Target)) return RoleId.Jackal;
                else if (RoleClass.SideKick.SideKickPlayer.IsCheckListPlayerControl(Target)) return RoleId.SideKick;

                //デフォルトロール
                else if (Target.IsCrew()) return RoleId.Crewmate;
                else if (Target.IsImpostor()) return RoleId.Impostor;
            }
            catch(SystemException Error)
            {
                Logger.Error($"Roleの取得に失敗しました。エラー:{Error}", "Player");
            }
            return RoleId.Crewmate;
        }


        public static bool IsAttribute_Role(this RoleId RoleId)
        {
            return IntroData.GetIntroData(RoleId).IsAttributeRole;
        }


        //OOの属性役職？
        public static bool IsAttributeRole(this PlayerControl Target, RoleId AttributeRole, bool IsCache = true)
        {
            if (Target.IsBot()) return false;

            RoleId RoleId;
            if (IsCache)
            {
                try
                {
                    RoleId = RoleCache.AttributeCache[Target.PlayerId];
                }
                catch
                {
                    RoleId = RoleId.Crewmate;
                }
            }
            else
            {
                RoleId = Target.GetRole();
            }
            return RoleId == AttributeRole;
        }


        //なんの重複？
        public static RoleId GetAttribute(this PlayerControl Target, bool IsCache = true)
        {
            if (IsCache)
            {
                try
                {
                    return RoleCache.AttributeCache[Target.PlayerId];
                }
                catch
                {
                    return RoleId.Crewmate;
                }
            }
            //キャッシュ化済みでなければ
            try
            {
                if (RoleClass.Debugger.DebuggerPlayer.IsCheckListPlayerControl(Target)) return RoleId.Debugger;
            }
            catch(SystemException Error)
            {
                Logger.Error($"Attributeの取得に失敗しました。エラー:{Error}", "Player");
            }
            return RoleId.Crewmate;
        }
    }


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
}
