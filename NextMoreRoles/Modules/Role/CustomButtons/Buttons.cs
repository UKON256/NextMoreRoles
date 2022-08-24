using TMPro;
using UnityEngine;
using NextMoreRoles.Modules.DatasManager;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.Role.CustomButtons
{
    static class CustomButtons
    {
        //=====クルー陣営=====//
        public static CustomButton SheriffFireButton;
        public static TMPro.TMP_Text SheriffCanFireCount;



        //=====インポ陣営=====//



        //====第三陣営=====//
        public static CustomButton JackalKillButton;
        public static CustomButton JackalSideKickButton;



        //=====重複=====//
        public static CustomButton DebuggerButton;



        public static void HudManagerStart_Postfix(HudManager __instance)
        {
            //=====クルー陣営=====//
            SheriffFireButton = new(
                ()=> {  },
                (bool IsAlive, RoleId Role)=> { return IsAlive && Role == RoleId.Sheriff; },
                ()=> { return PlayerControl.LocalPlayer.CanMove; },
                ()=> { return !PlayerControl.LocalPlayer.CanMove; },
                ()=> { Roles.Data.Crewmate.SheriffFunctions.OnMeetingEndEvent(); },
                __instance.KillButton.graphic.sprite,
                new Vector3(0f, 1f, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q
            );
            SheriffFireButton.ButtonText = ModTranslation.GetString("Fire");
            SheriffFireButton.ShowButtonText = true;



            //=====インポ陣営=====//



            //====第三陣営=====//
            JackalKillButton = new(
                ()=> {  },
                (bool IsAlive, RoleId Role)=> { return IsAlive && Role == RoleId.Sheriff; },
                ()=> { return PlayerControl.LocalPlayer.CanMove; },
                ()=> { return !PlayerControl.LocalPlayer.CanMove; },
                ()=> { Roles.Data.Neutral.JackalFunctions.OnMeetingEndEvent(); },
                __instance.KillButton.graphic.sprite,
                new Vector3(0f, 1f, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q
            );



            //=====重複=====//
            DebuggerButton = new(
                ()=> { Roles.Data.Attribute.DebuggerFunctions.OnClickEvent(); },
                (bool IsAlive, RoleId Role)=> { return PlayerControl.LocalPlayer.IsAttributeRole(RoleId.Debugger); },
                ()=> { return PlayerControl.LocalPlayer.CanMove; },
                ()=> { return !PlayerControl.LocalPlayer.CanMove; },
                ()=> {},
                RoleClass.Debugger.GetButtonSprite(),
                new Vector3(0, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.K,
                Mirror:true
            );
            DebuggerButton.MaxTimer = 0f;
            DebuggerButton.Timer = 0f;
            DebuggerButton.ButtonText = ModTranslation.GetString("Debug");
            DebuggerButton.ShowButtonText = true;
        }
    }
}
