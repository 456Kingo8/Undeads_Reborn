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
            ActorTrait LichLord = new ActorTrait()
            {
                id = "LichLord",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
                special_locale_id = "LichLord_id",
                special_locale_description = "LichLord_des",
                action_attack_target = Undead_Action.LichLord_attack,
                action_special_effect = Undead_Action.LichLord_action,
                
            };
            LichLord.base_stats = new BaseStats();
            LichLord.base_stats.addTag("Undead");
            LichLord.spells = new List<SpellAsset>() {Undead_Spell.summon_undeads};
            LichLord.decisions_assets = new DecisionAsset[] { Undead_Decision.speard_curse_biome };
            AssetManager.traits.add(LichLord);
            setAchievementUnlock(LichLord, "achievementGreatPlague");

            ActorTrait Undead = new ActorTrait()
            {
                id = "Undead_flag",
                path_icon = "Icons/TestIcon",
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
