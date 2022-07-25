using UnityEngine;
using NextMoreRoles.Modules;

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
            string CredentialsText = "";
            if (ThisAssembly.Git.Branch != "master")//masterビルド以外の時
            {
                //色+ブランチ名+コミット番号
                CredentialsText = $"\r\n<color=#a6d289>{ThisAssembly.Git.Branch}({ThisAssembly.Git.Commit})</color>";
            }
            CredentialsText += "Created by UKON256";
            Credentials.SetText(CredentialsText);
            Credentials.alignment = TMPro.TextAlignmentOptions.Center;
            Credentials.fontSize *= 0.9f;
            Credentials.transform.SetParent(AmongUsLogo.transform);

            var Version = UnityEngine.Object.Instantiate(Credentials);
            Version.transform.position = new Vector3(0, -0.35f, 0);
            Version.SetText(string.Format(ModTranslation.GetString("CreditsVersion"), NextMoreRolesPlugin.Version.ToString()));

            Credentials.transform.SetParent(AmongUsLogo.transform);
            Version.transform.SetParent(AmongUsLogo.transform);
        }
    }
}
