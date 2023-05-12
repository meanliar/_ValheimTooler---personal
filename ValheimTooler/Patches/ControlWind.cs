using HarmonyLib;
using ValheimTooler.Core;

namespace ValheimTooler.Patches
{
    [HarmonyPatch(typeof(Ship), "IsWindControllActive")]
    class WindControl
    {
        private static bool Prefix(ref Ship __instance, ref bool __result)
        {
            if (PlayerHacks.s_controlWind)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
