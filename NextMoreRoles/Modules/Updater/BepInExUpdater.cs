//From TheOtherRoles. thanks
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.IL2CPP.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using UnhollowerBaseLib.Attributes;


namespace NextMoreRoles.Modules.Updater;

public class BepInExUpdater : MonoBehaviour
{
    public const string MinimumBepInExVersion = "6.0.0.559";
    public const string BepInExDownloadURL = "https://builds.bepinex.dev/projects/bepinex_be/559/BepInEx_UnityIL2CPP_x86_fba4461_6.0.0-be.559.zip";
    public static bool UpdateRequired => typeof(IL2CPPChainloader).Assembly.GetName().Version < Version.Parse(MinimumBepInExVersion);

    public void Awake()
    {
        NextMoreRolesPlugin.Logger.LogMessage("BepInExのアップデートをリクエストしています...");
        this.StartCoroutine(CoUpdate());
    }

    [HideFromIl2Cpp]
    public IEnumerator CoUpdate()
    {
        Task.Run(() => MessageBox(IntPtr.Zero, "BepInExをアップデートしています。しばらくお待ちください...","Next More Roles", 0));
        UnityWebRequest www = UnityWebRequest.Get(BepInExDownloadURL);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            NextMoreRolesPlugin.Logger.LogError(www.error);
            yield break;
        }

        var zipPath = Path.Combine(Paths.GameRootPath, ".bepinex_update");
        File.WriteAllBytes(zipPath, www.downloadHandler.data);


        var tempPath = Path.Combine(Path.GetTempPath(), "NextMoreRolesUpdater.exe");
        var asm = Assembly.GetExecutingAssembly();
        var exeName = asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith("NextMoreRolesUpdater.exe"));

        using(var resource = asm.GetManifestResourceStream(exeName))
        {
            using(var file = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                resource!.CopyTo(file);
            }
        }

        var startInfo = new ProcessStartInfo(tempPath, $"--game-path \"{Paths.GameRootPath}\" --zip \"{zipPath}\"");
        startInfo.UseShellExecute = false;
        Process.Start(startInfo);
        Application.Quit();
    }

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);
}

[HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
public static class StopLoadingMainMenu
{
    public static bool Prefix()
    {
        return !BepInExUpdater.UpdateRequired;
    }
}