using FMOD;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using NeoModLoader.General.Event.Listeners;
using NeoModLoader.utils;
using System.IO;
using Undeads.Code;
using Undeads.Code.Behaviour;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections.Generic;

namespace Undeads
{
    public class Undeads_Reborn : BasicMod<Undeads_Reborn> ,IReloadable
    {
        public static List<long> LichLord_list = new List<long> ();
        public static List<long> LichLord_list_remove = new List<long>();

        public static Dictionary<long,float> LichLord_health_record = new();

        protected override void OnModLoad()
        {
            Config.isEditor = true;
            // Load your mod here
            // 加载你的mod内容
            TraitGroups.init();
            Undead_Status.init();
            Undead_Decision.init();//decision必须在spell之前
            Undead_Spell.init();//spell必须在trait之前
            Undead_Trait.init();
            Undead_ReligionTrait.init();
            Undead_Resource.init();
            Undead_Plot.init();
            Patches.init();
            Undead_Action.init();
            Undead_Era.init();
            // LogInfo(GetConfig()["Default"]["WhatToSay"].TextVal); // Call this only then you confirm it is a text config item
            //LogInfo(GetConfig()["Default"]["WhatToSay"].GetValue() as string);
        }
        [Hotfixable]
        public void Reload()
        {
            // 重载模组时重新加载语言文件  from inmny
            var locale_dir = GetLocaleFilesDirectory(GetDeclaration());
            foreach (var file in Directory.GetFiles(locale_dir))
            {
                if (file.EndsWith(".json"))
                {
                    LM.LoadLocale(Path.GetFileNameWithoutExtension(file), file);
                }
                else if (file.EndsWith(".csv"))
                {
                    LM.LoadLocales(file);
                }
            }
            LM.ApplyLocale();
        }
        [Hotfixable]
        public void Update()
        {
            if (World.world?.era_manager?._current_age == null) return;
            LichLord_list_remove.Clear();
            bool flag = false;
            if (LichLord_list.Count > 0)
            {
                foreach(var l in LichLord_list)
                {
                    var actor = World.world.units.get(l);
                    if (actor == null || ! actor.hasTrait("LichLord"))
                    {
                        LichLord_list_remove.Add(l);
                        continue;
                    }
                    if(Undead_Action.get_LichLord_title(actor) == 5) flag = true;
                    check_health(actor);
                }
            }

            foreach (var l in LichLord_list_remove)
            {
                LichLord_list.Remove(l);
                LichLord_health_record.Remove(l);
            }

            if(flag)
            {
                if(World.world.era_manager._current_age.id != "age_destruction")
                {
                    World.world.era_manager.setCurrentAge(AssetManager.era_library.get("age_destruction"), false);
                }
            }
            else
            {
                if (World.world.era_manager._current_age.id == "age_destruction")
                {
                    World.world.era_manager.startNextAge();
                }
            }
            if(Undead_Music.IsPlaying())
            {
                Undead_Music.SetVolume(PlayerConfig.getIntValue("volume_music") / 10000f * PlayerConfig.getIntValue("volume_master_sound"));
                if(LichLord_list.Count == 0) Undead_Music.Stop();
            }
        }

        public RESULT PlayLichLordMusic()
        {
            return Undead_Music.PlayFromMod(

                GetDeclaration().FolderPath,

                Path.Combine("GameResources/Audio", "决战时刻.ogg"),

                true,

                (float)PlayerConfig.getIntValue("volume_music") / 10000f * PlayerConfig.getIntValue("volume_master_sound")
            );

        }

        public static bool check_health(Actor actor)
        {
            bool flag = false;
            long l = actor.id;
            if (LichLord_health_record.ContainsKey(l))
            {
                if (LichLord_health_record[l] - actor.getHealthRatio() > 0.05f)
                {
                    flag = true;
                    MonoBehaviour.print("debug");
                    float eps = 0.05f;
                    if (actor.hasStatus("invincible")) eps = 0f;
                    else if (Undead_Action.get_LichLord_title(LichLord_health_record[l]) == 5) eps = 0.025f;
                    actor.data.health = (int)Mathf.Ceil(actor.getMaxHealth() * (LichLord_health_record[l] - eps));
                    if (eps > 0 && !actor.hasStatus("invincible"))
                    {
                        actor.addStatusEffect("invincible", 1f);
                    }
                    if(actor.data.health > 0) actor._alive = true;
                }
                LichLord_health_record[l] = actor.getHealthRatio();
            }
            else
            {
                LichLord_health_record.Add(l, actor.getHealthRatio());
            }
            return flag;
        }

    }
}