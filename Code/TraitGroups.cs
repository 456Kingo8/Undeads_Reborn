using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
    class TraitGroups
    {
        public static void init()
        {
            ActorTraitGroupAsset Undead_actor = new ActorTraitGroupAsset();
            Undead_actor.id = "Undead";
            Undead_actor.name = "actor_trait_group_Undead";
            Undead_actor.color = "#480073";
            AssetManager.trait_groups.add(Undead_actor);

            ReligionTraitGroupAsset Undead_religion = new ReligionTraitGroupAsset();
            Undead_religion.id = "Undead";
            Undead_religion.name = "religion_trait_group_Undead";
            Undead_religion.color = "#480073";
            AssetManager.religion_trait_groups.add(Undead_religion);
            //ActorTraitGroupAsset Shengs = new ActorTraitGroupAsset();
            //Shengs.id = "Blessed";
            //Shengs.name = "trait_group_Blessed";
            //Shengs.color = "#F7FF68";
            //AssetManager.trait_groups.add(Shengs);
        }
    }
}
