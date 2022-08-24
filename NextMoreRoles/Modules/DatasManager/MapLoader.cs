using BepInEx.IL2CPP.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

//SNRより参考
namespace NextMoreRoles.Modules.DatasManager
{
    class MapLoader
    {
        private static ShipStatus skeld;
        public static ShipStatus Skeld
        {
            get
            {
                return skeld;
            }
        }

        public static ShipStatus Polus;

        private static ShipStatus airship;
        public static ShipStatus Airship
        {
            get
            {
                return airship;
            }
        }
        public static GameObject SkeldObject => Skeld.gameObject;

        public static GameObject AirshipObject => Airship.gameObject;

        public static GameObject PolusObject => Polus.gameObject;



        public static IEnumerator LoadMaps()
        {
            while ((UnityEngine.Object)(object)AmongUsClient.Instance == null)
            {
                yield return null;
            }

            AsyncOperationHandle<GameObject> SkeldAsset = AmongUsClient.Instance.ShipPrefabs.ToArray()[0].LoadAsset<GameObject>();
            while (!SkeldAsset.IsDone)
            {
                yield return null;
            }
            skeld = SkeldAsset.Result.GetComponent<ShipStatus>();

            AsyncOperationHandle<GameObject> PolusAsset = AmongUsClient.Instance.ShipPrefabs.ToArray()[2].LoadAsset<GameObject>();
            while (!PolusAsset.IsDone)
            {
                yield return null;
            }
            Polus = PolusAsset.Result.GetComponent<ShipStatus>();

            AsyncOperationHandle<GameObject> AirshipAsset = AmongUsClient.Instance.ShipPrefabs.ToArray()[4].LoadAsset<GameObject>();
            while (!AirshipAsset.IsDone)
            {
                yield return null;
            }
            airship = AirshipAsset.Result.GetComponent<ShipStatus>();
        }



        public static class AmongUsClient_Awake
        {
            static bool Loaded = false;   //ロード終わったかどうか
            public static void Prefix(AmongUsClient __instance)
            {
                if (Loaded) return;
                Loaded = true;

                ((MonoBehaviour)(object)__instance).StartCoroutine(LoadMaps());
            }
        }
    }
}
