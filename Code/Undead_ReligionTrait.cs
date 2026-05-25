using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undeads.Code.Behaviour;

namespace Undeads.Code
{
     class Undead_ReligionTrait
     {
        public static void init()
        {
            ReligionTrait BattleContinue = new ReligionTrait
            {
                id = "Undead_Battle_Continue",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
                special_locale_id = "Undead_Battle_Continue_religion_id",
                special_locale_description = "Undead_Battle_Continue_religion_des",           
            };
            BattleContinue.base_stats = new BaseStats();
            BattleContinue.base_stats.addTag("Undead");
            AssetManager.religion_traits.add(BattleContinue);
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
