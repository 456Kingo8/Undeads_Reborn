using NeoModLoader.General;
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
        public static SpellAsset speard_curse_biome = new SpellAsset();
        public static SpellAsset curse_2 = new SpellAsset();
        public static SpellAsset curse_3 = new SpellAsset();
        public static SpellAsset curse_5 = new SpellAsset();
        public static SpellAsset corrupt_4 = new SpellAsset();
        public static SpellAsset corrupt_5 = new SpellAsset();
        public static SpellAsset soul_4 = new SpellAsset();
        public static SpellAsset special_5 = new SpellAsset();
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
            speard_curse_biome = spell;

            spell = new SpellAsset();
            spell.id = "Undead_Curse_2";
            spell.cost_mana = 5;
            spell.can_be_used_in_combat = true;
            spell.action = Undead_Action.curse_phrase_2;
            spell.min_distance = 0;
            AssetManager.spells.add(spell);
            curse_2 = spell;

            spell = new SpellAsset();
            spell.id = "Undead_Curse_3";
            spell.cost_mana = 10;
            spell.can_be_used_in_combat = true;
            spell.action = Undead_Action.curse_phrase_3;
            spell.min_distance = 0;
            AssetManager.spells.add(spell);
            curse_3 = spell;

            spell = new SpellAsset();
            spell.id = "Undead_Corrupt_4";
            spell.cost_mana = 4;
            spell.can_be_used_in_combat = false;
            spell.action = Undead_Action.Corrupt_4_spell;
            spell.min_distance = 0;
            AssetManager.spells.add(spell);
            corrupt_4 = spell;

            spell = new SpellAsset();
            spell.id = "Undead_Corrupt_5";
            spell.cost_mana = 30;
            spell.can_be_used_in_combat = true;
            spell.action = Undead_Action.Corrput_5_spell;
            spell.min_distance = 0;
            AssetManager.spells.add(spell);
            corrupt_5 = spell;

            spell = new SpellAsset();
            spell.id = "Undead_Soul_4";
            spell.cost_mana = 10;
            spell.can_be_used_in_combat = true;
            spell.action = Undead_Action.Soul_4_spell;
            spell.min_distance = 0;
            AssetManager.spells.add(spell);
            soul_4 = spell;

            spell = new SpellAsset();
            spell.id = "Undead_special_5";
            spell.cost_mana = 5;
            spell.can_be_used_in_combat = true;
            spell.action = Undead_Action.Special_5_spell;
            spell.min_distance = 0;
            AssetManager.spells.add(spell);
            special_5 = spell;
        }

    }
}
