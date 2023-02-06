using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Patches.LobbyPatches;

[HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
class SetupCustomOptions
{
    public static void Postfix(GameOptionsMenu __instance)
    {
        try
        {
            //* nullならセットアップ *//
            if (GameObject.Find("NMRSettings") != null)
            {
                GameObject.Find("NMRSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(Translator.GetString("NMRSetting"));
                return;
            }
            if (GameObject.Find("CrewmateSettings") != null)
            {
                GameObject.Find("CrewmateSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(Translator.GetString("CrewmateSetting"));
                return;
            }
            if (GameObject.Find("ImpostorSettings") != null)
            {
                GameObject.Find("ImpostorSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(Translator.GetString("ImpostorSetting"));
                return;
            }
            if (GameObject.Find("NeutralSettings") != null)
            {
                GameObject.Find("NeutralSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(Translator.GetString("NeutralSetting"));
                return;
            }
            if (GameObject.Find("CombinationSettings") != null)
            {
                GameObject.Find("CombinationSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(Translator.GetString("CombinationSetting"));
                return;
            }
            if (GameObject.Find("AttributeSettings") != null)
            {
                GameObject.Find("AttributeSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(Translator.GetString("AttributeSetting"));
                return;
            }


            var RoleTab = GameObject.Find("RoleTab");
            var GameTab = GameObject.Find("GameTab");
            var GameSettings = GameObject.Find("Main Camera/PlayerOptionsMenu(Clone)/Game Settings/");
            var GameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var Template = GameObject.Find("Main Camera/PlayerOptionsMenu(Clone)/Game Settings/GameGroup/SliderInner/KillDistance").GetComponent<StringOption>();

            var NMRSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var NMRMenu = NMRSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            NMRSettings.name = "NMRSettings";
            NMRSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "GenericSetting";
            var NMRTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var NMRTabHighlight = NMRTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            NMRTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TabIcons.NMR.png", 100f);

            var CrewmateSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var CrewmateMenu = CrewmateSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            CrewmateSettings.name = "CrewmateSettings";
            CrewmateSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "CrewmateSetting";
            var CrewmateTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var CrewmateTabHighlight = CrewmateTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            CrewmateTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TabIcons.Crewmate.png", 100f);

            var ImpostorSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var ImpostorMenu = ImpostorSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            ImpostorSettings.name = "ImpostorSettings";
            ImpostorSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "ImpostorSetting";
            var ImpostorTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var ImpostorTabHighlight = ImpostorTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            ImpostorTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TabIcons.Impostor.png", 100f);

            var NeutralSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var NeutralMenu = NeutralSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            NeutralSettings.name = "NeutralSettings";
            NeutralSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "NeutralSetting";
            var NeutralTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var NeutralTabHighlight = NeutralTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            NeutralTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TabIcons.Neutral.png", 100f);

            var CombinationSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var CombinationMenu = CombinationSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            CombinationSettings.name = "CombinationSettings";
            CombinationSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "CombinationSetting";
            var CombinationTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var CombinationTabHighlight = CombinationTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            CombinationTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TabIcons.Combination.png", 100f);

            var AttributeSettings = UnityEngine.Object.Instantiate(GameSettings, GameSettings.transform.parent);
            var AttributeMenu = AttributeSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            AttributeSettings.name = "AttributeSettings";
            AttributeSettings.transform.FindChild("GameGroup").FindChild("SliderInner").name = "AttributeSetting";
            var AttributeTab = UnityEngine.Object.Instantiate(RoleTab, RoleTab.transform.parent);
            var AttributeTabHighlight = AttributeTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            AttributeTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.TabIcons.Attribute.png", 100f);



            GameTab.transform.position += Vector3.left * 3f;
            NMRTab.transform.position += Vector3.left * 3f;
            CrewmateTab.transform.position += Vector3.left * 2f;
            ImpostorTab.transform.position += Vector3.left * 1f;
            //NeutralTab.transform.localPosition = Vector3.right * 1f;
            CombinationTab.transform.localPosition = Vector3.right * 1f;
            AttributeTab.transform.localPosition = Vector3.right * 2f;
            RoleTab.SetActive(false);

            var Tabs = new GameObject[] { GameTab, NMRTab, CrewmateTab, ImpostorTab, NeutralTab, CombinationTab, AttributeTab };
            // Tabsの数だけ繰り返す
            for (int i = 0; i < Tabs.Length; i++)
            {
                var Button = Tabs[i].GetComponentInChildren<PassiveButton>();
                int CopiedIndex = i;
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

                    //=====クリックしたタブの部分だけ表示する=====//
                    //バニラ
                    if (CopiedIndex == 0)
                    {
                        GameSettingMenu.RegularGameSettings.SetActive(true);
                        GameSettingMenu.GameSettingsHightlight.enabled = true;
                    }
                    //NMRのタブ
                    else if (CopiedIndex == 1)
                    {
                        NMRSettings.gameObject.SetActive(true);
                        NMRTabHighlight.enabled = true;
                    }
                    //クルータブ
                    else if (CopiedIndex == 2)
                    {
                        CrewmateSettings.gameObject.SetActive(true);
                        CrewmateTabHighlight.enabled = true;
                    }
                    //インポタブ
                    else if (CopiedIndex == 3)
                    {
                        ImpostorSettings.gameObject.SetActive(true);
                        ImpostorTabHighlight.enabled = true;
                    }
                    //第三タブ
                    else if (CopiedIndex == 4)
                    {
                        NeutralSettings.gameObject.SetActive(true);
                        NeutralTabHighlight.enabled = true;
                    }
                    //コンビ
                    else if (CopiedIndex == 5)
                    {
                        CombinationSettings.gameObject.SetActive(true);
                        CombinationTabHighlight.enabled = true;
                    }
                    //属性(重複)
                    else if (CopiedIndex == 6)
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

            // CustomOptionの数だけ繰り返す
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
        catch (SystemException Error)
        {
            Logger.Error($"カスタムオプションの構成に失敗しました。{Error}", "SetupCustomOptions");
        }
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
public class StringOptionEnablePatch
{
    public static bool Prefix(StringOption __instance) {
        CustomOption option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
        if (option == null) return true;

        __instance.OnValueChanged = new Action<OptionBehaviour>((o) => {});
        __instance.TitleText.text = option.Name;
        __instance.Value = __instance.oldValue = option.Selection;
        __instance.ValueText.text = option.Selections[option.Selection].ToString();

        return false;
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
public class StringOptionIncreasePatch
{
    public static bool Prefix(StringOption __instance)
    {
        CustomOption option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
        if (option == null) return true;
        option.UpdateSelection(option.Selection + 1);
        return false;
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
public class StringOptionDecreasePatch
{
    public static bool Prefix(StringOption __instance)
    {
        CustomOption option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
        if (option == null) return true;
        option.UpdateSelection(option.Selection - 1);
        return false;
    }
}

[HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
class GameOptionsUpdate
{
    public static CustomOptionType GetCustomOptionType(string name)
    {
        return name switch
        {
            "GenericSetting" => CustomOptionType.General,
            "CrewmateSetting" => CustomOptionType.Crewmate,
            "ImpostorSetting" => CustomOptionType.Impostor,
            "NeutralSetting" => CustomOptionType.Neutral,
            "CombinationSetting" => CustomOptionType.Combination,
            "AttributeSetting" => CustomOptionType.Attribute,
            _ => CustomOptionType.General,
        };
    }
    private static float Timer = 1f;
    static void Postfix(GameOptionsMenu __instance)
    {
        //バニラの設定を開いていたらそれを返す
        var GameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
        if (GameSettingMenu.RegularGameSettings.active) return;

        //タイマーが0.1未満ならやり直し
        Timer += Time.deltaTime;
        if (Timer < 0.1f) return;
        Timer = 0f;

        //アイテムの数(オプションの数)
        float ItemsCount = __instance.Children.Length;

        float Offset = 2.75f;
        CustomOptionType Type = GetCustomOptionType(__instance.name);
        foreach (CustomOption Option in CustomOption.Options)
        {
            if (Option.Type != Type) continue;

            //設定したタイプのやつになるまで繰り返す
            if (GameObject.Find("NMRTab") && Option.Type != CustomOptionType.General)
                continue;
            if (GameObject.Find("CrewmateTab") && Option.Type != CustomOptionType.Crewmate)
                continue;
            if (GameObject.Find("ImpostorTab") && Option.Type != CustomOptionType.Impostor)
                continue;
            if (GameObject.Find("NeutralTab") && Option.Type != CustomOptionType.Neutral)
                continue;
            if (GameObject.Find("CombinationTab") && Option.Type != CustomOptionType.Combination)
                continue;
            if (GameObject.Find("AttributeTab") && Option.Type != CustomOptionType.Attribute)
                continue;

            if (Option?.OptionBehaviour != null && Option.OptionBehaviour.gameObject != null)
            {
                bool Enabled = true;
                var Parent = Option.Parent;

                if (AmongUsClient.Instance?.AmHost == false && CustomOptions.HideSettings.GetBool())
                {
                    Enabled = false;
                }

                if (Option.IsHidden)
                {
                    Enabled = false;
                }

                while (Parent != null && Enabled)
                {
                    Enabled = Parent.Enabled;
                    Parent = Parent.Parent;
                }

                Option.OptionBehaviour.gameObject.SetActive(Enabled);
                if (Enabled)
                {
                    Offset -= Option.IsHeader ? 0.75f : 0.5f;
                    Option.OptionBehaviour.transform.localPosition = new Vector3(Option.OptionBehaviour.transform.localPosition.x, Offset, Option.OptionBehaviour.transform.localPosition.z);

                    if (Option.IsHeader)
                    {
                        ItemsCount += 0.5f;
                    }
                }
                else
                {
                    ItemsCount --;
                }
            }
        }
        __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -4.0f + ItemsCount * 0.5f;
    }
}



//左側のリストにオプションを乗せる
[HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
class GameOptionsDataPatch
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        return typeof(IGameOptionsExtensions).GetMethods().Where(x => x.ReturnType == typeof(string) && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(int));
    }
    public static string OptionToString(CustomOption Option)
    {
        return Option == null ? "" : $"{Option.GetName()}: {Option.GetString()}";
    }
    public static string OptionsToString(CustomOption Option, bool SkipFirst = false)
    {
        if (Option == null) return "";

        List<string> Options = new();
        if (!Option.IsHidden && !SkipFirst) Options.Add(OptionToString(Option));
        if (Option.Enabled)
        {
            foreach (CustomOption op in Option.Children)
            {
                string str = OptionsToString(op);
                if (str != "") Options.Add(str);
            }
        }
        return string.Join("\n", Options);
    }

    static void Postifx(ref string __result)
    {
        bool HideSettings = AmongUsClient.Instance?.AmHost == false && CustomOptions.HideSettings.GetBool();
        if (HideSettings) return;

        List<string> Pages = new();
        Pages.Add(__result);

        StringBuilder Entry = new();
        List<string> Entries = new();

        Entries.Add(OptionToString(CustomOptions.PresetSelection));

        var OptionName = ModHelpers.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), Translator.GetString("CrewmateRoles"));
        var Min = CustomOptions.CrewmateRolesMin.GetSelection();
        var Max = CustomOptions.CrewmateRolesMax.GetSelection();
        if (Min > Max) Min = Max;
        var OptionValue = (Min == Max) ? $"{Max}" : $"{Min} - {Max}";
        Entry.AppendLine($"{OptionName}: {OptionValue}");

        OptionName = ModHelpers.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), Translator.GetString("ImpostorRoles"));
        Min = CustomOptions.ImpostorRolesMin.GetSelection();
        Max = CustomOptions.ImpostorRolesMax.GetSelection();
        if (Min > Max) Min = Max;
        OptionValue = (Min == Max) ? $"{Max}" : $"{Min} - {Max}";
        Entry.AppendLine($"{OptionName}: {OptionValue}");

        OptionName = ModHelpers.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), Translator.GetString("NeutralRoles"));
        Min = CustomOptions.NeutralRolesMin.GetSelection();
        Max = CustomOptions.NeutralRolesMax.GetSelection();
        if (Min > Max) Min = Max;
        OptionValue = (Min == Max) ? $"{Max}" : $"{Min} - {Max}";
        Entry.AppendLine($"{OptionName}: {OptionValue}");

        Entries.Add(Entry.ToString().Trim('\r', '\n'));

        static void AddChildren(CustomOption Option, ref StringBuilder Entry, bool Indent = true)
        {
            if (!Option.Enabled) return;

            foreach (var Child in Option.Children)
            {
                if (!Child.IsHidden)
                    Entry.AppendLine((Indent ? "    " : "") + OptionToString(Child));
                AddChildren(Child, ref Entry, Indent);
            }
        }

        foreach (CustomOption Option in CustomOption.Options)
        {
            if ((Option == CustomOptions.PresetSelection) ||
                (Option == CustomOptions.CrewmateRolesMin) ||
                (Option == CustomOptions.CrewmateRolesMax) ||
                (Option == CustomOptions.ImpostorRolesMin) ||
                (Option == CustomOptions.ImpostorRolesMax) ||
                (Option == CustomOptions.NeutralRolesMin) ||
                (Option == CustomOptions.NeutralRolesMax))
            {
                continue;
            }

            if (Option.Parent == null)
            {
                if (!Option.Enabled) continue;

                Entry = new();
                if (!Option.IsHidden)
                    Entry.AppendLine(OptionsToString(Option));

                AddChildren(Option, ref Entry, !Option.IsHidden);
                Entries.Add(Entry.ToString().Trim('\r', '\n'));
            }
        }

        int MaxLines = 28;
        int LineCount = 0;
        string Page = "";
        foreach (var e in Entries)
        {
            int Lines = e.Count(c => c == '\n') + 1;

            if (LineCount + Lines > MaxLines)
            {
                Pages.Add(Page);
                Page = "";
                LineCount = 0;
            }

            Page = Page + e + "\n\n";
            LineCount += Lines + 1;
        }

        Page = Page.Trim('\r', '\n');
        if (Page != "")
        {
            Pages.Add(Page);
        }

        int NumPages = Pages.Count;
        int Counter = NextMoreRolesPlugin.OptionsPage %= NumPages;

        __result = Pages[Counter].Trim('\r', '\n') + "\n\n" + Translator.GetString("PressTabForMore") + $" ({Counter + 1}/{NumPages})";
    }
}



[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
public class HudManagerUpdate
{
    public static float
    MinX,/*-5.3F*/
    OriginalY = 2.9F,
    MinY = 2.9F;


    public static Scroller Scroller;
    private static Vector3 LastPosition;
    private static float lastAspect;
    private static bool setLastPosition = false;

    public static void Prefix(HudManager __instance)
    {
        if (__instance.GameSettings?.transform == null) return;

        Rect safeArea = Screen.safeArea;
        float aspect = Mathf.Min(Camera.main.aspect, safeArea.width / safeArea.height);
        float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
        MinX = 0.1f - safeOrthographicSize * aspect;

        if (!setLastPosition || aspect != lastAspect)
        {
            LastPosition = new Vector3(MinX, MinY);
            lastAspect = aspect;
            setLastPosition = true;
            if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
        }

        CreateScroller(__instance);

        Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

        if (!Scroller.gameObject.active) return;

        var rows = __instance.GameSettings.text.Count(c => c == '\n');
        float LobbyTextRowHeight = 0.06F;
        var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

        Scroller.ContentYBounds = new FloatRange(MinY, maxY);

        if (PlayerControl.LocalPlayer?.CanMove != true)
        {
            __instance.GameSettings.transform.localPosition = LastPosition;

            return;
        }

        if (__instance.GameSettings.transform.localPosition.x != MinX ||
        __instance.GameSettings.transform.localPosition.y < MinY) return;

        LastPosition = __instance.GameSettings.transform.localPosition;
    }

    private static void CreateScroller(HudManager __instance)
    {
        if (Scroller != null) return;

        Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
        Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
        Scroller.gameObject.layer = 5;

        Scroller.transform.localScale = Vector3.one;
        Scroller.allowX = false;
        Scroller.allowY = true;
        Scroller.active = true;
        Scroller.velocity = new Vector2(0, 0);
        Scroller.ScrollbarYBounds = new FloatRange(0, 0);
        Scroller.ContentXBounds = new FloatRange(MinX, MinX);
        Scroller.enabled = true;

        Scroller.Inner = __instance.GameSettings.transform;
        __instance.GameSettings.transform.SetParent(Scroller.transform);
    }
}
