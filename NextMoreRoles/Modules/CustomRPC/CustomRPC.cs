using System;
using HarmonyLib;
using Hazel;
using NextMoreRoles.Patches.GamePatches.GameEnds;

namespace NextMoreRoles.Modules.CustomRPC
{
    public enum RoleId
    {

    }

    public enum CustomRPC
    {
        SetRoomDestroyTimer,
        ShareMODVersion,
        UncheckedSetVanilaRole,
        SpawnBot,
        HaisonFlagUp,
    }

    public static class RPCProcedure
    {
        public static void SetRoomDestroyTimer(byte min, byte seconds)
        {
            Patches.LobbyPatches.ShareGameVersion.timer = (min * 60) + seconds;
        }
        public static void ShareMODVersion(int major, int minor, int build, int revision, Guid guid, int clientId)
        {
            System.Version ver;
            if (revision < 0)
                ver = new System.Version(major, minor, build);
            else
                ver = new System.Version(major, minor, build, revision);
            Patches.LobbyPatches.ShareGameVersion.GameStartManagerUpdatePatch.VersionPlayers[clientId] = new Patches.LobbyPatches.PlayerVersion(ver, guid);
        }
        public static void UncheckedSetVanilaRole(byte playerid, byte roletype)
        {
            var player = ModHelpers.playerById(playerid);
            if (player == null) return;
            DestroyableSingleton<RoleManager>.Instance.SetRole(player, (RoleTypes)roletype);
            player.Data.Role.Role = (RoleTypes)roletype;
        }

        public static void HaisonFlagUp()
        {
            GameEndsSetUp.IsHaison = true;
        }



        //RPC管理！
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        class RPCHandlerPatch
        {
            static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte CallId, [HarmonyArgument(1)] MessageReader Reader)
            {
                try
                {
                    byte PacketId = CallId;
                    switch ((CustomRPC)PacketId)
                    {
                        case CustomRPC.SetRoomDestroyTimer:
                            SetRoomDestroyTimer(Reader.ReadByte(), Reader.ReadByte());
                            break;
                        case CustomRPC.ShareMODVersion:
                            byte major = Reader.ReadByte();
                            byte minor = Reader.ReadByte();
                            byte patch = Reader.ReadByte();
                            int versionOwnerId = Reader.ReadPackedInt32();
                            byte revision = 0xFF;
                            Guid guid;
                            if (Reader.Length - Reader.Position >= 17)
                            { // enough bytes left to read
                                revision = Reader.ReadByte();
                                // GUID
                                byte[] gbytes = Reader.ReadBytes(16);
                                guid = new Guid(gbytes);
                            }
                            else
                            {
                                guid = new Guid(new byte[16]);
                            }
                            ShareMODVersion(major, minor, patch, revision == 0xFF ? -1 : revision, guid, versionOwnerId);
                            break;
                        case CustomRPC.UncheckedSetVanilaRole:
                            UncheckedSetVanilaRole(Reader.ReadByte(), Reader.ReadByte());
                            break;
                        case CustomRPC.HaisonFlagUp:
                            HaisonFlagUp();
                            break;
                    }
                    Logger.Info("CustomRPCを送信しました。コールID:"+CallId, "CustomRPC");
                }
                catch(SystemException Error)
                {
                    Logger.Error("CustomRPCにてエラーが発生しました。\nコールID:" +CallId+ "、\nエラー:"+Error, "CustomRPC");
                }
            }
        }
    }
}
