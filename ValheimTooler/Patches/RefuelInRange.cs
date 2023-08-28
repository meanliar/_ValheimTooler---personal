using HarmonyLib;
using UnityEngine;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{

    public class RefuelInRange_vars
    {
        public static bool refueller = false;

    }

    [HarmonyPatch(typeof(Fireplace), "Interact")]
    public class Fireplace_Interact_Postfix
    {
        private static void Postfix()
        {
            if (EntitiesItemsHacks.s_refuelallinrange && Camera.main != null)
            {
                Fireplace[] fireplaces = UnityEngine.Object.FindObjectsOfType<Fireplace>();
                foreach (Fireplace fireplace_ in fireplaces)
                {
                    var distance = Vector3.Distance(Camera.main.transform.position, fireplace_.transform.position);
                    if (distance < 51)
                    {
                        fireplace_.GetComponent<ZNetView>().GetZDO().Set("fuel", fireplace_.m_maxFuel);
                    }
                }

                Smelter[] smelters = UnityEngine.Object.FindObjectsOfType<Smelter>();
                foreach (Smelter smelter_ in smelters)
                {
                    var distance = Vector3.Distance(Camera.main.transform.position, smelter_.transform.position);
                    if (distance < 51 && smelter_.name.Contains("bathtub"))
                    {
                        float currentfuel = smelter_.GetComponent<ZNetView>().GetZDO().GetFloat("fuel");
                        if (currentfuel < smelter_.m_maxFuel)
                        {
                            RefuelInRange_vars.refueller = true;
                            smelter_.GetComponent<ZNetView>().InvokeRPC("AddFuel");
                            RefuelInRange_vars.refueller = false;
                        }
                    }
                }

                Turret[] turrets = UnityEngine.Object.FindObjectsOfType<Turret>();
                foreach (Turret turret_ in turrets)
                {
                    var distance = Vector3.Distance(Camera.main.transform.position, turret_.transform.position);
                    if (distance < 51)
                    {
                        if (turret_.GetAmmo() < turret_.m_maxAmmo && turret_.GetAmmo() > 0)
                        {
                            RefuelInRange_vars.refueller = true;
                            turret_.GetComponent<ZNetView>().GetZDO().Set("ammo", turret_.m_maxAmmo);
                            RefuelInRange_vars.refueller = false;
                        }
                    }
                }

            }
        }
    }

    [HarmonyPatch(typeof(Turret), "UseItem")]
    public class Turret_UseItem_Postfix
    {
        private static void Postfix()
        {
            if (EntitiesItemsHacks.s_refuelallinrange && Camera.main != null)
            {
                Fireplace[] fireplaces = UnityEngine.Object.FindObjectsOfType<Fireplace>();
                foreach (Fireplace fireplace_ in fireplaces)
                {
                    var distance = Vector3.Distance(Camera.main.transform.position, fireplace_.transform.position);
                    if (distance < 51)
                    {
                        fireplace_.GetComponent<ZNetView>().GetZDO().Set("fuel", fireplace_.m_maxFuel);
                    }
                }

                Smelter[] smelters = UnityEngine.Object.FindObjectsOfType<Smelter>();
                foreach (Smelter smelter_ in smelters)
                {
                    var distance = Vector3.Distance(Camera.main.transform.position, smelter_.transform.position);
                    if (distance < 51 && smelter_.name.Contains("bathtub"))
                    {
                        float currentfuel = smelter_.GetComponent<ZNetView>().GetZDO().GetFloat("fuel");
                        if (currentfuel < smelter_.m_maxFuel)
                        {
                            RefuelInRange_vars.refueller = true;
                            smelter_.GetComponent<ZNetView>().InvokeRPC("AddFuel");
                            RefuelInRange_vars.refueller = false;
                        }
                    }
                }

                Turret[] turrets = UnityEngine.Object.FindObjectsOfType<Turret>();
                foreach (Turret turret_ in turrets)
                {
                    var distance = Vector3.Distance(Camera.main.transform.position, turret_.transform.position);
                    if (distance < 51)
                    {
                        if (turret_.GetAmmo() < turret_.m_maxAmmo && turret_.GetAmmo() > 0)
                        {
                            turret_.GetComponent<ZNetView>().GetZDO().Set("ammo", turret_.m_maxAmmo);
                        }
                    }
                }

            }
        }
    }

    [HarmonyPatch(typeof(Smelter), "SetFuel")]
    public class Smelter_SetFuel_Prefix
    {
        static void Smelter_SetFuel(Smelter __instance, ref float fuel)
        {
            if (RefuelInRange_vars.refueller)
                fuel = __instance.m_maxFuel;
        }
    }


}
