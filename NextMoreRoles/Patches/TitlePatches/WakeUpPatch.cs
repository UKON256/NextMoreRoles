using UnityEngine;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.TitlePatches
{
    class WrapUpPatch
    {
        //実行元:HarmonyPatches.VersionShower.cs
        public static void ChangeAmongUsLogo(VersionShower __instance)
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
            Credentials.transform.localPosition += new Vector3(0, 0, 0);

            var Version = UnityEngine.Object.Instantiate(Credentials);
            Version.transform.position = new Vector3(0, -0.35f, 0);
            Version.SetText($"{NextMoreRolesPlugin.NextMoreRolesTitle} v{NextMoreRolesPlugin.Version.ToString()}");

            Credentials.transform.SetParent(AmongUsLogo.transform);
            Version.transform.SetParent(AmongUsLogo.transform);
        }
    }



    //実行元:HarmonyPatches.MainMenuManager.cs
    public class SetNMRLogo
    {
        public static SpriteRenderer Renderer;

        //実行元:HarmonyPatches.MainMenuManager.cs
        public static void SetLogo()
        {
            //あもあすロゴ！
            var AmongUsLogo = GameObject.Find("bannerLogo_AmongUs");
            if (AmongUsLogo != null)
            {
                AmongUsLogo.transform.localScale *= 0.6f;
                AmongUsLogo.transform.position += Vector3.up * 0.25f;
            }

            //NMRロゴ！
            var NMRLogo = new GameObject("NMRLogo");
            NMRLogo.transform.position = Vector3.up;
            NMRLogo.transform.localScale *= 0.55f;
            Renderer = NMRLogo.AddComponent<SpriteRenderer>();
            Renderer.sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Titles.TitleLogo.png", 150f);
        }
    }
}
