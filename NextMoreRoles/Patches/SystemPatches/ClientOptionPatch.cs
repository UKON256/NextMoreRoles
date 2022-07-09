using System;
using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UnityEngine;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;
using Object = UnityEngine.Object;

namespace NextMoreRoles.Patches.SystemPatches
{
    [HarmonyPatch]
    //オプション生成
    public static class ClientOptionsPatch
    {
        private static SelectionBehaviour[] AllOptions = {
            new SelectionBehaviour ("IsAutoUpdate", ()=> Configs.AutoUpdate.Value = !Configs.AutoUpdate.Value, Configs.AutoUpdate.Value)
        };

        private static GameObject PopUp;
        private static TextMeshPro TitleText;

        private static ToggleButtonBehaviour MoreOptions;
        private static List<ToggleButtonBehaviour> MODButton;
        private static TextMeshPro TitleTextTitle;

        private static ToggleButtonBehaviour ButtonPeefab;
        private static Vector3? _origin;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public static void MainMenuManager_StartPostfix(MainMenuManager __instance)
        {
            // Prefab for the title
            var Template = __instance.Announcement.transform.Find("Title_Text").gameObject.GetComponent<TextMeshPro>();
            Template.alignment = TextAlignmentOptions.Center;
            Template.transform.localPosition += Vector3.left * 0.2f;
            TitleText = Object.Instantiate(Template);
            Object.Destroy(TitleText.GetComponent<TextTranslatorTMP>());
            TitleText.gameObject.SetActive(false);
            Object.DontDestroyOnLoad(TitleText);
        }

        public static float xOffSet = 1.75f;
        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Update))]
        class OptionsUpdate
        {
            public static void Postfix(OptionsMenuBehaviour __instance)
            {
                if (__instance.CensorChatButton != null) __instance.CensorChatButton.gameObject.SetActive(false);
                if (__instance.EnableFriendInvitesButton != null) __instance.EnableFriendInvitesButton.gameObject.SetActive(false);
                if (__instance.StreamerModeButton != null) __instance.StreamerModeButton.gameObject.SetActive(false);
                if (__instance.ColorBlindButton != null) __instance.ColorBlindButton.gameObject.SetActive(false);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
        public static void OptionsMenuBehaviour_StartPostfix(OptionsMenuBehaviour __instance)
        {
            if (!__instance.CensorChatButton) return;
            if (!PopUp)
            {
                CreateCustom(__instance);
            }
        }





        //クラスたち
        private static void CreateCustom(OptionsMenuBehaviour prefab)
        {
            PopUp = Object.Instantiate(prefab.gameObject);
            Object.DontDestroyOnLoad(PopUp);
            var Transform = PopUp.transform;
            var Position = Transform.localPosition;
            Position.z = -810f;
            Transform.localPosition = Position;

            Object.Destroy(PopUp.GetComponent<OptionsMenuBehaviour>());
            foreach (var gObj in PopUp.gameObject.GetAllChilds())
            {
                if (gObj.name is not "Background" and not "CloseButton")
                    Object.Destroy(gObj);
            }
            PopUp.SetActive(false);
        }
        private static IEnumerable<GameObject> GetAllChilds(this GameObject Go)
        {
            for (var i = 0; i < Go.transform.childCount; i++)
            {
                yield return Go.transform.GetChild(i).gameObject;
            }
        }

        public static void UpdateTranslations()
        {
            if (TitleTextTitle)
                TitleTextTitle.text = ModTranslation.getString("MoreOptionsText");

            if (MoreOptions)
                MoreOptions.Text.text = ModTranslation.getString("MODOptionsText");
            try
            {
                MODButton[0].Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsCensorChat);
                MODButton[1].Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsEnableFriendInvites);
            }
            catch { }
            for (int i = 0; i < AllOptions.Length; i++)
            {
                if (i >= MODButton.Count) break;
                MODButton[i + 2].Text.text = ModTranslation.getString(AllOptions[i].Title);
            }
        }

        public class SelectionBehaviour
        {
            public string Title;
            public Func<bool> OnClick;
            public bool DefaultValue;

            public SelectionBehaviour(string title, Func<bool> onClick, bool defaultValue)
            {
                Title = title;
                OnClick = onClick;
                DefaultValue = defaultValue;
            }
        }
    }
}