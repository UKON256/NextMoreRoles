using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using NextMoreRoles.Modules.CustomOptions;
using NextMoreRoles.Modules.DatasManager;
using NextMoreRoles.Roles;
using NextMoreRoles.Patches.GamePatches.GameStart;

namespace NextMoreRoles.Modules.CustomRPC
{
    //RPCのリストぉ
    public enum CustomRPC
    {
        ShareOptions = 250,
        SetRoomDestroyTimer,
        ShareMODVersion,
        ShareBotData,
        SetRole,
        RPCShapeShift,
    }

    public static class RPCProcedure
    {
        //部屋の設定をシェアする
        public static void ShareOptions(int NumberOfOptions, MessageReader reader)
        {
            try
            {
                for (int i = 0; i < NumberOfOptions; i++)
                {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
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

        //役職をセットする
        public static void SetRole(byte PlayerId, byte SetRoleId)
        {
            var Player = ModHelpers.PlayerById(PlayerId);
            var RoleId = (RoleId)SetRoleId;
            //役職を消してから再設定する
            ////if (RoleId.IsAttribute_Role()) Player.RemoveRole();
            Player.SetRole(RoleId);
        }

        //シェイプシフト
        public static void RPCShapeShift(byte PlayerID, byte ShapeTargetID, byte IsAnimate)
        {
            PlayerControl Player = ModHelpers.PlayerById(PlayerID);
            PlayerControl ShapeTarget = ModHelpers.PlayerById(ShapeTargetID);
            bool Animate = false;

            if (IsAnimate != byte.MaxValue)
            {
                Animate = true;
            }
            Player.Shapeshift(ShapeTarget, Animate);
        }



        //RPC送信されたとき
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        class RPCHandlerPatch
        {
            static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
            {
                try
                {
                    byte PacketId = callId;
                    switch ((CustomRPC)PacketId)
                    {
                        case CustomRPC.ShareOptions:
                            ShareOptions((int)reader.ReadPackedUInt32(), reader);
                            break;

                        case CustomRPC.SetRoomDestroyTimer:
                            SetRoomDestroyTimer(reader.ReadByte(), reader.ReadByte());
                            break;

                        case CustomRPC.ShareMODVersion:
                            byte major = reader.ReadByte();
                            byte minor = reader.ReadByte();
                            byte patch = reader.ReadByte();
                            int versionOwnerId = reader.ReadPackedInt32();
                            byte revision = 0xFF;
                            Guid guid;
                            if (reader.Length - reader.Position >= 17)
                            { // enough bytes left to read
                                revision = reader.ReadByte();
                                // GUID
                                byte[] gbytes = reader.ReadBytes(16);
                                guid = new Guid(gbytes);
                            }
                            else
                            {
                                guid = new Guid(new byte[16]);
                            }
                            ShareMODVersion(major, minor, patch, revision == 0xFF ? -1 : revision, guid, versionOwnerId);
                            break;

                        //BOTデータシェア
                        case CustomRPC.ShareBotData:
                            ShareBotData(reader.ReadByte());
                            break;

                        //役職セット
                        case CustomRPC.SetRole:
                            SetRole(reader.ReadByte(), reader.ReadByte());
                            break;

                        //RPC版シェイプ
                        case CustomRPC.RPCShapeShift:
                            RPCShapeShift(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                            break;
                    }
                    Logger.Info($"RPCの受信に成功しました。ID:{callId}", "CustomRPC");
                }
                catch(SystemException Error)
                {
                    Logger.Error($"RPCの受信に失敗しました。ID:{callId}、エラー:{Error}", "CustomRPC");
                }
            }
        }
    }
}
