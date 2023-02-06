using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules
{
    public static class ModHelpers
    {
        public static string cs(Color c, string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
        public static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
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
        public static bool IsCheckListPlayerControl(this List<PlayerControl> ListData, PlayerControl CheckPlayer)
        {
            foreach (PlayerControl Player in ListData)
            {
                if (Player.PlayerId == CheckPlayer.PlayerId)
                {
                    return true;
                }
            }
            return false;
        }
        public static List<T> ToList<T>(this UnhollowerBaseLib.Il2CppArrayBase<T> Array)
        {
            List<T> list = new();
            foreach (var item in Array)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
