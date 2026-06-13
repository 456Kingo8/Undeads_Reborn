using NeoModLoader.api.attributes;
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
        [Hotfixable]
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
                for(int j = 0;j < strings.Count;j++)
                {
                    string s = strings[j];
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
                        if(s == "finish")
                        {
                            t.opposite_traits = new ();
                            foreach(string str in SUndead.Undead_Phrase(i))
                            {
                                t.opposite_traits.Add(AssetManager.religion_traits.get(str));
                            }
                        }
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
            start.base_stats["lifespan"] = -10;
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
            setAchievementUnlock(Blasphemy, "achievementCursedWorld");


            var trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_1_special);
            trait.base_stats["lifespan"] = 5;
            trait.base_stats["multiplier_health"] = 0.1f;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_2_curse);
            trait.spells.Add(Undead_Spell.curse_2);
            trait.base_stats["mana"] = 20;
            trait.base_stats["range"] = 2;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_2_soul);
            trait.base_stats["damage"] = 10;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_2_corrupt);
            trait.base_stats["speed"] = 3;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_2_finish);
            trait.spells.Add(Undead_Spell.curse_2);
            trait.base_stats["mana"] = 20;
            trait.base_stats["range"] = 2;
            trait.base_stats["damage"] = 10;
            trait.base_stats["speed"] = 3;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_soul);
            trait.base_stats["damage"] = 15;
            trait.action_attack_target = Undead_Action.Soul_3_action;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_curse);
            trait.spells.Add(Undead_Spell.curse_3);
            trait.base_stats["mana"] = 30;
            trait.base_stats["attack_speed"] = 2;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_corrupt);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait.base_stats["speed"] = 5;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_3_finish);
            trait.spells.Add(Undead_Spell.curse_3);
            trait.action_attack_target = Undead_Action.Soul_3_action;
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait.base_stats["mana"] = 30;
            trait.base_stats["attack_speed"] = 2;
            trait.base_stats["speed"] = 5;
            trait.base_stats["damage"] = 15;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_4_curse);
            trait.base_stats["damage"] = 10;
            trait.base_stats["multiplier_damage"] = 0.4f;
            trait.base_stats["attack_speed"] = 3;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_4_corrupt);
            trait.spells.Add(Undead_Spell.corrupt_4);
            trait.base_stats["speed"] = 10;
            trait.base_stats["lifespan"] = 20;
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_4_soul);
            trait.spells.Add(Undead_Spell.soul_4);
            trait.base_stats["mana"] = 100;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_4_finish);
            trait.spells.Add(Undead_Spell.corrupt_4);
            trait.spells.Add(Undead_Spell.soul_4);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait.base_stats["damage"] = 10;
            trait.base_stats["multiplier_damage"] = 0.4f;
            trait.base_stats["attack_speed"] = 3;
            trait.base_stats["mana"] = 100;
            trait.base_stats["speed"] = 10;
            trait.base_stats["lifespan"] = 20;

            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_curse);
            trait.spells.Add(Undead_Spell.curse_5);
            trait.base_stats["damage"] = 20;
            trait.base_stats["multiplier_damage"] = 0.6f;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_special);
            trait.action_special_effect= Undead_Action.Special_5_action;
            trait.spells.Add(Undead_Spell.special_5);
            trait.base_stats["health"] = 200;
            trait.base_stats["multiplier_health"] = 1f;
            trait.base_stats["armor"] = 15f;
            trait.base_stats["lifespan"] = 100;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_corrupt);
            trait.base_stats["speed"] = 15;
            trait.base_stats["mana"] = 100;
            trait.base_stats["attack_speed"] = 5;
            trait.spells.Add(Undead_Spell.corrupt_5);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait = AssetManager.religion_traits.get(SUndead.Undead_Phrase_5_finish);
            trait.base_stats["health"] = 200;
            trait.base_stats["multiplier_health"] = 1f;
            trait.base_stats["damage"] = 20;
            trait.base_stats["multiplier_damage"] = 0.5f;
            trait.base_stats["armor"] = 15f;
            trait.base_stats["speed"] = 15;
            trait.base_stats["lifespan"] = 100;
            trait.base_stats["mana"] = 100;
            trait.base_stats["attack_speed"] = 10;
            trait.spells.Add(Undead_Spell.curse_5);
            trait.spells.Add(Undead_Spell.corrupt_5);
            trait.spells.Add(Undead_Spell.special_5);
            trait.action_special_effect = Undead_Action.Corrupt_action;
            trait.action_special_effect += Undead_Action.Special_5_action;

            return;
        }

        public static void setAchievementUnlock(ReligionTrait trait,string achievement)
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
