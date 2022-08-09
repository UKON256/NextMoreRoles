using UnityEngine;
using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using NextMoreRoles.Modules;
using NextMoreRoles.Modules.CustomOptions;

namespace NextMoreRoles.Roles
{
    public static class RoleBase
    {
        public static Color CrewmateWhite = Palette.White;
        public static Color CrewmateBlue = Palette.CrewmateBlue;
        public static Color ImpostorRed = Palette.ImpostorRed;
    }


    [HarmonyPatch]
    public abstract class RoleBase<T> : Role where T : RoleBase<T>, new()
    {
        public static List<T> Players = new List<T>();
        public static RoleType RoleType;

        public void Init(PlayerControl Player)
        {
            this.Player = Player;
            Players.Add((T)this);
            AllRoles.Add(this);
            PostInit();
        }

        public static T local
        {
            get
            {
                return Players.FirstOrDefault(x => x.Player == PlayerControl.LocalPlayer);
            }
        }

        public static List<PlayerControl> AllPlayers
        {
            get
            {
                return Players.Select(x => x.Player).ToList();
            }
        }

        public static List<PlayerControl> LivingPlayers
        {
            get
            {
                return Players.Select(x => x.Player).Where(x => x.IsAlive()).ToList();
            }
        }

        public static List<PlayerControl> DeadPlayers
        {
            get
            {
                return Players.Select(x => x.Player).Where(x => !x.IsAlive()).ToList();
            }
        }

        public static bool Exists
        {
            get { return Players.Count > 0; }
        }

        public static T GetRole(PlayerControl Player = null)
        {
            Player = Player ?? PlayerControl.LocalPlayer;
            return Players.FirstOrDefault(x => x.Player == Player);
        }

        public static bool IsRole(PlayerControl Player)
        {
            return Players.Any(x => x.Player == Player);
        }

        public static T SetRole(PlayerControl Player)
        {
            if (!IsRole(Player))
            {
                T role = new T();
                role.Init(Player);
                return role;
            }
            return null;
        }

        public static void EraseRole(PlayerControl Player)
        {
            Players.DoIf(x => x.Player == Player, x => x.ResetRole());
            Players.RemoveAll(x => x.Player == Player && x.RoleId == RoleType);
            AllRoles.RemoveAll(x => x.Player == Player && x.RoleId == RoleType);
        }

        public static void SwapRole(PlayerControl p1, PlayerControl p2)
        {
            var index = Players.FindIndex(x => x.Player == p1);
            if (index >= 0)
            {
                Players[index].Player = p2;
            }
        }
    }



    public abstract class Role
    {
        public static List<Role> AllRoles = new();
        public PlayerControl Player;
        public RoleType RoleId;

        public abstract void OnMeetingStart();
        public abstract void OnMeetingEnd();
        public abstract void FixedUpdate();
        public abstract void OnKill(PlayerControl Target);
        public abstract void OnDeath(PlayerControl Killer = null);
        public abstract void HandleDisconnect(PlayerControl Player, DisconnectReasons Reason);
        public virtual void ResetRole() { }
        public virtual void PostInit() { }
        public virtual string ModifyNameText(string nameText) { return nameText; }
        public virtual string MeetingInfoText() { return ""; }

        public static void ClearAll()
        {
            AllRoles = new();
        }
    }
}
