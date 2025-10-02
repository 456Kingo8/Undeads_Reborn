using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
     class Undead_Trait
     {
        public static void init()
        {
            ActorTrait trait = new ActorTrait()
            {
                id = "Test",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
            };
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
