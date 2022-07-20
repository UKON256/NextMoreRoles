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
        public static PlayerControl Spawn(string name = "Bot", byte BotPlayerId = 1)
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
            /*
            if (id < 14) {
                id = 15;
            }
            */
            Bot.PlayerId = id;
            // Bot.PlayerId = BotPlayerId;
            GameData.Instance.AddPlayer(Bot);
            AmongUsClient.Instance.Spawn(Bot, -2, InnerNet.SpawnFlags.IsClientCharacter);
            Bot.transform.position = new Vector3(9999f, 9999f, 0);
            Bot.NetTransform.enabled = true;

            Bot.RpcSetName(name);
            Bot.RpcSetColor(1);
            Bot.RpcSetHat("hat_NoHat");
            Bot.RpcSetPet("peet_EmptyPet");
            Bot.RpcSetVisor("visor_EmptyVisor");
            Bot.RpcSetNamePlate("nameplate_NoPlate");
            Bot.RpcSetSkin("skin_None");
            GameData.Instance.RpcSetTasks(Bot.PlayerId, new byte[0]);
            Logger.Info("botスポーン!\nID:" + Bot.PlayerId + "\nBotName:" + Bot.name, "BotManager");
            AllBots.Add(Bot);
            MessageWriter writer = RPCHelper.StartRPC(CustomRPC.CustomRPC.SpawnBot);
            writer.Write(Bot.PlayerId);
            new LateTask(() => writer.EndRPC(), 0.5f);
            return Bot;
        }
        //デスポーン！
        public static void Despawn(PlayerControl Bot)
        {
            Logger.Info("ボットのデスポーン中です。\nID:" + Bot.PlayerId + "\nBotName:" + Bot.name, "BotManager");
            GameData.Instance.RemovePlayer(Bot.PlayerId);
            AmongUsClient.Instance.Despawn(Bot);
            AllBots.Remove(Bot);
        }
    }
}
