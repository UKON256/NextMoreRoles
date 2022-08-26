using UnityEngine;
using UnityEngine.UI;
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
                Modules.Role.ScientistVitalShower.Open(RoleClass.Debugger.DebugBackground, "DebugBackground", true);
                RoleClass.Debugger.NowTab = DebugTabs.Main;
                MakeButtons();

                //テキスト
                TMPro.TextMeshPro DisplayTitle = GameObject.Instantiate(CustomButtons.DebuggerButton.ActionButton.buttonLabelText, DebugPanel.transform.parent);
                DisplayTitle.text = ModTranslation.GetString("DebugDisplay");
                DisplayTitle.alignment = TMPro.TextAlignmentOptions.Left;
                DisplayTitle.transform.localScale *= 3f;
                DisplayTitle.transform.localPosition = new(-1.75f, 2.05f, -5.0f);
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
                    DebugPanel = new GameObject("DebugPanel");
                    DebugPanel.gameObject.AddComponent<SpriteRenderer>().sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Game.DebugDisplay_Plate.png", 150f);
                    DebugPanel.transform.SetParent(UnityEngine.GameObject.Find("DebugBackground").transform, false);
                    DebugPanel.gameObject.layer = LayerMask.NameToLayer("UI");

                    //パネルにイベントを追加

                    //テキスト
                    TMPro.TextMeshPro Label = GameObject.Instantiate(CustomButtons.DebuggerButton.ActionButton.buttonLabelText, DebugPanel.transform.parent);
                    Label.text = CustomOptions.cs(Panel.Color, Panel.Text);
                    Label.alignment = TMPro.TextAlignmentOptions.Center;
                    Label.transform.localScale *= 1.5f;

                    //スコア調整
                    if (xCount == 5)
                    {
                        xCount = 0;
                        yCount++;
                    }

                    //位置調整
                    DebugPanel.transform.localPosition = new(-2.75f + 2f*xCount, 1.6f + -1.5f*yCount, -5.0f);
                    Label.transform.localPosition = new(-2.75f + 2f*xCount, 1.6f + -1.5f*yCount, -5.0f);

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
        }
    }

    public class DebugDisplayPanel
    {
        public static List<DebugDisplayPanel> DebugPanels = new();

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
            Logger.Info("あああ", "");
            /*RoleClass.Debugger.NowTab = DebugTabs.ChangeRoles;
            DebuggerFunctions.DebugDisplay.DisableButtons();
            DebuggerFunctions.DebugDisplay.MakeButtons();
            SoundManager.Instance.PlaySound(FastDestroyableSingleton<MeetingHud>.Instance.VoteSound, false);*/
        });

        public static DebugDisplayPanel GoToFunctions = new("GoToFunctions", Color.black, DebugTabs.Main,
        ()=>{
            DebuggerFunctions.DebugDisplay.DisableButtons();
            RoleClass.Debugger.NowTab = DebugTabs.Functions;
            DebuggerFunctions.DebugDisplay.MakeButtons();
        });
    }

    public enum DebugTabs
    {
        Main,
        ChangeRoles,
        Functions,
    }
}
