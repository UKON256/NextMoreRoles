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
        public static Sprite ModStamp;
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
            Logger.LogInfo(NextMoreRoles.Modules.ModTranslation.getString("StartLogText"));
            // ロード！！！
            NextMoreRoles.Modules.ModTranslation.Load();
            Configs.Load();

            try
            {
                DirectoryInfo d = new(Path.GetDirectoryName(Application.dataPath) + @"\BepInEx\plugins");
                string[] files = d.GetFiles("*.dll.old").Select(x => x.FullName).ToArray(); // Getting old versions
                foreach (string f in files)
                    File.Delete(f);
            }
            catch (Exception Error)
            {
                NextMoreRolesPlugin.Logger.LogError("Exception occured when clearing old versions:\n" + Error);
            }

            var assembly = Assembly.GetExecutingAssembly();

            StringDATE = new Dictionary<string, Dictionary<int, string>>();
            Harmony.PatchAll();
            //SubmergedCompatibility.Initialize();
        }
        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
        public static class AmBannedPatch
        {
            public static void Postfix(out bool __result)
            {
                __result = false;
            }
        }
        [HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
        public static class ChatControllerAwakePatch
        {
            public static void Prefix()
            {
                SaveManager.chatModeType = 1;
                SaveManager.isGuest = false;
            }
            public static void Postfix(ChatController __instance)
            {
                SaveManager.chatModeType = 1;
                SaveManager.isGuest = false;

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    if (!__instance.isActiveAndEnabled) return;
                    __instance.Toggle();
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    __instance.SetVisible(false);
                    new LateTask(() =>
                    {
                        __instance.SetVisible(true);
                    }, 0f, "AntiChatBag");
                }
                if (__instance.IsOpen)
                {
                    if (__instance.animating)
                    {
                        __instance.BanButton.MenuButton.enabled = false;
                    }
                    else
                    {
                        __instance.BanButton.MenuButton.enabled = true;
                    }
                }
            }
        }
    }
}
