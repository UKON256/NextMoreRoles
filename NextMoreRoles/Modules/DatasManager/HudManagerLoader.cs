using System.Collections;
using BepInEx.IL2CPP.Utils;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NextMoreRoles.Modules.DatasManager
{
    class HudManagerLoader
    {
        private static HudManager hudmanager;
        public static HudManager HudManager
        {
            get
            {
                return hudmanager;
            }
        }



        public static IEnumerator LoadHud()
        {
            while ((UnityEngine.Object)(object)AmongUsClient.Instance == null)
            {
                yield return null;
            }
            AsyncOperationHandle<GameObject> HudAssets = AmongUsClient.Instance.ShipPrefabs.ToArray()[0].LoadAsset<GameObject>();

            while (!HudAssets.IsDone)
            {
                yield return null;
            }
            hudmanager = HudAssets.Result.GetComponent<HudManager>();
        }



        public static class AmongUsClient_Awake
        {
            static bool Loaded = false;   //ロード終わったかどうか
            public static void Prefix(AmongUsClient __instance)
            {
                if (Loaded) return;
                Loaded = true;

                ((MonoBehaviour)(object)__instance).StartCoroutine(LoadHud());
            }
        }
    }
}
