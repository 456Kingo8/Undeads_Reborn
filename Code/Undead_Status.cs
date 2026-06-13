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

            asset = new StatusAsset();
            asset.id = "Undead_Battle_Continue";
            asset.path_icon = "Icons/iconNecromancer";
            asset.duration = 5f;
            asset.locale_id = "Undead_Battle_Continue_status_id";
            asset.locale_description = "Undead_Battle_Continue_status_des";
            asset.action_finish = Undead_Action.Battle_Continue_finish;
            AssetManager.status.add(asset);

            asset = new StatusAsset();
            asset.id = "Undead_Corrupt_Buff_1";
            asset.path_icon = "Icons/iconNecromancer";
            asset.duration = 10f;
            asset.locale_id = "Undead_Corrupt_Buff_1_id";
            asset.locale_description = "Undead_Corrupt_Buff_1_des";
            asset.base_stats = new BaseStats();
            asset.base_stats["multiplier_damage"] = 0.2f;
            asset.base_stats["multiplier_health"] = 0.2f;
            asset.base_stats["multiplier_speed"] = 0.1f;
            asset.base_stats["armor"] = 10f;
            asset.action = Undead_Action.Corrput_Buff_action;
            asset.action_interval = 0.8f;
            AssetManager.status.add(asset);

            asset = new StatusAsset();
            asset.id = "Undead_Corrupt_Buff_2";
            asset.path_icon = "Icons/iconNecromancer";
            asset.duration = 20f;
            asset.locale_id = "Undead_Corrupt_Buff_2_id";
            asset.locale_description = "Undead_Corrupt_Buff_2_des";
            asset.base_stats["multiplier_damage"] = 0.5f;
            asset.base_stats["multiplier_health"] = 0.5f;
            asset.base_stats["multiplier_speed"] = 0.2f;
            asset.base_stats["armor"] = 20f;
            asset.action = Undead_Action.Corrput_Buff_action;
            asset.action_interval = 0.75f;
            AssetManager.status.add(asset);

            asset = new StatusAsset();
            asset.id = "Undead_Corrupt_Buff_3";
            asset.path_icon = "Icons/iconNecromancer";
            asset.duration = 30f;
            asset.locale_id = "Undead_Corrupt_Buff_3_id";
            asset.locale_description = "Undead_Corrupt_Buff_3_des";
            asset.base_stats["multiplier_damage"] = 1f;
            asset.base_stats["multiplier_health"] = 1f;
            asset.base_stats["multiplier_speed"] = 0.8f;
            asset.base_stats["armor"] = 30f;
            asset.action = Undead_Action.Corrput_Buff_action;
            asset.action_interval = 0.5f;
            AssetManager.status.add(asset);

            asset = new StatusAsset();
            asset.id = "Ark_Cooldown";//灵魂方舟冷却
            asset.path_icon = "Icons/iconNecromancer";
            asset.duration = 120f;
            asset.locale_id = "Undead_Ark_Cooldown_id";
            asset.locale_description = "Undead_Ark_Cooldown_des";
            AssetManager.status.add(asset);
        }
    }
}
