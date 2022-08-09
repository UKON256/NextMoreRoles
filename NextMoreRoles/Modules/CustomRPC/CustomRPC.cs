using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Modules.CustomRPC
{
    public enum CustomRPC
    {
        ShareOptions = 112,
        SetRoomDestroyTimer,
        ShareMODVersion,
        ShareBotData,
    }

    public static class RPCProcedure
    {
        //部屋の設定をシェアする
        public static void ShareOptions(int NumberOfOptions, MessageReader Reader)
        {
            try
            {
                for (int i = 0; i < NumberOfOptions; i++)
                {
                    uint optionId = Reader.ReadPackedUInt32();
                    uint selection = Reader.ReadPackedUInt32();
                    CustomOption Option = CustomOption.Options.FirstOrDefault(option => option.Id == (int)optionId);
                    Option.UpdateSelection((int)selection);
                }
            }
            catch (SystemException Error)
            {
                Logger.Error($"部屋設定のシェアに失敗しました。{Error}", "CustomRPC");
            }
        }

        //部屋が消し去られる時間をセットする
        public static void SetRoomDestroyTimer(byte Min, byte Seconds)
        {
            Patches.LobbyPatches.ShareGameVersion.Timer = (Min * 60) + Seconds;
        }

        //MODのバージョン、MODが入っているか否かをシェアする
        public static void ShareMODVersion(int major, int minor, int build, int revision, Guid guid, int clientId)
        {
            System.Version ver;
            if (revision < 0)
                ver = new System.Version(major, minor, build);
            else
                ver = new System.Version(major, minor, build, revision);
            Patches.LobbyPatches.ShareGameVersion.GameStartManagerUpdatePatch.VersionPlayers[clientId] = new Patches.LobbyPatches.PlayerVersion(ver, guid);
        }

        //BOTのデータを他人にシェアする
        public static void ShareBotData(byte BotId)
        {
            PlayerControl Bot = ModHelpers.PlayerById(BotId);
            if (Bot == null) return;
            if(BotManager.AllBots == null) BotManager.AllBots = new();
            BotManager.AllBots.Add(Bot);
        }

        //RPC管理
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        class RPCHandlerPatch
        {
            static void Postfix([HarmonyArgument(0)] byte CallId, [HarmonyArgument(1)] MessageReader Reader)
            {
                try
                {
                    byte PacketId = CallId;
                    switch (PacketId)
                    {
                        case (byte)CustomRPC.ShareOptions:
                            ShareOptions((int)Reader.ReadPackedUInt32(), Reader);
                            break;

                        case (byte)CustomRPC.SetRoomDestroyTimer:
                            SetRoomDestroyTimer(Reader.ReadByte(), Reader.ReadByte());
                            break;

                        case (byte)CustomRPC.ShareMODVersion:
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

                        case (byte)CustomRPC.ShareBotData:
                            ShareBotData(Reader.ReadByte());
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
