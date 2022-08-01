using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules
{
    public static class ModHelpers
    {
        public static Dictionary<byte, SpriteRenderer> MyRendCache = new();
        public static TextMeshPro NameText(this PlayerControl player)
        {
            return player.cosmetics.nameText;
        }
        public static TextMeshPro NameText(this PoolablePlayer player)
        {
            return player.transform.FindChild("NameText_TMP").GetComponent<TextMeshPro>();
        }
        public static SpriteRenderer MyRend(this PlayerControl player)
        {
            bool Isnull = true;
            if (MyRendCache.ContainsKey(player.PlayerId))
            {
                if (MyRendCache[player.PlayerId] == null) Isnull = true;
                else Isnull = false;
            }
            if (Isnull)
            {
                MyRendCache[player.PlayerId] = player.transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
            }
            return MyRendCache[player.PlayerId];
        }


        public static PlayerControl PlayerById(byte id)
        {
            foreach (CachedPlayer player in CachedPlayer.AllPlayers)
            {
                if (player.PlayerId == id)
                {
                    return player;
                }
            }
            return null;
        }

        public static InnerNet.ClientData GetClient(this PlayerControl player)
        {
            var client = AmongUsClient.Instance.allClients.GetFastEnumerator().ToArray().Where(cd => cd.Character.PlayerId == player.PlayerId).FirstOrDefault();
            return client;
        }
        public static int GetClientId(this PlayerControl player)
        {
            var client = player.GetClient();
            if (client == null) return -1;
            return client.Id;
        }
    }
}
