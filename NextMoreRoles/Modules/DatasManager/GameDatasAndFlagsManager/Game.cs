namespace NextMoreRoles.Modules.DatasManager
{
    //##=====リセット=====##//
    class ResetGameDatas
    {
        //実行元:Modules.DatasManager.Reset.cs
        public static void Load()
        {

        }

        //実行元:Modules.DatasManager.Reset.cs
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.DatasManager.MeetingDatas.IsMeeting = false;
        }
    }





    //##=====緊急会議系のフラグ=====##//
    static class MeetingDatas
    {
        //現在ミーティング中かどうか。
        //変更元: TRUE:HarmonyPatches.MeetingHud.cs FALSE:GamePatches.GameStart.ClearAndReloads.cs
        public static bool IsMeeting;
    }
}
