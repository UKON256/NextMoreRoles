using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using NextMoreRoles.Roles;
using NextMoreRoles.Mode;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;
using NextMoreRoles.Patches.TitleMenuPatches;
using LogType = BepInEx.Logging.LogLevel;

namespace NextMoreRoles
{
    [BepInPlugin(Id, "NextMoreRoles", VersionString)]
    [BepInIncompatibility("com.emptybottle.townofhost")]
    [BepInIncompatibility("me.eisbison.theotherroles")]
    [BepInIncompatibility("me.yukieiji.extremeroles")]
    [BepInIncompatibility("com.tugaru.TownOfPlus")]
    [BepInProcess("Among Us.exe")]
    [HarmonyPatch]
    public class NextMoreRolesPlugin : BasePlugin
    {
        public static string NextMoreRolesTitle = $@"<color=#7dff7d>Next</color><color=#00ffff>More</color><color=#ff0000>Roles</color>";

        public const string Id = "jp.nextmoreroles";
        public const string VersionString = "1.0.0";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public static int OptionsPage = 1;
        public Harmony Harmony { get; } = new Harmony(Id);
        public static NextMoreRolesPlugin Instance;
        public static Dictionary<string, Dictionary<int, string>> StringDATE;

        public override void Load()
        {
            Instance = this;
            Logger = BepInEx.Logging.Logger.CreateLogSource("NextMoreRoles");

            // 初期化
            try
            {
                RoleBase.Load();
                ModeBase.Load();

                Translator.Load();
                ChangeName.Load();
                Configs.Load();
                CustomOptionHolder.Load();

                var assembly = Assembly.GetExecutingAssembly();
                StringDATE = new Dictionary<string, Dictionary<int, string>>();
                Harmony.PatchAll();

                NextMoreRoles.Logger.Info("スタートアップが正常に終了しました", "Main");
            }
            catch(SystemException Error)
            {
                NextMoreRoles.Logger.Error("スタートアップに失敗しました。\nエラー:"+Error, "Main");
            }
            //ロード終了
        }



        //* ModStamp表示 *//
        [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
        class ShowModStamp
        {
            private static void Postfix(ModManager __instance)
            {
                __instance.ShowModStamp();
            }
        }



        //* PingにMOD情報を記載 *//
        private static string BaseCredentials = $@"<size=130%>{NextMoreRolesTitle}</size> v{NextMoreRolesPlugin.Version}";
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        public static void PingSetMODName(PingTracker __instance)
        {
            __instance.text.text = $"{BaseCredentials}\n{__instance.text.text}";
        }
    }



    //TOHより参考にさせていただきました
    public class Logger
    {
        public static void Info(string Text, string ActedFile, bool IsSendInGame = false, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string fileName = "") =>
            SendMessages(Text, LogType.Info, ActedFile, IsSendInGame, lineNumber, fileName);
        public static void Warn(string Text, string ActedFile, bool IsSendInGame = false, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string fileName = "") =>
            SendMessages(Text, LogType.Warning, ActedFile, IsSendInGame, lineNumber, fileName);
        public static void Error(string Text, string ActedFile, bool IsSendInGame = false, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string fileName = "") =>
            SendMessages(Text, LogType.Error, ActedFile, IsSendInGame, lineNumber, fileName);
        public static void Fatal(string Text, string ActedFile, bool IsSendInGame = false, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string fileName = "") =>
            SendMessages(Text, LogType.Fatal, ActedFile, IsSendInGame, lineNumber, fileName);
        public static void Msg(string Text, string ActedFile, bool IsSendInGame = false, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string fileName = "") =>
            SendMessages(Text, LogType.Message, ActedFile, IsSendInGame, lineNumber, fileName);



        public static void SendMesagesInGame(string Text)
        {
            if (DestroyableSingleton<HudManager>._instance) DestroyableSingleton<HudManager>.Instance.Notifier.AddItem(Text);
        }
        private static void SendMessages(string Text, LogType LogType = LogType.Info, string tag = "", bool IsSendInGame = false ,int lineNumber = 0, string fileName = "")
        {
            var logger = NextMoreRoles.NextMoreRolesPlugin.Logger;
            string t = DateTime.Now.ToString("HH:mm:ss");
            if (IsSendInGame) SendMesagesInGame($"[{tag}]{Text}");

            Text = Text.Replace("\r", "\\r").Replace("\n", "\\n");
            string log_text = $"[{t}][{tag}]{Text}";

            //Typeで送信
            switch (LogType)
            {
                case LogType.Info:
                    logger.LogInfo(log_text);
                    break;
                case LogType.Warning:
                    logger.LogWarning(log_text);
                    break;
                case LogType.Error:
                    logger.LogError(log_text);
                    break;
                case LogType.Fatal:
                    logger.LogFatal(log_text);
                    break;
                case LogType.Message:
                    logger.LogMessage(log_text);
                    break;
                default:
                    logger.LogWarning("ログのタイプが無効です");
                    logger.LogInfo(log_text);
                    break;
            }
        }
    }
}
