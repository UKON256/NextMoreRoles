using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using NextMoreRoles.Modules.FlagManager;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    //全員のENDをカスタム
    public class GameEndsSetUp
    {
        public static TMPro.TMP_Text TextRenderer;

        [HarmonyPatch(typeof(EndGameNavigation), nameof(EndGameNavigation.ShowProgression))]
        class ShowProgressionPatch
        {
            static void Prefix()
            {
                if (TextRenderer != null)
                {
                    TextRenderer.gameObject.SetActive(false);
                }
            }
        }

        //実行元:HarmonyPatches.EndGameManager.cs
        public static void OnGameEnd_Prefix(EndGameResult EndGameResult)
        {
            //Postfixで使うやつ
            AdditionalTempData.GameOverReason = EndGameResult.GameOverReason;
            //ここを変えるとアーソニストとかが勝っても別の奴になる
            if ((int)EndGameResult.GameOverReason >= 10) EndGameResult.GameOverReason = GameOverReason.ImpostorByKill;
        }
        //実行元:HarmonyPatches.EndGamamanager.cs
        public static void OnGameEnd_Postfix(EndGameManager __instance)
        {
            try
            {
                //リセットとか
                var GameOverReason = AdditionalTempData.GameOverReason;
                AdditionalTempData.Clear();

                // フラグを建てる(下の方が優先)
                bool CrewWin = GameOverReason == GameOverReason.HumansByVote || GameOverReason == GameOverReason.HumansByTask || GameOverReason == GameOverReason.HumansDisconnect;
                bool ImpostorWin = GameOverReason == GameOverReason.ImpostorByVote || GameOverReason == GameOverReason.ImpostorByKill || GameOverReason == GameOverReason.ImpostorBySabotage || GameOverReason == GameOverReason.ImpostorDisconnect;

                //第三が勝ってようが関係ない奴たち
                bool IsEveryoneDied = Players.GetAllAlivePlayer == null;
                bool IsHaison = GameOverReason == (GameOverReason)CustomGameOverReason.Haison;


                //クルー勝利
                if (CrewWin)
                {
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        if (p.IsCrew())
                        {
                            WinningPlayerData Wpd = new(p.Data);
                            TempData.winners.Add(Wpd);
                        }
                    }
                    AdditionalTempData.WinCondition = WinCondition.CrewmateWin;
                }

                //インポ勝利
                if (ImpostorWin)
                {
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        if (p.IsImpostor() || p.IsMad())
                        {
                            WinningPlayerData Wpd = new(p.Data);
                            TempData.winners.Add(Wpd);
                        }
                    }
                    AdditionalTempData.WinCondition = WinCondition.ImpostorWin;
                }

                //全滅
                if (IsEveryoneDied)
                {
                    TempData.winners.Clear();
                    AdditionalTempData.WinCondition = WinCondition.EveryoneDied;
                }

                //廃村
                else if (IsHaison)
                {
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        if (p.IsPlayer())
                        {
                            WinningPlayerData Wpd = new(p.Data);
                            TempData.winners.Add(Wpd);
                        }
                    }
                    AdditionalTempData.WinCondition = WinCondition.Haison;
                }

                //いずれもあてはまらないときにエラーエンド
                else
                {
                    TempData.winners.Clear();
                    AdditionalTempData.WinCondition = WinCondition.ErrorEnd;
                }
            }
            catch(SystemException Error)
            {
                Logger.Error($"試合判定にてエラーが発生しました。エラー:{Error}", "GameEnds");
            }
        }
    }

    class EverythingUp
    {
        //実行元:HarmonyPatches.EndGameManager.cs
        public static void Prefix()
        {
        }

        //実行元:HarmonyPatches.EndGameManager.cs
        public static void Postfix(EndGameManager __instance)
        {
            string EndText = "";
            Color EndBackColor = Color.white;
            Color EndTextColor = Color.black;

            GameObject BonusTextObject = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            BonusTextObject.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y - 0.8f, __instance.WinText.transform.position.z);
            BonusTextObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            GameEndsSetUp.TextRenderer = BonusTextObject.GetComponent<TMPro.TMP_Text>();
            GameEndsSetUp.TextRenderer.text = "";

            var WinCondition = AdditionalTempData.WinCondition;
            //勝利によって条件分岐
            switch(WinCondition)
            {
                //OO勝利
                //クルー
                case WinCondition.CrewmateWin:
                    EndText = ModTranslation.GetString("CrewmateWin");
                    EndBackColor = Palette.CrewmateBlue;
                    EndTextColor = Color.white;
                    break;

                //インポ
                case WinCondition.ImpostorWin:
                    EndText = ModTranslation.GetString("ImpostorWin");
                    EndBackColor = Palette.ImpostorRed;
                    break;


                // OO勝利じゃないやつたち
                //廃村
                case WinCondition.Haison:
                    EndText = ModTranslation.GetString("Haison");
                    EndBackColor = new Color32(163, 163, 163, byte.MaxValue);
                    EndTextColor = Color.white;
                    break;

                //全滅
                case WinCondition.EveryoneDied:
                    EndText = ModTranslation.GetString("EveryoneDied");
                    EndBackColor = Palette.DisabledGrey;
                    break;

                //いずれも当てはまらないときにエラーエンド
                case WinCondition.ErrorEnd:
                    EndText = ModTranslation.GetString("ErrorEnd");
                    EndBackColor = Palette.DisabledGrey;
                    break;
            };

            //リセット
            Patches.GamePatches.GameStart.GameStart_ClearAndReloads.ClearAndReloads();

            //背景、テキスト、もろもろセット
            Logger.Info("=====試合終了=====", "GameEnds");
            Logger.Info($"要因:{WinCondition}", "GameEnds");

            //テキストの色とか内容を変更
            GameEndsSetUp.TextRenderer.text = EndText;
            GameEndsSetUp.TextRenderer.color = EndTextColor == Color.black ? EndBackColor:EndTextColor; //EndTextColorが他で代入されてなければ背景色に、代入されてればそれを
            //背景の変更
            __instance.BackgroundBar.material.SetColor("_Color", EndBackColor);

            //リセット
            AdditionalTempData.Clear();
        }
    }
}
