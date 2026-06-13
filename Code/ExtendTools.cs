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

        public static void refresh(this Actor actor)
        {
            var dict = actor.getStatuses();
            foreach (var item in dict)
            {
                actor.finishStatusEffect(item.asset.id);
            }
            actor.refresh_Trait();
        }

        public static void refresh_Trait(this Actor actor)
        {
            actor.removeTrait("tumor_infection");
            actor.removeTrait("plague");
            actor.removeTrait("mush_spores");
            actor.removeTrait("infected");
            actor.removeTrait("tumor_infection");
            actor.removeTrait("madness");
            actor.removeTrait("desire_alien_mold");
            actor.removeTrait("desire_golden_egg");
            actor.removeTrait("desire_computer");
            actor.removeTrait("desire_harp");
            actor.removeTrait("crippled");
            actor.removeTrait("skin_burns");
            actor.addTrait("immune");
        }
    }
}
