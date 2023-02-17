using System.Collections.Generic;
using UnityEngine;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules.CustomRPC;

namespace NextMoreRoles.Modules
{
    public static class BotManager
    {
        public static List<PlayerControl> AllBots = new();

        //* スポーン *//
        public static PlayerControl Spawn(string Name = "BOT", byte BotPlayerId = 1)
        {
            byte id = 0;
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.PlayerId > id)
                {
                    id = p.PlayerId;
                }
            }
            var Bot = Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);

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

            GameData.Instance.RpcSetTasks(Bot.PlayerId, new byte[0]);
            RPCSender.CallRPC(RPCId.ShareBotData, new(Bot.PlayerId));
            AllBots.Add(Bot);
            return Bot;
        }

        //* デスポーン *//
        public static void Despawn(PlayerControl Bot)
        {
            GameData.Instance.RemovePlayer(Bot.PlayerId);
            AmongUsClient.Instance.Despawn(Bot);
            AllBots.Remove(Bot);
        }

        //* すべてデスポーン *//
        public static void AllBotsDespawn()
        {
            foreach (PlayerControl Bots in AllBots)
            {
                GameData.Instance.RemovePlayer(Bots.PlayerId);
                Bots.Despawn();
                Logger.Info("BOTをすべてデスポーンしました", "BotManager");
            }
        }

        //* 投票させる *//
        public static void Vote(PlayerControl Bot, MeetingHud __instance)
        {
            __instance.CastVote(Bot.PlayerId, Bot.PlayerId);
        }
    }
}
