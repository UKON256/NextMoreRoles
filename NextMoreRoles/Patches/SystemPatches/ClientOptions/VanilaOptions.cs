using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Button;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;
using Object = UnityEngine.Object;

namespace NextMoreRoles.Patches.SystemPatches.ClientOptions
{
    public static class ClientVanillaOptions
    {
        private static GameObject PopUp;
        private static TextMeshPro TitleText;
        private static ToggleButtonBehaviour MoreOptions;
        private static List<ToggleButtonBehaviour> ModButtons;
        private static TextMeshPro TitleTextTitle;
        public static ToggleButtonBehaviour ButtonPrefab;
        public static Vector3? _origin;

        //実行元:ClientOptionsMain.cs
        public static void MainMenu_Start_Postfix(MainMenuManager __instance)
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
        public static Vector3? origin;
        public static float xOffset = 1.75f;

        //実行元:ClientOptionsMain.cs
        public static void OptionsMenu_Start_Postfix(OptionsMenuBehaviour __instance)
        {
            if (!__instance.CensorChatButton) return;
            if (!PopUp)
            {
                CreateCustom(__instance);
            }

            if (!ButtonPrefab)
            {
                ButtonPrefab = Object.Instantiate(__instance.CensorChatButton);
                Object.DontDestroyOnLoad(ButtonPrefab);
                ButtonPrefab.name = "CensorChatPrefab";
                ButtonPrefab.gameObject.SetActive(false);
            }

            SetUpOptions(__instance);
            InitializeMoreButton(__instance);
        }

        //実行元:ClientOptionsMain.cs
        public static void OptionsMenu_Update_Postfix(OptionsMenuBehaviour __instance)
        {
            if (__instance.CensorChatButton != null) __instance.CensorChatButton.gameObject.SetActive(false);
            if (__instance.EnableFriendInvitesButton != null) __instance.EnableFriendInvitesButton.gameObject.SetActive(false);
            if (__instance.StreamerModeButton != null) __instance.StreamerModeButton.gameObject.SetActive(false);
            if (__instance.ColorBlindButton != null) __instance.ColorBlindButton.gameObject.SetActive(false);
        }



        //クラスたち
        private static void CreateCustom(OptionsMenuBehaviour Prefab)
        {
            PopUp = Object.Instantiate(Prefab.gameObject);
            Object.DontDestroyOnLoad(PopUp);
            var transform = PopUp.transform;
            var pos = transform.localPosition;
            pos.z = -810f;
            transform.localPosition = pos;

            Object.Destroy(PopUp.GetComponent<OptionsMenuBehaviour>());
            foreach (var gObj in PopUp.gameObject.GetAllChilds())
            {
                if (gObj.name != "Background" && gObj.name != "CloseButton")
                    Object.Destroy(gObj);
            }
            PopUp.SetActive(false);
        }

        //初期化
        private static void InitializeMoreButton(OptionsMenuBehaviour __instance)
        {
            MoreOptions = Object.Instantiate(ButtonPrefab, __instance.CensorChatButton.transform.parent);
            var transform = __instance.CensorChatButton.transform;
            _origin ??= transform.localPosition;
            transform.localPosition = _origin.Value + Vector3.left * 2.6f;
            MoreOptions.transform.localPosition = _origin.Value + Vector3.right * 2.6f;
            var trans = MoreOptions.transform.localPosition;
            MoreOptions.gameObject.SetActive(true);
            trans = MoreOptions.transform.position;
            MoreOptions.Text.text = ModTranslation.GetString("VanillaOptions");
            var MoreOptionsButton = MoreOptions.GetComponent<PassiveButton>();
            MoreOptionsButton.OnClick = new ButtonClickedEvent();
            MoreOptionsButton.OnClick.AddListener((Action)(() =>
            {
                if (!PopUp) return;
                if (__instance.transform.parent && __instance.transform.parent == FastDestroyableSingleton<HudManager>.Instance.transform)
                {
                    PopUp.transform.SetParent(FastDestroyableSingleton<HudManager>.Instance.transform);
                    PopUp.transform.localPosition = new Vector3(0, 0, -800f);
                }
                else
                {
                    PopUp.transform.SetParent(null);
                    Object.DontDestroyOnLoad(PopUp);
                }
                CheckSetTitle();
                RefreshOpen(__instance);
            }));
        }

