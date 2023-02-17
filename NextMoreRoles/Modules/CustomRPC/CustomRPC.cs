using System;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using NextMoreRoles.Roles;
using NextMoreRoles.Mode;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Modules.CustomRPC;

//* RPCのリスト *//
public enum RPCId
{
    ShareOptions = 35,
    ShareBotData,

    RPCClearAndReload,
    SetRole,
    SwapRole,
}


//* 処理 *//
public static class RPCProcedure
{
    //* 部屋の設定をシェアする *//
    public static void ShareOptions(int NumberOfOptions, MessageReader reader) {
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

    //* BOTのデータを他人にシェアする *//
    public static void ShareBotData(byte BotId) {
        PlayerControl Bot = ModHelpers.PlayerById(BotId);
        if (Bot == null) return;
        if(BotManager.AllBots == null) BotManager.AllBots = new();
        BotManager.AllBots.Add(Bot);
    }

    //* 役職をセットする *//
    public static void SetRole(byte PlayerId, byte RoleId) {
        var Player = ModHelpers.PlayerById(PlayerId);
        var Role = RoleBase.RoleBaseCaches[(RoleId)RoleId];

        NextMoreRolesPlugin.Logger.LogInfo($"{Player.ToString()} => {Role.ToString()} 役職を付与");
        if (Role.IsNormalRole()) RoleHelper.RoleCache[PlayerId] = Role;
        else if (Role.IsAttributeRole()) RoleHelper.AttributeCache[PlayerId] = Role;
        else if (Role.IsGhostRole()) RoleHelper.GhostRoleCache[PlayerId] = Role;
        Role.HistoryBeenPlayers.Add(Player);
    }
    //* 役職を交換する *//
    public static void SwapRole(byte targetPlayerId, byte targetPlayerId2) {
        var Player = ModHelpers.PlayerById(targetPlayerId);
        var Player2 = ModHelpers.PlayerById(targetPlayerId2);

        var tmp = (byte)Player2.GetRole().RoleId;
        var tmp2 = (byte)Player.GetRole().RoleId;
        SetRole(targetPlayerId, tmp);
        SetRole(targetPlayerId2, tmp2);
    }

    public static void RPCClearAndReload() {
        ModeBase.Modes.Do(x => x.ClearAndReload());

        RoleHelper.RoleCache = new();
        RoleHelper.AttributeCache = new();
        RoleBase.Roles.Do(x => x.HistoryBeenPlayers = new());
        RoleBase.Roles.Do(x => x.ClearAndReload());
    }



    //* RPCが送信されたとき *//
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            try
            {
                byte PacketId = callId;
                switch ((RPCId)PacketId)
                {
                    case RPCId.ShareOptions:
                        ShareOptions((int)reader.ReadPackedUInt32(), reader);
                        break;

                    case RPCId.ShareBotData:
                        ShareBotData(reader.ReadByte());
                        break;

                    case RPCId.SetRole:
                        SetRole(reader.ReadByte(), reader.ReadByte());
                        break;
                    case RPCId.SwapRole:
                        SwapRole(reader.ReadByte(), reader.ReadByte());
                        break;

                    case RPCId.RPCClearAndReload:
                        RPCClearAndReload();
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
