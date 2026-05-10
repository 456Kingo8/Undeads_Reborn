using ai.behaviours;
using UnityEngine;

namespace Undeads.Code.Behaviour
{
    public class BehSpeardCurseBiome : BehaviourActionActor
    {
        public override BehResult execute(Actor pObject)
        {
            World.world.StartCoroutine(Undead_Action.Spread_Biome(pObject, "biome_corrupted",10));
            return BehResult.Continue;
        }
    }
}