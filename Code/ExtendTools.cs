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
        private static readonly ConditionalWeakTable<Status, StatusExtend> _extends = new();

        public static StatusExtend GetExtend(this Status status, bool create = true)
        {
            if (!_extends.TryGetValue(status, out StatusExtend extend) && create)
            {
                extend = new StatusExtend();
                _extends.Add(status, extend);
            }

            return extend;
        }

        public static void addStatusEffect(this BaseSimObject @base, string pID, Actor pFrom, float pOverrideTimer = 0f, bool pColorEffect = true)
        {
            @base.addStatusEffect(pID,pOverrideTimer,pColorEffect);
            @base.a._active_status_dict.TryGetValue(pID,out Status stat);
            StatusExtend extend = stat.GetExtend();
            extend.pFrom = pFrom;
        }
    }
}
