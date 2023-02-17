using HarmonyLib;
using UnityEngine;

namespace NextMoreRoles.Patches.LobbyPatches;

[HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
class QuickStartGame_Update
{
    static void Prefix()
    {
        if (Input.GetKeyDown(KeyCode.F7)) GameStartManager.Instance.ResetStartState();
        if (Input.GetKeyDown(KeyCode.F8)) GameStartManager.Instance.countDownTimer = 0;
    }
}
