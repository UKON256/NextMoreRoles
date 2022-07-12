using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Modules;

namespace NextMoreRoles
{
    [BepInPlugin(Id, "NextMoreRoles", VersionString)]
    //[BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    public class NextMoreRolesPlugin : BasePlugin
    {
        public const string Id = "jp.nextmoreroles";
        public const string VersionString = "1.0.0";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public static int optionsPage = 1;
        public Harmony Harmony { get; } = new Harmony(Id);
        public static NextMoreRolesPlugin Instance;
        public static Dictionary<string, Dictionary<int, string>> StringDATE;
        public static bool IsUpdate = false;
        public static string NewVersion = "";
        public static string thisname;

        public override void Load()
        {
            Logger = Log;
            Instance = this;
            // ロード！！！
            NextMoreRolesPlugin.Logger.LogInfo("ロードを開始しています");
            try
            {
                ModTranslation.Load();
                Configs.Load();
                NextMoreRolesPlugin.Logger.LogInfo("ロードが正常に終了しました");
            }
            catch(SystemException Error)
            {
                NextMoreRolesPlugin.Logger.LogError("ロードに失敗しました。エラー:"+Error);
            }
            //ロード終了！

            //色々開始！
            NextMoreRolesPlugin.Logger.LogInfo("スタートアップを開始しています");
            try
            {
                /*try
                {
                    DirectoryInfo d = new(Path.GetDirectoryName(Application.dataPath) + @"\BepInEx\plugins");
                    string[] files = d.GetFiles("*.dll.old").Select(x => x.FullName).ToArray(); // Getting old versions
                    foreach (string f in files)
                    File.Delete(f);
                }
                catch (Exception Error)
                {
                    NextMoreRolesPlugin.Logger.LogError("Exception occured when clearing old versions:\n" + Error);
                }*/
                var assembly = Assembly.GetExecutingAssembly();
                StringDATE = new Dictionary<string, Dictionary<int, string>>();
                Harmony.PatchAll();
                //SubmergedCompatibility.Initialize();
                NextMoreRolesPlugin.Logger.LogInfo("スタートアップが正常に終了しました");
            }
            catch(SystemException Error)
            {
                NextMoreRolesPlugin.Logger.LogError("スタートアップに失敗しました。エラー:"+Error);
            }
            //色々終わり！
        }

        [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
        class ShowModStamp
        {
            private static void Postfix(ModManager __instance)
            {
                __instance.ShowModStamp();
            }
        }

        //実行元:Patches.HarmonyPatches.PingTracker
        private static string BaseCredentials = $@"<size=130%><color>Next</color><color>More</color><color=#ff0000>Roles</color></size> v{NextMoreRolesPlugin.Version}";
        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        private static void PingSetMODName(PingTracker __instance)
        {
            __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
            //__instance.text.text = $"{BaseCredentials}\n{__instance.text.text}";
            if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
            {
                __instance.text.text = $"{BaseCredentials}\n{__instance.text.text}";
            }
        }
    }
}
