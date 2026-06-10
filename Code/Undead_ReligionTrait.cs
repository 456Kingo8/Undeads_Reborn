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
                foreach(string s in strings)
                {
                    string id = $"Undeads_Phrase_{i}_{s}";
                    if(SUndead.Undeads_Phrase(i).Contains(id) || s == "finish")
                    {
                        ReligionTrait t = new ReligionTrait
                        {
                            id = id,
                            path_icon = "Icons/TestIcon",
                            group_id = "Undead_Phrase_" + i,
                            special_locale_id = $"Undead_{id}_id",
                            special_locale_description = $"Undead_{id}_des",
                            special_locale_description_2 = $"Undead_{id}_des2",
                        };
                        t.base_stats = new BaseStats();
                        t.base_stats.addTag("Undead");
                        AssetManager.religion_traits.add(t);
                    }                
                }
            }
            ReligionTrait start = new ReligionTrait
            {
                id = "Undead_Phrase_Start",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
                special_locale_id = "Undead_Phrase_Start_id",
                special_locale_description = "Undead_Phrase_Start_des",
                special_locale_description_2 = "Undead_Phrase_Start_des2",
            };
            start.base_stats = new BaseStats();
            start.base_stats.addTag("Undead");
            start.plot_id = "Undead_Research_Secret";
            AssetManager.religion_traits.add(start);

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
