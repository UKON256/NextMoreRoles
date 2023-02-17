using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches;
class DebugMode
{
    //* 起動イントロをカット *//
    [HarmonyPatch]
    class CutSplash
    {
        [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
        static void Prefix(SplashManager __instance)
        {
            if (Configs.IsDebugMode.Value)
            {
                __instance.sceneChanger.AllowFinishLoadingScene();
                __instance.startedSceneLoad = true;
            }
        }
    }

    //* 最少人数を変更 *//
    [HarmonyPatch]
    class ChangeMinPlayer
    {
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        static void Postfix(GameStartManager __instance)
        {
            if (Configs.IsDebugMode.Value) __instance.MinPlayers = 1;
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        static void Postfix()
        {
            if (Configs.IsDebugMode.Value) GameStartManager.Instance.countDownTimer = 0;
        }
    }

    //* ボットをスポーンさせる *//
    [HarmonyPatch]
    class Bot
    {
        //ボットをスポーンさせる
        [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
        static void Postfix()
        {
            if (!Input.GetKey(KeyCode.LeftControl)) return;

            if (Input.GetKeyDown(KeyCode.G)) BotManager.Spawn();
        }

        //ボットに投票させる
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
        static void Postfix(MeetingHud __instance)
        {
            if (!Configs.IsDebugMode.Value && BotManager.AllBots == null) return;

            new LateTask(
                () =>
                {
                    foreach(PlayerControl Bot in BotManager.AllBots)
                    {
                        BotManager.Vote(Bot, __instance);
                    }
                }, 2.5f, "Vote");
        }
    }

}
