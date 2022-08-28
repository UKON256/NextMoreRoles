using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;
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
                Modules.Role.ScientistVitalShower.Open("DebugBackground", true);
                RoleClass.Debugger.NowTab = DebugTabs.Main;
                MakeButtons();

                //テキスト
                TextMeshPro DisplayTitle = GameObject.Instantiate(CustomButtons.DebuggerButton.ActionButton.buttonLabelText, DebugPanel.transform.parent);
                DisplayTitle.text = ModTranslation.GetString("DebugDisplay");
                DisplayTitle.alignment = TextAlignmentOptions.Left;
                DisplayTitle.transform.localScale *= 3f;
                DisplayTitle.transform.localPosition = new(-1.75f, 2.05f, -50.0f);
            }

            public static List<GameObject> Panels;
            public static GameObject DebugPanel;
            public static void MakeButtons()
            {
                int xCount = 0;
                int yCount = 0;
                Panels = new();
                //作る
                foreach (DebugDisplayPanel Panel in DebugDisplayPanel.DebugPanels)
                {
                    if (Panel.Tab != RoleClass.Debugger.NowTab) continue;

                    //パネル
                    DebugPanel = UnityEngine.Object.Instantiate(UnityEngine.GameObject.Find("MenuButton"));
                    DebugPanel.gameObject.GetComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Game.DebugDisplay_Plate.png", 150f);
                    DebugPanel.gameObject.layer = LayerMask.NameToLayer("UI");
                    DebugPanel.gameObject.GetComponent<PassiveButton>().OnClick = new();
                    DebugPanel.gameObject.GetComponent<PassiveButton>().OnClick.AddListener(Panel.Action);
                    GameObject.Destroy(DebugPanel.gameObject.GetComponent<AspectPosition>());
                    DebugPanel.transform.localScale = new(1f, 1f, 1f);
                    DebugPanel.transform.SetParent(UnityEngine.GameObject.Find("DebugBackground").transform, false);
                    DebugPanel.name = "DebugPanel";
                    Panels.Add(DebugPanel);

                    //テキスト
                    TextMeshPro Label = GameObject.Instantiate(CustomButtons.DebuggerButton.ActionButton.buttonLabelText, DebugPanel.transform.parent);
                    Label.text = CustomOptions.cs(Panel.Color, Panel.Text);
                    Label.alignment = TextAlignmentOptions.Center;
                    Label.transform.localScale *= 1.5f;
                    Label.name = "DebugPanelLabel";

                    //スコア調整
                    if (xCount == 5)
                    {
                        xCount = 0;
                        yCount++;
                    }

                    //位置調整
                    DebugPanel.transform.localPosition = new(-2.75f + 2f*xCount, 1.6f + -1.5f*yCount, -50.0f);
                    Label.transform.localPosition = new(-2.75f + 2f*xCount, 1.6f + -1.5f*yCount, -50.0f);

                    xCount++;
                }
            }

            public static void DisableButtons()
            {
                foreach (GameObject Panel in Panels)
                {
                    Panel.gameObject.SetActive(false);
                    Panels.Remove(Panel);
                }
            }

            public static void DisplayPanelFuncTemplate()
            {
                DebuggerFunctions.DebugDisplay.DisableButtons();
                new LateTask(()=>
                {
                    SoundManager.Instance.PlaySound(FastDestroyableSingleton<MeetingHud>.Instance.VoteSound, false);
                    DebuggerFunctions.DebugDisplay.MakeButtons();
                }, 0.2f, "MakeDebugPanels");
            }
        }
    }

    public class DebugDisplayPanel
    {
        public static List<DebugDisplayPanel> DebugPanels = new();

        public PassiveButton PanelButton;

        public string Text;
        public Color Color;
        public DebugTabs Tab;
        public Action Action;

        public DebugDisplayPanel(string Text, Color Color, DebugTabs Tab, Action Action)
        {
            this.Text = ModTranslation.GetString(Text);
            this.Color = Color;
            this.Tab = Tab;
            this.Action = Action;
            DebugPanels.Add(this);
        }



        public static DebugDisplayPanel GoToChangeRole = new("GoToChangeRole", Color.red, DebugTabs.Main,
        ()=>{
            RoleClass.Debugger.NowTab = DebugTabs.ChangeRoles;
            DebuggerFunctions.DebugDisplay.DisplayPanelFuncTemplate();

        });

        public static DebugDisplayPanel GoToFunctions = new("GoToFunctions", Color.black, DebugTabs.Main,
        ()=>{

            RoleClass.Debugger.NowTab = DebugTabs.Functions;
            DebuggerFunctions.DebugDisplay.DisplayPanelFuncTemplate();
        });
    }

    public enum DebugTabs
    {
        Main,
        ChangeRoles,
        Functions,
    }
}
