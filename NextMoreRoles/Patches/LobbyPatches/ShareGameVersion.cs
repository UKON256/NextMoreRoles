using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using NextMoreRoles.Modules.CustomRPC;
using NextMoreRoles.Modules;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Patches.LobbyPatches
{
    class ShareGameVersion
    {
        public static bool IsVersionOK = false;
        public static bool IsChangeVersion = false;
        public static bool IsRPCSend = false;
        public static float timer = 600;
        public static float RPCTimer = 1f;

        //実行元:HarmonyPatches.AmongUsClient.cs
        public class AmongUsClientOnPlayerJoinedPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.LocalPlayer != null)
                {
                    try
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareMODVersion, Hazel.SendOption.Reliable, -1);
                        writer.Write((byte)NextMoreRolesPlugin.Version.Major);
                        writer.Write((byte)NextMoreRolesPlugin.Version.Minor);
                        writer.Write((byte)NextMoreRolesPlugin.Version.Build);
                        writer.WritePacked(AmongUsClient.Instance.ClientId);
                        writer.Write((byte)(NextMoreRolesPlugin.Version.Revision < 0 ? 0xFF : NextMoreRolesPlugin.Version.Revision));
                        writer.Write(Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.ToByteArray());
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.ShareMODVersion(NextMoreRolesPlugin.Version.Major, NextMoreRolesPlugin.Version.Minor, NextMoreRolesPlugin.Version.Build, NextMoreRolesPlugin.Version.Revision, Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId, AmongUsClient.Instance.ClientId);
                        NextMoreRolesPlugin.Logger.LogInfo("バージョンシェアに成功しました。");
                    }
                    catch(SystemException Error)
                    {
                        NextMoreRolesPlugin.Logger.LogError("バージョンシェアに失敗しました。エラー:"+Error);
                    }
                }
            }
        }

        //実行元:HarmonyPatches.GameStartManager.cs
        public class GameStartManagerStartPatch
        {
            public static void Postfix()
            {
                timer = 600f;
                RPCTimer = 1f;
                /*GameStartManagerUpdatePatch.Proce = 0;
                GameStartManagerUpdatePatch.LastBlockStart = false;
                GameStartManagerUpdatePatch.VersionPlayers = new Dictionary<int, PlayerVersion>();*/
            }
        }

        public class GameStartManagerUpdatePatch
        {
            private static bool update = false;
            public static Dictionary<int, PlayerVersion> VersionPlayers = new();
            public static int Proce;
            private static string currentText = "";
            public static bool LastBlockStart;

        }
    }

    public class PlayerVersion
    {
        public readonly Version version;
        public readonly Guid guid;

        public PlayerVersion(Version version, Guid guid)
        {
            this.version = version;
            this.guid = guid;
        }

        public bool GuidMatches()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.Equals(this.guid);
        }
    }
}
