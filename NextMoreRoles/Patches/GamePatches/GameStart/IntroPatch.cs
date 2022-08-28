using System;
using BepInEx.IL2CPP.Utils.Collections;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;
using NextMoreRoles.Roles;
using NextMoreRoles.Modules.DatasManager;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutscene_OnDestroy
    {
        static void Prefix()
        {
            //タスクリストの役職説明を更新する
            RoleSet.RefreshRoleDescription(PlayerControl.LocalPlayer);
        }
    }

    //役職名表示！
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    class IntroCutscene_ShowRole
    {
        static bool Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
        {
            __result = SetupRole(__instance).WrapToIl2Cpp();
            return false;   //役職表示をさせない
        }

        private static IEnumerator SetupRole(IntroCutscene __instance)
        {
            try
            {
                var PlayerRole = PlayerControl.LocalPlayer.GetRole();
                var PlayerAttribute = PlayerControl.LocalPlayer.GetAttribute();
                var IntroInfo = IntroData.GetIntroData(PlayerRole);

                //音声再生
                IntroData.PlayIntroSound(PlayerRole);

                __instance.YouAreText.color = IntroInfo.Color;                  //あなたのロールは...を役職の色に変更
                __instance.RoleText.text = IntroInfo.Name;                      //役職名を変更
                __instance.RoleText.color = IntroInfo.Color;                    //役職名の色を変更
                __instance.RoleBlurbText.text = IntroInfo.IntroDescription;     //イントロの簡易説明を変更
                __instance.RoleBlurbText.color = IntroInfo.Color;               //イントロの簡易説明の色を変更

                //重複を持っていたらメッセージ追記
                if (PlayerControl.LocalPlayer.HasAttribute())
                {
                    var AttributeInfo = IntroData.GetIntroData(PlayerAttribute);
                    __instance.RoleBlurbText.text += "\n" + CustomOptions.cs(AttributeInfo.Color, AttributeInfo.IntroDescription);
                }

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
            }
            catch(SystemException Error)
            {
                Logger.Error($"役職名の表示にてエラーが発生しました。エラー:{Error}", "IntroPatch");
            }

            //メッセージ表示2.5秒後にすべて非表示にする
            yield return new WaitForSeconds(2.5f);
           /* __instance.ourCrewmate.gameObject.SetActive(false);     //プレイヤーを消す
            __instance.YouAreText.gameObject.SetActive(false);      //あなたのロールは....を消す
            __instance.RoleText.gameObject.SetActive(false);        //役職名を消す
            __instance.RoleBlurbText.gameObject.SetActive(false);   //役職のイントロ説明文を消す
*/
            yield break;
        }
    }



    [HarmonyPatch]
    class IntroPatch
    {
        //表示する人たち
        public static void SetupIntroTeamPlayers(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
        {
            try
            {
                //クルー or マッド or フレンドなら全員表示
                if (PlayerControl.LocalPlayer.IsCrew() || PlayerControl.LocalPlayer.IsMad() || PlayerControl.LocalPlayer.IsFriend())
                {
                    Il2CppSystem.Collections.Generic.List<PlayerControl> CrewmateTeams = new();
                    CrewmateTeams.Add(PlayerControl.LocalPlayer);
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        //プレイヤーがLocalPlayer以外なら追加
                        if (p.PlayerId != CachedPlayer.LocalPlayer.PlayerId)
                        {
                            CrewmateTeams.Add(p);
                        }
                    }
                    yourTeam = CrewmateTeams;
                }
                //インポならインポ全員表示
                else if (PlayerControl.LocalPlayer.IsImpostor())
                {
                    Il2CppSystem.Collections.Generic.List<PlayerControl> ImpostorTeams = new();
                    ImpostorTeams.Add(PlayerControl.LocalPlayer);
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        //プレイヤーがLocalPlayer以外なら追加
                        if (p.PlayerId != CachedPlayer.LocalPlayer.PlayerId && p.IsImpostor())
                        {
                            ImpostorTeams.Add(p);
                        }
                    }
                    yourTeam = ImpostorTeams;
                }
                //第三なら自分だけ表示
                else if (PlayerControl.LocalPlayer.IsNeutral())
                {
                    Il2CppSystem.Collections.Generic.List<PlayerControl> SoloTeam = new();
                    SoloTeam.Add(PlayerControl.LocalPlayer);
                    yourTeam = SoloTeam;
                }
            }
            catch(SystemException Error)
            {
                Logger.Error($"イントロのプレイヤーセットアップに失敗しました。エラー:{Error}", "IntroPatch");
            }
        }

        //陣営表示
        public static void SetupIntroTeam(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
        {
            switch (IntroData.GetIntroData(PlayerControl.LocalPlayer.GetRole(), PlayerControl.LocalPlayer).Team)
            {
                case RoleType.Crewmate:
                    __instance.BackgroundBar.material.color = Palette.CrewmateBlue;
                    __instance.TeamTitle.text = ModTranslation.GetString("Crewmate");
                    __instance.TeamTitle.color = Palette.CrewmateBlue;
                    break;

                case RoleType.Impostor:
                    __instance.BackgroundBar.material.color = Palette.ImpostorRed;
                    __instance.TeamTitle.text = ModTranslation.GetString("Impostor");
                    __instance.TeamTitle.color = Palette.ImpostorRed;
                    __instance.ImpostorText.text = ModTranslation.GetString("ImpostorIntroDescription");;
                    break;

                case RoleType.Neutral:
                    Color32 NeutralColor = new(127, 127, 127, byte.MaxValue);
                    __instance.BackgroundBar.material.color = NeutralColor;
                    __instance.TeamTitle.text = ModTranslation.GetString("Neutral");
                    __instance.TeamTitle.color = NeutralColor;
                    __instance.ImpostorText.text = ModTranslation.GetString("NeutralIntroDescription");
                    break;

            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay)
            {
                SetupIntroTeamPlayers(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay)
            {
                SetupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
            {
                SetupIntroTeamPlayers(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
            {
                SetupIntroTeam(__instance, ref yourTeam);
            }
        }
    }
}
