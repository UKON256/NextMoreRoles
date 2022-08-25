using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using NextMoreRoles.Helpers;

namespace NextMoreRoles.Modules.Role
{
    class ScientistVitalShower
    {
        public static void Open(VitalsMinigame MiniGame, string Name, bool DoesPanelsDelete = false)
        {
            //開く
            foreach (RoleBehaviour Role in RoleManager.Instance.AllRoles)
            {
                if (Role.Role == RoleTypes.Scientist)
                {
                    MiniGame = UnityEngine.Object.Instantiate(Role.gameObject.GetComponent<ScientistRole>().VitalsPrefab, Camera.main.transform, false);
                    break;
                }
            }
            MiniGame.transform.SetParent(Camera.main.transform, false);
            MiniGame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
            MiniGame.Begin(null);
            MiniGame.name = Name;

            if (!DoesPanelsDelete) return;

            //パネル削除
            foreach (VitalsPanel Panel in MiniGame.vitals)
            {
                Panel.gameObject.SetActive(false);
            }
        }
    }
}
