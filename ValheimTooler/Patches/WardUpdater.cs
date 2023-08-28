using HarmonyLib;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{

    [HarmonyPatch(typeof(PrivateArea), "AddPermitted")]
    public class PrivateArea_AddPermitted_Prefix
    {
        private static void Prefix(ref long playerID, ref string playerName)
        {
            if (EntitiesItemsHacks.s_wardupdateractive && Player.m_localPlayer != null)
            {
                playerID = EntitiesItemsHacks.player_id;
                playerName = EntitiesItemsHacks.player_name;
            }
        }
    }

}
