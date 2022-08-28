using System;
using System.Collections.Generic;
using UnityEngine;
using NextMoreRoles.Modules.CustomOptions;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Roles
{
    public enum RoleType
    {
        Crewmate,
        Impostor,
        Neutral,
        Attribute,
    }

    public static class RoleClass
    {
        //役職のデータリセット  実行元:GamePatches.GameStart.ClearAndReloads.cs
        public static void ClearAndReloads()
        {
            try
            {
                //=====クルーメイト陣営=====//
                Sheriff.ClearAndReload();

                //=====インポスター陣営=====//
                Madmate.ClearAndReload();
                SerialKiller.ClearAndReload();

                //=====    第三陣営    =====//
                Jackal.ClearAndReload();
                SideKick.ClearAndReload();

                //=====デバッグ用=====//
                Debugger.ClearAndReload();
            }
            catch(SystemException Error)
            {
                Logger.Error($"役職のリセットに失敗しました。エラー:{Error}", "RoleClass");
            }
        }



        //======クルーメイト陣営=====//
        public static class Sheriff
        {
            public static List<PlayerControl> SheriffPlayer;
            public static Color32 Color = new Color32(248, 205, 70, byte.MaxValue);

            //設定(CustomOption)
            public static float FireCool;
            public static int CanFireLimit;
            public static bool CanFireMadmate;
            public static bool CanFireNeutral;

            public static void ClearAndReload()
            {
                SheriffPlayer = new();
                FireCool = CustomOptions.SheriffFireCool.GetFloat();
                CanFireLimit = CustomOptions.SheriffCanFireLimit.GetInt();
                CanFireMadmate = CustomOptions.SheriffCanFireMadmate.GetBool();
                CanFireNeutral = CustomOptions.SheriffCanFireNeutral.GetBool();
            }
        }



        //=====インポスター陣営=====//
        public static class Madmate
        {
            public static List<PlayerControl> MadmatePlayer;
            public static Color32 Color = Palette.ImpostorRed;

            //設定(CustomOption)
            public static bool CanVent;
            public static bool IsImpostorVision;
            public static bool CanKnowImpostor;
            public static int MadmateNeedsTask;

            public static void ClearAndReload()
            {
                MadmatePlayer = new();
                CanVent = CustomOptions.MadmateCanVent.GetBool();
                IsImpostorVision = CustomOptions.MadmateIsImpostorVision.GetBool();
                CanKnowImpostor = CustomOptions.MadmateCanKnowImpostor.GetBool();

                //タスク系
                if (CanKnowImpostor)
                {
                    int CommonTask = CustomOptions.MadmateTask.CommonTasks;
                    int LongTask = CustomOptions.MadmateTask.LongTasks;
                    int ShortTask = CustomOptions.MadmateTask.ShortTasks;
                    int TotalTasks = CommonTask + LongTask + ShortTask;
                    //マッドのトータルタスクが0なら他プレイヤーと同じタスク量に
                    if (TotalTasks == 0)
                    {
                        CommonTask = PlayerControl.GameOptions.NumCommonTasks;
                        LongTask = PlayerControl.GameOptions.NumLongTasks;
                        ShortTask = PlayerControl.GameOptions.NumShortTasks;
                    }
                    MadmateNeedsTask = (int)(TotalTasks * (int.Parse(CustomOptions.MadmateTask.GetString().Replace("%", "")) / 100f));
                }
            }
        }

        public static class SerialKiller
        {
            public static List<PlayerControl> SerialKillerPlayer;
            public static Color32 Color = Palette.ImpostorRed;

            //設定(CustomOption)
            public static float KillCool;
            public static float SucideTime;             //自殺時間
            public static bool IsMeetingReset;          //会議でリセット


            public static void ClearAndReload()
            {
                SerialKillerPlayer = new();
                KillCool = CustomOptions.SerialKillerKillCool.GetFloat();
                SucideTime = CustomOptions.SerialKillerSucideTime.GetFloat();
                IsMeetingReset = CustomOptions.SerialKillerIsMeetingReset.GetBool();
            }
        }



        //=====第三陣営=====//
        public static class Jackal
        {
            public static List<PlayerControl> JackalPlayer;
            public static Color32 Color = new Color32(65, 105, 255, byte.MaxValue);

            //設定(CustomOption)
            public static float KillCool;
            public static bool CanVent;
            public static bool IsImpostorVision;        //インポの視界
            public static bool CanMakeSideKick;
            public static float MakeCool;               //指名クール

            public static void ClearAndReload()
            {
                JackalPlayer = new();
                KillCool = CustomOptions.JackalKillCool.GetFloat();
                CanVent = CustomOptions.JackalCanVent.GetBool();
                IsImpostorVision = CustomOptions.JackalIsImpostorVision.GetBool();
                CanMakeSideKick = CustomOptions.JackalCanMakeSideKick.GetBool();
                MakeCool = CustomOptions.SideKickMakeCool.GetFloat();
            }
        }

        public static class SideKick
        {
            public static List<PlayerControl> SideKickPlayer;
            public static Color32 Color = new Color32(65, 105, 255, byte.MaxValue);

            //設定(CustomOption)
            public static bool SideKickCanPromotion;
            public static bool SideKickCanVent;
            public static bool SideKickCanMakeSideKick; //昇格サイドキックがサイドキックを作れる
            public static bool SideKickCanKill;

            public static void ClearAndReload()
            {
                SideKickPlayer = new();
                SideKickCanPromotion = CustomOptions.SideKickCanPromotion.GetBool();
                SideKickCanVent = CustomOptions.SideKickCanVent.GetBool();
                SideKickCanMakeSideKick = CustomOptions.SideKickCanMakeSideKick.GetBool();
                SideKickCanKill = CustomOptions.SideKickCanKill.GetBool();
            }
        }



        //=====デバッグ用=====//
        public static class Debugger
        {
            public static List<PlayerControl> DebuggerPlayer;
            public static Color32 Color = Palette.DisabledGrey;
            public static Sprite ButtonSprite;
            public static Data.Attribute.DebugTabs NowTab;

            //設定(CustomOption)

            public static Sprite GetButtonSprite()
            {
                if (ButtonSprite) return ButtonSprite;
                ButtonSprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Game.RoleButtons.6Debugger.png", 115f);
                return ButtonSprite;
            }

            public static void ClearAndReload()
            {
                DebuggerPlayer = new();
            }
        }
    }
}
