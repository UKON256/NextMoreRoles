using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace NextMoreRoles.Helpers
{
    public class CachedPlayer
    {
        public static readonly Dictionary<IntPtr, CachedPlayer> PlayerPtrs = new();
        public static readonly List<CachedPlayer> AllPlayers = new();
        public static CachedPlayer LocalPlayer;

        public Transform transform;
        public PlayerControl PlayerControl;
        public PlayerPhysics PlayerPhysics;
        public CustomNetworkTransform NetTransform;
        public GameData.PlayerInfo Data;
        public byte PlayerId;
        public uint NetId;

        public static implicit operator bool(CachedPlayer player)
        {
            return player != null && player.PlayerControl;
        }

        public static implicit operator PlayerControl(CachedPlayer player) => player.PlayerControl;
        public static implicit operator PlayerPhysics(CachedPlayer player) => player.PlayerPhysics;
    }

    [HarmonyPatch]
    public static class CachedPlayerPatches
    {
        [HarmonyPatch]
        private class CacheLocalPlayerPatch
        {
            [HarmonyTargetMethod]
            public static MethodBase TargetMethod()
            {
                var type = typeof(PlayerControl).GetNestedTypes(AccessTools.all).FirstOrDefault(t => t.Name.Contains("Start"));
                return AccessTools.Method(type, nameof(IEnumerator.MoveNext));
            }

            [HarmonyPostfix]
            public static void SetLocalPlayer()
            {
                var localPlayer = PlayerControl.LocalPlayer;
                if (!localPlayer)
                {
                    CachedPlayer.LocalPlayer = null;
                    return;
                }

                var cached = CachedPlayer.AllPlayers.FirstOrDefault(p => p.PlayerControl.Pointer == localPlayer.Pointer);
                if (cached != null)
                {
                    CachedPlayer.LocalPlayer = cached;
                    return;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
        public static void CachePlayerPatch(PlayerControl __instance)
        {
            if (__instance.notRealPlayer) return;
            var player = new CachedPlayer
            {
                transform = __instance.transform,
                PlayerControl = __instance,
                PlayerPhysics = __instance.MyPhysics,
                NetTransform = __instance.NetTransform
            };
            CachedPlayer.AllPlayers.Add(player);
            CachedPlayer.PlayerPtrs[__instance.Pointer] = player;

#if DEBUG
            foreach (var cachedPlayer in CachedPlayer.AllPlayers)
            {
                if (!cachedPlayer.PlayerControl || !cachedPlayer.PlayerPhysics || !cachedPlayer.NetTransform || !cachedPlayer.transform)
                {
                    Logger.Error("CachedPlayer {cachedPlayer.PlayerControl.name} has null fields", "CachedPlayer");
                }
            }
#endif
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
        public static void RemoveCachedPlayerPatch(PlayerControl __instance)
        {
            if (__instance.notRealPlayer) return;
            CachedPlayer.AllPlayers.RemoveAll(p => p.PlayerControl.Pointer == __instance.Pointer);
            CachedPlayer.PlayerPtrs.Remove(__instance.Pointer);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameData), nameof(GameData.Deserialize))]
        public static void AddCachedDataOnDeserialize()
        {
            foreach (CachedPlayer cachedPlayer in CachedPlayer.AllPlayers)
            {
                cachedPlayer.Data = cachedPlayer.PlayerControl.Data;
                cachedPlayer.PlayerId = cachedPlayer.PlayerControl.PlayerId;
                cachedPlayer.NetId = cachedPlayer.PlayerControl.NetId;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameData), nameof(GameData.AddPlayer))]
        public static void AddCachedDataOnAddPlayer()
        {
            foreach (CachedPlayer cachedPlayer in CachedPlayer.AllPlayers)
            {
                cachedPlayer.Data = cachedPlayer.PlayerControl.Data;
                cachedPlayer.PlayerId = cachedPlayer.PlayerControl.PlayerId;
                cachedPlayer.NetId = cachedPlayer.PlayerControl.NetId;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Deserialize))]
        public static void SetCachedPlayerId(PlayerControl __instance)
        {
            CachedPlayer.PlayerPtrs[__instance.Pointer].PlayerId = __instance.PlayerId;
            CachedPlayer.PlayerPtrs[__instance.Pointer].NetId = __instance.NetId;
        }
    }
}
