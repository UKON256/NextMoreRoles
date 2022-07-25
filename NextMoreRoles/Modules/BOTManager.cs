using System;
using System.Collections.Generic;
using Hazel;
using UnityEngine;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules
{
    // Spawn dummys
    class BotManager
    {
        public static List<PlayerControl> AllBots = new();
        //スポーン！
        public static PlayerControl Spawn(string Name = "BOTだよぉ", byte BotPlayerId = 1)
        {
            byte id = 0;
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.PlayerId > id)
                {
                    id = p.PlayerId;
                }
            }
            var Bot = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);

            id++;
            Bot.PlayerId = id;
            GameData.Instance.AddPlayer(Bot);
            AmongUsClient.Instance.Spawn(Bot, -2, InnerNet.SpawnFlags.IsClientCharacter);
            Bot.transform.position = new Vector3(9999f, 9999f, 0);
            Bot.NetTransform.enabled = true;

            //BOTのあれこれを設定
            Bot.RpcSetName(Name);
            Bot.RpcSetColor(1);
            Bot.RpcSetHat("hat_NoHat");
            Bot.RpcSetPet("peet_EmptyPet");
            Bot.RpcSetVisor("visor_EmptyVisor");
            Bot.RpcSetNamePlate("nameplate_NoPlate");
            Bot.RpcSetSkin("skin_None");

            //RPC送信
            GameData.Instance.RpcSetTasks(Bot.PlayerId, new byte[0]);
            AllBots.Add(Bot);
            MessageWriter writer = RPCHelper.StartRPC(CustomRPC.CustomRPC.SpawnBot);
            writer.Write(Bot.PlayerId);
            new LateTask(() => writer.EndRPC(), 0.5f);
            Logger.Info("BOTをスポーンしました", "BotManager");
            return Bot;
        }
        //デスポーン！
        public static void Despawn(PlayerControl Bot)
        {
            GameData.Instance.RemovePlayer(Bot.PlayerId);
            AmongUsClient.Instance.Despawn(Bot);
            AllBots.Remove(Bot);
        }
        //すべてデスポーン！
        public static void AllBotsDespawn()
        {
            foreach (PlayerControl Bots in AllBots)
            {
                GameData.Instance.RemovePlayer(Bots.PlayerId);
                Bots.Despawn();
                Logger.Info("BOTをすべてデスポーンしました", "BotManager");
            }
        }



        //BOTフラグ！
        public static bool IsBot(PlayerControl Target)
        {
            try
            {
                if (Target == null || Target.Data.Disconnected) return false;
                //BOTのどれかが目標のIDが一緒ならBOT
                foreach (PlayerControl Bots in AllBots)
                {
                    if (Bots.PlayerId == Target.PlayerId) return true;
                }
                return false;
            }
            catch(SystemException Error)
            {
                Logger.Error($"Botフラグが正常に動作しませんでした。\nエラー:{Error}", "BotManager");
                return false;
            }
        }
        public static bool IsPlayer(PlayerControl Target)
        {
            return !IsBot(Target);
        }
    }
}
