using System;
using System.Linq;
using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine;
using Hazel;
using NextMoreRoles.Roles;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules.CustomRPC;

namespace NextMoreRoles.Modules.CustomOptions;

public enum CustomOptionType
{
    General,    //メインタブ
    Crewmate,   //クルータブ
    Impostor,   //インポタブ
    Neutral,    //第三タブ
    Combination,//コンビネーション
    Attribute,  //属性タブ
}
public class CustomOption
{
    public static List<CustomOption> Options = new();
    public static int Preset = 0;

    public int Id;
    public string Name;
    public string Format;
    public System.Object[] Selections;

    public int DefaultSelection;
    public ConfigEntry<int> Entry;
    public int Selection;
    public OptionBehaviour OptionBehaviour;
    public CustomOption Parent;
    public List<CustomOption> Children;
    public bool IsHeader;
    public bool IsHidden;
    public CustomOptionType Type;
    public virtual bool Enabled
    {
        get
        {
            return GetBool();
        }
    }

    //Null用のやつ
    public CustomOption()
    {

    }

    public CustomOption(int Id, CustomOptionType Type, string Name, System.Object[] Selections, System.Object DefaultValue, CustomOption Parent, bool IsHeader, bool IsHidden, string Format)
    {
        this.Id = Id;
        this.Name = Name;
        this.Format = Format;
        this.Selections = Selections;
        int Index = Array.IndexOf(Selections, DefaultValue);
        this.DefaultSelection = Index >= 0 ? Index : 0;
        this.Parent = Parent;
        this.IsHeader = IsHeader;
        this.IsHidden = IsHidden;
        this.Type = Type;

        this.Children = new List<CustomOption>();
        if (Parent != null)
        {
            Parent.Children.Add(this);
        }

        Selection = 0;
        if (Id > 0)
        {
            Entry = NextMoreRolesPlugin.Instance.Config.Bind($"Preset{Preset}", Id.ToString(), DefaultSelection);
            Selection = Mathf.Clamp(Entry.Value, 0, Selections.Length - 1);

            if (Options.Any(x => x.Id == Id))
            {
                Logger.Warn($"設定IDが重複しています。ID:{Id}", "CustomOptions");
            }
        }
        Options.Add(this);
    }

    public static CustomOption Create(int Id, CustomOptionType Type, string Name, string[] Selections, CustomOption Parent = null, bool IsHeader = false, bool IsHidden = false, string Format = "")
    {
        return new CustomOption(Id, Type, Name, Selections, "", Parent, IsHeader, IsHidden, Format);
    }
    //Floatタイプを作成
    public static CustomOption Create(int Id, CustomOptionType Type, string Name, float DefaultValue, float Min, float Max, float Step, CustomOption Parent = null, bool IsHeader = false, bool IsHidden = false, string Format = "")
    {
        List<float> selections = new();
        for (float s = Min; s <= Max; s += Step)
            selections.Add(s);
        return new CustomOption(Id, Type, Name, selections.Cast<object>().ToArray(), DefaultValue, Parent, IsHeader, IsHidden, Format);
    }
    //Boolタイプを作成
    public static CustomOption Create(int Id, CustomOptionType Type, string Name, bool DefaultValue, CustomOption Parent = null, bool IsHeader = false, bool IsHidden = false, string Format = "")
    {
        return new CustomOption(Id, Type, Name, new string[] { "OptionOff", "OptionOn" }, DefaultValue ? "OptionOn" : "OptionOff", Parent, IsHeader, IsHidden, Format);
    }
    public static CustomRoleOption Create(int Id, RoleBase Role)
    {
        var roleInfo = Role.GetRoleBase();
        CustomOptionType type = roleInfo.RoleType switch
        {
            RoleType.Crewmate => CustomOptionType.Crewmate,
            RoleType.Impostor => CustomOptionType.Impostor,
            RoleType.Neutral => CustomOptionType.Neutral,
            RoleType.Attribute => CustomOptionType.Attribute,
            _ => CustomOptionType.General,
        };
        return new CustomRoleOption(Id, type, roleInfo.RoleNameKey, roleInfo.RoleNameColor);
    }

    public static void SwitchPreset(int newPreset)
    {
        CustomOption.Preset = newPreset;
        foreach (CustomOption option in CustomOption.Options)
        {
            if (option.Id <= 0) continue;

            option.Entry = NextMoreRolesPlugin.Instance.Config.Bind($"Preset{Preset}", option.Id.ToString(), option.DefaultSelection);
            option.Selection = Mathf.Clamp(option.Entry.Value, 0, option.Selections.Length - 1);
            if (option.OptionBehaviour is not null and StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = option.Selection;
                stringOption.ValueText.text = option.GetString();
            }
        }
    }

