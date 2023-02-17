using System;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using BepInEx.IL2CPP.Utils.Collections;
using NextMoreRoles.Roles;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules;

namespace NextMoreRoles.Patches.GamePatches.GameStart;

class Intropatch
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    class IntroCutscene_ShowRole
    {
        static bool Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
        {
            __result = SetupRole(__instance).WrapToIl2Cpp();
            return false;
        }

        private static IEnumerator SetupRole(IntroCutscene __instance)
        {
            var role = RoleHelper.GetLocalPlayerRole();
            //var attribute = RoleHelper.GetLocalPlayerAttribute();

            //音声再生
            SoundManager.Instance.PlaySound(role.GetIntroSound(), false);

            __instance.YouAreText.color = role.RoleNameColor;             //あなたのロールは...を役職の色に変更
            __instance.RoleText.text = role.GetRoleName();                //役職名を変更
            __instance.RoleText.color = role.RoleNameColor;               //役職名の色を変更
            __instance.RoleBlurbText.text = role.GetIntroDescription();   //イントロの簡易説明を変更
            __instance.RoleBlurbText.color = role.RoleNameColor;          //イントロの簡易説明の色を変更

            //重複を持っていたらメッセージ追記
            //if (PlayerControl.LocalPlayer.HasAttribute()) { __instance.RoleBlurbText.text += "\n" + ModHelpers.cs(attribute.RoleNameColor, attribute.GetIntroDescription()); }

            //プレイヤーを再表示&位置変更
            __instance.ourCrewmate = __instance.CreatePlayer(0, 1, PlayerControl.LocalPlayer.Data, false);
            __instance.ourCrewmate.gameObject.SetActive(false);
            __instance.ourCrewmate.transform.localPosition = new Vector3(0f, -1.05f, -18f);
            __instance.ourCrewmate.transform.localScale = new Vector3(1f, 1f, 1f);

            //字幕を再表示する(Prefixで消している)
            __instance.ourCrewmate.gameObject.SetActive(true);
            __instance.YouAreText.gameObject.SetActive(true);
            __instance.RoleText.gameObject.SetActive(true);
            __instance.RoleBlurbText.gameObject.SetActive(true);

            //メッセージ表示2.5秒後にすべて非表示にする
            yield return new WaitForSeconds(2.5f);
            __instance.ourCrewmate.gameObject.SetActive(false);     //プレイヤーを消す
            __instance.YouAreText.gameObject.SetActive(false);      //あなたのロールは....を消す
            __instance.RoleText.gameObject.SetActive(false);        //役職名を消す
            __instance.RoleBlurbText.gameObject.SetActive(false);   //役職のイントロ説明文を消す

            yield break;
        }
    }



    [HarmonyPatch]
    class IntroPatch
    {
        //* 表示プレイヤーの変更 *//
        private static void SetupIntroTeamPlayers(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
        {
            var role = PlayerControl.LocalPlayer.GetRole();
            Il2CppSystem.Collections.Generic.List<PlayerControl> TeamPlayers = new();
            TeamPlayers.Add(PlayerControl.LocalPlayer);

            //* クルーなら自分以外全員追加 *//
            if (role.IsCrewmateRole()) {
                foreach (PlayerControl p in CachedPlayer.AllPlayers) {
                    if (p.PlayerId != CachedPlayer.LocalPlayer.PlayerId){ TeamPlayers.Add(p); }
                }
            }
            //* インポならその陣営を追加 */
            else if (role.IsImpostorRole()) {
                foreach (PlayerControl p in CachedPlayer.AllPlayers) {
                    if (p.PlayerId != CachedPlayer.LocalPlayer.PlayerId && p.Data.Role.IsImpostor) { TeamPlayers.Add(p); }
                }
            }
            yourTeam = TeamPlayers;
        }

        //* 陣営の表示 *//
        private static void SetupIntroTeam(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
        {
            var roleTeam = RoleHelper.GetLocalPlayerRole().RoleType;

            switch (roleTeam) {
                case RoleType.Crewmate:
                    __instance.BackgroundBar.material.color = Palette.CrewmateBlue;
                    __instance.TeamTitle.text = Translator.GetString("Crewmate");
                    __instance.TeamTitle.color = Palette.CrewmateBlue;
                    break;

                case RoleType.Impostor:
                    __instance.BackgroundBar.material.color = Palette.ImpostorRed;
                    __instance.TeamTitle.text = Translator.GetString("Impostor");
                    __instance.TeamTitle.color = Palette.ImpostorRed;
                    __instance.ImpostorText.text = Translator.GetString("ImpostorIntroDescription");;
                    break;

                case RoleType.Neutral:
                    Color32 NeutralColor = new(125, 125, 125, byte.MaxValue);
                    __instance.BackgroundBar.material.color = NeutralColor;
                    __instance.TeamTitle.text = Translator.GetString("Neutral");
                    __instance.TeamTitle.color = NeutralColor;
                    __instance.ImpostorText.text = Translator.GetString("NeutralIntroDescription");
                    break;
            }
        }



        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch
        {
            static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                SetupIntroTeamPlayers(__instance, ref teamToDisplay);
            }
            static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                SetupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch
        {
            static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                SetupIntroTeamPlayers(__instance, ref yourTeam);
            }

            static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                SetupIntroTeam(__instance, ref yourTeam);
            }
        }
    }
}
