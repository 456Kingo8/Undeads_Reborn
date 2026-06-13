using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

            if (__instance.hasReligion() && !__instance.hasStatus("Ark_Cooldown")&&__instance.religion.has_Undead_Trait(SUndead.Undead_Phrase_5_soul, 5) && !__instance.hasTrait("death_mark"))
            {
                if(__instance.hasCity() && __instance.city.buildings.Count > 0)
                {
                    WorldTile tile = __instance.city.buildings.GetRandom().current_tile;
                    ActionLibrary.teleportEffect(__instance, tile);
                    __instance.cancelAllBeh();
                    __instance.spawnOn(tile);
                    __instance.data.health = 1;
                    __instance.refresh();
                    __instance.addStatusEffect("Undead_Corrupt_Buff_2");
                    __instance.addStatusEffect("Ark_Cooldown");
                    __instance._alive = true;
                }
                return false;
            }

            if (__instance.hasReligion() && __instance.religion.has_Undead_Trait(SUndead.Undead_Phrase_1_special,1) && !__instance.hasTrait("death_mark"))
            {
                __instance.data.health = 1;
                __instance._alive = true;
                __instance.addStatusEffect("Undead_Battle_Continue",5f * SUndead.get_Phrase_index(__instance.religion));
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


        #region 给又臭又长的原版数据更新加上宗教数值
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "updateStats")]
        public static bool Actor_updateStats(Actor __instance)
        {
            if (!__instance.isStatsDirty())
            {
                return false;
            }

            __instance._stats_dirty = false;
            __instance.stats_dirty_version++;
            __instance.updateCachedStatusEffects();//此处消灭了一个无限递归

            __instance.checkGrowthEvent();
            __instance.decisions_counter = 0;
            __instance.batch.c_stats_dirty.Remove(__instance.a);
            if (!__instance.isAlive())
            {
                return false;
            }
            __instance.s_action_attack_target = null;
            __instance.s_get_hit_action = null;
            __instance._s_special_effect_augmentations.Clear();
            __instance._s_special_effect_augmentations_timers.Clear();
            __instance.stats.clear();
            __instance.clearCombatActions();
            __instance.clearSpells();
            if (__instance.hasSubspecies())
            {
                __instance.stats.mergeStats(__instance.subspecies.base_stats, 1f);
                if (__instance.isSexMale())
                {
                    __instance.stats.mergeStats(__instance.subspecies.base_stats_male, 1f);
                }
                else
                {
                    __instance.stats.mergeStats(__instance.subspecies.base_stats_female, 1f);
                }
            }
            else
            {
                __instance.stats.mergeStats(__instance.asset.base_stats, 1f);
            }
            if (__instance.hasClan())
            {
                __instance.stats.mergeStats(__instance.clan.base_stats, 1f);
                if (__instance.isSexMale())
                {
                    __instance.stats.mergeStats(__instance.clan.base_stats_male, 1f);
                }
                else
                {
                    __instance.stats.mergeStats(__instance.clan.base_stats_female, 1f);
                }
            }
            if (__instance.hasLanguage())
            {
                __instance.stats.mergeStats(__instance.language.base_stats, 1f);
            }
            if (__instance.hasCulture())
            {
                __instance.stats.mergeStats(__instance.culture.base_stats, 1f);
            }
            if (__instance.hasReligion())
            {
                __instance.stats.mergeStats(__instance.religion.base_stats, 1f);
            }

            BaseStats stats = __instance.stats;
            stats["diplomacy"] = stats["diplomacy"] + __instance.data["diplomacy"];
            stats = __instance.stats;
            stats["stewardship"] = stats["stewardship"] + __instance.data["stewardship"];
            stats = __instance.stats;
            stats["intelligence"] = stats["intelligence"] + __instance.data["intelligence"];
            stats = __instance.stats;
            stats["warfare"] = stats["warfare"] + __instance.data["warfare"];
            __instance._cache_check_has_status_removed_on_damage = false;
            if (__instance.hasAnyStatusEffect())
            {
                foreach (Status tStatus in __instance.getStatuses())
                {
                    __instance.stats.mergeStats(tStatus.asset.base_stats, 1f);
                    if (tStatus.asset.removed_on_damage)
                    {
                        __instance._cache_check_has_status_removed_on_damage = true;
                    }
                    if (!string.IsNullOrEmpty(tStatus.asset.decision_id))
                    {
                        DecisionAsset[] array = __instance.decisions;
                        int num = __instance.decisions_counter;
                        __instance.decisions_counter = num + 1;
                        array[num] = tStatus.asset.getDecisionAsset();
                    }
                }
            }
            if (!__instance.hasWeapon())
            {
                EquipmentAsset tDefaultWeapon = AssetManager.items.get(__instance.asset.default_attack);
                if (tDefaultWeapon != null)
                {
                    __instance.stats.mergeStats(tDefaultWeapon.base_stats, 1f);
                }
            }
            __instance.checkAttackTypes();
            foreach (ActorTrait tTrait in __instance.traits)
            {
                if (!tTrait.only_active_on_era_flag || ((!tTrait.era_active_moon || World.world_era.flag_moon) && (!tTrait.era_active_night || World.world_era.overlay_darkness)))
                {
                    if (tTrait.action_get_hit != null)
                    {
                        __instance.s_get_hit_action = (GetHitAction)Delegate.Combine(__instance.s_get_hit_action, tTrait.action_get_hit);
                    }
                    __instance.stats.mergeStats(tTrait.base_stats, 1f);
                }
            }
            __instance.is_forced_socialize_icon = (__instance.hasStatus("possessed") && __instance.hasTag("strong_mind"));
            if (__instance.hasStatus("budding"))
            {
                stats = __instance.stats;
                stats["diplomacy"] = stats["diplomacy"] * 2f;
                stats = __instance.stats;
                stats["stewardship"] = stats["stewardship"] * 2f;
                stats = __instance.stats;
                stats["intelligence"] = stats["intelligence"] * 2f;
                stats = __instance.stats;
                stats["warfare"] = stats["warfare"] * 2f;
            }
            if (__instance.isSapient())
            {
                __instance.s_personality = null;
                if (__instance.isKing() || __instance.isCityLeader())
                {
                    string tPersonality = "balanced";
                    float tHighStat = __instance.stats["diplomacy"];
                    if (__instance.stats["diplomacy"] > __instance.stats["stewardship"])
                    {
                        tPersonality = "diplomat";
                        tHighStat = __instance.stats["diplomacy"];
                    }
                    else if (__instance.stats["diplomacy"] < __instance.stats["stewardship"])
                    {
                        tPersonality = "administrator";
                        tHighStat = __instance.stats["stewardship"];
                    }
                    if (__instance.stats["warfare"] > tHighStat)
                    {
                        tPersonality = "militarist";
                    }
                    __instance.s_personality = AssetManager.personalities.get(tPersonality);
                    __instance.stats.mergeStats(__instance.s_personality.base_stats, 1f);
                }
            }
            float tBonusFromLevelHealth = (float)__instance.data.level * SimGlobals.m.level_mod_bonus_health * __instance.stats["health"];
            float tBonusFromLevelMana = (float)__instance.data.level * SimGlobals.m.level_mod_bonus_mana * __instance.stats["mana"];
            float tBonusFromLevelStamina = (float)__instance.data.level * SimGlobals.m.level_mod_bonus_stamina * __instance.stats["stamina"];
            stats = __instance.stats;
            stats["health"] = stats["health"] + tBonusFromLevelHealth;
            stats = __instance.stats;
            stats["mana"] = stats["mana"] + tBonusFromLevelMana;
            stats = __instance.stats;
            stats["stamina"] = stats["stamina"] + tBonusFromLevelStamina;
            stats = __instance.stats;
            stats["skill_combat"] = stats["skill_combat"] + (float)((int)(__instance.stats["warfare"] / 5f)) * 0.01f;
            stats = __instance.stats;
            stats["skill_spell"] = stats["skill_spell"] + (float)((int)(__instance.stats["intelligence"] / 5f)) * 0.01f;
            if (__instance.data.level > 5)
            {
                stats = __instance.stats;
                stats["skill_combat"] = stats["skill_combat"] + 0.1f;
                stats = __instance.stats;
                stats["skill_spell"] = stats["skill_spell"] + 0.1f;
            }
            __instance.addSpecialEffectAugmentations(__instance.traits);
            __instance.checkActionsFromAllMetas();
            __instance.recalcCombatActions();
            __instance.recalcSpells();
            __instance.registerDecisions();
            bool tHadStatusUnconscious = __instance._has_tag_unconscious;
            __instance.has_tag_generate_light = __instance.hasTag("generate_light");
            __instance._has_tag_unconscious = __instance.hasTag("unconscious");
            __instance.has_tag_immunity_cold = __instance.hasTag("immunity_cold");
            if (__instance._has_tag_unconscious)
            {
                if (!tHadStatusUnconscious)
                {
                    if (__instance.batch.rnd.NextBool())
                    {
                        __instance._rotation_direction = RotationDirection.Left;
                    }
                    else
                    {
                        __instance._rotation_direction = RotationDirection.Right;
                    }
                }
                __instance.timer_jump_animation = 0f;
            }
            __instance._has_trait_weightless = __instance.hasTrait("weightless");
            __instance._has_status_sleeping = __instance.hasStatus("sleeping");
            __instance._has_status_strange_urge = __instance.hasStatus("strange_urge");
            __instance._has_status_possessed = __instance.hasStatus("possessed");
            __instance._has_status_tantrum = __instance.hasStatus("tantrum");
            __instance._has_status_drowning = __instance.hasStatus("drowning");
            __instance._has_status_invincible = __instance.hasStatus("invincible");
            __instance.is_immovable = __instance.isImmovable();
            __instance.is_ai_frozen = __instance.isAiFrozen();
            __instance._has_stop_idle_animation = __instance.hasStopIdleAnimation();
            __instance._ignore_fights = __instance.isIgnoreFights();
            if (__instance.hasSubspecies())
            {
                __instance._has_emotions = __instance.subspecies.can_process_emotions;
            }
            else
            {
                __instance._has_emotions = false;
            }
            if (!__instance.hasWeapon())
            {
                EquipmentAsset tDefaultItemAttackAsset = AssetManager.items.get(__instance.asset.default_attack);
                __instance.addDefaultItemAttackActions(tDefaultItemAttackAsset);
                if (tDefaultItemAttackAsset.item_modifiers != null)
                {
                    for (int i = 0; i < tDefaultItemAttackAsset.item_modifiers.Length; i++)
                    {
                        ItemModAsset tModData = tDefaultItemAttackAsset.item_modifiers[i];
                        if (tModData != null)
                        {
                            __instance.addDefaultItemAttackActions(tModData);
                        }
                    }
                }
            }
            if (__instance.canUseItems())
            {
                foreach (ActorEquipmentSlot tSlot in __instance.equipment)
                {
                    if (!tSlot.isEmpty())
                    {
                        Item tItem = tSlot.getItem();
                        __instance.addItemActions(tItem.getAsset());
                        if (tItem.action_attack_target != null)
                        {
                            __instance.s_action_attack_target = (AttackAction)Delegate.Combine(__instance.s_action_attack_target, tItem.action_attack_target);
                        }
                        foreach (string ptr in tItem.data.modifiers)
                        {
                            string tModID = ptr;
                            ItemModAsset tModData2 = AssetManager.items_modifiers.get(tModID);
                            __instance.addItemActions(tModData2);
                        }
                    }
                }
            }
            if (__instance._s_special_effect_augmentations.Count == 0)
            {
                __instance.batch.c_augmentation_effects.Remove(__instance.a);
            }
            else
            {
                __instance.batch.c_augmentation_effects.Add(__instance.a);
            }
            __instance._has_any_sick_trait = __instance.calculateIsSick();
            __instance._has_trait_peaceful = __instance.hasTrait("peaceful");
            __instance._has_trait_clone = __instance.hasTrait("clone");
            if (__instance.canUseItems())
            {
                foreach (ActorEquipmentSlot tSlot2 in __instance.equipment)
                {
                    if (!tSlot2.isEmpty())
                    {
                        Item tItem2 = tSlot2.getItem();
                        float tStatsMultiplier = 1f;
                        if (tItem2.isBroken())
                        {
                            tStatsMultiplier = 0.5f;
                        }
                        ItemTools.mergeStatsWithItem(__instance.stats, tItem2, false, tStatsMultiplier);
                    }
                }
            }
            if (__instance.asset.only_melee_attack)
            {
                __instance.stats["range"] = __instance.asset.base_stats["range"];
            }
            __instance.stats.normalize();
            stats = __instance.stats;
            stats["cities"] = stats["cities"] + (float)((int)__instance.stats["stewardship"] / 6 + 1);
            stats = __instance.stats;
            stats["bonus_towers"] = stats["bonus_towers"] + (float)((int)(__instance.stats["warfare"] / 10f));
            stats = __instance.stats;
            stats["mana"] = stats["mana"] + (float)((int)(__instance.stats["intelligence"] * SimGlobals.m.MANA_PER_INTELLIGENCE));
            __instance.stats.checkMultipliers();
            if (__instance.isSapient())
            {
                __instance.calculateOffspringBasedOnAge();
            }
            if (__instance.hasRangeAttack())
            {
                stats = __instance.stats;
                stats["range"] = stats["range"] + __instance.stats["range"] * World.world_era.range_weapons_multiplier;
            }
            stats = __instance.stats;
            stats["damage"] = stats["damage"] + __instance.stats["warfare"] / 5f;
            if (__instance.isBaby())
            {
                __instance.stats["damage"] = __instance.stats["damage"] * 0.5f;
                __instance.stats["health"] = __instance.stats["health"] * 0.5f;
            }
            __instance.stats.normalize();
            if (__instance.getHealth() > __instance.getMaxHealth())
            {
                __instance.setMaxHealth();
            }
            if (__instance.getHappiness() > __instance.getMaxHappiness())
            {
                __instance.setMaxHappiness();
            }
            if (__instance.getStamina() > __instance.getMaxStamina())
            {
                __instance.setMaxStamina();
            }
            if (__instance.getMana() > __instance.getMaxMana())
            {
                __instance.setMaxMana();
            }
            if (__instance.event_full_stats)
            {
                __instance.event_full_stats = false;
                __instance.setMaxHealth();
                __instance.setMaxStamina();
                __instance.setMaxMana();
            }
            if (__instance.isHovering())
            {
                __instance.batch.c_hovering.Add(__instance.a);
            }
            else
            {
                __instance.move_jump_offset.y = 0f;
                __instance.batch.c_hovering.Remove(__instance.a);
            }
            if (__instance.isPollinator())
            {
                __instance.batch.c_pollinating.Add(__instance.a);
            }
            else
            {
                __instance.batch.c_pollinating.Remove(__instance.a);
            }
            __instance.target_scale = __instance.stats["scale"];
            if (__instance.attack_timer > __instance.getAttackCooldown())
            {
                __instance.attack_timer = __instance.getAttackCooldown();
            }
            return false;
        }


        //显然 我并不知道怎么加入条件判断的IL代码
        //[HarmonyPatch(typeof(Actor), "updateStats")]
        //public static IEnumerable<CodeInstruction> Actor_updateStats_Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    var codeMatcher = new CodeMatcher(instructions);

        //    codeMatcher.MatchStartForward(new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld), new CodeMatch(OpCodes.Callvirt))
        //    .MatchForward(false,new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld), new CodeMatch(OpCodes.Callvirt))
        //    .MatchForward(false, new CodeMatch(OpCodes.Ldarg_0), new CodeMatch(OpCodes.Ldfld), new CodeMatch(OpCodes.Callvirt))
        //    .InsertAndAdvance(
        //                            new CodeInstruction(OpCodes.Ldloc_0),
        //                            new CodeInstruction(OpCodes.Call, typeof(Convert).GetMethod(
        //                                                              nameof(Actor.hasReligion),
        //                                                              new[] { typeof(string) })),
        //                            new CodeInstruction(OpCodes.Brfalse_S),

        //        );



        //    return codeMatcher.Instructions();
        //}

        #endregion



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
            if (pActor.a.hasReligion() && pActor.a.religion.has_Undead_Trait(SUndead.Undead_Phrase_2_corrupt, 2) || pActor.a.hasTrait("Undead_flag"))
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
