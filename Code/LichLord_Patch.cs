using ai.behaviours;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
    class LichLord_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BehTrySleep), "execute")]
        public static bool BehTrySleep_execute(BehTrySleep __instance, ref Actor pActor, ref BehResult __result)
        {
            if (pActor.hasTrait("LichLord"))
            {
                __result = BehResult.Continue;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "addAggro", new Type[] { typeof(Actor) })]
        public static bool Actor_addAggro(Actor __instance, ref Actor pActor)
        {
            if (pActor.isRekt())
            {
                return false;
            }
            if (pActor == __instance)
            {
                return false;
            }
            if (__instance.hasTrait("LichLord") || pActor.hasTrait("LichLord"))
            {
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "addForce")]
        public static bool Actor_addForce(Actor __instance)
        {
            if (__instance.hasTrait("LichLord")) return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "makeStunned")]
        public static bool Actor_makeStunned(Actor __instance)
        {
            if (__instance.hasTrait("LichLord")) return false;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CombatActionLibrary), "tryToCastSpell")]
        public static void CombatActionLibrary_tryToCastSpell(CombatActionLibrary __instance, ref AttackData pData)
        {
            if (pData.initiator.a.hasTrait("LichLord"))
            {
                pData.initiator.a._active_status_dict.TryGetValue("recovery_spell", out var value);
                if(value != null)value.finish();
            }
        }
    }
}
