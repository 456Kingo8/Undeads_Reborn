using ModTemplate.Code;
using NeoModLoader.api;

namespace Undeads_Reborn
{
    public class Undeads_Reborn : BasicMod<Undeads_Reborn>
    {
        protected override void OnModLoad()
        {
            // Load your mod here
            // 加载你的mod内容
            Undead_Trait.init();
            // LogInfo(GetConfig()["Default"]["WhatToSay"].TextVal); // Call this only then you confirm it is a text config item
            LogInfo(GetConfig()["Default"]["WhatToSay"].GetValue() as string);
        }
    }
}