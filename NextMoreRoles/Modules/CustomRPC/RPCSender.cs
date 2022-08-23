using Hazel;
using System;
using System.Collections.Generic;

namespace NextMoreRoles.Modules.CustomRPC
{
    static class RPCSender
    {
        //public static List<byte> RPCInformations = new();
        //RPCをまとめて送信する
        public static void CallRPC(CustomRPC RPCId, List<byte> Informations)
        {
            try
            {
                //初期化
                //RPCInformations = new();

                //RPC予約
                MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCId, SendOption.Reliable, -1);

                //情報(byte)をすべて書き込む
                foreach (byte Info in Informations)
                {
                    //RPCInformations.Add(Info);
                    Writer.Write(Info);
                }

                //RPCがおわおわおわりーおわおわりー
                AmongUsClient.Instance.FinishRpcImmediately(Writer);

                //ログ
                Logger.Info($"RPCを送信しました。Id:{RPCId}", "RPCSender");
            }
            catch(SystemException Error)
            {
                Logger.Error($"RPCの送信に失敗しました。エラー:{Error}", "RPCSender");
            }
        }
    }
}
