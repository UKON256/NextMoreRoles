using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    class KeyboardJoystick_Update
    {
        //即開始:LobbyPatches.QuickStart
        static void Postfix(KeyboardJoystick __instance)
        {
            //ホスト
            if (!AmongUsClient.Instance.AmHost) return;

            //デバッグモードかつホスト
            if (!Configs.IsDebugMode.Value) return;
            if (Input.GetKeyDown(KeyCode.G)) NextMoreRoles.Patches.GamePatches.DebugModePatch.BotSpawn();
        }
    }

    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class DebugManager
    {
        static void Postfix()
        {
            if (AmongUsClient.Instance.GameState == AmongUsClient.GameStates.Started)
            {
                //廃村
                if (AmongUsClient.Instance.AmHost && Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
                {
                    NextMoreRoles.Patches.GamePatches.HaisonAndMeetingSkip.Haison();
                }
                //会議を強制終了
                if (AmongUsClient.Instance.AmHost && Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
                {
                    NextMoreRoles.Patches.GamePatches.HaisonAndMeetingSkip.MeetingSkip();
                }
            }
        }
    }
}
