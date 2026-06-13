using Undeads.Code;
using NeoModLoader.api;
using Undeads.Code.Behaviour;
using UnityEngine;
using NeoModLoader.utils;
using UnityEngine.PlayerLoop;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using System.IO;

namespace Undeads
{
    public class Undeads_Reborn : BasicMod<Undeads_Reborn> ,IReloadable
    {
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
    }
}