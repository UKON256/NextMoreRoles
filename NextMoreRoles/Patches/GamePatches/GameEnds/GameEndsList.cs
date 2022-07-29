namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
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
