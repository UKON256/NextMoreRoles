using System.Collections.Generic;
using UnityEngine;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles
{
    public static class RoleClass
    {
        //役職のデータリセット  実行元:GamePatches.GameStart.ClearAndReloads.cs
        public static void ClearAndReload()
        {
            //=====クルーメイト陣営=====//
            Sheriff.ClearAndReload();

            //=====インポスター陣営=====//
            Madmate.ClearAndReload();
            SerialKiller.ClearAndReload();

            //=====    第三陣営    =====//
            Jackal.ClearAndReload();
            SideKick.ClearAndReload();
        }



        //======クルーメイト陣営=====//
        public static class Sheriff
        {
            public static List<PlayerControl> SheriffPlayer;
            public static Color Color = new Color32(248, 205, 70, byte.MaxValue);

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
            public static Color Color = Palette.ImpostorRed;

            //設定(CustomOption)
            public static bool CanVent;
            public static bool IsImpostorView;
            public static bool CanKnowImpostor;
            public static int MadmateNeedsTask;

            public static void ClearAndReload()
            {
                MadmatePlayer = new();

                //タスク系
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

        public static class SerialKiller
        {
            public static List<PlayerControl> SerialKillerPlayer;
            public static Color Color = Palette.ImpostorRed;

            //設定(CustomOption)

            public static void ClearAndReload()
            {
                SerialKillerPlayer = new();
            }
        }



        //=====第三陣営=====//
        public static class Jackal
        {
            public static List<PlayerControl> JackalPlayer;
            public static Color Color = new Color32(65, 105, 255, byte.MaxValue);

            //設定(CustomOption)

            public static void ClearAndReload()
            {
                JackalPlayer = new();
            }
        }

        public static class SideKick
        {
            public static List<PlayerControl> SideKickPlayer;
            public static Color Color = new Color32(65, 105, 255, byte.MaxValue);

            //設定(CustomOption)

            public static void ClearAndReload()
            {
                SideKickPlayer = new();
            }
        }
    }
}
