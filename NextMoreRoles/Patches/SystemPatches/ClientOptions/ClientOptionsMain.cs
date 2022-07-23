using HarmonyLib;

namespace NextMoreRoles.Patches.SystemPatches.ClientOptions
{
    public static class Harmony_ClientOptionsMain
    {
        //言語が設定されたときに実行される奴
        [HarmonyPatch(typeof(LanguageSetter), nameof(LanguageSetter.SetLanguage))]
        class SetLanguagePatch
        {
            static void Postfix()
            {
                ClientModOptions.UpdateTranslations();
                ClientVanillaOptions.UpdateTranslations();
            }
        }



        //設定ボタンを押したときに実行される奴
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        class MainMenu_Start_Postfix
        {
            static void Postfix(MainMenuManager __instance)
            {
                NextMoreRoles.Patches.SystemPatches.ClientOptions.ClientVanillaOptions.MainMenu_Start_Postfix(__instance);
                NextMoreRoles.Patches.SystemPatches.ClientOptions.ClientModOptions.MeinMenu_Start_Postfix(__instance);
            }
        }



        //オプションのあれこれが始まった時の奴
        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
        class OptionsMenu_Start
        {
            static void Postfix(OptionsMenuBehaviour __instance)
            {
                NextMoreRoles.Patches.SystemPatches.ClientOptions.ClientVanillaOptions.OptionsMenu_Start_Postfix(__instance);
                NextMoreRoles.Patches.SystemPatches.ClientOptions.ClientModOptions.OptionsMenu_Start_Postfix(__instance);
            }
        }



        //オプションのあれこれが開いているときの奴
        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Update))]
        class OptionsMenu_Update
        {
            static void Postfix(OptionsMenuBehaviour __instance)
            {
                //
                NextMoreRoles.Patches.SystemPatches.ClientOptions.ClientVanillaOptions.OptionsMenu_Update_Postfix(__instance);
            }
        }
    }
}
