
namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    public static class GameStart_ClearAndReloads
    {
        //実行元:HarmonyPatches.GameStartManager.cs、   終了時:Patches.GamePatches.GameEnds.GameEnds.cs
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.FlagManager.FlagReset.ClearAndReloads();
            NextMoreRoles.Patches.GamePatches.GameEnds.AdditionalTempData.Clear();
            //RoleClass.ClearAndReloads();
        }
    }
}
