
namespace NextMoreRoles.Patches.LobbyPatches
{
    class QuickGameStart
    {
        //実行元:HarmonyPatches.GameStartManager.cs
        public static void QuickStart()
        {
            GameStartManager.Instance.countDownTimer = 0;
        }
        //実行元:HarmonyPatches.GameStartManager.cs
        public static void QuickStartCancel()
        {
            GameStartManager.Instance.ResetStartState();
        }
    }
}
