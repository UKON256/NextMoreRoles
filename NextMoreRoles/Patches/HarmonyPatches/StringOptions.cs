using HarmonyLib;
using System.Linq;
using System;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Patches.HarmonyPatches
{
    //有効になった時に値を設定(これがないと初期設定がバニラの設定になる)
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption Option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (Option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = Option.GetName();
            __instance.Value = __instance.oldValue = Option.Selection;
            __instance.ValueText.text = Option.GetString();

            return false;
        }
    }

    //+ボタンを押したときに変更(これがないと+を押したときにバニラの設定になる)
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption Option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (Option == null) return true;
            Option.UpdateSelection(Option.Selection + 1);
            return false;
        }
    }

    //-ボタンを押したときに変更(これがないと-を押したときにバニラの設定になる)
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption Option = CustomOption.Options.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (Option == null) return true;
            Option.UpdateSelection(Option.Selection - 1);
            return false;
        }
    }
}
