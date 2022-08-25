using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
                Modules.Role.ScientistVitalShower.Open(RoleClass.Debugger.DebugBackground, "DebugBackground", true);
                RoleClass.Debugger.NowTab = DebugTabs.Main;
                MakeButtons();
            }

            public static void MakeButtons()
            {
                //今までのボタンを消す
                DeleteButtons();

                //作る
                foreach (DebugDisplayPlate Panel in DebugDisplayPlate.DebugPanels)
                {
                    if (Panel.Tab != RoleClass.Debugger.NowTab) continue;

                    var DebugPanel = new GameObject("DebugPanel");
                    DebugPanel.gameObject.AddComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Game.DebugDisplay_Plate.png", 150f);
                    DebugPanel.transform.localPosition = new Vector3(0.0f, 0.0f, -55f);
                    DebugPanel.transform.SetParent(Camera.main.transform, false);
                }
            }

            public static void DeleteButtons()
            {
            }
        }
    }

    public class DebugDisplayPlate
    {
        public static List<DebugDisplayPlate> DebugPanels = new();

        public string Text;
        public DebugTabs Tab;
        public Action Action;

        public DebugDisplayPlate(string Text, DebugTabs Tab, Action Action)
        {
            this.Text = ModTranslation.GetString(Text);
            this.Tab = Tab;
            this.Action = Action;
            DebugPanels.Add(this);
        }



        public static DebugDisplayPlate GoToChangeRole = new("GoToChangeRole", DebugTabs.Main,
        ()=>{
            RoleClass.Debugger.NowTab = DebugTabs.ChangeRoles;
            DebuggerFunctions.DebugDisplay.MakeButtons();
        });
    }

    public enum DebugTabs
    {
        All,
        Main,
        ChangeRoles,
        Functions,
    }
}
