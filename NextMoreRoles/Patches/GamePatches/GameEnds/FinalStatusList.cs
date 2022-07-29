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

    //プレイヤーの最終状態のリスト
    enum FinalPlayerStatus
    {
        Alive,          //生存
        Killed,         //キル
        Exiled,         //追放
        Sabotage,       //サボタージュで死亡
        Disconnected,   //切断
        Unknown,        //不明(エラーてきな)
    }
}
