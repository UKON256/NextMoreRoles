using System.Collections;
using BepInEx.IL2CPP.Utils;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NextMoreRoles.Modules.DatasManager
{
    class MeetingHudLoader
    {
        private static MeetingHud meetingHud;
        public static MeetingHud MeetingHud
        {
            get
            {
                return meetingHud;
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
            meetingHud = HudAssets.Result.GetComponent<MeetingHud>();
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
