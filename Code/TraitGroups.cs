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
            ActorTraitGroupAsset Undead = new ActorTraitGroupAsset();
            Undead.id = "Undead";
            Undead.name = "trait_group_Undead";
            Undead.color = "#480073";
            AssetManager.trait_groups.add(Undead);

            //ActorTraitGroupAsset Shengs = new ActorTraitGroupAsset();
            //Shengs.id = "Blessed";
            //Shengs.name = "trait_group_Blessed";
            //Shengs.color = "#F7FF68";
            //AssetManager.trait_groups.add(Shengs);
        }
    }
}
