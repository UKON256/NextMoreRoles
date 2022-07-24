
namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    public static class GameStart_ClearAndReloads
    {
        //実行元:HarmonyPatches.GameStartManager.cs
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.FlagManager.FlagReset.ClearAndReloads();
            //RoleClass.ClearAndReloads();
        }
    }
}
