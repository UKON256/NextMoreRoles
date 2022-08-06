using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx.Configuration;
using HarmonyLib;
using Hazel;
using UnityEngine;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Patches.GamePatches
{
    class CreateOptionTab
    {
        public static void Open_Postfix(GameOptionsMenu __instance)
        {
            if (GameObject.Find("NMRSettings") != null)
            {
                GameObject.Find("NMRSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(ModTranslation.GetString("NextMoreRolesSetting"));
                return;
            }
            if (GameObject.Find("CrewmateSettings") != null)
            {
                GameObject.Find("CrewmateSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(ModTranslation.GetString("CrewmateSetting"));
                return;
            }
            if (GameObject.Find("ImpostorSettings") != null)
            {
                GameObject.Find("ImpostorSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(ModTranslation.GetString("ImpostorSetting"));
                return;
            }
            if (GameObject.Find("NeutralSettings") != null)
            {
                GameObject.Find("NeutralSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(ModTranslation.GetString("NeutralSetting"));
                return;
            }
            if (GameObject.Find("CombinationSettings") != null)
            {
                GameObject.Find("CombinationSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(ModTranslation.GetString("CombinationSetting"));
                return;
            }
            if (GameObject.Find("AttributeSettings") != null)
            {
                GameObject.Find("AttributeSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(ModTranslation.GetString("AttributeSetting"));
                return;
            }

            var RoleTab = GameObject.Find("RoleTab");
            var GameTab = GameObject.Find("GameTab");
            var GameSettings = GameObject.Find("Game Settings");
            var GameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var Template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();

            var NMRSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var NMRMenu = NMRSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            NMRSettings.name = "NMRSettings";
            NMRSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var NMRTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var NMRTabHighlight = NMRTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            NMRTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Lobby.TabIcons.NMR.png", 100f);

            var CrewmateSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var CrewmateMenu = CrewmateSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            CrewmateSettings.name = "CrewmateSettings";
            CrewmateSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var CrewmateTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var CrewmateTabHighlight = CrewmateTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            CrewmateTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Lobby.TabIcons.Crewmate.png", 100f);

            var ImpostorSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var ImpostorMenu = ImpostorSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            ImpostorSettings.name = "ImpostorSettings";
            ImpostorSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var ImpostorTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var ImpostorTabHighlight = ImpostorTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            ImpostorTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Lobby.TabIcons.Impostor.png", 100f);

            var NeutralSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var NeutralMenu = NeutralSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            NeutralSettings.name = "NeutralSettings";
            NeutralSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var NeutralTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var NeutralTabHighlight = NeutralTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            NeutralTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Lobby.TabIcons.Neutral.png", 100f);

            var CombinationSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var CombinationMenu = CombinationSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            CombinationSettings.name = "CombinationSettings";
            CombinationSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var CombinationTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var CombinationTabHighlight = CombinationTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            CombinationTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Lobby.TabIcons.Combination.png", 100f);

            var AttributeSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var AttributeMenu = AttributeSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            AttributeSettings.name = "AttributeSettings";
            AttributeSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var AttributeTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var AttributeTabHighlight = AttributeTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            AttributeTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Lobby.TabIcons.Attribute.png", 100f);



            GameTab.transform.position += Vector3.left * 3f;
            NMRTab.transform.position += Vector3.left * 3f;
            CrewmateTab.transform.position += Vector3.left * 2f;
            ImpostorTab.transform.position += Vector3.left * 1f;
            //NeutralTab.transform.localPosition = Vector3.right * 1f;
            CombinationTab.transform.localPosition = Vector3.right * 1f;
            AttributeTab.transform.localPosition = Vector3.right * 2f;
            RoleTab.SetActive(false);

            var Tabs = new GameObject[] { GameTab, NMRTab, CrewmateTab, ImpostorTab, NeutralTab, CombinationTab, AttributeTab };
            //Tabsの数だけ繰り返す
            for (int i = 0; i < Tabs.Length; i++)
            {
                var Button = Tabs[i].GetComponentInChildren<PassiveButton>();
                Button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                Button.OnClick.AddListener((System.Action)(() =>
                {
                    GameSettingMenu.RegularGameSettings.SetActive(false);
                    NMRSettings.gameObject.SetActive(false);
                    CrewmateSettings.gameObject.SetActive(false);
                    ImpostorSettings.gameObject.SetActive(false);
                    NeutralSettings.gameObject.SetActive(false);
                    CombinationSettings.gameObject.SetActive(false);
                    AttributeSettings.gameObject.SetActive(false);

                    GameSettingMenu.GameSettingsHightlight.enabled = false;
                    NMRTabHighlight.enabled = false;
                    CrewmateTabHighlight.enabled = false;
                    ImpostorTabHighlight.enabled = false;
                    NeutralTabHighlight.enabled = false;
                    CombinationTabHighlight.enabled = false;
                    AttributeTabHighlight.enabled = false;

                    //=====現在実行中のところだけ実行する=====//
                    //バニラ
                    if (i == 0)
                    {
                        GameSettingMenu.RegularGameSettings.SetActive(true);
                        GameSettingMenu.GameSettingsHightlight.enabled = true;
                    }
                    //NMRのタブ
                    else if (i == 1)
                    {
                        NMRSettings.gameObject.SetActive(true);
                        NMRTabHighlight.enabled = true;
                    }
                    //クルータブ
                    else if (i == 2)
                    {
                        CrewmateSettings.gameObject.SetActive(true);
                        CrewmateTabHighlight.enabled = true;
                    }
                    //インポタブ
                    else if (i == 3)
                    {
                        ImpostorSettings.gameObject.SetActive(true);
                        ImpostorTabHighlight.enabled = true;
                    }
                    //第三タブ
                    else if (i == 4)
                    {
                        NeutralSettings.gameObject.SetActive(true);
                        NeutralTabHighlight.enabled = true;
                    }
                    //コンビ
                    else if (i == 5)
                    {
                        CombinationSettings.gameObject.SetActive(true);
                        CombinationTabHighlight.enabled = true;
                    }
                    //属性(重複)
                    else if (i == 5)
                    {
                        AttributeSettings.gameObject.SetActive(true);
                        AttributeTabHighlight.enabled = true;
                    }
                } ));
            }

            foreach (OptionBehaviour option in NMRMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            foreach (OptionBehaviour option in CrewmateMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            foreach (OptionBehaviour option in ImpostorMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            foreach (OptionBehaviour option in NeutralMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            foreach (OptionBehaviour option in CombinationMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            foreach (OptionBehaviour option in AttributeMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);

            List<OptionBehaviour> NMROptions = new();
            List<OptionBehaviour> CrewmateOptions = new();
            List<OptionBehaviour> ImpostorOptions = new();
            List<OptionBehaviour> NeutralOptions = new();
            List<OptionBehaviour> CombinationOptions = new();
            List<OptionBehaviour> AttributeOptions = new();

            List<Transform> Menus = new() { NMRMenu.transform, CrewmateMenu.transform, ImpostorMenu.transform, NeutralMenu.transform, CombinationMenu.transform, AttributeMenu.transform };
            List<List<OptionBehaviour>> OptionBehaviours = new() { NMROptions, CrewmateOptions, ImpostorOptions, NeutralOptions, CombinationOptions, AttributeOptions };

            //CustomOptionの数だけ繰り返す
            for (int i = 0; i < CustomOption.Options.Count; i++)
            {
                CustomOption Option = CustomOption.Options[i];
                if (Option.OptionBehaviour == null)
                {
                    StringOption StringOption = UnityEngine.Object.Instantiate(Template, Menus[(int)Option.Type]);
                    OptionBehaviours[(int)Option.Type].Add(StringOption);
                    StringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                    StringOption.TitleText.text = Option.Name;
                    StringOption.Value = StringOption.oldValue = Option.Selection;
                    StringOption.ValueText.text = Option.Selections[Option.Selection].ToString();

                    Option.OptionBehaviour = StringOption;
                }
                Option.OptionBehaviour.gameObject.SetActive(true);
            }

            NMRMenu.Children = NMROptions.ToArray();
            NMRSettings.gameObject.SetActive(false);

            CrewmateMenu.Children = CrewmateOptions.ToArray();
            CrewmateSettings.gameObject.SetActive(false);

            ImpostorMenu.Children = ImpostorOptions.ToArray();
            ImpostorSettings.gameObject.SetActive(false);

            NeutralMenu.Children = NeutralOptions.ToArray();
            NeutralSettings.gameObject.SetActive(false);

            CombinationMenu.Children = CombinationOptions.ToArray();
            CombinationSettings.gameObject.SetActive(false);

            AttributeMenu.Children = AttributeOptions.ToArray();
            AttributeSettings.gameObject.SetActive(false);

            var NumImpostorsOption = __instance.Children.FirstOrDefault(x => x.name == "NumImpostors").TryCast<NumberOption>();
            if (NumImpostorsOption != null) NumImpostorsOption.ValidRange = new FloatRange(0f, 15f);

            var PlayerSpeedModOption = __instance.Children.FirstOrDefault(x => x.name == "PlayerSpeed").TryCast<NumberOption>();
            if (PlayerSpeedModOption != null) PlayerSpeedModOption.ValidRange = new FloatRange(-5.5f, 5.5f);

            var KillCoolOption = __instance.Children.FirstOrDefault(x => x.name == "KillCooldown").TryCast<NumberOption>();
            if (KillCoolOption != null) KillCoolOption.ValidRange = new FloatRange(2.5f, 60f);

            var CommonTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
            if (CommonTasksOption != null) CommonTasksOption.ValidRange = new FloatRange(0f, 4f);

            var ShortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
            if (ShortTasksOption != null) ShortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var LongTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
            if (LongTasksOption != null) LongTasksOption.ValidRange = new FloatRange(0f, 15f);
        }
    }
}
