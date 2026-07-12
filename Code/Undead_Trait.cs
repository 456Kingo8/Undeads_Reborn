using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undeads.Code.Behaviour;

namespace Undeads.Code
{
     class Undead_Trait
     {
        public static void init()
        {
            //测速特质，准备弃用
            //ActorTrait LichLord = new ActorTrait()
            //{
            //    id = "LichLord",
            //    path_icon = "Icons/iconNecrolord",
            //    group_id = "Undead",
            //    special_locale_id = "LichLord_id",
            //    special_locale_description = "LichLord_des",
            //    action_attack_target = Undead_Action.LichLord_attack,
            //    action_special_effect = Undead_Action.LichLord_action,
                
            //};
            //LichLord.base_stats = new BaseStats();
            //LichLord.base_stats.addTag("Undead");
            //LichLord.spells = new List<SpellAsset>() {Undead_Spell.summon_undeads};
            //LichLord.decisions_assets = new DecisionAsset[] { Undead_Decision.speard_curse_biome };
            //AssetManager.traits.add(LichLord);
            //setAchievementUnlock(LichLord, "achievementGreatPlague");

            ActorTrait Undead = new ActorTrait()
            {
                id = "Undead_flag",
                path_icon = "Icons/iconNecroflag",
                group_id = "Undead",
                special_locale_id = "Undead_flag_id",
                special_locale_description = "Undead_flag_des",
                action_attack_target = Undead_Action.Undead_attack,
                action_special_effect = Undead_Action.Undead_action
            };
            Undead.base_stats = new BaseStats();
            Undead.base_stats["mana"] = 30;
            Undead.base_stats["lifespan"] = 20;
            AssetManager.traits.add(Undead);


            ActorTrait trait = new ActorTrait()//腐化之心
            {
                id = "Undead_corrupt_lord",
                path_icon = "Icons/iconNecrolord",
                group_id = "Undead",
                special_locale_id = "Undead_corrupt_lord_id",
                special_locale_description = "Undead_corrupt_lord_des",
                special_locale_description_2 = "Undead_corrupt_lord_des2",
                //action_special_effect = Undead_Action.Undead_action
            };
            trait.base_stats = new BaseStats();
            trait.base_stats["mana"] = 300;
            trait.base_stats["lifespan"] = 100;
            AssetManager.traits.add(trait);
            setAchievementUnlock(trait, "achievementGreatPlague");

            trait = new ActorTrait()//骸骨军团
            {
                id = "Undead_skeleton_lord",
                path_icon = "Icons/iconNecrolord",
                group_id = "Undead",
                special_locale_id = "Undead_skeleton_lord_id",
                special_locale_description = "Undead_skeleton_lord_des",
                special_locale_description_2 = "Undead_skeleton_lord_des2",
                //action_special_effect = Undead_Action.Undead_action
            };
            trait.action_special_effect = Undead_Action.ske_zom_lord_action;
            trait.base_stats = new BaseStats();
            trait.base_stats["mana"] = 200;
            trait.base_stats["range"] = 2;
            trait.base_stats["lifespan"] = 150;
            trait.spells = new List<SpellAsset>() { Undead_Spell.summon_undeads };
            AssetManager.traits.add(trait);
            setAchievementUnlock(trait, "achievementGreatPlague");

            trait = new ActorTrait()//尸群领主
            {
                id = "Undead_zombie_lord",
                path_icon = "Icons/iconNecrolord",
                group_id = "Undead",
                special_locale_id = "Undead_zombie_lord_id",
                special_locale_description = "Undead_zombie_lord_des",
                special_locale_description_2 = "Undead_zombie_lord_des2",
                //action_special_effect = Undead_Action.Undead_action
            };
            trait.base_stats = new BaseStats();
            trait.action_special_effect = Undead_Action.ske_zom_lord_action;
            trait.base_stats["mana"] = 200;
            trait.base_stats["range"] = 2;
            trait.base_stats["lifespan"] = 150;
            trait.spells = new List<SpellAsset>() { Undead_Spell.summon_undeads };
            AssetManager.traits.add(trait);
            setAchievementUnlock(trait, "achievementGreatPlague");

            trait = new ActorTrait()//灵魂学者
            {
                id = "Undead_soul_lord",
                path_icon = "Icons/iconNecrolord",
                group_id = "Undead",
                special_locale_id = "Undead_soul_lord_id",
                special_locale_description = "Undead_soul_lord_des",
                special_locale_description_2 = "Undead_soul_lord_des2",
                //action_special_effect = Undead_Action.Undead_action
            };
            trait.base_stats = new BaseStats();
            trait.base_stats["mana"] = 1000;
            trait.base_stats["range"] = 1;
            trait.base_stats["lifespan"] = 10;
            AssetManager.traits.add(trait);
            setAchievementUnlock(trait, "achievementGreatPlague");

            trait = new ActorTrait()//瘟疫医生
            {
                id = "Undead_plague_lord",
                path_icon = "Icons/iconNecrolord",
                group_id = "Undead",
                special_locale_id = "Undead_plague_lord_id",
                special_locale_description = "Undead_plague_lord_des",
                special_locale_description_2 = "Undead_plague_lord_des2",
                //action_special_effect = Undead_Action.Undead_action
            };
            trait.base_stats = new BaseStats();
            trait.base_stats["mana"] = 100;
            trait.base_stats["range"] = 4;
            trait.base_stats["lifespan"] = 50;
            AssetManager.traits.add(trait);
            setAchievementUnlock(trait, "achievementGreatPlague");


            return;
        }


        public static void setAchievementUnlock(ActorTrait trait,string achievement)
        {
            trait.achievement_id = achievement;
            Achievement plague = AssetManager.achievements.get(achievement);
            plague.unlock_assets.Add(trait);
            if(plague.isUnlocked())
            {
                trait.unlock();
            }
        }
     }
}
