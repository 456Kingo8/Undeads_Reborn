using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
    class Undead_Status
    {
        public static void init()
        {
            StatusAsset asset = new StatusAsset();
            asset.id = "whisper_of_death";
            asset.path_icon = "Icons/iconNecromancer";
            asset.duration = 30;
            asset.action_death = Undead_Action.whisper_of_death_Action_death;
            asset.action = Undead_Action.whisper_of_death_Action;
            asset.locale_id = "whisper_of_death_id";
            asset.locale_description = "whisper_of_death_des";
            asset.action_interval = 0.1f;
            AssetManager.status.add(asset);
        }
    }
}