        private static void RefreshOpen(OptionsMenuBehaviour __instance)
        {
            PopUp.gameObject.SetActive(false);
            PopUp.gameObject.SetActive(true);
            SetUpOptions(__instance);
        }

        private static void CheckSetTitle()
        {
            if (!PopUp || PopUp.GetComponentInChildren<TextMeshPro>() || !TitleText) return;

            var title = TitleTextTitle = Object.Instantiate(TitleText, PopUp.transform);
            title.GetComponent<RectTransform>().localPosition = Vector3.up * 2.3f;
            title.gameObject.SetActive(true);
            title.text = ModTranslation.GetString("VanillaOptionsTitle");
            title.name = "TitleText";
        }

        private static void SetUpOptions(OptionsMenuBehaviour __instance)
        {
            if (PopUp.transform.GetComponentInChildren<ToggleButtonBehaviour>()) return;

            ModButtons = new List<ToggleButtonBehaviour>();
            for (var i = 0; i < 4; i++)
            {
                ToggleButtonBehaviour mainbutton = null;
                switch (i)
                {
                    case 0:
                        mainbutton = __instance.CensorChatButton;
                        break;
                    case 1:
                        mainbutton = __instance.EnableFriendInvitesButton;
                        break;
                    case 2:
                        mainbutton = __instance.StreamerModeButton;
                        break;
                    case 3:
                        mainbutton = __instance.ColorBlindButton;
                        break;
                }
                var button = Object.Instantiate(ButtonPrefab, PopUp.transform);
                var pos = new Vector3(i % 2 == 0 ? -1.17f : 1.17f, 1.3f - i / 2 * 0.8f, -.5f);

                var transform = button.transform;
                transform.localPosition = pos;

                button.onState = mainbutton.onState;
                button.Background.color = mainbutton.onState ? Color.green : Palette.ImpostorRed;
                try
                {
                    switch (i)
                    {
                        case 0:
                            button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsCensorChat);
                            break;
                        case 1:
                            button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsEnableFriendInvites);
                            break;
                        case 2:
                            button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsStreamerMode);
                            break;
                        case 3:
                            button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsColorblind);
                            break;
                    }
                }
                catch
                {
                    switch (i)
                    {
                        case 0:
                            button.Text.text = __instance.CensorChatButton.Text.text;
                            break;
                        case 1:
                            button.Text.text = __instance.EnableFriendInvitesButton.Text.text;
                            break;
                        case 2:
                            button.Text.text = __instance.StreamerModeButton.Text.text;
                            break;
                        case 3:
                            button.Text.text = __instance.ColorBlindButton.Text.text;
                            break;
                    }
                }
                button.Text.fontSizeMin = button.Text.fontSizeMax = 2.2f;
                button.Text.font = Object.Instantiate(TitleText.font);
                button.Text.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);

                button.name = mainbutton.name;
                button.gameObject.SetActive(true);

                var passiveButton = button.GetComponent<PassiveButton>();
                var colliderButton = button.GetComponent<BoxCollider2D>();

                colliderButton.size = new Vector2(2.2f, .7f);

                passiveButton.OnClick = mainbutton.GetComponent<PassiveButton>().OnClick;
                passiveButton.OnClick.AddListener((Action)(() =>
                {
                    button.onState = !button.onState;
                    button.Background.color = button.onState ? Color.green : Palette.ImpostorRed;
                }));
                passiveButton.OnMouseOver = mainbutton.GetComponent<PassiveButton>().OnMouseOver;
                passiveButton.OnMouseOut = mainbutton.GetComponent<PassiveButton>().OnMouseOut;

                passiveButton.OnMouseOver.AddListener((Action)(() => button.Background.color = new Color32(34, 139, 34, byte.MaxValue)));
                passiveButton.OnMouseOut.AddListener((Action)(() => button.Background.color = button.onState ? Color.green : Palette.ImpostorRed));

                foreach (var spr in button.gameObject.GetComponentsInChildren<SpriteRenderer>())
                    spr.size = new Vector2(2.2f, .7f);
                ModButtons.Add(button);
            }
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
                TitleTextTitle.text = ModTranslation.GetString("VanillaOptions");

            if (MoreOptions)
                MoreOptions.Text.text = ModTranslation.GetString("VanillaOptions");
            try
            {
                ModButtons[0].Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsCensorChat);
                ModButtons[1].Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsEnableFriendInvites);
            }
            catch { }
        }
    }
}
