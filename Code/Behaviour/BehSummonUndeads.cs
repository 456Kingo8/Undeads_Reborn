using ai.behaviours;

namespace Undeads.Code.Behaviour
{
    public class BehSummonUndeads : BehaviourActionActor
    {
        public override BehResult execute(Actor pObject)
        {
            SpellAsset tSpellAsset = AssetManager.spells.get("summon_undeads");
            if(tSpellAsset.action != null && pObject.hasEnoughMana(tSpellAsset.cost_mana)) 
            {
                pObject.restoreMana(-tSpellAsset.cost_mana);
                tSpellAsset.action.RunAnyTrue(pObject, pObject, pObject.current_tile);
            }
            return BehResult.Continue;
        }
    }
}
