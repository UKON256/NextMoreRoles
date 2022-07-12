using System;
using BepInEx.Configuration;
namespace NextMoreRoles
{
    public static class Configs
    {
        public static ConfigEntry<string> Ip { get; set; }
        public static ConfigEntry<ushort> Port { get; set; }

        public static ConfigEntry<bool> AutoUpdate { get; set; }
        public static ConfigEntry<bool> DebugMode { get; set; }
        public static ConfigEntry<bool> IsVersionErrorView { get; set; }
        public static ConfigEntry<string> NextMoreCosmeticsURL { get; set; }
        public static ConfigEntry<bool> HideTaskArrows { get; set; }
        public static ConfigEntry<bool> EnableHorseMode { get; set; }
        public static ConfigEntry<bool> IsDownloadNextMoreSkins { get; set; }
        public static ConfigEntry<bool> IsDownloadOtherSkins { get; set; }
        //実行元:Main.cs
        public static void Load()
        {
            try
            {
                Ip = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
                Port = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Custom Server Port", (ushort)22023);

                AutoUpdate = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Auto Update", true);
                DebugMode = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Debug Mode", false);
                HideTaskArrows = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "HideTaskArrows", false);
                EnableHorseMode = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "EnableHorseMode", false);
                NextMoreRolesPlugin.Logger.LogInfo("Configの読み込みに成功しました。");
            }
            catch (SystemException Error)
            {
                NextMoreRolesPlugin.Logger.LogError("Configの読み込みに失敗しました。エラー:"+Error);
            }
        }
    }
}
