using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undeads.Code.Behaviour;

namespace Undeads.Code
{
    class Undead_Spell
    {
        public static SpellAsset summon_undeads = new SpellAsset();
        public static void init()
        {
            SpellAsset spell = new SpellAsset();
            spell.id = "summon_undeads";
            spell.cost_mana = 30;
            spell.cast_target = CastTarget.Himself;
            spell.can_be_used_in_combat = true;
            spell.min_distance = 0;
            spell.action = Undead_Action.summon_undead;
            spell.decisions_assets = new DecisionAsset[] { Undead_Decision.summon_undeads };
            AssetManager.spells.add(spell);
            summon_undeads = spell;

            spell = new SpellAsset();
            spell.id = "speard_curse_biome";
            spell.cost_mana = 20;
            spell.cast_target = CastTarget.Himself;
            spell.can_be_used_in_combat = true;
            spell.min_distance = 0;
            spell.action = Undead_Action.speard_curse_biome;
            spell.decisions_assets = new DecisionAsset[] { Undead_Decision.speard_curse_biome };
            AssetManager.spells.add(spell);
            summon_undeads = spell;
        }

    }
}
