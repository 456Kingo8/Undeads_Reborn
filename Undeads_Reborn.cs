using Undeads.Code;
using NeoModLoader.api;
using Undeads.Code.Behaviour;

namespace Undeads
{
    public class Undeads_Reborn : BasicMod<Undeads_Reborn>
    {
        protected override void OnModLoad()
        {
            // Load your mod here
            // 加载你的mod内容
            TraitGroups.init();
            Undead_Status.init();
            Undead_Decision.init();//decision必须在spell之前
            Undead_Spell.init();//spell必须在trait之前
            Undead_Trait.init();
            // LogInfo(GetConfig()["Default"]["WhatToSay"].TextVal); // Call this only then you confirm it is a text config item
            LogInfo(GetConfig()["Default"]["WhatToSay"].GetValue() as string);
        }
    }
}