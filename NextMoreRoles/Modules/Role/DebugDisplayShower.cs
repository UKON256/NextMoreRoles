using System.Collections.Generic;
using System;
using UnityEngine;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules.Role
{
    class DebugDisplayShower
    {
        public static void Reset()
        {
            NowOpen = false;
            NowTab = DebugTabs.Main;
        }

        public static bool NowOpen = false;
        public static GameObject DebugBackground;
        public static SpriteRenderer SpriteRenderer;
        public static Minigame DebugBackgrounda;
        public static void Open()
        {
            if (NowOpen) return;
            NowTab = DebugTabs.Main;
            NowOpen = true;



            //非表示にする
            var TaskDisplay = GameObject.Find("TaskDisplay");
            var Buttons = GameObject.Find("Buttons");
            var RoomTracker = GameObject.Find("RoomTracker_TMP");
            TaskDisplay.SetActive(false);
            Buttons.SetActive(false);
            RoomTracker.SetActive(false);

            //背景ディスプレイ
            if (DebugBackground == null)
            {
                DebugBackground = new("DebugBackground");
                DebugBackground.transform.localScale *= 1.5f;
                //DebugBackground.GetComponent<Renderer>().material.color -= new Color(0f, 0f, 0f, 50f);

                SpriteRenderer = DebugBackground.AddComponent<SpriteRenderer>();
                SpriteRenderer.sprite = ResourcesManager.LoadSpriteFromResources("NextMoreRoles.Resources.Game.DebugDisplay_Background.png", 150f);
                DebugBackground.transform.SetParent(Camera.main.transform, false);
            }
            DebugBackground.SetActive(true);
        }

        public static void Close()
        {
            NowOpen = false;

            //背景を非表示
            DebugBackground.SetActive(false);

            var TaskDisplay = GameObject.Find("TaskDisplay");
            var Buttons = GameObject.Find("Buttons");
            var RoomTracker = GameObject.Find("RoomTracker_TMP");
            TaskDisplay.SetActive(true);
            Buttons.SetActive(true);
            RoomTracker.SetActive(true);
        }

        public static DebugTabs NowTab;
        public static void CreateButtons()
        {
            //テンプレート


            //ボタン削除
        }



        public class DebugDisplayButton
        {
            public List<DebugDisplayButton> Buttons = new();
            public string Text;
            public DebugTabs Tab;
            public Action Action;

            public DebugDisplayButton(string Text, DebugTabs Tab, Action Action)
            {
                this.Text = ModTranslation.GetString(Text);
                this.Tab = Tab;
                this.Action = Action;
            }
        }

        public enum DebugTabs
        {
            All,
            Main,
            ChangeRoles,
            Functions,
        }



        public static DebugDisplayButton GoToChangeRoleTab = new("GoToChangeRole", DebugTabs.Main,
        ()=> {
            NowTab = DebugTabs.ChangeRoles;
            CreateButtons();
        });
    }
}
