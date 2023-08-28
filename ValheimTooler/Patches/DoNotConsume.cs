using System;
using HarmonyLib;
using UnityEngine;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Inventory), "RemoveOneItem", new Type[] { typeof(ItemDrop.ItemData) })]
    public class Inventory_RemoveOneItem_Patch
    {
        private static bool Prefix(ref Inventory __instance, ref bool __result,
            ItemDrop.ItemData item)
        {
            if (PlayerHacks.s_doNotConsume)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Inventory), "RemoveItem", new Type[] { typeof(ItemDrop.ItemData), typeof (int) })]
    public class Inventory_RemoveItem_Patch
    {
        private static bool Prefix(ref Inventory __instance, ref bool __result,
            ItemDrop.ItemData item, int amount)
        {
            if (PlayerHacks.s_doNotConsume && !string.IsNullOrEmpty(item.m_shared.m_ammoType))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Fireplace), "Interact")]
    public class Fireplace_Interact_Patch
    {
        private static bool Prefix(ref Inventory __instance, ref bool __result,
            Humanoid user, bool hold, bool alt,
            ZNetView ___m_nview, float ___m_maxFuel, ItemDrop ___m_fuelItem)
        {
            if (PlayerHacks.s_doNotConsume && !hold && ___m_nview.HasOwner())
            {
                if ((float)Mathf.CeilToInt(___m_nview.GetZDO().GetFloat("fuel")) >= ___m_maxFuel)
                {
                    user.Message(MessageHud.MessageType.Center, Localization.instance.Localize("$msg_cantaddmore", ___m_fuelItem.m_itemData.m_shared.m_name));
                    __result = false;
                    return false;
                }
                user.Message(MessageHud.MessageType.Center, Localization.instance.Localize("$msg_fireadding", ___m_fuelItem.m_itemData.m_shared.m_name));
                ___m_nview.InvokeRPC("AddFuel");
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Smelter), "OnAddFuel")]
    public class Smelter_OnAddFuel_Patch
    {
        private static bool Prefix(ref Smelter __instance, ref bool __result,
            Switch sw, Humanoid user, ItemDrop.ItemData item,
            ZNetView ___m_nview, int ___m_maxFuel, ItemDrop ___m_fuelItem)
        {
            if (PlayerHacks.s_doNotConsume)
            {
                if ((float)___m_nview.GetZDO().GetFloat("fuel") > (float)(___m_maxFuel - 1))
                {
                    user.Message(MessageHud.MessageType.Center, "$msg_itsfull");
                    __result = false;
                    return false;
                }
                user.Message(MessageHud.MessageType.Center, "$msg_added " + ___m_fuelItem.m_itemData.m_shared.m_name);
                ___m_nview.InvokeRPC("AddFuel");
                __result = true;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Smelter), "OnAddOre")]
    public class Smelter_OnAddOre_Patch
    {
        private static bool Prefix(ref Smelter __instance, ref bool __result, ref float ___m_addedOreTime,
            Switch sw, Humanoid user, ItemDrop.ItemData item,
            ZNetView ___m_nview, int ___m_maxOre, float ___m_addOreAnimationDuration,
            Animator[] ___m_animators)
        {
            if (PlayerHacks.s_doNotConsume && (__instance.name.Contains("charcoal_kiln") || __instance.name.Contains("bathtub")))
            {
                if ((int)___m_nview.GetZDO().GetInt("queued") >= ___m_maxOre)
                {
                    user.Message(MessageHud.MessageType.Center, "$msg_itsfull");
                    __result = false;
                    return false;
                }
                user.Message(MessageHud.MessageType.Center, "$msg_added " + "Wood");
                ___m_nview.InvokeRPC("AddOre", "Wood");
                ___m_addedOreTime = Time.time;
                if (___m_addOreAnimationDuration > 0f)
                {
                    Animator[] animators = ___m_animators;
                    foreach (Animator animator in animators)
                    {
                        if (animator.gameObject.activeInHierarchy)
                        {
                            animator.SetBool("active", true);
                            animator.SetFloat("activef", true ? 1f : 0f);
                        }
                    }
                }
                __result = true;
                return false;
            }
            return true;
        }
    }

}
