using HarmonyLib;
using UnityEngine;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{

    [HarmonyPatch(typeof(Player), "Repair")]
    class Player_Repair_Postfix_Patch
    {
        public static int destroyMask = LayerMask.GetMask(new string[]
        {
            "Default",
            "static_solid",
            "Default_small",
            "piece",
            "piece_nonsolid",
            "terrain",
            "vehicle"
        });

        private static void Postfix(ItemDrop.ItemData toolItem, Piece repairPiece)
        {
            if (EntitiesItemsHacks.s_repairallinrange && Player.m_localPlayer != null)
            {
                Collider[] array = Physics.OverlapSphere(Player.m_localPlayer.transform.position, 50f, destroyMask);
                for (int i = 0; i < array.Length; i++)
                {
                    Piece piece = array[i].GetComponentInParent<Piece>();
                    if (piece)
                    {
                        if (!piece.IsCreator())
                        {
                            continue;
                        }
                        if (!Traverse.Create(Player.m_localPlayer).Method("CheckCanRemovePiece", new object[] { piece }).GetValue<bool>())
                        {
                            continue;
                        }
                        ZNetView component = piece.GetComponent<ZNetView>();
                        if (component == null)
                        {
                            continue;
                        }
                        WearNTear wnt = piece.GetComponent<WearNTear>();
                        if (!wnt || !wnt.Repair())
                            continue;
                    }
                }
            }
        }
    }
}
