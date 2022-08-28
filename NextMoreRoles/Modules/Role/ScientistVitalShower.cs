using UnityEngine;

namespace NextMoreRoles.Modules.Role
{
    class ScientistVitalShower
    {
        public static VitalsMinigame Minigame;
        public static void Open(string Name, bool DoesPanelsDelete = false)
        {
            //開く
            foreach (RoleBehaviour Role in RoleManager.Instance.AllRoles)
            {
                if (Role.Role == RoleTypes.Scientist)
                {
                    Minigame = UnityEngine.Object.Instantiate(Role.gameObject.GetComponent<ScientistRole>().VitalsPrefab, Camera.main.transform, false);
                    break;
                }
            }
            Minigame.transform.SetParent(Camera.main.transform, false);
            Minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
            Minigame.Begin(null);
            Minigame.name = Name;

            if (!DoesPanelsDelete) return;

            //パネル削除
            foreach (VitalsPanel Panel in Minigame.vitals)
            {
                Panel.gameObject.SetActive(false);
            }
        }
    }
}
