using HarmonyLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    //ゲームが始まった時実行される奴
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    class MainMenuManager_Start
    {
        static void Postfix(MainMenuManager __instance)
        {
            NextMoreRoles.Patches.TitlePatches.SetNMRLogo.SetLogo();
            NextMoreRoles.Patches.HarmonyPatches.Harmony_ClientOptionsMain.MainMenu_Start_Postfix.Postfix(__instance);
        }
    }
}
