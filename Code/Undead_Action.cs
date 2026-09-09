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

        public static List<string> zombie_id = new List<string>() {"zombie_human", "zombie_elf", "zombie_orc", "zombie_dwarf", "zombie_animal_fox", "zombie_animal_buffalo", "zombie_animal_monkey", "zombie_animal_rhino", "zombie_animal_frog", "zombie_animal_snake", "zombie_animal_dog", "zombie_animal_wolf", "zombie_animal_bear", "zombie_grasshopper", "zombie_plague_doctor", "zombie_white_mage", "zombie_evil_mage" };
        public static List<string> zombie_id_strong = new List<string>() { "zombie_human", "zombie_elf", "zombie_orc", "zombie_dwarf","zombie_animal_bear", "zombie_grasshopper", "zombie_plague_doctor", "zombie_white_mage", "zombie_evil_mage" };
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
            if(a.hasTrait("LichLord"))
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
            pTarget.a.data.health = Mathf.Max(0, pTarget.a.data.health - Mathf.Max((int)(pTarget.a.getMaxHealth() * 0.055), 2));
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
            int title = get_LichLord_title(pSelf.a);
            if (pSelf.a.isAlive() && pTarget != null)
            {
                foreach(Actor act in Finder.getUnitsFromChunk(pTarget.current_tile, 2, 10))
                {
                    if (act.kingdom != pSelf.a.kingdom && !act.hasTag("Undead"))
                    {
                        if(title == 5)//毁灭之名：改血+即死
                        {
                            act.data.health = Mathf.Max(0, act.data.health - Mathf.Max((int)(act.getMaxHealth() * 0.1), 20));
                            if (act.data.health == 0)
                            {
                                bool flag = turn_into_Undeads(act, pTile, pSelf);
                                act.die(flag, AttackType.Plague);
                                return true;
                            }
                            act.addStatusEffect("whisper_of_death", pSelf.a);
                        }
                        else if(title == 0) //瘟疫医生
                        {
                            if (Randy.randomChance(0.3f)) act.addStatusEffect("ash_fever");
                        }
                        else if (title == 1)//尸群领主
                        {
                            if (Randy.randomChance(0.3f)) act.addStatusEffect("cough");
                            if (Randy.randomChance(0.3f)) act.addStatusEffect("poisoned");
                        }
                        else if (title == 2)//骸骨军团
                        {
                            if (Randy.randomChance(0.3f)) act.addTrait("one_eyed");
                            if (Randy.randomChance(0.3f)) act.addTrait("crippled");
                        }
                        else if (title == 3)//灵魂学者
                        {
                            if (Randy.randomChance(0.5f)) act.addStatusEffect("cursed");
                        }
                        else if (title == 4)//腐化之心：吸血
                        {
                            pSelf.a.restoreHealth((int)Mathf.Max(pSelf.a.getMaxHealth() * 0.001f, 1));
                        }
                    }
                }
            }
            return true;
        }

        public static bool LichLord_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            //唤起：每次判定恢复1%最大生命值与2%最大魔力值
            float hpRate = 0.01f;
            float manaRate = 0.02f;
            if (pTarget.a.hasTrait("LichLord"))
            {
                int title = get_LichLord_title(pTarget.a);
                if (title == 4)
                {
                    //腐化之心头衔：唤起效果翻倍
                    hpRate = 0.02f;
                    manaRate = 0.04f;
                }
                else if (title == 5)
                {
                    //毁灭之名头衔：唤起效果减半
                    hpRate = 0.005f;
                    manaRate = 0.01f;
                }
            }
            pTarget.a.restoreHealthPercent(hpRate);
            pTarget.a.restoreManaPercent(manaRate);
            return LichLord_attack(pTarget,null,pTile);
        }

        //亡灵君主 - 头衔大师
        public static readonly string[] LichLord_Title_Names = { "瘟疫医生", "尸群领主", "骸骨军团", "灵魂学者", "腐化之心", "毁灭之名" };
        public static readonly string[] LichLord_Title_Traits = { "Undead_plague_lord", "Undead_zombie_lord", "Undead_skeleton_lord", "Undead_soul_lord", "Undead_corrupt_lord" };
        private static Dictionary<int, int> _lichlord_title_cache = new Dictionary<int, int>();

        public static int get_LichLord_title(Actor actor)
        {
            if (actor.getMaxHealth() <= 0) return 0;
            float hpPercent = (float)actor.getHealth() / (float)actor.getMaxHealth();
            if (hpPercent <= 0.20f) return 5;//毁灭之名
            if (hpPercent > 0.84f) return 0;//瘟疫医生
            if (hpPercent > 0.68f) return 1;//尸群领主
            if (hpPercent > 0.52f) return 2;//骸骨军团
            if (hpPercent > 0.36f) return 3;//灵魂学者
            return 4;//腐化之心
        }
        public static int get_LichLord_title(float percent)
        {
            if (percent <= 0.20f) return 5;//毁灭之名
            if (percent > 0.84f) return 0;//瘟疫医生
            if (percent > 0.68f) return 1;//尸群领主
            if (percent > 0.52f) return 2;//骸骨军团
            if (percent > 0.36f) return 3;//灵魂学者
            return 4;//腐化之心
        }
        public static string strip_LichLord_title(string name)
        {
            foreach (string title in LichLord_Title_Names)
            {
                string suffix = "·" + title;
                if (name.EndsWith(suffix))
                {
                    return name.Substring(0, name.Length - suffix.Length);
                }
            }
            return name;
        }

        public static bool LichLord_title_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            Actor actor = pTarget.a;
            if (!actor.hasTrait("LichLord")) return true;
            int title = get_LichLord_title(actor);
            int id = actor.GetHashCode();
            if (_lichlord_title_cache.TryGetValue(id, out int oldTitle) && oldTitle == title)
            {
                return true;
            }
            _lichlord_title_cache[id] = title;

            //移除旧头衔特质
            foreach (string trait in LichLord_Title_Traits)
            {
                if (actor.hasTrait(trait)) actor.removeTrait(trait);
            }
            //赋予新头衔特质
            if (title == 5)
            {
                //毁灭之名：获得全部头衔特质
                foreach (string trait in LichLord_Title_Traits)
                {
                    actor.addTrait(trait);
                }
                actor.addStatusEffect("Undead_Destruction_Name", float.PositiveInfinity);
            }
            else
            {
                actor.finishStatusEffect("Undead_Destruction_Name");
                actor.addTrait(LichLord_Title_Traits[title]);
            }

            //更新姓名后缀
            string baseName = strip_LichLord_title(actor.getName());
            actor.setName(baseName + "·" + LichLord_Title_Names[title]);
            return true;
        }

        //亡灵君主 - 亡灵召唤：消耗魔力大范围召唤僵尸、骷髅、幽灵
        public static bool LichLord_summon(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            Actor act = pSelf.a;
            int range = 10;
            if (act.hasTrait("LichLord"))
            {
                int title = get_LichLord_title(act);
                if (title == 1 || title == 5) range = 16;//尸群领主：召唤+6范围
                else if (title == 2) range = 14;//骸骨军团：召唤+4范围
                else if (title == 3) range = 12;//灵魂学者：召唤+2范围
                else if (title == 5) range = 20; //毁灭之名：召唤+10范围
            }
            World.world.StartCoroutine(Spread_Spell(pSelf, range, 0.05f, LichLord_summon_tile));
            return true;
        }

        public static bool LichLord_summon_tile(BaseSimObject pTarget, WorldTile pTile = null)
        {
            Actor act = pTarget.a;
            int title = act.hasTrait("LichLord") ? get_LichLord_title(act) : 0;
            Actor actor = null;
            bool adamantine = (title == 2 || title == 5);//骸骨军团/毁灭之名：精金装备
            float skeleton_chance = 0.4f;
            float zombie_chance = 0.35f;
            if(title == 1)
            {
                skeleton_chance = 0f;
                zombie_chance = 1f;
            }
            if(title == 2)
            {
                skeleton_chance = 1f;
                zombie_chance = 0f;
            }
            if(title == 3)
            {
                skeleton_chance = 0f;
                zombie_chance = 0f;
            }

            if (Randy.randomChance(skeleton_chance))
            {
                //召唤骷髅
                actor = World.world.units.createNewUnit("skeleton", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: false, pAdultAge: true);
                EffectsLibrary.spawnAt("fx_create_skeleton", pTile.posV3, 0.1f);
                string gearTier = adamantine ? "adamantine" : "mythril";
                if (Randy.randomChance(0.5f)) actor.equipment.weapon.setItem(newItem("sword_" + gearTier, pTarget.kingdom, pTarget.name), actor);
                else actor.equipment.weapon.setItem(newItem("bow_" + gearTier, pTarget.kingdom, pTarget.name), actor);
                actor.equipment.armor.setItem(newItem("armor_" + gearTier, pTarget.kingdom, pTarget.name), actor);
                actor.equipment.boots.setItem(newItem("boots_" + gearTier, pTarget.kingdom, pTarget.name), actor);
                actor.equipment.helmet.setItem(newItem("helmet_" + gearTier, pTarget.kingdom, pTarget.name), actor);
                actor.equipment.ring.setItem(newItem("ring_" + gearTier, pTarget.kingdom, pTarget.name), actor);
                actor.addTrait("veteran");
            }
            else if (Randy.randomChance(zombie_chance))
            {
                //召唤僵尸
                if (Randy.randomChance(0.02f)) actor = World.world.units.createNewUnit("zombie_dragon", pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                else actor = World.world.units.createNewUnit(zombie_id_strong.GetRandom(), pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                EffectsLibrary.spawn("fx_spawn", pTile);
                actor.addTrait("regeneration");
                actor.addTrait("hard_skin", true);
                actor.addTrait("strong", true);
                actor.addTrait("giant", true);
                actor.addTrait("fase", true);
                actor.addTrait("dash");
            }
            else
            {
                //召唤幽灵
                actor = World.world.units.createNewUnit("ghost", pTile, false, 0f, null, null, true, false, false, false);
                actor.subspecies.removeTrait("reproduction_soulborne");
            }

            if (actor != null)
            {
                actor.makeWait(1f);
                if (act.kingdom != null) actor.joinKingdom(act.kingdom);
                if (act.city != null && actor.asset.id == "skeleton") actor.joinCity(act.city);
                actor.addTrait("fire_proof");
                actor.addTrait("acid_proof");
                actor.addTrait("immune");
                actor.removeTrait("zombie");
                actor.addTrait("Undead_flag");

                //腐化等级
                string corruptBuff = "Undead_Corrupt_Buff_1";
                if (actor.asset.id.Contains("zombie") && (title == 1 || title == 5)) corruptBuff = "Undead_Corrupt_Buff_3";//尸群领主：僵尸3级腐化
                else if (actor.asset.id == "skeleton" && (title == 2 || title == 5)) corruptBuff = "Undead_Corrupt_Buff_2";//骸骨军团：骷髅2级腐化
                actor.addStatusEffect(corruptBuff, float.PositiveInfinity);
                pTile.stopFire();
            }
            return true;
        }


        [Hotfixable]
        public static bool summon_undead(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            World.world.StartCoroutine(Spread_Spell(pSelf, 6 , 0.1f , lord_summon));
            return true;
        }

        public static bool ske_zom_lord_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            string id = pTarget.a.hasTrait("Undead_zombie_lord") ? "zombie" : "skeleton";
            int cnt = 0;
            foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile,1,8,false))
            {
                if(actor.asset.id.Contains(id) && actor.kingdom == pTarget.kingdom)
                {
                    cnt++;
                    actor.addStatusEffect("Undead_army_of_lord");
                }
            }
            if(cnt >= 30) pTarget.a.addStatusEffect("Undead_lord_of_army");
            if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(cnt/200);
            return true;
        }

        //骸骨军团和尸群领主公用的action
        public static bool lord_summon(BaseSimObject pTarget, WorldTile pTile = null)
        {
            float chance_skeleton = 0.4f;
            float chance_zombie = 0.2f;
            Actor actor = null;
            Actor act = pTarget.a;
            if (act.hasTrait("Undead_skeleton_lord"))
            {
                chance_skeleton = 0.6f;
                chance_zombie = 0;
            }
            else if (act.hasTrait("Undead_zombie_lord"))
            {
                chance_skeleton = 0;
                chance_zombie = 0.6f;
            }

            if (chance_skeleton != 0 && Randy.randomChance(chance_skeleton))
            {
                BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_create_skeleton", pTile.posV3, 0.1f);
                actor = World.world.units.createNewUnit("skeleton", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: false, pAdultAge: true);
                if (Randy.randomChance(0.5f)) actor.equipment.weapon.setItem(newItem("sword_mythril", pTarget.kingdom, pTarget.name), actor);
                else actor.equipment.weapon.setItem(newItem("bow_mythril", pTarget.kingdom, pTarget.name), actor);
                actor.equipment.armor.setItem(newItem("armor_mythril", pTarget.kingdom, pTarget.name), actor);
                actor.equipment.boots.setItem(newItem("boots_mythril", pTarget.kingdom, pTarget.name), actor);
                actor.equipment.helmet.setItem(newItem("helmet_mythril", pTarget.kingdom, pTarget.name), actor);
                actor.equipment.ring.setItem(newItem("ring_mythril", pTarget.kingdom, pTarget.name), actor);
                actor.addTrait("veteran");
            }
            if (actor == null && chance_zombie != 0 && Randy.randomChance(chance_zombie))
            {
                if (Randy.randomChance(0.02f)) actor = World.world.units.createNewUnit("zombie_dragon", pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                else actor = World.world.units.createNewUnit(zombie_id_strong.GetRandom(), pTile, pMiracleSpawn: false, 0f, null, pSpawnWithItems: true);
                EffectsLibrary.spawn("fx_spawn", pTile);
                actor.addTrait("regeneration");
                actor.addTrait("hard_skin", true);
                actor.addTrait("strong", true);
                actor.addTrait("giant", true);
                actor.addTrait("fase", true);
                actor.addTrait("dash");
            }
            if (actor != null)
            {
                actor.makeWait(1f);
                if (act.kingdom != null) actor.joinKingdom(act.kingdom);
                if (act.city != null && actor.asset.id == "skeleton") actor.joinCity(pTarget.a.city);
                actor.addTrait("fire_proof");
                actor.addTrait("acid_proof");
                actor.addTrait("immune");
                actor.removeTrait("zombie");
                actor.addStatusEffect("Undead_Corrupt_Buff_1", float.PositiveInfinity);
                actor.addStatusEffect("Undead_Corrupt_Buff_3");
                actor.addTrait("Undead_flag");
                pTile.stopFire();
            }
            return true;
        }
        //腐化之心 - 腐化之唤：半径4格内友方单位获得腐化之唤buff + 3级腐化效果
        public static bool Corrupt_Call_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, 4f, false))
            {
                if (actor.kingdom == pTarget.kingdom && !actor.hasTrait("Undead_corrupt_lord"))
                {
                    actor.addStatusEffect("Undead_Corrupt_Call", 10f);
                    actor.addStatusEffect("Undead_Corrupt_Buff_3", 30f);
                }
            }
            return true;
        }

        public static bool Corrupt_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.current_tile.getBiome()?.id == "biome_corrupted")
            {
                if(!pTarget.a.hasReligion()) return false;

                if (pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_5_corrupt, 5))
                {
                    pTarget.addStatusEffect("Undead_Corrupt_Buff_3");
                    if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(0.04f);
                    pTarget.a.restoreManaPercent(0.04f);
                    pTarget.a.restoreStaminaPercent(0.04f);
                }
                else if (pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_4_corrupt, 4))
                {
                    pTarget.addStatusEffect("Undead_Corrupt_Buff_2");
                    if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(0.02f);
                    pTarget.a.restoreManaPercent(0.02f);
                    pTarget.a.restoreStaminaPercent(0.02f);
                }

                else if (pTarget.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_3_corrupt, 3))
                {
                    pTarget.addStatusEffect("Undead_Corrupt_Buff_1");
                    if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(0.01f);
                    pTarget.a.restoreManaPercent(0.01f);
                    pTarget.a.restoreStaminaPercent(0.01f);
                }

            }
            return true;
        }


        public static bool Corrupt_4_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (!pSelf.a.hasReligion()) return false;
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
            if (!pSelf.a.hasReligion()) return false;
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
            pTarget.a.restoreHealthPercent(rate);
            return true;
        }

        public static bool Special_5_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            pSelf.a.refresh_Trait();
            pSelf.a.restoreHealthPercent(0.05f);
            return true;
        }
        [Hotfixable]
        public static bool Corrput_5_spell(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(pSelf.a.hasTrait("Undead_skeleton_lorg") || pSelf.a.hasTrait("Undead_zombie_lord")) World.world.StartCoroutine(Spread_Spell(pSelf, 8, 0.1f, lord_summon));
            else World.world.StartCoroutine(Spread_Spell(pSelf, "biome_corrupted", 7, 0.1f, summon));
            return true;

            static bool summon(BaseSimObject pTarget, WorldTile pTile = null)
            {
                Actor actor = null;
                if (Randy.randomChance(0.23f))
                {
                    actor = World.world.units.createNewUnit("skeleton", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                    EffectsLibrary.spawnAt("fx_create_skeleton", pTile.posV3, 0.1f);
                }
                else if (Randy.randomChance(0.125f))
                {
                    if (Randy.randomChance(0.02f)) actor = World.world.units.createNewUnit("zombie_dragon", pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                    else actor = World.world.units.createNewUnit(zombie_id.GetRandom(), pTile, pMiracleSpawn: false, 0f, null, null, pSpawnWithItems: true, pAdultAge: true);
                    EffectsLibrary.spawn("fx_spawn", pTile);
                }
                if (actor != null)
                {
                    actor.makeWait(1f);
                    if (pTarget.kingdom != null) actor.joinKingdom(pTarget.kingdom);
                    if(pTarget.a.city != null && actor.asset.id == "skeleton") actor.joinCity(pTarget.a.city);
                    actor.addTrait("fire_proof");
                    actor.addTrait("acid_proof");
                    actor.addTrait("immune");
                    actor.removeTrait("zombie");
                    actor.addStatusEffect("Undead_Corrupt_Buff_3");
                    actor.addTrait("Undead_flag");
                    pTile.stopFire();
                }
                return true;
            }
        }

        public static bool Corrput_Buff_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if(pTarget.a.hasStatus("Undead_Corrupt_Buff_3"))
            {
                if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(0.04f);
                pTarget.a.restoreManaPercent(0.04f);
                pTarget.a.restoreStaminaPercent(0.04f);
            }
            else if (pTarget.a.hasStatus("Undead_Corrupt_Buff_2"))
            {
                if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(0.02f);
                pTarget.a.restoreManaPercent(0.02f);
                pTarget.a.restoreStaminaPercent(0.02f);
            }
            else if (pTarget.a.hasStatus("Undead_Corrupt_Buff_1"))
            {
                if (!pTarget.a.hasTrait("LichLord")) pTarget.a.restoreHealthPercent(0.01f);
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


        [Hotfixable]
        public static Item newItem(string id,Kingdom kingdom,string pWho)
        {
            return World.world.items.generateItem(AssetManager.items.get(id), kingdom,pWho);
        }

        //灵魂学者 - 灵魂风暴：攻击附带真实伤害，随半径5格内有灵魂生物数量增多
        public static bool Soul_Storm_attack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) return true;
            float range = 5f;
            if (pSelf.a.hasTrait("LichLord")) range = 16f;//灵魂学者头衔：灵魂风暴判定范围增加至半径16
            int soulCount = 0;
            foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, range, false))
            {
                if (actor.Undead_has_soul()) soulCount++;
            }
            float baseDamage = pSelf.a.stats["damage"];
            float extraDamage = baseDamage * (1 + soulCount * 0.15f);
            pTarget.a.getHit(extraDamage, true, AttackType.Other, pSkipIfShake: false, pCheckDamageReduction: false);
            return true;
        }

        public static IEnumerable<Actor> findTraitAroundTileChunk(WorldTile pTile, string pTrait)
        {
            foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 0f, false))
            {
                if (actor.hasTrait(pTrait))
                {
                    yield return actor;
                }
            }
            yield break;
        }

        //瘟疫医生 - 感染之触：亡灵瘟疫随攻击和行动蔓延
        public static bool Plague_Spread_attack(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) return true;
            pTarget.a.addStatusEffect("Undead_Plague", pSelf.a, 30f);
            float chance = 0.4f;
            if (pSelf.a.hasTrait("LichLord")) chance = 1f;
            foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, 3f, false))
            {
                if (Randy.randomChance(chance))
                {
                    actor.addStatusEffect("Undead_Plague", pSelf.a, 30f);
                }
            }
            return true;
        }

        public static bool Plague_Spread_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, 4f, false))
            {
                actor.addStatusEffect("Undead_Plague", pTarget.a, 30f);
            }
            return true;
        }

        //瘟疫医生 - 基因编辑：亡灵瘟疫对友方增益，对敌方减益
        public static bool Undead_Plague_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            Actor target = pTarget.a;
            //获取瘟疫来源，判断阵营
            FromExtend ext = null;
            target._active_status_dict.TryGetValue("Undead_Plague", out Status value);
            if (value != null) ext = value.GetExtend();
            if (ext != null && ext.pFrom != null && ext.pFrom.kingdom != null)
            {
                if (target.kingdom == ext.pFrom.kingdom)
                {
                    //友方增益
                    target.addTrait("immune");
                    if(!target.hasTrait("LichLord"))
                    {
                        target.restoreHealthPercent(0.02f);
                        target.restoreManaPercent(0.02f);
                    }
                    if(ext.pFrom.hasTrait("LichLord"))
                    {
                        target.addStatusEffect("Undead_Corrupt_Buff_3", 10f);
                    }
                    else target.addStatusEffect("Undead_Corrupt_Buff_1", 5f);
                }
                else
                {
                    //敌方减益
                    target.getHit(Mathf.Max(target.getMaxHealth() * 0.01f, 3), true, AttackType.Plague, pSkipIfShake: false, pCheckDamageReduction: false);
                    if (ext.pFrom.hasTrait("LichLord"))
                    {
                        target.getHit(Mathf.Max(target.getMaxHealth() * 0.03f, 10), false, AttackType.Other, pSkipIfShake: false, pCheckDamageReduction: false);
                    }
                    target.addTrait(_cure_traits.GetRandom());
                }
            }
            else
            {
                target.finishStatusEffect("Undead_Plague");
            }
            return true;
        }

        public static bool Undead_Plague_action_death(BaseSimObject pTarget, WorldTile pTile = null)
        {
            Actor target = pTarget.a;
            FromExtend ext = null;
            target._active_status_dict.TryGetValue("Undead_Plague", out Status value);
            if (value != null) ext = value.GetExtend();
            if(ext.pFrom != null)
            {
                foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, 4f, false))
                {
                    actor.addStatusEffect("Undead_Plague", ext.pFrom, 30f);
                    if (actor.kingdom.isEnemy(pTarget.kingdom))
                    {
                        actor.getHit(25,true,AttackType.Plague,pCheckDamageReduction:false);
                    }
                }
            }
            return true;
        }

        //瘟疫医生 - 治愈之光：治疗半径6格内友方，移除负面特质和状态
        private static readonly string[] _cure_statuses = { "cursed", "poisoned", "cough", "ash_fever", "whisper_of_death" };
        private static readonly string[] _cure_traits = { "infected", "mush_spores", "tumor_infection", "one_eyed", "crippled" };
        public static bool Heal_Light_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, 6f, false))
            {
                if (actor.kingdom == pTarget.kingdom)
                {
                    if(!actor.hasTrait("LichLord"))actor.restoreHealthPercent(0.05f);
                    foreach (string status in _cure_statuses)
                    {
                        if (actor.hasStatus(status)) actor.finishStatusEffect(status);
                    }
                    foreach (string trait in _cure_traits)
                    {
                        if (actor.hasTrait(trait)) actor.removeTrait(trait);
                    }
                }
            }
            return true;
        }

        //灵魂学者 - 灵魂增殖：每3s所在城市获得1灵魂碎片
        private static Dictionary<int, float> _soul_proliferation_cooldown = new Dictionary<int, float>();
        public static bool Soul_Proliferation_action(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.a.city == null || pTarget.a.city.storages.Count == 0) return true;
            int id = pTarget.a.GetHashCode();
            float now = Time.time;
            if (!_soul_proliferation_cooldown.TryGetValue(id, out float last) || now - last >= 3f)
            {
                _soul_proliferation_cooldown[id] = now;
                pTarget.a.city.storages.GetRandom()?.addResources("Undead_Soul_Pieces", 1);
            }
            return true;
        }
    }
}
