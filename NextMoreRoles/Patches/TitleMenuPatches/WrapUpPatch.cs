using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.TitleMenuPatches;

[HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
class WrapUpPatch
{
    static void Postfix(VersionShower __instance)
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

[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
class StartMainMenu
{
    public static void Postfix()
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
        NMRLogo.AddComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TitleLogo.png", 150f);
    }
}
