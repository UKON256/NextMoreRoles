using HarmonyLib;
using UnityEngine;

using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    //始まった時
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    class HudManager_Start
    {
        static void Postfix(HudManager __instance)
        {
            Logger.Info("========試合開始========", "HudManager");
            NextMoreRoles.Modules.Role.CustomButtons.CustomButtons.HudManagerStart_Postfix(__instance);
        }
    }

    //毎時実行されるよ！
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManager_Update
    {
        static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
            NextMoreRoles.Patches.LobbyPatches.HudManagerUpdate.Prefix(__instance);
        }
        static void Postfix(HudManager __instance)
        {
            LateTask.Update(Time.deltaTime);;
            NextMoreRoles.Modules.Role.CustomButtons.CustomButton.HudUpdate();
        }
    }
}
