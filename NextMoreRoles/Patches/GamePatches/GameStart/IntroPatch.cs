using HarmonyLib;
using UnityEngine;
using System.Collections;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules.Intro;

namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutscene_OnDestroy
    {

    }

    //役職名表示！
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    class IntroCutscene_ShowRole
    {
        static void Prefix()
        {

        }

        private static IEnumerator SetupRole(IntroCutscene __instance)
        {
            yield return new WaitForSeconds(2.5f);
            __instance.YouAreText.gameObject.SetActive(false);
            __instance.RoleText.gameObject.SetActive(false);
            __instance.RoleBlurbText.gameObject.SetActive(false);
            __instance.ourCrewmate.gameObject.SetActive(false);

            yield break;
        }
    }



    [HarmonyPatch]
    class IntroPatch
    {
        //表示する人たち
        public static void SetupIntroTeamPlayers(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> YourTeam)
        {
            //クルー or マッド or フレンドなら全員表示
            if (PlayerControl.LocalPlayer.IsCrew() || PlayerControl.LocalPlayer.IsMad() || PlayerControl.LocalPlayer.IsFriend())
            {
                Il2CppSystem.Collections.Generic.List<PlayerControl> CrewmateTeam = new();
                CrewmateTeam.Add(PlayerControl.LocalPlayer);
                foreach (PlayerControl p in CachedPlayer.AllPlayers)
                {
                    //プレイヤーがLocalPlayer以外なら追加
                    if (p.PlayerId != CachedPlayer.LocalPlayer.PlayerId)
                    {
                        CrewmateTeam.Add(p);
                    }
                }
                YourTeam = CrewmateTeam;
            }
            //インポならインポ全員
            else if (PlayerControl.LocalPlayer.IsImpostor())
            {
                Il2CppSystem.Collections.Generic.List<PlayerControl> ImpostorTeams = new();
                ImpostorTeams.Add(PlayerControl.LocalPlayer);
                foreach (PlayerControl p in CachedPlayer.AllPlayers)
                {
                    if (p.PlayerId != CachedPlayer.LocalPlayer.PlayerId && p.IsImpostor())
                    {
                        ImpostorTeams.Add(p);
                    }
                }
                YourTeam = ImpostorTeams;
            }
            //第三なら一人
            else if (PlayerControl.LocalPlayer.IsNeutral())
            {
                Il2CppSystem.Collections.Generic.List<PlayerControl> OnlyOne = new();
                OnlyOne.Add(PlayerControl.LocalPlayer);
            }
        }

        //陣営表示
        public static void SetupIntroTeam(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
        {
            Color NeutralColor = new(127, 127, 127, byte.MaxValue);
            if (PlayerControl.LocalPlayer.IsNeutral())
            {
                __instance.BackgroundBar.material.color = NeutralColor;
                __instance.TeamTitle.text = ModTranslation.GetString("Neutral");
                __instance.TeamTitle.color = NeutralColor;
                __instance.ImpostorText.text = ModTranslation.GetString("NeutralIntroDescription");
            }
            else if (PlayerControl.LocalPlayer.IsMad())
            {
                __instance.BackgroundBar.material.color = Palette.ImpostorRed;
                __instance.TeamTitle.text = ModTranslation.GetString("Impostor");
                __instance.TeamTitle.color = Palette.ImpostorRed;
                __instance.ImpostorText.text = "";
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

        private static void PlayIntroSound()
        {
            var IntroType = IntroData.IntroDatasCache[PlayerControl.LocalPlayer.GetRole()];
        }
    }
}
