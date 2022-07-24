using UnityEngine;

namespace NextMoreRoles.Patches.GamePatches
{
    class PingMessages
    {
        public static void SetPingMessages(PingTracker __instance)
        {
            __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
            string PingText = __instance.text.text;

            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            NextMoreRolesPlugin.PingSetMODName(__instance);
            NextMoreRoles.Patches.GamePatches.DebugModePatch.PingSetDebugMode(__instance);
            //ブランチがmaster以外の時にブランチ名を表示
            if (ThisAssembly.Git.Branch != "master" || Configs.IsDebugMode.Value)
            {
                //改行+Branch名+コミット番号
                __instance.text.text += "\n" + ($"ブランチ:{ThisAssembly.Git.Branch}({ThisAssembly.Git.Commit})");
            }

            //Pingを表示
            __instance.text.text += "\n" + PingText;

            //座標変更
            __instance.transform.localPosition = new Vector3(3.5f, __instance.transform.localPosition.y, __instance.transform.localPosition.z);
        }
    }
}
