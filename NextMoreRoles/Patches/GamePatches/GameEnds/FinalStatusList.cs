using System;
using System.Collections.Generic;
using UnityEngine;
using NextMoreRoles.Modules.DatasManager;

namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    class FinalStatusPatch
    {
        public static class FinalStatusDatas
        {
            public static List<Tuple<Vector3, bool>> LocalPlayerPositions = new();
            public static List<DeadPlayer> DeadPlayers = new();
            public static Dictionary<int, FinalPlayerStatus> FinalStatuses = new();

            public static void Clear()
            {
                LocalPlayerPositions = new();
                DeadPlayers = new();
                FinalStatuses = new();
            }
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
