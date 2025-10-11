using ai.behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code.Behaviour
{
    class Undead_Decision
    {
        public static DecisionAsset summon_undeads = new DecisionAsset();
        public static void init()
        {
            DecisionAsset dec = new DecisionAsset();
            dec.id = "summon_undeads";
            dec.priority = NeuroLayer.Layer_4_Critical;
            dec.path_icon = "Icons/TestIcon";
            dec.cooldown = 10;
            dec.unique = true;
            dec.weight = 2f;
            AssetManager.decisions_library.add(dec);
            summon_undeads = dec;

            BehaviourTaskActor task = new BehaviourTaskActor();
            task.id = "summon_undeads";
            task.addBeh(new BehSummonUndeads());
            task.setIcon("Icons/TestIcon");
            task.locale_key = "task_summon_undeads_id";
            AssetManager.tasks_actor.add(task);
        }
    }
}
