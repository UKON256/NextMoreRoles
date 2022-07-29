using System.Collections.Generic;

namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    // 勝利状態関係
    //ＯＯ勝利のリスト
    enum WinCondition
    {
        CrewmateWin,
        ImpostorWin,

        //OO勝利じゃないやつたち
        Haison,         //廃村
        EveryoneDied,   //全滅
        ErrorEnd,       //バグ
    }
    //終わる理由 (もし理由が↓ならWinConditionをセット～のような)
    enum CustomGameOverReason
    {
        CrewmateWin = 7,//クルー
        ImpostorWin = 8,//インポ

        //OO勝利じゃないやつ
        Haison,         //廃村
    }
}
