using HarmonyLib;
using UnityEngine;

using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    //毎時実行されるよ！
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManager_Update
    {
        static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
            NextMoreRoles.Patches.GamePatches.HudManagerUpdate.Prefix(__instance);
        }
        static void Postfix(HudManager __instance)
        {
            LateTask.Update(Time.deltaTime);;
        }
    }
}
