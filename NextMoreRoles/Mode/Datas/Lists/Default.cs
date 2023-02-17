using HarmonyLib;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Mode;

public class Default : ModeBase
{
    public Default() : base(ModeId.Default, ModeId.Default.ToString()) {}

    public override void ClearAndReload() { }
}
