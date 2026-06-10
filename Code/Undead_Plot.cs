using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Undeads.Code
{
    class Undead_Plot
    {
        public static void init()
        {
            PlotAsset Research_Secret = AssetManager.plots_library.clone("Undead_Research_Secret", "new_book");
            Research_Secret.group_id = "religion";
            Research_Secret.money_cost = 0;
            //Research_Secret.path_icon = "Icon/TestIcon";
            Research_Secret.progress_needed = 60;
            Research_Secret.min_level = 0;
            Research_Secret.can_be_done_by_king = true;
            Research_Secret.can_be_done_by_leader = true;
            Research_Secret.check_is_possible = delegate(Actor pActor)
            {
                string str = get_Next_Research(pActor.religion);
                if (str == null) return false;
                int cost = get_Research_Cost(str);
                if (pActor.city != null && pActor.city.storages.Count > 0)
                {
                    int cur = pActor.city.storages.GetRandom().resources.get("Undead_Soul_Pieces");
                    if (cost < cur) return true;
                }
                return false;


            };
            Research_Secret.action = delegate (Actor pActor)
            {
                string next = get_Next_Research(pActor.religion);
                if (next == null) return false;
                pActor.religion.addTrait(next);//如果religion为null，则next==null，此情况已排除，故以下不再需要确定religion是否存在
                if(next.Contains("finish"))
                {
                    switch(next)
                    {
                        case "Undeads_Phrase_1_finish":
                            foreach(string str in SUndead.Undeads_Phrase_1_normal)
                                pActor.religion.removeTrait(str);
                            break;
                        case "Undeads_Phrase_2_finish":
                            foreach (string str in SUndead.Undeads_Phrase_2_normal)
                                pActor.religion.removeTrait(str);
                            break;
                        case "Undeads_Phrase_3_finish":
                            foreach (string str in SUndead.Undeads_Phrase_3_normal)
                                pActor.religion.removeTrait(str);
                            break;
                        case "Undeads_Phrase_4_finish":
                            foreach (string str in SUndead.Undeads_Phrase_4_normal)
                                pActor.religion.removeTrait(str);
                            break;
                        case "Undeads_Phrase_5_finish":
                            foreach (string str in SUndead.Undeads_Phrase_5_normal)
                                pActor.religion.removeTrait(str);
                            break;
                        default: break;
                    }
                }
                if (pActor.city != null && pActor.city.storages.Count > 0)
                {
                    pActor.city.storages.GetRandom()?.takeResource("Undead_Soul_Pieces", get_Research_Cost(next));
                }
                return true;
            };
        }

        public static string get_Next_Research(Religion religion)
        {
            string result = null;
            bool flag = true;
            List<string> list = new List<string>();
            if (religion == null) return null;
            if (religion.hasTrait(SUndead.Undeads_Phrase_5_finish))
            {
                return null;
            }
            else if (religion.hasTrait(SUndead.Undeads_Phrase_4_finish))
            {
                foreach (string s in SUndead.Undeads_Phrase_5_normal)
                {
                    if (!religion.hasTrait(s))
                    {
                        flag = false;
                        list.Add(s);
                    }
                }
                if (flag) result = SUndead.Undeads_Phrase_5_finish;
                else result = list.GetRandom();
            }
            else if (religion.hasTrait(SUndead.Undeads_Phrase_3_finish))
            {
                foreach (string s in SUndead.Undeads_Phrase_4_normal)
                {
                    if (!religion.hasTrait(s))
                    {
                        flag = false;
                        list.Add(s);
                    }
                }
                if (flag) result = SUndead.Undeads_Phrase_4_finish;
                else result = list.GetRandom();
            }
            else if (religion.hasTrait(SUndead.Undeads_Phrase_2_finish))
            {
                foreach (string s in SUndead.Undeads_Phrase_3_normal)
                {
                    if (!religion.hasTrait(s))
                    {
                        flag = false;
                        list.Add(s);
                    }
                }
                if (flag) result = SUndead.Undeads_Phrase_3_finish;
                else result = list.GetRandom();
            }
            else if (religion.hasTrait(SUndead.Undeads_Phrase_1_finish))
            {
                foreach (string s in SUndead.Undeads_Phrase_2_normal)
                {
                    if (!religion.hasTrait(s))
                    {
                        flag = false;
                        list.Add(s);
                    }
                }
                if (flag) result = SUndead.Undeads_Phrase_2_finish;
                else result = list.GetRandom();
            }
            else
            {
                foreach (string s in SUndead.Undeads_Phrase_1_normal)
                {
                    if (!religion.hasTrait(s))
                    {
                        flag = false;
                        list.Add(s);
                    }
                }
                if (flag) result = SUndead.Undeads_Phrase_1_finish;
                else result = list.GetRandom();
            }
            return result;
        }

        public static int get_Research_Cost(string research)
        {
            if (SUndead.Undeads_Phrase_5_normal.Contains(research)) return 400;
            if (SUndead.Undeads_Phrase_4_normal.Contains(research)) return 200;
            if (SUndead.Undeads_Phrase_3_normal.Contains(research)) return 100;
            if (SUndead.Undeads_Phrase_2_normal.Contains(research)) return 50;
            if (SUndead.Undeads_Phrase_1_normal.Contains(research)) return 25;
            return 0;
        }
    }
}
