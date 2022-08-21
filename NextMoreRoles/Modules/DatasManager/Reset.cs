
namespace NextMoreRoles.Modules.DatasManager
{
    class Reset
    {
        //実行元:Main.cs
        public static void Load()
        {
            NextMoreRoles.Modules.DatasManager.ResetRoleCache.Load();
            NextMoreRoles.Modules.DatasManager.ResetGameDatas.Load();
            NextMoreRoles.Modules.DatasManager.ResetPlayerDatas.Load();
        }

        //試合が始まった時
        //実行元:Patches.GameStart.ClearAndReloads.cs
        public static void ClearAndReloads()
        {
            NextMoreRoles.Modules.DatasManager.ResetRoleCache.ClearAndReloads();
            NextMoreRoles.Modules.DatasManager.ResetGameDatas.ClearAndReloads();
            NextMoreRoles.Modules.DatasManager.ResetPlayerDatas.ClearAndReloads();
        }
    }
}
