using System.Xml.Schema;
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
        public static ConfigEntry<bool> HideTaskArrows { get; set; }
        public static ConfigEntry<bool> EnableHorseMode { get; set; }
        //実行元:Main.cs
        public static void Load()
        {
            try
            {
                //Config作成
                Ip = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
                Port = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Custom Server Port", (ushort)22023);

                AutoUpdate = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Auto Update", true);
                DebugMode = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "Debug Mode", false);
                HideTaskArrows = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "HideTaskArrows", false);
                EnableHorseMode = NextMoreRolesPlugin.Instance.Config.Bind("Custom", "EnableHorseMode", false);
                //Logger.Info("Configの読み込みに成功しました。", "Config");
            }
            catch (SystemException Error)
            {
                Logger.Error("Configの読み込みに失敗しました。エラー:"+Error, "Config");
            }
        }
    }
}
