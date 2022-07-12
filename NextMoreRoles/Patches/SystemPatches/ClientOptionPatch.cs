using System;
using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;

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
        private static List<ToggleButtonBehaviour> MODButtons;
        private static TextMeshPro TitleTextTitle;

        private static ToggleButtonBehaviour ButtonPrefab;
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



        //初期化！
        private static void InitializeMoreButton(OptionsMenuBehaviour __instance)
        {
            MoreOptions = Object.Instantiate(ButtonPrefab, __instance.CensorChatButton.transform.parent);
            var Transform = __instance.CensorChatButton.transform;
            _origin ??= Transform.localPosition;

            Transform.localPosition = _origin.Value + Vector3.left * 1.3f;
            MoreOptions.transform.localPosition = _origin.Value + Vector3.right * 1.3f;
            var Pos = MoreOptions.transform.localPosition;
            MoreOptions.transform.localScale *= 1.1f;
            float Count = 1.55f;
            MoreOptions.transform.localPosition = new Vector3(Pos.x*1.5f, Pos.y * Count, Pos.z);
            var Trans = MoreOptions.transform.localPosition;
            MoreOptions.gameObject.SetActive(true);
            Trans = MoreOptions.transform.position;
            MoreOptions.Text.text = ModTranslation.getString("MODOptionsText");
            var moreOptionsButton = MoreOptions.GetComponent<PassiveButton>();
            moreOptionsButton.OnClick = new ButtonClickedEvent();
            moreOptionsButton.OnClick.AddListener((Action)(() =>
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

            var Title = TitleTextTitle = Object.Instantiate(TitleText, PopUp.transform);
            Title.GetComponent<RectTransform>().localPosition = Vector3.up * 2.3f;
            Title.gameObject.SetActive(true);
            Title.text = ModTranslation.getString("moreOptionsText");
            Title.name = "TitleText";
        }
        private static void SetUpOptions(OptionsMenuBehaviour __instance)
        {
            if (PopUp.transform.GetComponentInChildren<ToggleButtonBehaviour>()) return;

            MODButtons = new List<ToggleButtonBehaviour>();
            for (var i = 0; i < 4; i++)
            {
                ToggleButtonBehaviour Mainbutton = null;
                switch (i)
                {
                    case 0:
                        Mainbutton = __instance.CensorChatButton;
                        break;
                    case 1:
                        Mainbutton = __instance.EnableFriendInvitesButton;
                        break;
                    case 2:
                        Mainbutton = __instance.StreamerModeButton;
                        break;
                    case 3:
                        Mainbutton = __instance.ColorBlindButton;
                        break;
                }
                var Button = Object.Instantiate(ButtonPrefab, PopUp.transform);
                var Pos = new Vector3(i % 2 == 0 ? -1.17f : 1.17f, 1.3f - i / 2 * 0.8f, -.5f);

                var Transform = Button.transform;
                Transform.localPosition = Pos;

                Button.onState = Mainbutton.onState;
                Button.Background.color = Mainbutton.onState ? Color.green : Palette.ImpostorRed;
                try
                {
                    switch (i)
                    {
                        case 0:
                            Button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsCensorChat);
                            break;
                        case 1:
                            Button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsEnableFriendInvites);
                            break;
                        case 2:
                            Button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsStreamerMode);
                            break;
                        case 3:
                            Button.Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsColorblind);
                            break;
                    }
                }
                catch
                {
                    switch (i)
                    {
                        case 0:
                            Button.Text.text = __instance.CensorChatButton.Text.text;
                            break;
                        case 1:
                            Button.Text.text = __instance.EnableFriendInvitesButton.Text.text;
                            break;
                        case 2:
                            Button.Text.text = __instance.StreamerModeButton.Text.text;
                            break;
                        case 3:
                            Button.Text.text = __instance.ColorBlindButton.Text.text;
                            break;
                    }
                }
                Button.Text.fontSizeMin = Button.Text.fontSizeMax = 2.2f;
                Button.Text.font = Object.Instantiate(TitleText.font);
                Button.Text.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);

                Button.name = Mainbutton.name;
                Button.gameObject.SetActive(true);

                var passiveButton = Button.GetComponent<PassiveButton>();
                var colliderButton = Button.GetComponent<BoxCollider2D>();

                colliderButton.size = new Vector2(2.2f, .7f);

                passiveButton.OnClick = Mainbutton.GetComponent<PassiveButton>().OnClick;
                passiveButton.OnClick.AddListener((Action)(() =>
                {
                    Button.onState = !Button.onState;
                    Button.Background.color = Button.onState ? Color.green : Palette.ImpostorRed;
                }));
                passiveButton.OnMouseOver = Mainbutton.GetComponent<PassiveButton>().OnMouseOver;
                passiveButton.OnMouseOut = Mainbutton.GetComponent<PassiveButton>().OnMouseOut;

                passiveButton.OnMouseOver.AddListener((Action)(() => Button.Background.color = new Color32(34, 139, 34, byte.MaxValue)));
                passiveButton.OnMouseOut.AddListener((Action)(() => Button.Background.color = Button.onState ? Color.green : Palette.ImpostorRed));

                foreach (var spr in Button.gameObject.GetComponentsInChildren<SpriteRenderer>())
                    spr.size = new Vector2(2.2f, .7f);
                MODButtons.Add(Button);
            }
            for (var i = 4; i < AllOptions.Length + 4; i++)
            {
                var info = AllOptions[i - 4];

                var button = Object.Instantiate(ButtonPrefab, PopUp.transform);
                var pos = new Vector3(i % 2 == 0 ? -1.17f : 1.17f, 1.3f - i / 2 * 0.8f, -.5f);

                var transform = button.transform;
                transform.localPosition = pos;

                button.onState = info.DefaultValue;
                button.Background.color = button.onState ? Color.green : Palette.ImpostorRed;

                button.Text.text = ModTranslation.getString(info.Title);
                button.Text.fontSizeMin = button.Text.fontSizeMax = 2.2f;
                button.Text.font = Object.Instantiate(TitleText.font);
                button.Text.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);

                button.name = info.Title.Replace(" ", "") + "Toggle";
                button.gameObject.SetActive(true);

                var passiveButton = button.GetComponent<PassiveButton>();
                var colliderButton = button.GetComponent<BoxCollider2D>();

                colliderButton.size = new Vector2(2.2f, .7f);

                passiveButton.OnClick = new ButtonClickedEvent();
                passiveButton.OnMouseOut = new UnityEvent();
                passiveButton.OnMouseOver = new UnityEvent();

                passiveButton.OnClick.AddListener((Action)(() =>
                {
                    button.onState = info.OnClick();
                    button.Background.color = button.onState ? Color.green : Palette.ImpostorRed;
                }));

                passiveButton.OnMouseOver.AddListener((Action)(() => button.Background.color = new Color32(34, 139, 34, byte.MaxValue)));
                passiveButton.OnMouseOut.AddListener((Action)(() => button.Background.color = button.onState ? Color.green : Palette.ImpostorRed));

                foreach (var spr in button.gameObject.GetComponentsInChildren<SpriteRenderer>())
                    spr.size = new Vector2(2.2f, .7f);
                var trans = transform.position;
                MODButtons.Add(button);
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
                TitleTextTitle.text = ModTranslation.getString("MoreOptionsText");

            if (MoreOptions)
                MoreOptions.Text.text = ModTranslation.getString("MODOptionsText");
            try
            {
                MODButtons[0].Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsCensorChat);
                MODButtons[1].Text.text = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsEnableFriendInvites);
            }
            catch { }
            for (int i = 0; i < AllOptions.Length; i++)
            {
                if (i >= MODButtons.Count) break;
                MODButtons[i + 2].Text.text = ModTranslation.getString(AllOptions[i].Title);
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
