using UnityEngine;
using System;
using System.Collections.Generic;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.Role.CustomButtons;

namespace NextMoreRoles.Roles.Data.Attribute
{
    class DebuggerFunctions
    {
        public static void OnMeetingEndEvent()
        {
            CustomButtons.DebuggerButton.MaxTimer = 0f;
            CustomButtons.DebuggerButton.Timer = 0f;
        }


        public class DebugDisplay
        {
            public static void OpenDisplay()
            {
                RoleClass.Debugger.NowTab = DebugTabs.Main;
                NextMoreRoles.Modules.Role.ScientistVitalShower.Open(RoleClass.Debugger.DebugBackground, "DebugBackground", true);
            }

            public static void MakeButton(string Text, Transform Transform)
            {

            }

            public static void Close()
            {

            }
        }
    }



    public class DebugPanels
    {
        public List<DebugPanels> Panels = new();
        public string Text;
        public DebugTabs Tab;
        public Action Action;

        public DebugPanels(string Text, DebugTabs Tab, Action Action)
        {
            this.Text = ModTranslation.GetString(Text);
            this.Tab = Tab;
            this.Action = Action;
            Panels.Add(this);
        }
    }

    public enum DebugTabs
    {
        All,
        Main,
        ChangeRoles,
        Functions,
    }
}
