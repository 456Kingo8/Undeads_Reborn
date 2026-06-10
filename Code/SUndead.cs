using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Undeads.Code
{
    static class SUndead
    {
        public static readonly string Undeads_Phrase_1_soul = "Undeads_Phrase_1_soul";
        public static string Undeads_Phrase_1_curse = null;
        public static string Undeads_Phrase_1_corrupt = null;
        public static readonly string Undeads_Phrase_1_craft = "Undeads_Phrase_1_craft";
        public static readonly string Undeads_Phrase_1_special = "Undeads_Phrase_1_special";
        public static readonly string Undeads_Phrase_1_finish = "Undeads_Phrase_1_finish";
        public static List<string> Undeads_Phrase_1_normal = new List<string>() { Undeads_Phrase_1_soul, Undeads_Phrase_1_craft, Undeads_Phrase_1_special };


        public static readonly string Undeads_Phrase_2_soul = "Undeads_Phrase_2_soul";
        public static readonly string Undeads_Phrase_2_curse = "Undeads_Phrase_2_curse";
        public static readonly string Undeads_Phrase_2_corrupt = "Undeads_Phrase_2_corrupt";
        public static readonly string Undeads_Phrase_2_craft = "Undeads_Phrase_2_craft";
        public static readonly string Undeads_Phrase_2_special = "Undeads_Phrase_2_special";
        public static readonly string Undeads_Phrase_2_finish = "Undeads_Phrase_2_finish";
        public static List<string> Undeads_Phrase_2_normal = new List<string>() { Undeads_Phrase_2_soul,Undeads_Phrase_2_curse,Undeads_Phrase_2_corrupt, Undeads_Phrase_2_craft, Undeads_Phrase_2_special };

        public static readonly string Undeads_Phrase_3_soul = "Undeads_Phrase_3_soul";
        public static readonly string Undeads_Phrase_3_curse = "Undeads_Phrase_3_curse";
        public static readonly string Undeads_Phrase_3_corrupt = "Undeads_Phrase_3_corrupt";
        public static string Undeads_Phrase_3_craft = null;
        public static readonly string Undeads_Phrase_3_special = "Undeads_Phrase_3_special";
        public static readonly string Undeads_Phrase_3_finish = "Undeads_Phrase_3_finish";
        public static List<string> Undeads_Phrase_3_normal = new List<string>() { Undeads_Phrase_3_soul, Undeads_Phrase_3_curse, Undeads_Phrase_3_corrupt, Undeads_Phrase_3_special };

        public static readonly string Undeads_Phrase_4_soul = "Undeads_Phrase_4_soul";
        public static readonly string Undeads_Phrase_4_curse = "Undeads_Phrase_4_curse";
        public static readonly string Undeads_Phrase_4_corrupt = "Undeads_Phrase_4_corrupt";
        public static readonly string Undeads_Phrase_4_craft = "Undeads_Phrase_4_craft";
        public static string Undeads_Phrase_4_special = null;
        public static readonly string Undeads_Phrase_4_finish = "Undeads_Phrase_4_finish";
        public static List<string> Undeads_Phrase_4_normal = new List<string>() { Undeads_Phrase_4_soul, Undeads_Phrase_4_curse, Undeads_Phrase_4_corrupt, Undeads_Phrase_4_craft };

        public static readonly string Undeads_Phrase_5_soul = "Undeads_Phrase_5_soul";
        public static readonly string Undeads_Phrase_5_curse = "Undeads_Phrase_5_curse";
        public static readonly string Undeads_Phrase_5_corrupt = "Undeads_Phrase_5_corrupt";
        public static string Undeads_Phrase_5_craft = null;
        public static readonly string Undeads_Phrase_5_special = "Undeads_Phrase_5_special";
        public static readonly string Undeads_Phrase_5_finish = "Undeads_Phrase_5_finish";
        public static List<string> Undeads_Phrase_5_normal = new List<string>() { Undeads_Phrase_5_soul, Undeads_Phrase_5_curse, Undeads_Phrase_5_corrupt, Undeads_Phrase_5_special };

        public static List<string> Undeads_Phrase(int phrase)
        {
            switch (phrase)
            {
                case 1:
                    return Undeads_Phrase_1_normal;
                case 2:
                    return Undeads_Phrase_2_normal;
                case 3:
                    return Undeads_Phrase_3_normal;
                case 4:
                    return Undeads_Phrase_4_normal;
                case 5:
                    return Undeads_Phrase_5_normal;
                default:
                    MonoBehaviour.print("Undead_Error");
                    return null;
            }
        }


    }
}
