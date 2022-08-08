using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnhollowerBaseLib;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.InitializeOptions))]
    class GameSettingMenu_Init
    {
        static void Prefix(GameSettingMenu __instance)
        {
            //部屋を立て直さなくてもマップ変更できるようにする
            __instance.HideForOnline = new Il2CppReferenceArray<Transform>(0);
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    class GameSettingMenu_Start_ChangeMap
    {
        static void Postifx(GameSettingMenu __instance)
        {
            try
            {
                var MapNameTransform = __instance.AllItems.FirstOrDefault(x => x.name.Equals("MapName", StringComparison.OrdinalIgnoreCase));
                if (MapNameTransform == null) return;
                MapNameTransform.gameObject.active = true;

                foreach (Transform i in __instance.AllItems.ToList())
                {
                    float num = -0.5f;
                    if (i.name.Equals("MapName", StringComparison.OrdinalIgnoreCase)) num = 0.25f;
                    if (i.name.Equals("NumImpostors", StringComparison.OrdinalIgnoreCase)) num = -0.5f;
                    if (i.name.Equals("ResetToDefault", StringComparison.OrdinalIgnoreCase)) num = 0f;
                    i.position += new Vector3(0, num, 0);
                }
                __instance.Scroller.ContentYBounds.max += 0.5F;
            }
            catch(SystemException Error)
            {
                Logger.Error($"設定の変更に失敗しました。エラー:{Error}", "GameSettingMenu");
            }
        }
    }
}
