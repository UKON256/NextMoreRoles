using Hazel;
using System;
using System.Collections.Generic;
using NextMoreRoles.Roles;

namespace NextMoreRoles.Modules.CustomRPC;

static class RPCSender
{
    //RPCをまとめて送信する
    public static void CallRPC(RPCId RPCId, List<byte> Informations)
    {
        try
        {
            //RPC予約
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCId, SendOption.Reliable, -1);

            //情報(byte)をすべて書き込む
            foreach (byte Info in Informations)
            {
                Writer.Write(Info);
            }

            //RPCおわり
            AmongUsClient.Instance.FinishRpcImmediately(Writer);

            //ログ
            Logger.Info($"RPCを送信しました。Id:{RPCId}", "RPCSender");
        }
        catch(SystemException Error)
        {
            Logger.Error($"RPCの送信に失敗しました。エラー:{Error}", "RPCSender");
        }
    }

    //RPCを特定の人にのみ送信する
    public static void CallRPC(RPCId RPCId, List<byte> Informations, PlayerControl TargetPlayer)
    {
        try
        {
            var Target = TargetPlayer != null ? TargetPlayer.GetClientId() : -1;

            //RPC予約
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCId, SendOption.Reliable, Target);

            //情報(byte)をすべて書き込む
            foreach (byte Info in Informations)
            {
                Writer.Write(Info);
            }

            //RPCおわり
            AmongUsClient.Instance.FinishRpcImmediately(Writer);

            //ログ
            Logger.Info($"RPCを送信しました。Id:{RPCId}", "RPCSender");
        }
        catch(SystemException Error)
        {
            Logger.Error($"RPCの送信に失敗しました。エラー:{Error}", "RPCSender");
        }
    }



    //* テンプレート *//
    public static void RPCSetRole(this PlayerControl target, RoleId roleId) {
        //if (target.GetRole().RoleId == roleId) return;

        CallRPC(RPCId.SetRole, new(){ target.PlayerId, (byte)roleId });
        RPCProcedure.SetRole(target.PlayerId, (byte)roleId);
    }
}
