using Hazel;

namespace NextMoreRoles.Modules.CustomRPC
{
    public static class RPCHelper
    {
        public static MessageWriter StartRPC(RpcCalls RPCId, PlayerControl SendTarget = null)
        {
            return StartRPC(PlayerControl.LocalPlayer.NetId, (byte)RPCId, SendTarget);
        }
        public static MessageWriter StartRPC(uint NetId, RpcCalls RPCId, PlayerControl SendTarget = null)
        {
            return StartRPC(NetId, (byte)RPCId, SendTarget);
        }
        public static MessageWriter StartRPC(CustomRPC RPCId, PlayerControl SendTarget = null)
        {
            return StartRPC(PlayerControl.LocalPlayer.NetId, (byte)RPCId, SendTarget);
        }
        public static MessageWriter StartRPC(uint NetId, CustomRPC RPCId, PlayerControl SendTarget = null)
        {
            return StartRPC(NetId, (byte)RPCId, SendTarget);
        }
        public static MessageWriter StartRPC(byte RPCId, PlayerControl SendTarget = null)
        {
            return StartRPC(PlayerControl.LocalPlayer.NetId, (byte)RPCId, SendTarget);
        }
        public static MessageWriter StartRPC(uint NetId, byte RPCId, PlayerControl SendTarget = null)
        {
            var target = SendTarget != null ? SendTarget.GetClientId() : -1;
            return AmongUsClient.Instance.StartRpcImmediately(NetId, RPCId, Hazel.SendOption.Reliable, target);
        }
        public static void EndRPC(this MessageWriter Writer)
        {
            AmongUsClient.Instance.FinishRpcImmediately(Writer);
        }
        public static void RPCGameOptionsPrivate(GameOptionsData Data, PlayerControl target)
        {
            MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)2, Hazel.SendOption.None, target.GetClientId());
            messageWriter.WriteBytesAndSize(Data.ToBytes((byte)5));
            messageWriter.EndMessage();
        }
    }
}
