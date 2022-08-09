using System.Collections.Generic;

namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    static class AdditionalTempData
    {
        // なんか.....あれだよ！色々！
        public static List<PlayerRoleInfo> PlayerRoles;
        public static GameOverReason GameOverReason;
        public static WinCondition WinCondition;
        public static List<WinCondition> AdditionalWinConditions;

        //実行元:GamePatches.GameEnds.GameEnds.cs
        public static void Clear()
        {
            PlayerRoles = new();
            AdditionalWinConditions = new();
            WinCondition = WinCondition.ErrorEnd;
        }
        internal class PlayerRoleInfo
        {
            public string PlayerName { get; set; }
            public string NameSuffix { get; set; }
            //public List<RoleInfo> Roles {get;set;}
            public string RoleString { get; set; }
            public int TasksCompleted  {get;set;}
            public int TasksTotal  {get;set;}
        }
    }

    //ＯＯ勝利のリスト
    enum WinCondition
    {
        CrewmateWin,        //クルー
        ImpostorWin,        //インポ

        //OO勝利じゃないやつたち
        Haison,             //廃村
        EveryoneDied,       //全滅
        ErrorEnd,           //バグ
    }

    //終わる理由 (もし理由が↓ならWinConditionをセット～のような)
    enum CustomGameOverReason
    {
        //OO勝利
        //クルー、インポはバニラにあるのでなし！

        //OO勝利じゃないやつ
        Haison,             //廃村
    }

    //音声の種類
    enum EndSounds
    {
        ImpostorWin,
        CrewmateWin,
    }
}
