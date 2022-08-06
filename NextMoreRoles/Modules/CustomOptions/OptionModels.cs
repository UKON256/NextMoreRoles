using System;
using System.Linq;
using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine;

namespace NextMoreRoles.Modules.CustomOptions
{
    public enum CustomOptionType
    {
        General,    //メインタブ
        Crewmate,   //クルータブ
        Impostor,   //インポタブ
        Neutral,    //第三タブ
        Modifier,   //属性タブ
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

        public bool Enabled => this.GetBool();

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

        //
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
            return Format != "" ? sel : ModTranslation.GetString(sel);
        }
    }
}
