using UnityEngine;

namespace NextMoreRoles.Patches.TitlePatches
{
    class WrapUpPatch
    {
        public static void SetAmongUsLogo(VersionShower __instance)
        {
            var AmongUsLogo = GameObject.Find("bannerLogo_AmongUs");
            if (AmongUsLogo == null) return;

            var Credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
            Credentials.transform.position = new Vector3(0, 0, 0);
            Credentials.SetText($"\r\n<color=#a6d289>{ThisAssembly.Git.Branch}({ThisAssembly.Git.Commit})</color>");
            Credentials.alignment = TMPro.TextAlignmentOptions.Center;
            Credentials.fontSize *= 0.75f;
            Credentials.transform.SetParent(AmongUsLogo.transform);
        }
    }
}
