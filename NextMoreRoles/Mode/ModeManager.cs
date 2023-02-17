using System.Linq;

namespace NextMoreRoles.Mode;

public enum ModeId
{
    Default,
    HideNSeek,
    LevelUp,
    RandomSwap,

    // インスタンス用Id(使わない)
    ModeBase,
}

class ModeManager
{
    public static ModeId CurrentGameModeId = ModeId.Default;

    public static bool IsMode(ModeId[] ModeIds)
    {
        bool isDefault = AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay;
        foreach (ModeId ModeId in ModeIds)
        {
            if (isDefault && ModeId == ModeId.Default) return true;
            if (IsMode(ModeId)) return true;
            return false;
        }
        return false;
    }
    public static bool IsMode(ModeId modeId)
    {
        return CurrentGameModeId == modeId;
    }

    public static ModeBase GetCurrentModeBase(bool Cached = true)
    {
        try
        {
            if (Cached) { return ModeBase.ModeCaches[CurrentGameModeId]; }
            else { return ModeBase.Modes.FirstOrDefault(x => x.ModeId == CurrentGameModeId); }
        } catch {
            return null;
        }
    }

    public static ModeBase GetModeBase(ModeId ModeId, bool Cached = true)
    {
        try
        {
            if (Cached) { return ModeBase.ModeCaches[ModeId]; }
            else { return ModeBase.Modes.FirstOrDefault(x => x.ModeId == ModeId); }
        } catch {
            return null;
        }
    }
}
