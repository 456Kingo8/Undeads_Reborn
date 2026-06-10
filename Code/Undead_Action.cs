using ai;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace Undeads.Code
{
    class Undead_Action
    {

        public static List<string> zombie_id = new List<string>() { "zombie_dragon", "zombie_human", "zombie_elf", "zombie_orc", "zombie_dwarf", "zombie_animal_fox", "zombie_animal_buffalo", "zombie_animal_hyena", "zombie_animal_crocodile", "zombie_animal_monkey", "zombie_animal_rhino", "zombie_animal_frog", "zombie_animal_snake", "zombie_animal_dog", "zombie_animal_wolf", "zombie_animal_bear", "zombie_animal_piranha", "zombie_animal_rabbit", "zombie_animal_cat", "zombie_animal_raccoon", "zombie_animal_seal", "zombie_animal_ostrich", "zombie_animal_unicorn", "zombie_animal_rat", "zombie_animal_chicken", "zombie_animal_sheep", "zombie_animal_cow", "zombie_animal_penguin", "zombie_animal_armadillo", "zombie_animal_alpaca", "zombie_animal_capybara", "zombie_animal_goat", "zombie_animal_scorpion", "zombie_animal_turtle", "zombie_animal_crab", "zombie_animal_crystal_sword", "zombie_animal_smore", "zombie_animal_acid_blob", "zombie_animal_flower_bud", "zombie_animal_lemon_snail", "zombie_animal_garl", "zombie_bee", "zombie_fly", "zombie_butterfly", "zombie_grasshopper", "zombie_beetle", "zombie_cold_one", "zombie_necromancer", "zombie_druid", "zombie_plague_doctor", "zombie_white_mage", "zombie_evil_mage", "zombie_demon", "zombie_animal_fairy", "zombie_bandit", "zombie_alien", "zombie_civ_cat", "zombie_civ_dog", "zombie_civ_chicken", "zombie_civ_rabbit", "zombie_civ_monkey", "zombie_civ_fox", "zombie_civ_sheep", "zombie_civ_cow", "zombie_civ_armadillo", "zombie_civ_wolf", "zombie_civ_bear", "zombie_civ_rhino", "zombie_civ_buffalo", "zombie_civ_hyena", "zombie_civ_rat", "zombie_civ_alpaca", "zombie_civ_capybara", "zombie_civ_goat", "zombie_civ_scorpion", "zombie_civ_crab", "zombie_civ_penguin", "zombie_civ_turtle", "zombie_civ_crocodile", "zombie_civ_snake", "zombie_civ_frog", "zombie_civ_piranha", "zombie_civ_liliar", "zombie_civ_garlic_man", "zombie_civ_lemon_man", "zombie_civ_acid_gentleman", "zombie_civ_crystal_golem", "zombie_civ_candy_man", "zombie_civ_beetle", "zombie_civ_seal", "zombie_civ_unicorn", "zombie_greg" };
        public static void init()
        {
            foreach(ActorAsset asset in AssetManager.actor_library.list)
            {
                if(asset.id.Contains("zombie"))
                {
                    zombie_id.Add(asset.id);
                }
            }
            //MonoBehaviour.print(zombie_id.ToJson());
        }
        public static bool turn_into_Undeads(BaseSimObject pTarget = null, WorldTile pTile = null,BaseSimObject pFrom = null)
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
                if(pFrom != null)
                {
                    actor.kingdom = pFrom.kingdom;
                }
                flag = true;
            }

            if (pTarget.a.asset.has_soul)
            {
                Actor tGhost = World.world.units.createNewUnit("ghost", pTile, false, 0f, null, null, true, false, false, false);
                tGhost.removeTrait("blessed");
                ActorTool.copyUnitToOtherUnit(pTarget.a, tGhost, true);
                if (pFrom != null)
                {
                    tGhost.kingdom = pFrom.kingdom;
                }
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
                if (pFrom != null)
                {
                    actor.kingdom = pFrom.kingdom;
                }
                flag = true;
            }
            if(flag)
            {
                EffectsLibrary.spawn("fx_spawn", a.current_tile);
                a.setTransformed();
            }
            return flag;
        }
        public static bool whisper_of_death_Action_death(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            pTarget.a._active_status_dict.TryGetValue("whisper_of_death", out Status value);
            if (value != null)
            {
                StatusExtend ext = value.GetExtend();
                if (ext != null)
                {
                    turn_into_Undeads(pTarget, pTile, ext.pFrom);
                }
            }
            return true;
        }

        public static bool whisper_of_death_Action(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            pTarget.a._active_status_dict.TryGetValue("whisper_of_death", out Status value);
            StatusExtend ext = value?.GetExtend();
            if(ext != null && ext.pFrom != null && ext.pFrom.kingdom == pTarget.kingdom)
            {
                return false;
            }
            pTarget.a.data.health = Mathf.Max(0, pTarget.a.data.health - Mathf.Max((int)(pTarget.a.getMaxHealth() * 0.04), 2));
            if (pTarget.a.data.health == 0)
            {
                if (value != null && ext != null)
                {
                    bool flag = turn_into_Undeads(pTarget, pTile, ext.pFrom);
                    pTarget.a.die(flag, AttackType.Plague);
                }
                pTarget.a.die(false, AttackType.Plague);
            }
            return true;
        }

        public static bool LichLord_attack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(pTarget != null && pTarget.isActor() && pTarget.a.data.health <= 0) turn_into_Undeads(pTarget.a, pTile, pSelf);
            if (pSelf.a.isAlive() && pTarget != null)
            {
                foreach(Actor act in Finder.getUnitsFromChunk(pTarget.current_tile, 2, 10))
                {
                    if (act.kingdom != pSelf.a.kingdom && !act.hasTag("Undead"))
                    {
                        
                        act.data.health = Mathf.Max(0, act.data.health - Mathf.Max((int)(act.getMaxHealth() * 0.1), 20));
                        if (act.data.health == 0)
                        {
                            bool flag = turn_into_Undeads(act, pTile,pSelf);
                            act.die(flag, AttackType.Plague);
                            return true;
                        }
                        act.addStatusEffect("whisper_of_death",pSelf.a);
                    }
                }
            }
            return true;
        }

        public static bool LichLord_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            pTarget.a.restoreHealthPercent(0.05f);
            pTarget.a.restoreManaPercent(0.02f);
            return LichLord_attack(pTarget,null,pTile);
        }


        public static bool summon_undead(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Spread_Biome(pSelf, "biome_corrupted",8,0.1f,true,summon));
            return true;

            bool summon(BaseSimObject pTarget, WorldTile pTile = null)
            {
                if(Randy.randomChance(0.4f))
                {
                    ActionLibrary.spawnSkeleton(pTarget, pTile);
                }
                else if(Randy.randomChance(0.33f))
                {
                    World.world.units.createNewUnit(zombie_id.GetRandom(), pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                    EffectsLibrary.spawn("fx_spawn", pTile);
                }
                return true;
            }
        }

        public static bool speard_curse_biome(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Undead_Action.Spread_Biome(pSelf, "biome_corrupted", 10));
            return true;
        }
        /// <summary>
        /// 一个基于协程的宽度优先搜索，用于批量修改群系
        /// <example>
        /// <para>例如:</para>
        ///     <code>
        ///         World.world.StartCoroutine(Spread_Biome(pTarget:actor, biome_id:"biome_corrupted",range:8 ,delay_time:0.5f,action:testAction));
        ///         
        ///         void testAction(BaseSimObject pTarget, WorldTile pTile = null)
        ///         {
        ///             return;
        ///         }
        ///     </code>
        ///     可在生物actor所在格子为起始点，产生哈夫曼距离8格的诅咒之地扩散，每次扩散间隔0.5秒，
        ///     <para>每扩散一格进行一次<c>testAction</c>调用</para>
        ///     <para><c>testAction</c>参数中  <c>pTarget</c>为<c>actor</c>,<c>pTile</c>为对应格子</para>
        /// </example>
        /// </summary>
        /// <param name="pTarget">一个生物，对应群系扩散的起始格子</param>
        /// <param name="biome_id">所需扩散的群系</param>
        /// <param name="range">扩散范围(哈夫曼距离)</param>
        /// <param name="delay_time">每次扩散之间延迟时间</param>
        /// <param name="overlay">是否允许群系重叠扩散</param>
        /// <param name="action">自定义<c>WorldAction</c>,每次扩散时在所扩散的格子调用</param>
        /// <returns></returns>

        public static IEnumerator Spread_Biome(BaseSimObject pTarget,string biome_id,int range,float delay_time = 1f,bool overlay = false,WorldAction action = null)
        {
            BiomeAsset biome = AssetManager.biome_library.get(biome_id);
            if (pTarget == null || !pTarget.current_tile.Type.can_be_biome) yield break;
            WorldTile tile = pTarget.current_tile;
            TopTileType toptile,high,low;
            if (tile.top_type == null || tile.top_type.id != biome.tile_high || tile.top_type.id != biome.tile_low || overlay)
            {
                high = AssetManager.top_tiles.get(biome.tile_high);
                low = AssetManager.top_tiles.get(biome.tile_low);
                Queue<Tuple<WorldTile,int>> q = new();
                Dictionary<WorldTile, bool> dict = new();
                q.Enqueue(new Tuple<WorldTile, int>(tile,0));
                dict.Add(tile, true);
                int cnt = 0;
                while (q.Count > 0)
                {
                    var t = q.Peek().Item1;
                    var depth = q.Peek().Item2;
                    q.Dequeue();
                    while (Config.paused) yield return new WaitForSeconds(0.4f);
                    if (!t.Type.can_be_biome) continue;
                    if ((t.top_type == high || t.top_type == low) && !overlay) continue;
                    if (cnt < depth)
                    {
                        if (cnt > range) yield break;
                        cnt++;
                        yield return new WaitForSeconds(delay_time / Config.time_scale_asset.multiplier);
                    }
                    toptile = t.main_type.rank_type == TileRank.Low ? low : high;
                    MapAction.growGreens(t, toptile);
                    action?.RunAnyTrue(pTarget,t);
                    foreach (WorldTile pT in t.neighbours)
                    {
                        if (dict.ContainsKey(pT)) continue;
                        else
                        {
                            dict.Add(pT, true);
                            q.Enqueue(new Tuple<WorldTile, int>(pT, depth + 1));
                        }
                    }
                }
            }
            yield break;
        }

        public static bool Battle_Continue_finish(BaseSimObject pTarget, WorldTile pTile = null)
        {
            pTarget.a.addTrait("death_mark");
            pTarget.a.die();
            return true;
        }
    }
}
