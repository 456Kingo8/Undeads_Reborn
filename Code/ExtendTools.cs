using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Undeads.Code
{
    public static class ExtendTools
    {
        private static readonly ConditionalWeakTable<Status, FromExtend> _extends = new();

        public static FromExtend GetExtend(this Status status, bool create = true)
        {
            if (!_extends.TryGetValue(status, out FromExtend extend) && create)
            {
                extend = new FromExtend();
                _extends.Add(status, extend);
            }

            return extend;
        }

        public static void addStatusEffect(this BaseSimObject @base, string pID, Actor pFrom, float pOverrideTimer = 0f, bool pColorEffect = true)
        {
            @base.addStatusEffect(pID,pOverrideTimer,pColorEffect);
            @base.a._active_status_dict.TryGetValue(pID,out Status stat);
            FromExtend extend = stat.GetExtend();
            extend.pFrom = pFrom;
        }


        public static bool has_Undead_Trait(this Religion religion, string str, int phrase)
        {
            return religion.hasTrait(str) || religion.hasTrait($"Undead_Phrase_{phrase}_finish");
        }

        public static int count_Soul(this City city)
        {
            if (city.storages.Count == 0) return 0;
            else
            {
                int t = 0;
                foreach (Building b in city.storages)
                {
                    t += b.getResourcesAmount("Undead_Soul_Pieces");
                }
                return t;
            }
        }

        public static void remove_Soul(this City city, int cost)
        {
            if (city.storages.Count == 0) return;
            else
            {
                foreach (Building b in city.storages)
                {

                    int has = b.getResourcesAmount("Undead_Soul_Pieces");
                    int t;
                    if (has > cost)
                    {
                        t = cost;
                    }
                    else
                    {
                        t = has;
                    }
                    cost -= t;
                    b.addResources("Undead_Soul_Pieces", -t);

                }
            }
        }
    }
}
