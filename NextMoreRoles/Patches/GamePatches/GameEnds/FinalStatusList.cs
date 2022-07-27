
namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    //カスタム勝利
    enum CustomGameEndReason
    {
        Haison,
    }

    //試合最終結果のやつ
    enum CustomFinalStatus
    {
        Alive,          //生存
        Killed,         //キル
        Exiled,         //追放
        Disconnected,   //切断
    }
}
