using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules.DatasManager;

namespace NextMoreRoles.Patches.GamePatches.GameEnds
{
    //全員のENDをカスタム EverythingUpの前に実行
    public class GameEndsSetUp
    {
        public static TMPro.TMP_Text TextRenderer;

        //XPとかをもらうときにOO勝利を非表示
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
                if (NextMoreRoles.Modules.Role.DebugDisplayShower.NowOpen) NextMoreRoles.Modules.Role.DebugDisplayShower.Close();



                //===== フラグを建てる(下の方が優先)=====//
                //クルー
                bool IsCrewmateWin = GameOverReason == GameOverReason.HumansByTask ||
                GameOverReason == GameOverReason.HumansByVote ||
                GameOverReason == GameOverReason.HumansDisconnect;

                //インポ
                bool IsImpostorWin = GameOverReason == GameOverReason.ImpostorByKill ||
                GameOverReason == GameOverReason.ImpostorBySabotage ||
                GameOverReason == GameOverReason.ImpostorByVote ||
                GameOverReason == GameOverReason.ImpostorDisconnect;

                //第三

                //第三が勝ってようが関係ない奴たち
                bool IsEveryoneDied = PlayerDatas.GetAllAlivePlayer() == null;
                bool IsHaison = GameOverReason == (GameOverReason)CustomGameOverReason.Haison;



                //リセット(これをしないと同じプレイヤーが二人でたりする)
                TempData.winners.Clear();
                WinningPlayerData Wpd;



                //=====フラグによって判定、表示する人を変える=====//
                //クルー
                if (IsCrewmateWin)
                {
                    AdditionalTempData.WinCondition = WinCondition.CrewmateWin;
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        if (p.IsCrew())
                        {
                            Wpd = new(p.Data);
                            TempData.winners.Add(Wpd);
                        }
                    }
                }

