using System;
using System.Reflection;
using System.Collections.Generic;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Mode;

public class ModeBase
{
    public static Dictionary<ModeId, ModeBase> ModeCaches = new();
    public static List<ModeBase> Modes = new();
    public static List<string> ModeNames = new();
    public static void Load()
    {
        /*var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (!typeof(ModeBase).IsAssignableFrom(type) || type.IsAbstract) continue;

            var modeId = (ModeId)Enum.Parse(typeof(ModeId), type.Name);
            var modeBase = Activator.CreateInstance(type) as ModeBase;
            ModeCaches[modeId] = modeBase;
        }*/
    }

    public ModeId ModeId;
    public string ModeNameKey;
    public bool CanHostOnly;

    public ModeBase() {}
    public ModeBase(
            ModeId ModeId,
            string ModeNameKey,
            bool CanHostOnly = false)
    {
        this.ModeId = ModeId;
        this.ModeNameKey = ModeNameKey;
        this.CanHostOnly = CanHostOnly;

        if (ModeId != ModeId.ModeBase) {
            ModeNames.Add(Translator.GetString(this.ModeNameKey));
            Modes.Add(this);
        }
    }

    //* 関数 *//
    public virtual void SetupModeOption() { }
    public virtual void ClearAndReload() { ModeManager.CurrentGameModeId = (ModeId)CustomOptionHolder.ModeSetting.GetSelection(); }
    public virtual void WhenShowTeam() { }
    public virtual void WhenShowRole() { }

    //* 戻り値あり *//
    public string GetModeName() => Translator.GetString(this.ModeNameKey);
    public bool CanHostOnlyMode() => this.CanHostOnly;
    public ModeBase GetMode() => this;
}
