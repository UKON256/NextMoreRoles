using Hazel;
using System;
using System.Collections.Generic;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules.CustomRPC
{
    class RPCSender
    {
        //RPCをまとめて送信するよぉ
        public static void CallRPC(CustomRPC RPC, Action RPCProduce, List<byte> Informations)
        {
            //RPC予約
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.NetId, (byte)RPC, SendOption.Reliable, -1);

            //情報の数だけWriterに刻む(パケット送信)
            foreach (byte Info in Informations)
            {
                Writer.Write(Info);
            }
            //RPC予約終了
            AmongUsClient.Instance.FinishRpcImmediately(Writer);

            //内容を実行！ (例：RPCProduce.SetRole(RoleId.Debugger))
            RPCProduce();
        }
    }
}
