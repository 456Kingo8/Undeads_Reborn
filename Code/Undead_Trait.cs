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
            ActorTrait LichLord = new ActorTrait()
            {
                id = "LichLord",
                path_icon = "Icons/TestIcon",
                group_id = "Undead",
                special_locale_id = "LichLord_id",
                special_locale_description = "LichLord_des",
                action_attack_target = Undead_Action.LichLord_attack
            };
            AssetManager.traits.add(LichLord);
            setAchievementUnlock(LichLord, "achievementGreatPlague");



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
