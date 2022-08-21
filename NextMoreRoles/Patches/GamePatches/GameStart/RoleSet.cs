using System.Collections.Generic;
using NextMoreRoles.Modules.CustomOptions;
using NextMoreRoles.Roles;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomRPC;
using NextMoreRoles.Modules.DatasManager;
using NextMoreRoles.Helpers;
using UnityEngine;
using Hazel;

namespace NextMoreRoles.Patches.GamePatches.GameStart
{
    static class RoleSet
    {
        public static void SetUpRoles()
        {
            if (!AmongUsClient.Instance.AmHost) return;

            //役職数の最小・最大を代入
            var CrewmateMin = CustomOptions.CrewmateRolesMin.GetInt();
            var CrewmateMax = CustomOptions.CrewmateRolesMax.GetInt();
            var ImpostorMin = CustomOptions.ImpostorRolesMin.GetInt();
            var ImpostorMax = CustomOptions.ImpostorRolesMax.GetInt();
            var NeutralMin = CustomOptions.NeutralRolesMin.GetInt();
            var NeutralMax = CustomOptions.NeutralRolesMax.GetInt();

            //役職数の最小が最大より多ければ最大を最小の値に代入する
            if (CrewmateMin > CrewmateMax) CrewmateMin = CrewmateMax;
            if (ImpostorMin > ImpostorMax) ImpostorMin = ImpostorMax;
            if (NeutralMin > NeutralMax) NeutralMin = NeutralMax;

            //全員にとりあえず役職を付与
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.IsCrew())
                {
                    p.RPCSetRole(RoleId.Crewmate);
                }
                else
                {
                    p.RPCSetRole(RoleId.Impostor);
                }
            }

            //重複をセット
            SetAttributeRole();

            //キャッシュをリセットする
            Reset.ClearAndReloads();
        }


        public static void SetAttributeRole()
        {
            if (CustomOptions.DebuggerOption.GetBool())
            {
                PlayerControl.LocalPlayer.SetRole(RoleId.Debugger);
            }
        }


        //役職の役職説明(タスクリストの奴)を再設定
        public static void RefreshRoleDescription(PlayerControl Target)
        {
            if (Target == null) return;
            List <IntroData> Infos = new() { IntroData.GetIntroData(Target.GetRole(), Target) };

            foreach (IntroData RoleInfo in Infos)
            {
                var Task = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                Task.transform.SetParent(Target.transform, false);
                Task.Text = CustomOptions.cs(RoleInfo.Color, $"{ModTranslation.GetString(RoleInfo.Name)}: {RoleInfo.GameDescription}");

                //重複を持っていたら追記
                if (Target.HasAttribute())
                {
                    var AttributeInfo = IntroData.GetIntroData(Target.GetAttribute(), Target);
                    Task.Text += "\n" + CustomOptions.cs(AttributeInfo.Color, $"{ModTranslation.GetString(AttributeInfo.Name)}: {AttributeInfo.GameDescription}");;
                }

                //役職イントロを先頭にいれる
                Target.myTasks.Insert(0, Task);
            }
        }



        //=====役職をリストに追加してから付与する=====//
        //追加
        public static void SetRole(this PlayerControl Target, RoleId Role)
        {
            switch (Role)
            {
                //=====クルー陣営=====//
                case RoleId.Sheriff:
                    RoleClass.Sheriff.SheriffPlayer.Add(Target);
                    break;

                //=====インポ陣営=====//
                case RoleId.Madmate:
                    RoleClass.Madmate.MadmatePlayer.Add(Target);
                    break;

                case RoleId.SerialKiller:
                    RoleClass.SerialKiller.SerialKillerPlayer.Add(Target);
                    break;

                //=====第三陣営=====//
                case RoleId.Jackal:
                    RoleClass.Jackal.JackalPlayer.Add(Target);
                    break;

                case RoleId.SideKick:
                    RoleClass.SideKick.SideKickPlayer.Add(Target);
                    break;

                //=====重複陣営=====//
                case RoleId.Debugger:
                    RoleClass.Debugger.DebuggerPlayer.Add(Target);
                    break;
            }

            if (Role.IsAttribute_Role())
            {
                ResetRoleCache.ResetAttributeCache();
            }
            else
            {
                ResetRoleCache.ResetRoleChache();
            }

            //現状セットされてる役職と自分が違うかつターゲットのIDと自分のIDが一致すれば役職の説明を更新する
            bool Flag = Target.GetRole() != Role && Target.PlayerId == CachedPlayer.LocalPlayer.PlayerId;
            if (Flag)
            {
                RefreshRoleDescription(PlayerControl.LocalPlayer);
            }

            Logger.Info($"役職が変更されました。PlayerName:{Target.Data.PlayerName} => {Role}", "RoleSet");
        }

        public static PlayerControl ClearTarget;
        //削除
        public static void RemoveRole(this PlayerControl Target)
        {
            static bool ClearRemove(PlayerControl p)
            {
                return p.PlayerId == ClearTarget.PlayerId;
            }
            ClearTarget = Target;

            switch (Target.GetRole())
            {
                //=====クルー陣営=====//
                case RoleId.Sheriff:
                    RoleClass.Sheriff.SheriffPlayer.RemoveAll(ClearRemove);
                    break;

                //=====インポ陣営=====//
                case RoleId.Madmate:
                    RoleClass.Madmate.MadmatePlayer.RemoveAll(ClearRemove);
                    break;

                case RoleId.SerialKiller:
                    RoleClass.SerialKiller.SerialKillerPlayer.RemoveAll(ClearRemove);
                    break;

                //=====第三陣営=====//
                case RoleId.Jackal:
                    RoleClass.Jackal.JackalPlayer.RemoveAll(ClearRemove);
                    break;

                case RoleId.SideKick:
                    RoleClass.SideKick.SideKickPlayer.RemoveAll(ClearRemove);
                    break;

                //=====重複役職=====//
                case RoleId.Debugger:
                    RoleClass.Debugger.DebuggerPlayer.RemoveAll(ClearRemove);
                    break;
            }

            //役職キャッシュ再設定
            ResetRoleCache.ClearAndReloads();
        }



        //SetRoleするRPCを送信する
        public static void RPCSetRole(this PlayerControl Target, RoleId RoleId)
        {
            MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetRole, SendOption.Reliable, -1);
            Writer.Write(Target.PlayerId);
            Writer.Write((byte)RoleId);
            AmongUsClient.Instance.FinishRpcImmediately(Writer);
            RPCProcedure.SetRole(Target.PlayerId, (byte)RoleId);
        }
    }
}
