using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
    class Undead_Era
    {
        public static void init()
        {
            WorldAgeAsset asset = new WorldAgeAsset();
            asset.overlay_chaos = true;
            asset.particles_rain = true;
            asset.particles_ash = true;
            asset.particles_magic = true;
            asset.particles_sun = true;
            asset.flag_winter = true;
            asset.id = "age_destruction";
            asset.years_min = 1000000;
            asset.years_max = 1000000;
            asset.path_icon = AssetManager.era_library.get("age_despair").path_icon;
            asset.flag_crops_grow = false;
            asset.title_color = Toolbox.makeColor("#E6503A");
            asset.era_effect_overlay_alpha = 0.45f;
            AssetManager.era_library.add(asset);
        }
    }
}
