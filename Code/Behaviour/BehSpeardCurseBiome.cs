using ai.behaviours;
using UnityEngine;

namespace Undeads.Code.Behaviour
{
    public class BehSpeardCurseBiome : BehaviourActionActor
    {
        public override BehResult execute(Actor pObject)
        {
            SpellAsset tSpellAsset = AssetManager.spells.get("speard_curse_biome");
            if (tSpellAsset.action != null && pObject.hasEnoughMana(tSpellAsset.cost_mana))
            {
                pObject.restoreMana(-tSpellAsset.cost_mana);
                tSpellAsset.action.RunAnyTrue(pObject, pObject, pObject.current_tile);
            }
            return BehResult.Continue;
        }
    }
}