                //インポ
                else if (IsImpostorWin)
                {
                    AdditionalTempData.WinCondition = WinCondition.ImpostorWin;
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        if (p.IsImpostor() || p.IsMad())
                        {
                            Wpd = new(p.Data);
                            TempData.winners.Add(Wpd);
                        }
                    }
                }

                //第三

                //全滅
                else if (IsEveryoneDied)
                {
                    TempData.winners = new();
                    AdditionalTempData.WinCondition = WinCondition.EveryoneDied;
                }

                //廃村以外にいずれもあてはまらないときにエラーエンド
                else
                {
                    TempData.winners = new();
                    AdditionalTempData.WinCondition = WinCondition.ErrorEnd;
                }

                //廃村
                if (IsHaison)
                {
                    TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                    foreach (PlayerControl p in CachedPlayer.AllPlayers)
                    {
                        Wpd = new(p.Data);
                        TempData.winners.Add(Wpd);
                    }
                    AdditionalTempData.WinCondition = WinCondition.Haison;
                }
            }
            catch(SystemException Error)
            {
                Logger.Error($"試合判定にてエラーが発生しました。エラー:{Error}", "GameEnds");
            }
        }
    }

    //OnGameEndの後に実行
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
            bool IsVisibleWinText = true;

            //プレイヤー全削除
            foreach (PoolablePlayer pb in __instance.transform.GetComponentsInChildren<PoolablePlayer>())
            {
                UnityEngine.Object.Destroy(pb.gameObject);
            }
            //プレイヤーを並べていく
            int Number = Mathf.CeilToInt(7.5f);
            List<WinningPlayerData> list = TempData.winners.ToArray().ToList().OrderBy(delegate (WinningPlayerData b)
            {
                if (!b.IsYou)
                {
                    return 0;
                }
                return -1;
            }).ToList<WinningPlayerData>();
            for (int i = 0; i < list.Count; i++)
            {
                WinningPlayerData winningPlayerData2 = list[i];
                int Number2 = (i % 2 == 0) ? -1 : 1;
                int Number3 = (i + 1) / 2;
                float Number4 = (float)Number3 / (float)Number;
                float Number5 = Mathf.Lerp(1f, 0.75f, Number4);
                float Number6 = (float)((i == 0) ? -8 : -1);
                PoolablePlayer PoolablePlayer = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, __instance.transform);
                PoolablePlayer.transform.localPosition = new Vector3(1f * (float)Number2 * (float)Number3 * Number5, FloatRange.SpreadToEdges(-1.125f, 0f, Number3, Number), Number6 + (float)Number3 * 0.01f) * 0.9f;
                float Number7 = Mathf.Lerp(1f, 0.65f, Number4) * 0.9f;
                Vector3 vector = new(Number7, Number7, 1f);
                PoolablePlayer.transform.localScale = vector;
                PoolablePlayer.UpdateFromPlayerOutfit((GameData.PlayerOutfit)winningPlayerData2, PlayerMaterial.MaskType.ComplexUI, winningPlayerData2.IsDead, true);
                if (winningPlayerData2.IsDead)
                {
                    PoolablePlayer.cosmetics.currentBodySprite.BodySprite.sprite = PoolablePlayer.cosmetics.currentBodySprite.GhostSprite;
                    PoolablePlayer.SetDeadFlipX(i % 2 == 0);
                }
                else
                {
                    PoolablePlayer.SetFlipX(i % 2 == 0);
                }

                PoolablePlayer.cosmetics.nameText.color = Color.white;
                PoolablePlayer.cosmetics.nameText.transform.localScale = new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
                PoolablePlayer.cosmetics.nameText.transform.localPosition = new Vector3(PoolablePlayer.cosmetics.nameText.transform.localPosition.x, PoolablePlayer.cosmetics.nameText.transform.localPosition.y, -15f);
                PoolablePlayer.cosmetics.nameText.text = winningPlayerData2.PlayerName;

                foreach (var Data in AdditionalTempData.PlayerRoles)
                {
                    if (Data.PlayerName != winningPlayerData2.PlayerName) continue;
                    PoolablePlayer.cosmetics.nameText.text += Data.NameSuffix + $"\n<size=80%>{Data.RoleString}</size>";
                }
            }



            //"勝利"または&OO勝利のテキスト
            GameObject BonusTextObject = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            BonusTextObject.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y - 0.8f, __instance.WinText.transform.position.z);
            BonusTextObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            GameEndsSetUp.TextRenderer = BonusTextObject.GetComponent<TMPro.TMP_Text>();
            GameEndsSetUp.TextRenderer.text = "";

            var WinCondition = AdditionalTempData.WinCondition;
            //=====勝利によって条件分岐=====//
            switch(WinCondition)
            {
                //OO勝利
                //クルー
                case WinCondition.CrewmateWin:
                    EndText = ModTranslation.GetString("Crewmate");
                    EndBackColor = Palette.CrewmateBlue;
                    EndTextColor = Palette.White;
                    break;

                //インポ
                case WinCondition.ImpostorWin:
                    EndText = ModTranslation.GetString("Impostor");
                    EndBackColor = Palette.ImpostorRed;
                    break;

                // OO勝利じゃないやつたち
                //廃村
                case WinCondition.Haison:
                    EndText = ModTranslation.GetString("Haison");
                    EndBackColor = new Color32(163, 163, 163, byte.MaxValue);
                    EndTextColor = Palette.White;
                    IsVisibleWinText = false;
                    __instance.WinText.text = EndText;
                    __instance.WinText.color = EndBackColor;
                    break;

                //全滅
                case WinCondition.EveryoneDied:
                    EndText = ModTranslation.GetString("EveryoneDied");
                    EndBackColor = Palette.DisabledGrey;
                    EndTextColor = Palette.White;
                    IsVisibleWinText = false;
                    __instance.WinText.text = EndText;
                    break;

                //いずれも当てはまらないときにエラーエンド
                default:
                    EndText = ModTranslation.GetString("ErrorEnd");
                    EndBackColor = Palette.DisabledGrey;
                    IsVisibleWinText = false;
                    break;
            };

            //ログ
            Logger.Info("========試合終了========", "GameEnds");
            Logger.Info($"要因:{WinCondition}", "GameEnds");

            //陣営の方のテキストの色と内容
            GameEndsSetUp.TextRenderer.text = IsVisibleWinText ? string.Format(EndText + ModTranslation.GetString("Win")) : EndText;  //もしIsVisibleWinTextがtrueならOO勝利のテキストを
            GameEndsSetUp.TextRenderer.color = EndTextColor == Color.black ? EndBackColor:EndTextColor;                                     //EndTextColorが他で代入されてなければ背景色に、代入されてればそれを
            //背景の変更
            __instance.BackgroundBar.material.SetColor("_Color", EndBackColor);

            //リセット
            AdditionalTempData.Clear();
            ResetRoleCache.ClearCache();
        }
    }
}
