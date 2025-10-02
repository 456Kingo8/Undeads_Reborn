using ai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Undeads.Code
{
    class Undead_Action
    {
        public static bool turn_into_Undeads(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            Actor a = pTarget.a;
            if (a == null)
            {
                return false;
            }
            if (!a.inMapBorder())
            {
                return false;
            }
            if (a.isAlreadyTransformed())
            {
                return false;
            }
            bool flag = false;
            if (!string.IsNullOrEmpty(a.asset.skeleton_id))
            {
                string skeleton_id = a.asset.skeleton_id;
                a.finishStatusEffect("cursed");
                a.removeTrait("infected");
                a.removeTrait("mush_spores");
                a.removeTrait("tumor_infection");
                Subspecies subspecies = null;
                if (a.hasSubspecies())
                {
                    subspecies = a.subspecies.getSkeletonForm();
                }

                Actor actor = World.world.units.createNewUnit(skeleton_id, a.current_tile, pMiracleSpawn: false, 0f, subspecies, null, pSpawnWithItems: false);
                Subspecies subspecies2 = actor.subspecies;
                if (subspecies2.isJustCreated())
                {
                    subspecies?.setSkeletonForm(subspecies2);
                }

                ActorTool.copyUnitToOtherUnit(a, actor);
                if (!a.getName().StartsWith("Un"))
                {
                    actor.setName("Un" + Toolbox.LowerCaseFirst(a.getName()));
                }
                flag = true;
            }

            if (pTarget.a.asset.has_soul)
            {
                Actor tGhost = World.world.units.createNewUnit("ghost", pTile, false, 0f, null, null, true, false, false, false);
                tGhost.removeTrait("blessed");
                ActorTool.copyUnitToOtherUnit(pTarget.a, tGhost, true);
                flag = true;
            }
            if(pTarget.a.asset.can_turn_into_zombie)
            {
                a.finishStatusEffect("cursed");
                a.removeTrait("infected");
                a.removeTrait("mush_spores");
                a.removeTrait("tumor_infection");
                string zombieID = a.asset.getZombieID();
                if (a.asset.id == "dragon")
                {
                    a.removeTrait("fire_blood");
                    a.removeTrait("fire_proof");
                }

                Actor actor = World.world.units.createNewUnit(zombieID, a.current_tile, pMiracleSpawn: false, 0f, null, a.subspecies, pSpawnWithItems: false);
                ActorTool.copyUnitToOtherUnit(a, actor);
                actor.removeTrait("fast");
                actor.removeTrait("agile");
                actor.removeTrait("genius");
                actor.removeTrait("peaceful");
                if (!a.getName().StartsWith("Un"))
                {
                    actor.setName("Un" + Toolbox.LowerCaseFirst(a.getName()));
                }
                flag = true;
            }
            if(flag)
            {
                EffectsLibrary.spawn("fx_spawn", pTile);
                a.setTransformed();
            }
            return true;
        }
        
        public static bool whisper_of_death_Action(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if(pTarget.a.isAlive())
            {
                pTarget.a.data.health = Mathf.Max(0, pTarget.a.data.health - Mathf.Max((int)(pTarget.a.getMaxHealth()* 0.04),2));
                if (pTarget.a.data.health == 0)
                {
                    turn_into_Undeads(pTarget, pTile);
                    pTarget.a.die(true, AttackType.Plague);
                    
                }
            }
            return true;
        }

        public static bool LichLord_attack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(pSelf.a.isAlive())
            {
                foreach(Actor act in Finder.getUnitsFromChunk(pTile, 2, 10))
                {
                    if (act.kingdom.asset.id != "undead")
                    {
                        act.addStatusEffect("whisper_of_death");
                        act.data.health = Mathf.Max(0, act.data.health - Mathf.Max((int)(act.getMaxHealth() * 0.1), 20));
                        if (act.data.health == 0)
                        {
                            turn_into_Undeads(act, pTile);
                            act.die(true, AttackType.Plague);
                        }
                    }
                }
            }
            return true;
        }
    }
}
