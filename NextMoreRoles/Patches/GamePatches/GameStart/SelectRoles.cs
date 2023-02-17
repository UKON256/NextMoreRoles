using System.Net.NetworkInformation;
using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using NextMoreRoles.Roles;
using NextMoreRoles.Helpers;
using NextMoreRoles.Modules.CustomRPC;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Patches.GamePatches.GameStart;

[HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
class SelectRoles
{
    private static int CrewmateMin;
    private static int CrewmateMax;
    private static int ImpostorMin;
    private static int ImpostorMax;
    private static int NeutralMin;
    private static int NeutralMax;
    private static List<PlayerControl> CrewmatePlayers = new();
    private static List<PlayerControl> ImpostorPlayers = new();
    static void Postfix()
    {
        Logger.Info("==============================RoleSet開始==============================", "SelectRoles");
        if (AmongUsClient.Instance.AmHost) {
            // 最小数が最大数を超えていた場合、最小値を最大値と一緒にする
            if (CrewmateMin > CrewmateMax) CrewmateMin = CrewmateMax;
            if (ImpostorMin > ImpostorMax) ImpostorMin = ImpostorMax;
            if (NeutralMin > NeutralMax) NeutralMin = NeutralMax;

            CrewmateMin = CustomOptionHolder.CrewmateRolesMin.GetInt();
            CrewmateMax = CustomOptionHolder.CrewmateRolesMin.GetInt();
            ImpostorMin = CustomOptionHolder.ImpostorRolesMin.GetInt();
            ImpostorMax = CustomOptionHolder.ImpostorRolesMin.GetInt();
            NeutralMin = CustomOptionHolder.NeutralRolesMin.GetInt();
            NeutralMax = CustomOptionHolder.NeutralRolesMin.GetInt();

            CrewmatePlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.Role.IsImpostor).ToList();
            ImpostorPlayers = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.Role.IsImpostor).ToList();

            SelectCrewmateRoles();
            SelectImpostorRoles();
            SelectNeutralRoles();
            SelectCombinationRoles();

            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                p.RPCSetRole(RoleId.Jackal);
            }
        }
        Logger.Info("==============================RoleSet終了==============================", "SelectRoles");
    }



    //* クルーメイトの選定 *//
    private static void SelectCrewmateRoles()
    {
        if (CrewmateMax == 0) return;

        int RoleRange = UnityEngine.Random.Range(CrewmateMin, CrewmateMax);
    }


    //* インポスターの選定 *//
    private static void SelectImpostorRoles()
    {
        if (ImpostorMax == 0) return;

        int RoleRange = UnityEngine.Random.Range(ImpostorMin, ImpostorMax);
    }


    //* 第三陣営の選定*//
    private static void SelectNeutralRoles()
    {
        if (NeutralMax == 0) return;

        int RoleRange = UnityEngine.Random.Range(NeutralMin, NeutralMax);
    }

    //* コンビネーションの選定 *//
    private static void SelectCombinationRoles()
    {

    }
}
