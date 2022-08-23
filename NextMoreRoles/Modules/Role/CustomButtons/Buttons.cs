using UnityEngine;
using NextMoreRoles.Modules.DatasManager;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.Role.CustomButtons
{
    static class CustomButtons
    {
        //=====クルー陣営=====//



        //=====インポ陣営=====//



        //====第三陣営=====//



        //=====重複=====//
        public static CustomButton DebuggerButton;



        public static void HudManagerStart_Postfix(HudManager __instance)
        {
            //=====クルー陣営=====//



            //=====インポ陣営=====//



            //====第三陣営=====//



            //=====重複=====//
            DebuggerButton = new(
                ()=> { Roles.Data.Attribute.DebuggerFunctions.OnMeetingEndEvent(); },
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
            DebuggerButton.Timer = 0.1f;
            DebuggerButton.ButtonText = ModTranslation.GetString("Debug");
            DebuggerButton.ShowButtonText = true;
        }
    }
}
