using HarmonyLib;
using UnityEngine;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{

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
            }
        }
    }
}
