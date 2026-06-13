using ai;
using HarmonyLib;
using NeoModLoader.api.attributes;
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

        public static List<string> zombie_id = new List<string>() {"zombie_human", "zombie_elf", "zombie_orc", "zombie_dwarf", "zombie_animal_fox", "zombie_animal_buffalo", "zombie_animal_monkey", "zombie_animal_rhino", "zombie_animal_frog", "zombie_animal_snake", "zombie_animal_dog", "zombie_animal_wolf", "zombie_animal_bear", "zombie_grasshopper", "zombie_necromancer", "zombie_plague_doctor", "zombie_white_mage", "zombie_evil_mage" };
        public static void init()
        {
            return;
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
                actor.addTrait("Undead_flag");
                actor.addTrait("fire_proof");
                actor.addTrait("acid_proof");
                actor.addTrait("immune");
                ActorTool.copyUnitToOtherUnit(a, actor);//记得修patch里trait collection的bug
                if (!a.getName().StartsWith("Un"))
                {
                    actor.setName("Un" + Toolbox.LowerCaseFirst(a.getName()));
                }
                if(pFrom != null)
                {
                    if (pFrom.kingdom != null) actor.kingdom = pFrom.kingdom;
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
                    if (pFrom.kingdom != null) tGhost.kingdom = pFrom.kingdom;
                }
                tGhost.addTrait("Undead_flag");
                tGhost.addTrait("fire_proof");
                tGhost.addTrait("acid_proof");
                tGhost.addTrait("immune");
                tGhost.subspecies.removeTrait("reproduction_soulborne");
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
                actor.removeTrait("zombie");
                actor.addTrait("Undead_flag");
                actor.addTrait("fire_proof");
                actor.addTrait("acid_proof");
                actor.addTrait("immune");
                if (!a.getName().StartsWith("Un"))
                {
                    actor.setName("Un" + Toolbox.LowerCaseFirst(a.getName()));
                }
                if (pFrom != null)
                {
                    if (pFrom.kingdom != null) actor.kingdom = pFrom.kingdom;
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

        public static bool Undead_attack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) return false;
            Actor tActor = pTarget.a;
            if (Randy.randomChance(0.24f)) tActor.addStatusEffect("cough");
            if (Randy.randomChance(0.24f)) tActor.addStatusEffect("poisoned");
            if (Randy.randomChance(0.12f)) tActor.addStatusEffect("ash_fever");
            if (Randy.randomChance(0.12f)) tActor.addStatusEffect("cursed");
            return true;
        }

        public static bool Undead_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.current_tile.getBiome()?.id == "biome_corrupted")
            {
                pTarget.addStatusEffect("Undead_Corrupt_Buff_3");
            }
            return true;
        }

        public static bool whisper_of_death_Action_death(BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            pTarget.a._active_status_dict.TryGetValue("whisper_of_death", out Status value);
            if (value != null)
            {
                FromExtend ext = value.GetExtend();
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
            FromExtend ext = value?.GetExtend();
            pTarget.a.data.health = Mathf.Max(0, pTarget.a.data.health - Mathf.Max((int)(pTarget.a.getMaxHealth() * 0.05), 2));
            if (pTarget.a.data.health == 0)
            {
                if (value != null && ext != null)
                {
                    bool flag = turn_into_Undeads(pTarget, pTile, ext.pFrom);
                    pTarget.a.die(flag, AttackType.Plague);
                    return true;
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
                    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_create_skeleton", pTile.posV3, 0.1f);
                    Actor actor = World.world.units.createNewUnit("skeleton", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                    actor.makeWait(0.8f);
                    actor.addTrait("Undead_flag");
                    City city2 = pSelf.a.city;
                    Kingdom kingdom = pSelf.kingdom;
                    if (!city2.isRekt() && city2.kingdom == kingdom)
                    {
                        actor.joinCity(pSelf.a.city);
                    }
                    else
                    {
                        actor.joinKingdom(kingdom);
                    }
                    actor.religion = pSelf.a.religion;
                    actor.addStatusEffect("Undead_Corrupt_Buff_3");
                }
                else if(Randy.randomChance(0.33f))
                {
                    if (Randy.randomChance(0.02f))
                    {
                        Actor act = World.world.units.createNewUnit("zombie_dragon", pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                        EffectsLibrary.spawn("fx_spawn", pTile);
                        act.religion = pSelf.a.religion;
                        act.addTrait("Undead_flag");
                        act.makeWait(0.8f);
                        act.addStatusEffect("Undead_Corrupt_Buff_3");
                    }
                    else 
                    {
                        Actor act = World.world.units.createNewUnit(zombie_id.GetRandom(), pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                        EffectsLibrary.spawn("fx_spawn", pTile);
                        act.religion = pSelf.a.religion;
                        act.addTrait("Undead_flag");
                        act.makeWait(0.8f);
                        act.addStatusEffect("Undead_Corrupt_Buff_3");
                    }
                }
                return true;
            }
        }

        public static bool Corrupt_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.current_tile.getBiome()?.id == "biome_corrupted")
            {
                if(!pTarget.a.hasReligion()) return false;

                if (pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_5_corrupt, 5))
                {
                    pTarget.addStatusEffect("Undead_Corrupt_Buff_3");
                    pTarget.a.restoreHealthPercent(0.04f);
                    pTarget.a.restoreManaPercent(0.04f);
                    pTarget.a.restoreStaminaPercent(0.04f);
                }
                else if (pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_corrupt, 4))
                {
                    pTarget.addStatusEffect("Undead_Corrupt_Buff_2");
                    pTarget.a.restoreHealthPercent(0.02f);
                    pTarget.a.restoreManaPercent(0.02f);
                    pTarget.a.restoreStaminaPercent(0.02f);
                }

                else if (pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_3_corrupt, 3))
                {
                    pTarget.addStatusEffect("Undead_Corrupt_Buff_1");
                    pTarget.a.restoreHealthPercent(0.01f);
                    pTarget.a.restoreManaPercent(0.01f);
                    pTarget.a.restoreStaminaPercent(0.01f);
                }

            }
            return true;
        }


        public static bool Corrupt_4_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Spread_Biome(pSelf, "biome_corrupted", 4, 0.1f, true));
            return true;
        }

        [Hotfixable]
        public static bool Soul_3_action(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(!pSelf.a.hasReligion()) return false;
            float damage = pSelf.a.stats["damage"];
            int index = 3;
            if (pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_soul, 4)) index = 4;
            if (pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_5_soul, 5)) index = 5;
            pTarget.getHit(damage * Mathf.Pow(1.25f ,(index - 2)),true,AttackType.Other,pSkipIfShake:false,pCheckDamageReduction:false);
            return true;
        }

        public static bool Soul_4_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Spread_Spell(pSelf,4, 0.1f, summon));
            return true;

            static bool summon(BaseSimObject pTarget, WorldTile pTile = null)
            {
                if (Randy.randomChance(0.24f))
                {
                    Actor tGhost = World.world.units.createNewUnit("ghost", pTile, false, 0f, null, null, true, false, false, false);
                    tGhost.kingdom = pTarget.kingdom;
                    if(pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_5_corrupt,5)) tGhost.addTrait("Undead_flag");
                    tGhost.addTrait("acid_proof");
                    tGhost.addTrait("immune");
                    tGhost.subspecies.removeTrait("reproduction_soulborne");
                }
                return true;
            }

        }

        public static bool Special_5_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            float f = (float)pTarget.a.getMaxHealth()/ (10 * (pTarget.a.data.health + 50));
            float rate = Mathf.Clamp(f,0.05f,0.3f);
            //MonoBehaviour.print($"debug{f}  {rate}");
            pTarget.a.restoreHealthPercent(rate);
            return true;
        }

        public static bool Special_5_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            pSelf.a.refresh_Trait();
            pSelf.a.restoreHealthPercent(0.05f);
            return true;
        }

        public static bool Corrput_5_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Spread_Spell(pSelf, "biome_corrupted", 7, 0.1f, summon));
            return true;

            static bool summon(BaseSimObject pTarget, WorldTile pTile = null)
            {
                if (Randy.randomChance(0.23f))
                {
                    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_create_skeleton", pTile.posV3, 0.1f);
                    Actor actor = World.world.units.createNewUnit("skeleton", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                    actor.makeWait(1f);
                    if(pTarget.kingdom != null) actor.joinKingdom(pTarget.kingdom);
                    actor.addTrait("fire_proof");
                    actor.addTrait("acid_proof");
                    actor.addTrait("immune");
                    actor.addTrait("Undead_flag");
                    actor.addStatusEffect("Undead_Corrupt_Buff_3");
                }
                else if (Randy.randomChance(0.125f))
                {
                    if (Randy.randomChance(0.02f))
                    {
                        Actor actor = World.world.units.createNewUnit("zombie_dragon", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                        actor.makeWait(1f);
                        if (pTarget.kingdom != null) actor.joinKingdom(pTarget.kingdom);
                        actor.addTrait("fire_proof");
                        actor.addTrait("acid_proof");
                        actor.addTrait("immune");
                        actor.removeTrait("zombie");
                        actor.addStatusEffect("Undead_Corrupt_Buff_3");
                        actor.addTrait("Undead_flag");
                    }
                    else
                    {
                        Actor actor = World.world.units.createNewUnit(zombie_id.GetRandom(), pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                        actor.makeWait(1f);
                        if (pTarget.kingdom != null) actor.joinKingdom(pTarget.kingdom);
                        actor.addTrait("fire_proof");
                        actor.addTrait("acid_proof");
                        actor.addTrait("immune");
                        actor.removeTrait("zombie");
                        actor.addStatusEffect("Undead_Corrupt_Buff_3");
                        actor.addTrait("Undead_flag");
                    }
                    EffectsLibrary.spawn("fx_spawn", pTile);
                }
                pTile.stopFire();
                return true;
            }
        }

        public static bool Corrput_Buff_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(pTarget.a.hasStatus("Undead_Corrupt_Buff_3"))
            {
                pTarget.a.restoreHealthPercent(0.04f);
                pTarget.a.restoreManaPercent(0.04f);
                pTarget.a.restoreStaminaPercent(0.04f);
            }
            else if (pTarget.a.hasStatus("Undead_Corrupt_Buff_2"))
            {
                pTarget.a.restoreHealthPercent(0.02f);
                pTarget.a.restoreManaPercent(0.02f);
                pTarget.a.restoreStaminaPercent(0.02f);
            }
            else if (pTarget.a.hasStatus("Undead_Corrupt_Buff_1"))
            {
                pTarget.a.restoreHealthPercent(0.01f);
                pTarget.a.restoreManaPercent(0.01f);
                pTarget.a.restoreStaminaPercent(0.01f);
            }
            return true;
        }


        [Hotfixable]
        public static bool speard_curse_biome(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Undead_Action.Spread_Biome(pSelf, "biome_corrupted", 10));
            return true;
        }
        [Hotfixable]
        public static bool curse_phrase_2(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pSelf.a.religion == null) return false;//理论上不存在这种情况，但还是防一下
            int radius = 4;
            if (pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_curse, 4))
            {
                radius = 6;
            }

            foreach (Actor tActor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, radius, false))
            {
                if (tActor.kingdom != null &&  tActor.kingdom.isEnemy(pSelf.kingdom))
                {

                    if(pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_curse,4))
                    {
                        tActor.removeTrait("immune");
                        tActor.removeTrait("poison_immune");
                        tActor.getHit(tActor.getMaxHealth() * 0.025f, true, AttackType.Poison, pSkipIfShake: false, pCheckDamageReduction: true);
                        tActor.getHit(tActor.getMaxHealth() * 0.025f, true, AttackType.Plague, pSkipIfShake: false, pCheckDamageReduction: false);
                    }
                    tActor.addStatusEffect("cough");
                    tActor.addStatusEffect("poisoned");
                }
            }
            return true;
        }


        [Hotfixable]
        public static bool curse_phrase_3(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pSelf.a.religion == null) return false;//理论上不存在这种情况，但还是防一下
            foreach (Actor tActor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, 3f, false))
            {
                if (tActor.kingdom != null && tActor.kingdom.isEnemy(pSelf.kingdom))
                {
                    if (pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_curse, 4))
                    {
                        tActor.removeTrait("immune");
                        tActor.removeTrait("poison_immune");
                        tActor.getHit(tActor.getMaxHealth() * 0.05f, true, AttackType.Plague, pSkipIfShake: false, pCheckDamageReduction: true);
                    }
                    if (tActor.hasStatus("ash_fever"))
                    {
                        if(Randy.randomChance(0.5f)) tActor.addStatusEffect("cursed");
                    }
                    else if(tActor.hasStatus("cursed"))
                    {
                        if (Randy.randomChance(0.5f)) tActor.addStatusEffect("ash_fever");
                    }
                    else
                    {
                        if (Randy.randomChance(0.5f)) tActor.addStatusEffect("cursed");
                        else tActor.addStatusEffect("ash_fever");
                    }
                }
            }
            return true;
        }
        [Hotfixable]
        public static bool curse_phrase_5(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pSelf.a.religion == null) return false;//理论上不存在这种情况，但还是防一下
            int radius = 2;
            if (pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_curse, 4))
            {
                radius = 4;//理论上也不存在这种情况，但还是防一下
            }
            foreach (Actor tActor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, radius, false))
            {
                if (tActor.kingdom != null && tActor.kingdom.isEnemy(pSelf.kingdom))
                {
                    if (pSelf.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_curse, 4))
                    {
                        tActor.removeTrait("immune");
                        tActor.removeTrait("poison_immune");
                        tActor.getHit(tActor.getMaxHealth() * 0.05f, true, AttackType.Age, pSkipIfShake: false, pCheckDamageReduction: true);
                    }
                    tActor.addStatusEffect("whisper_of_death", pSelf.a);
                }
            }
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
                    if (!t.Type.can_be_biome)
                    {
                        if(t.Type.ground) action?.RunAnyTrue(pTarget, t);
                        continue;
                    }
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

        public static IEnumerator Spread_Spell(BaseSimObject pTarget, string biome_id, int range, float delay_time = 1f, WorldAction action = null)
        {
            if (pTarget == null) yield break;
            BiomeAsset biome = AssetManager.biome_library.get(biome_id);
            WorldTile tile = pTarget.current_tile;
            TopTileType toptile, high, low;
            high = AssetManager.top_tiles.get(biome.tile_high);
            low = AssetManager.top_tiles.get(biome.tile_low);
            Queue<Tuple<WorldTile, int>> q = new();
            Dictionary<WorldTile, bool> dict = new();
            q.Enqueue(new Tuple<WorldTile, int>(tile, 0));
            dict.Add(tile, true);
            int cnt = 0;
            while (q.Count > 0)
            {
                var t = q.Peek().Item1;
                var depth = q.Peek().Item2;
                q.Dequeue();
                while (Config.paused) yield return new WaitForSeconds(0.4f);
                if (cnt < depth)
                {
                    if (cnt > range) yield break;
                    cnt++;
                    yield return new WaitForSeconds(delay_time / Config.time_scale_asset.multiplier);
                }
                if (t.Type.can_be_biome)
                {
                    toptile = t.main_type.rank_type == TileRank.Low ? low : high;
                    MapAction.growGreens(t, toptile);
                }
                else
                {
                    World.world.flash_effects.flashPixel(t, 20);
                }
                action?.RunAnyTrue(pTarget, t);
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
            yield break;
        }
        public static IEnumerator Spread_Spell(BaseSimObject pTarget, int range, float delay_time = 1f, WorldAction action = null)
        {
            if (pTarget == null) yield break;
            WorldTile tile = pTarget.a.current_tile;
            Queue<Tuple<WorldTile, int>> q = new();
            Dictionary<WorldTile, bool> dict = new();
            q.Enqueue(new Tuple<WorldTile, int>(tile, 0));
            dict.Add(tile, true);
            int cnt = 0;
            while (q.Count > 0)
            {
                var t = q.Peek().Item1;
                var depth = q.Peek().Item2;
                q.Dequeue();
                while (Config.paused) yield return new WaitForSeconds(0.4f);
                if (cnt < depth)
                {
                    if (cnt > range) yield break;
                    cnt++;
                    yield return new WaitForSeconds(delay_time / Config.time_scale_asset.multiplier);
                }
                World.world.flash_effects.flashPixel(t, 20);
                action?.RunAnyTrue(pTarget, t);
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
