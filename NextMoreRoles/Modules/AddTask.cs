using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Modules
{
    class AddTask
    {
        public static (CustomOptions.CustomOption, CustomOptions.CustomOption, CustomOptions.CustomOption) TaskSetting(int CommonId, int ShortId, int LongId, CustomOptions.CustomOption Child = null, CustomOptionType Type = CustomOptionType.General)
        {
            CustomOptions.CustomOption CommonOption = CustomOptions.CustomOption.Create(CommonId, Type, "CommonTasks", 1, 0, 12, 1, Child);
            CustomOptions.CustomOption ShortOption = CustomOptions.CustomOption.Create(ShortId, Type, "ShortTasks", 1, 0, 69, 1, Child);
            CustomOptions.CustomOption LongOption = CustomOptions.CustomOption.Create(LongId, Type, "LongTasks", 1, 0, 45, 1, Child);
            return (CommonOption, ShortOption, LongOption);
        }
    }
}
