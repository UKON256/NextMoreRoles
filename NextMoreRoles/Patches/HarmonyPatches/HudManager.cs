using HarmonyLib;
using UnityEngine;

using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    //毎時実行されるよ！
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManager_Update
    {
        private static void Postfix(HudManager __instance)
        {
            LateTask.Update(Time.deltaTime);;
        }
    }
}
