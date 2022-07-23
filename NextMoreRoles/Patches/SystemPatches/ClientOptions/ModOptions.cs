using System;
using System.Collections.Generic;
using TMPro;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Button;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;
using Object = UnityEngine.Object;

// 設定ボタンを押したときのMOD設定達
namespace NextMoreRoles.Patches.SystemPatches.ClientOptions
{
    [HarmonyPatch]
    public static class ClientModOptions
    {
        private static SelectionBehaviour[] AllOptions = {
            new SelectionBehaviour("IsAutoUpdate", ()=> Configs.IsAutoUpdate.Value = !Configs.IsAutoUpdate.Value, true)
        };

        private static GameObject PopUp;
        private static TextMeshPro TitleText;
        private static ToggleButtonBehaviour MoreOptions;
        private static List<ToggleButtonBehaviour> ModButtons;
        private static TextMeshPro TitleTextTitle;
        private static ToggleButtonBehaviour ButtonPrefab;
        private static Vector3? _origin;

        //実行元:ClientOptionsMain.cs
        public static void MeinMenu_Start_Postfix(MainMenuManager __instance)
        {
            var Template  = __instance.Announcement.transform.Find("Title_Text").gameObject.GetComponent<TextMeshPro>();
            Template.alignment = TextAlignmentOptions.Center;
            Template.transform.localPosition += Vector3.left * 0.2f;
            TitleText = Object.Instantiate(Template);
            Object.Destroy(TitleText.GetComponent<TextTranslatorTMP>());
            TitleText.gameObject.SetActive(false);
            Object.DontDestroyOnLoad(TitleText);
        }
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

            SetUpOptions();
            InitializeMoreButton(__instance);
        }



        //クラスたち
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

        private static void CreateCustom(OptionsMenuBehaviour prefab)
        {
            PopUp = Object.Instantiate(prefab.gameObject);
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
            MoreOptions.Text.text = ModTranslation.GetString("ModOptions");
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
                RefreshOpen();
            }));
        }

        private static void RefreshOpen()
        {
            PopUp.gameObject.SetActive(false);
            PopUp.gameObject.SetActive(true);
            SetUpOptions();
        }

        private static void CheckSetTitle()
        {
            if (!PopUp || PopUp.GetComponentInChildren<TextMeshPro>() || !TitleText) return;

            var title = TitleTextTitle = Object.Instantiate(TitleText, PopUp.transform);
            title.GetComponent<RectTransform>().localPosition = Vector3.up * 2.3f;
            title.gameObject.SetActive(true);
            title.text = ModTranslation.GetString("ModOptionsTitle");
            title.name = "TitleText";
        }

        private static void SetUpOptions()
        {
            if (PopUp.transform.GetComponentInChildren<ToggleButtonBehaviour>()) return;

            ModButtons = new List<ToggleButtonBehaviour>();

            for (var i = 0; i < AllOptions.Length; i++)
            {
                var info = AllOptions[i];

                var button = Object.Instantiate(ButtonPrefab, PopUp.transform);
                var pos = new Vector3(i % 2 == 0 ? -1.17f : 1.17f, 1.3f - i / 2 * 0.8f, -.5f);

                var transform = button.transform;
                transform.localPosition = pos;

                button.onState = info.DefaultValue;
                button.Background.color = button.onState ? Color.green : Palette.ImpostorRed;

                button.Text.text = ModTranslation.GetString(info.Title);
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

        //実行元:ClientOptuionsMain.cs
        public static void UpdateTranslations()
        {
            if (TitleTextTitle)
                TitleTextTitle.text = ModTranslation.GetString("ModOptions");

            if (MoreOptions)
                MoreOptions.Text.text = ModTranslation.GetString("ModOptions");

            for (int i = 0; i < AllOptions.Length; i++)
            {
                if (i >= ModButtons.Count) break;
                ModButtons[i].Text.text = ModTranslation.GetString(AllOptions[i].Title);
            }
        }
    }
}
