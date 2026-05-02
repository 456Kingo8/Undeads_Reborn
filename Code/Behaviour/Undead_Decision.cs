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
        public static DecisionAsset speard_curse_biome = new DecisionAsset();
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
            add_Task(dec.id, "Icons/TestIcon", new BehSummonUndeads());

            dec = new DecisionAsset();
            dec.id = "speard_curse_biome";
            dec.priority = NeuroLayer.Layer_4_Critical;
            dec.path_icon = "Icons/TestIcon";
            dec.cooldown = 8;
            dec.unique = true;
            dec.weight = 2f;
            AssetManager.decisions_library.add(dec);
            speard_curse_biome = dec;
            add_Task(dec.id, "Icons/TestIcon", new BehSpeardCurseBiome());


        }

        private static BehaviourTaskActor add_Task(string id,string icon,BehaviourActionActor pBeh)
        {
            BehaviourTaskActor task = new BehaviourTaskActor();
            task.id = id;
            task.addBeh(pBeh);
            task.setIcon(icon);
            task.locale_key = "task_" + id;
            AssetManager.tasks_actor.add(task);
            return task;
        }
    }
}
