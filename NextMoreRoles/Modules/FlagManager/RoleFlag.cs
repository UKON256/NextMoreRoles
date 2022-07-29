//役職関連のフラグ管理
namespace NextMoreRoles.Modules.FlagManager
{
    public static class RoleFlag
    {
        public static bool IsImpostor(this PlayerControl Target)
        {
            return Target != null && Target.Data.Role.IsImpostor;
        }
        public static bool IsCrew(this PlayerControl Target)
        {
            return !IsImpostor(Target) && !IsMad(Target) && !IsNeutral(Target) && !IsFriend(Target);
        }
        public static bool IsNeutral(this PlayerControl Target)
        {
            var IsNeutral = false;
            /*switch (Target.)
            {
                case
                    //第三か
                    IsNeutral = true;
                    break;
            }*/
            return IsNeutral;
        }

        public static bool IsMad(this PlayerControl Target)
        {
            var IsMad = false;
            /*switch (Target.GetRole)
            {
                case
            }*/
            return IsMad;
        }

        public static bool IsFriend(this PlayerControl Target)
        {
            var IsFriend = false;
            /*switch (Target.GetRole())
            {
                case
            }*/
            return IsFriend;
        }
    }
}
