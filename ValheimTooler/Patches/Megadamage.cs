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
    [HarmonyPatch(typeof(HitData), "SetAttacker")]
    class HitData_SetAttacker_Postfix
    {
        private static void Postfix(Character attacker,
            ref HitData __instance)
        {
            if (PlayerHacks.s_megadamage && attacker.IsPlayer() && ((Player)attacker).GetPlayerID() == Player.m_localPlayer.GetPlayerID())
            {
                __instance.m_damage.Modify(100);
            }
        }
    }
}
