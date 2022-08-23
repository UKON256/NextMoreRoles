using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NextMoreRoles.Helpers;
using NextMoreRoles.Roles;
using NextMoreRoles.Modules.DatasManager;

namespace NextMoreRoles.Modules.Role.CustomButtons
{
    public class CustomButton
    {
        public static List<CustomButton> Buttons = new();
        public ActionButton ActionButton;
        public Vector3 LocalScale = Vector3.one;
        public float MaxTimer = float.MaxValue;
        public float Timer = 0f;
        public bool EffectCancellable = false;
        public bool IsEffectActive = false;
        public bool ShowButtonText = true;
        public string ButtonText = null;

        private readonly Action OnClickEvent;
        private readonly Func<bool, RoleId, bool> HasButton;
        private readonly Func<bool> CouldUse;
        private readonly Func<bool> CouldCountCool;
        private readonly Action OnMeetingEndsEvent;
        public Sprite ButtonSprite;
        public Vector3 PositionOffset;
        private HudManager HudManager;
        private readonly KeyCode? HotKey;
        public bool HasEffect;
        public float EffectDuration;
        private readonly Action OnEffectEnds;
        private readonly bool Mirror;
        public Color? Color;

        public CustomButton(Action OnClickEvent, Func<bool, RoleId, bool>HasButton, Func<bool> CouldUse, Func<bool> CouldCountCool, Action OnMeetingEndsEvent, Sprite ButtonSprite, Vector3 PositionOffset, HudManager HudManager, ActionButton TextTemplate, KeyCode? HotKey, bool HasEffect, float EffectDuration, Action OnEffectEnds, bool Mirror = false, string ButtonText = "", Color? Color = null)
        {
            this.OnClickEvent = OnClickEvent;
            this.HasButton = HasButton;
            this.CouldUse = CouldUse;
            this.CouldCountCool = CouldCountCool;
            this.OnMeetingEndsEvent = OnMeetingEndsEvent;
            this.ButtonSprite = ButtonSprite;
            this.PositionOffset = PositionOffset;
            this.HudManager = HudManager;
            this.HotKey = HotKey;
            this.HasEffect = HasEffect;
            this.EffectDuration = EffectDuration;
            this.OnEffectEnds = OnEffectEnds;
            this.Mirror = Mirror;
            this.Color = Color;
            Timer = 16.2f;
            Buttons.Add(this);

            ActionButton = UnityEngine.Object.Instantiate(TextTemplate, TextTemplate.transform.parent);
            PassiveButton Button = ActionButton.GetComponent<PassiveButton>();
            Button.OnClick = new Button.ButtonClickedEvent();
            Button.OnClick.AddListener((UnityEngine.Events.UnityAction)OnClickEvent);

            LocalScale = ActionButton.transform.localScale;
            if (TextTemplate)
            {
                UnityEngine.Object.Destroy(ActionButton.buttonLabelText);
                ActionButton.buttonLabelText = UnityEngine.Object.Instantiate(TextTemplate.buttonLabelText, ActionButton.transform);
            }
            SetActive(false);
        }

        public CustomButton(Action OnClickEvent, Func<bool, RoleId, bool> HasButton, Func<bool> CouldUse, Func<bool> CouldCountCool, Action OnMeetingEndsEvent, Sprite ButtonSprite, Vector3 PositionOffset, HudManager HudManager, ActionButton TextTemplate, KeyCode? HotKey, bool Mirror = false, string ButtonText = "", Color? Color = null)
        : this(OnClickEvent, HasButton, CouldUse, CouldCountCool, OnMeetingEndsEvent, ButtonSprite, PositionOffset, HudManager, TextTemplate, HotKey, false, 0f, () => { }, Mirror, ButtonText, Color) { }

        private void SetActive(bool IsActive)
        {
            if (IsActive)
            {
                ActionButton.gameObject.SetActive(true);
                ActionButton.graphic.enabled = true;
            }
            else
            {
                ActionButton.gameObject.SetActive(false);
                ActionButton.graphic.enabled = false;
            }
        }

        private void Update(bool isAlive, RoleId role)
        {
            var LocalPlayer = CachedPlayer.LocalPlayer;
            var Moveable = LocalPlayer.PlayerControl.moveable;

            //会議中、追放画面中、条件、プレイヤーのデータがなければボタンを消す
            if (LocalPlayer.Data == null || MeetingHud.Instance || ExileController.Instance || !HasButton(isAlive, role))
            {
                SetActive(false);
                return;
            }
            SetActive(HudManager.UseButton.isActiveAndEnabled);

            ActionButton.graphic.sprite = ButtonSprite;
            if (ShowButtonText && ButtonText != "")
            {
                ActionButton.OverrideText(ButtonText);
            }
            ActionButton.buttonLabelText.enabled = ShowButtonText;

            if (HudManager.UseButton != null)
            {
                Vector3 pos = HudManager.UseButton.transform.localPosition;
                if (Mirror) pos = new Vector3(-pos.x, pos.y, pos.z);
                ActionButton.transform.localPosition = pos + PositionOffset;
            }
            if (CouldUse())
            {
                ActionButton.graphic.color = ActionButton.buttonLabelText.color = Palette.EnabledColor;
                ActionButton.graphic.material.SetFloat("_Desat", 0f);
            }
            else
            {
                ActionButton.graphic.color = ActionButton.buttonLabelText.color = Palette.DisabledClear;
                ActionButton.graphic.material.SetFloat("_Desat", 1f);
            }

            if (Color != null)
            {
                ActionButton.graphic.color = (Color)Color;
            }

            if (Timer >= 0)
            {
                if ((HasEffect && IsEffectActive) ||
                    (!LocalPlayer.PlayerControl.inVent && Moveable && !CouldCountCool()))
                    Timer -= Time.deltaTime;
            }

            if (Timer <= 0 && HasEffect && IsEffectActive)
            {
                IsEffectActive = false;
                ActionButton.cooldownTimerText.color = Palette.EnabledColor;
                OnEffectEnds();
            }

            ActionButton.SetCoolDown(Timer, (HasEffect && IsEffectActive) ? EffectDuration : MaxTimer);
            if ((HotKey.HasValue && Input.GetButtonDown(HotKey.Value.ToString()))) OnClickEvent();
        }



        void OnClick()
        {
            if ((this.Timer < 0f) || (this.HasEffect && this.IsEffectActive && this.EffectCancellable))
            {
                ActionButton.graphic.color = new Color(1f, 1f, 1f, 0.3f);
                this.OnClick();

                if (this.HasEffect && !this.IsEffectActive)
                {
                    this.Timer = this.EffectDuration;
                    ActionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    this.IsEffectActive = true;
                }
            }
        }

        public static void HudUpdate()
        {
            Buttons.RemoveAll(item => item.ActionButton == null);

            bool isAlive = PlayerControl.LocalPlayer.IsAlive();
            RoleId role = PlayerControl.LocalPlayer.GetRole();
            foreach (CustomButton btn in Buttons)
            {
                try
                {
                    btn.Update(isAlive, role);
                }
                catch (SystemException Error)
                {
                    Logger.Error($"ボタンのアップデートにてエラーが発生しました。エラー:{Error}", "CustomButton");
                }
            }
        }
    }
}
