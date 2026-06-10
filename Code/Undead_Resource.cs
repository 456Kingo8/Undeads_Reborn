using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
    class Undead_Resource
    {
        public static void init()
        {
            ResourceAsset asset = AssetManager.resources.clone("Undead_Soul_Pieces", "adamantine");
            asset.maximum = 100000;
            asset.restore_happiness = 100;
            asset.restore_mana = 100;
            asset.restore_stamina = 100;
            asset.storage_max = 100000;
            asset.path_icon = "testicon2";

        }
    }
}
