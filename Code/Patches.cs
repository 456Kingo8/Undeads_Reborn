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
        public static bool Actor_die(Actor __instance)
        {
            if (__instance.isAlive() && __instance.hasTrait("LichLord"))
            {
                MonoBehaviour.print("debug1");
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
                __instance.makeStunned(2f);//提供1秒眩晕
                World.world.StartCoroutine(Undead_Action.Spread_Biome(__instance, "biome_grass", 8,0.2f,true,restore));
                return false;

                bool restore(BaseSimObject pTarget, WorldTile pTile = null)
                {
                    pTarget.a.restoreHealthPercent(0.01f);
                    __instance.changeMana((int)(__instance.getMaxMana() * -0.01f));
                    return true;
                }
            }
            return true;
        }
    }
}