    public static void ShareOptionSelections()
    {
        if (CachedPlayer.AllPlayers.Count <= 1 || (AmongUsClient.Instance?.AmHost == false && PlayerControl.LocalPlayer == null)) return;

        MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)RPCId.ShareOptions, SendOption.Reliable);
        messageWriter.WritePacked((uint)CustomOption.Options.Count);
        foreach (CustomOption option in CustomOption.Options)
        {
            messageWriter.WritePacked((uint)option.Id);
            messageWriter.WritePacked((uint)Convert.ToUInt32(option.Selection));
        }
        messageWriter.EndMessage();
    }


    public virtual int GetSelection()
    {
        return Selection;
    }

    public virtual bool GetBool()
    {
        return Selection > 0;
    }

    public virtual float GetFloat()
    {
        return (float)Selections[Selection];
    }

    public virtual int GetInt()
    {
        return (int)GetFloat();
    }

    public virtual string GetString()
    {
        string sel = Selections[Selection].ToString();
        return Format != "" ? sel : Translator.GetString(sel);
    }

    public virtual string GetName()
    {
        return Translator.GetString(Name);
    }

    public virtual void UpdateSelection(int newSelection)
    {
        Selection = Mathf.Clamp((newSelection + Selections.Length) % Selections.Length, 0, Selections.Length - 1);
        if (OptionBehaviour is not null and StringOption stringOption)
        {
            stringOption.oldValue = stringOption.Value = Selection;
            stringOption.ValueText.text = GetString();

            if (AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer)
            {
                if (Id == 0) SwitchPreset(Selection);
                else if (Entry != null) Entry.Value = Selection;

                ShareOptionSelections();
            }
        }
    }
}



public class CustomOptionBlank : CustomOption
{
    public CustomOptionBlank(CustomOption Parent)
    {
        this.Parent = Parent;
        this.Id = -1;
        this.Name = "";
        this.IsHeader = false;
        this.IsHidden = true;
        this.Children = new List<CustomOption>();
        this.Selections = new string[] { "" };

        Options.Add(this);
    }

    public override int GetSelection()
    {
        return 0;
    }

    public override bool GetBool()
    {
        return true;
    }

    public override float GetFloat()
    {
        return 0f;
    }

    public override string GetString()
    {
        return "";
    }

    public override void UpdateSelection(int newSelection)
    {
        return;
    }
}

public class CustomRoleOption : CustomOption
{
    public static List<CustomRoleOption> RoleOptions = new();

    public CustomOption countOption = null;

    public RoleId RoleId;

    public int Rate
    {
        get
        {
            return GetSelection();
        }
    }

    public bool IsRoleEnable
    {
        get
        {
            return GetSelection() != 0;
        }
    }

    /*public IntroData Intro
    {
        get
        {
            return IntroData.GetIntroData(RoleId);
        }
    }*/

    public int Count
    {
        get
        {
            return countOption != null ? Mathf.RoundToInt(countOption.GetFloat()) : 1;
        }
    }

    public (int, int) Data
    {
        get
        {
            return (Rate, Count);
        }
    }

    public CustomRoleOption(int Id, CustomOptionType Type, string Name, Color Color, int Max = 15) :
        base(Id, Type, ModHelpers.cs(Color, Name), CustomOptionHolder.Rates, "", null, true, false, "NoTranslate")
    {
        try
        {
            this.RoleId = RoleBase.Roles.FirstOrDefault((_) =>
            {
                return _.RoleNameKey == Name;
            }).RoleId;
        }
        catch { }
        RoleOptions.Add(this);
        if (Max > 1)
            countOption = CustomOption.Create(Id + 1, Type, "PlayerCount", 1f, 1f, 15f, 1f, this, Format: "unitPlayers");
    }
}

public class CustomTasksOption : CustomOption
{
    public CustomOption CommonTasksOption = null;
    public CustomOption LongTasksOption = null;
    public CustomOption ShortTasksOption = null;

    public int CommonTasks { get { return Mathf.RoundToInt(CommonTasksOption.GetSelection()); } }
    public int LongTasks { get { return Mathf.RoundToInt(LongTasksOption.GetSelection()); } }
    public int ShortTasks { get { return Mathf.RoundToInt(ShortTasksOption.GetSelection()); } }
    public CustomTasksOption(int Id, CustomOptionType Type, int CommonDef, int LongDef, int ShortDef, CustomOption Parent = null)
    {
        CommonTasksOption = Create(Id + 1, Type, "CommonTasks", CommonDef, 0f, 4f, 1f, Parent, Format:"NoTranslate");
        LongTasksOption = Create(Id + 2, Type, "LongTasks", LongDef, 0f, 15f, 1f, Parent, Format:"NoTranslate");
        ShortTasksOption = Create(Id + 3, Type, "ShortTasks", ShortDef, 0f, 23f, 1f, Parent, Format:"NoTranslate");
    }
}
