using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Undeads.Code
{
    class Patches
    {
        public static void init()
        {
            Harmony.CreateAndPatchAll(typeof(Patches));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "die")]
        public static bool Actor_die(Actor __instance,ref AttackType pType,ref bool pCountDeath,ref bool pLogFavorite)
        {
            if (__instance.hasStatus("Undead_Battle_Continue") && !__instance.hasTrait("death_mark")) return false;
            if(__instance.hasReligion() && __instance.religion.has_Undead_Trait(SUndead.Undead_Phrase_1_special,1) && !__instance.hasTrait("death_mark"))
            {
                __instance.data.health = 1;
                __instance._alive = true;
                __instance.addStatusEffect("Undead_Battle_Continue");
                return false;
            }

            if (__instance.isAlive() && __instance.hasTrait("LichLord") && __instance.data.health == 0)
            {
                BiomeAsset biomeAsset = AssetManager.biome_library.get("biome_corrupted");
                WorldTile worldTile = null;
                if (biomeAsset.getTileHigh().hashset.Count > 0)
                {
                    worldTile = biomeAsset.getTileHigh().hashset.GetRandom();
                }
                else if (biomeAsset.getTileLow().hashset.Count > 0)
                {
                    worldTile = biomeAsset.getTileLow().hashset.GetRandom();
                }
                if (worldTile == null) return true;

                ActionLibrary.teleportEffect(__instance, worldTile);
                __instance.cancelAllBeh();
                __instance.spawnOn(worldTile);
                __instance.setHealth(1);//生命强制变为1，防止进入无敌状态
                __instance.makeStunned(2f);//提供2秒眩晕
                World.world.StartCoroutine(Undead_Action.Spread_Biome(__instance, "biome_grass", 8,0.2f,true,restore));
                return false;

                bool restore(BaseSimObject pTarget, WorldTile pTile = null)
                {
                    pTarget.a.restoreHealthPercent(0.01f);
                    __instance.changeMana((int)(__instance.getMaxMana() * -0.01f));
                    return true;
                }
            }

            if(__instance.hasReligion() && __instance.religion.has_Undead_Trait(SUndead.Undead_Phrase_1_soul,1))
            {
                if(__instance.city!= null && __instance.city.storages.Count > 0)
                {
                    __instance.city.storages.GetRandom()?.addResources("Undead_Soul_Pieces", 1);
                }  
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "pickupResourcesFromKill")]
        public static void Actor_pickupResourcesFromKill(Actor pAttacker)
        {
            if(pAttacker.religion == null) return;
            if (pAttacker.religion.has_Undead_Trait(SUndead.Undead_Phrase_2_soul,2) && pAttacker.city != null && pAttacker.city.storages.Count > 0)
            {
                pAttacker.city.storages.GetRandom()?.addResources("Undead_Soul_Pieces", 1);
            }
            return;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), "spawnGhost")]
        public static bool ActionLibrary_spawnGhost(ref BaseSimObject pTarget)
        {
            if (pTarget.isActor() && pTarget.a.hasReligion() && pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_2_corrupt, 2))
            {
                return false;
            }
            if (pTarget.isActor() && pTarget.a.asset.id.Contains("zombie")&&pTarget.a.kingdom.isCiv())
            {
                return false;
            }
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), "giveCursed")]
        public static bool ActionLibrary_giveCursed(ref BaseSimObject pActor)
        {
            if (pActor.a.hasReligion() && pActor.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_2_corrupt, 2))
            {
                return false;
            }
            return true;
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(ActionLibrary), "giveCursed")]
        //public static bool ActionLibrary_giveCursed(ref BaseSimObject pActor)
        //{
        //    if (pActor.a.hasReligion() && pActor.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_3_corrupt, 3))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}
