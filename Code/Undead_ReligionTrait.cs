using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undeads.Code.Behaviour;
using UnityEngine;

namespace Undeads.Code
{
     class Undead_ReligionTrait
     {
        public static void init()
        {

            List<string> strings = new List<string>() { "soul","curse", "corrupt", "craft", "special", "finish"};
            for(int i = 1;i <= 5;i++)
            {
                int rarity = 0;
                if(i == 1) rarity = 0;
                if (i == 2) rarity = 1;
                if (i == 3) rarity = 1;
                if(i == 4) rarity = 2;
                if(i == 5) rarity = 3;
                foreach (string s in strings)
                {
                    string id = $"Undead_Phrase_{i}_{s}";
                    if(SUndead.Undead_Phrase(i).Contains(id) || s == "finish")
                    {
                        ReligionTrait t = new ReligionTrait
                        {
                            id = id,
                            path_icon = "Icons/TestIcon",
                            group_id = "Undead_Phrase_" + i,
                            special_locale_id = $"{id}_id",
                            special_locale_description = $"{id}_des",
                            special_locale_description_2 = $"{id}_des2",
                            rarity = (Rarity)rarity
                        };
                        t.base_stats = new BaseStats();
                        t.base_stats.addTag("Undead");
                        t.spells = new List<SpellAsset>();
                        AssetManager.religion_traits.add(t);
                    }                
                }
            }
            ReligionTrait start = new ReligionTrait//化身天灾
            {
                id = "Undead_Phrase_Start",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
                special_locale_id = "Undead_Phrase_Start_id",
                special_locale_description = "Undead_Phrase_Start_des",
                special_locale_description_2 = "Undead_Phrase_Start_des2"
            };
            start.base_stats = new BaseStats();
            start.base_stats.addTag("Undead");
            start.plot_id = "Undead_Research_Secret";
            start.spawn_random_trait_allowed = false;
            start.rarity = Rarity.R3_Legendary;
            AssetManager.religion_traits.add(start);

            ReligionTrait Blasphemy = new ReligionTrait//渎神仪式
            {
                id = "Undead_Blasphemy",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
                special_locale_id = "Undead_Blasphemy_id",
                special_locale_description = "Undead_Blasphemy_des",
                special_locale_description_2 = "Undead_Blasphemy_des2"
            };
            Blasphemy.base_stats = new BaseStats();
            Blasphemy.base_stats.addTag("Undead");
            Blasphemy.plot_id = "Undead_Blasphemy_Plot";
            Blasphemy.spawn_random_trait_allowed = true;
            Blasphemy.spawn_random_rate = 1;
            Blasphemy.rarity = Rarity.R3_Legendary;
            AssetManager.religion_traits.add(Blasphemy);

            var trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_2_curse);
            trait.spells.Add(Undead_Spell.curse_2);
            
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_2_finish);
            trait.spells.Add(Undead_Spell.curse_2);
            trait.base_stats["mana"] = 100;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_curse);
            trait.spells.Add(Undead_Spell.curse_3);
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_corrupt);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_finish);
            trait.spells.Add(Undead_Spell.curse_3);
            trait.action_special_effect = Undead_Action.Corrupt_action;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_4_corrupt);
            trait.spells.Add(Undead_Spell.corrupt_4);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_4_finish);
            trait.spells.Add(Undead_Spell.corrupt_4);
            trait.action_special_effect = Undead_Action.Corrupt_action;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_curse);
            trait.spells.Add(Undead_Spell.curse_5);
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_corrupt);
            trait.spells.Add(Undead_Spell.corrupt_5);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_finish);
            trait.spells.Add(Undead_Spell.curse_5);
            trait.spells.Add(Undead_Spell.corrupt_5);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            return;
        }

        public static void setAchievementUnlock(ActorTrait trait,string achievement)
        {
            trait.achievement_id = achievement;
            Achievement p = AssetManager.achievements.get(achievement);
            p.unlock_assets.Add(trait);
            if(p.isUnlocked())
            {
                trait.unlock();
            }
        }
     }
}
