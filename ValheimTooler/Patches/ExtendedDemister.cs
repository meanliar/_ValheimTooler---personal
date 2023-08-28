using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Demister), "OnEnable")]
    public static class Demister_OnEnable_Postfix
    {
        public static void Postfix(ref Demister __instance)
        {
            if (EntitiesItemsHacks.s_extendeddemister && Player.m_localPlayer != null)
            {
                var demister_ = Player.m_localPlayer.GetInventory().GetEquipedtems()
                                        .FirstOrDefault(i => i.m_dropPrefab.name == "Demister");
                var swordmistwalker_ = Player.m_localPlayer.GetInventory().GetEquipedtems()
                                        .FirstOrDefault(i => i.m_dropPrefab.name == "SwordMistwalker");

                if (!__instance.isActiveAndEnabled || (demister_ == null && swordmistwalker_ == null))
                    return;
                __instance.m_forceField.endRange = 100f;
            }
        }
    }
}
