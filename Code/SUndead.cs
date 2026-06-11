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
        public static readonly string Undead_Phrase_Start = "Undead_Phrase_Start";

        public static readonly string Undead_Phrase_1_soul = "Undead_Phrase_1_soul";
        public static string Undead_Phrase_1_curse = null;
        public static string Undead_Phrase_1_corrupt = null;
        public static readonly string Undead_Phrase_1_craft = "Undead_Phrase_1_craft";
        public static readonly string Undead_Phrase_1_special = "Undead_Phrase_1_special";
        public static readonly string Undead_Phrase_1_finish = "Undead_Phrase_1_finish";
        public static List<string> Undead_Phrase_1_normal = new List<string>() { Undead_Phrase_1_soul, Undead_Phrase_1_craft, Undead_Phrase_1_special };


        public static readonly string Undead_Phrase_2_soul = "Undead_Phrase_2_soul";
        public static readonly string Undead_Phrase_2_curse = "Undead_Phrase_2_curse";
        public static readonly string Undead_Phrase_2_corrupt = "Undead_Phrase_2_corrupt";
        public static readonly string Undead_Phrase_2_craft = "Undead_Phrase_2_craft";
        public static readonly string Undead_Phrase_2_special = "Undead_Phrase_2_special";
        public static readonly string Undead_Phrase_2_finish = "Undead_Phrase_2_finish";
        public static List<string> Undead_Phrase_2_normal = new List<string>() { Undead_Phrase_2_soul,Undead_Phrase_2_curse,Undead_Phrase_2_corrupt, Undead_Phrase_2_craft, Undead_Phrase_2_special };

        public static readonly string Undead_Phrase_3_soul = "Undead_Phrase_3_soul";
        public static readonly string Undead_Phrase_3_curse = "Undead_Phrase_3_curse";
        public static readonly string Undead_Phrase_3_corrupt = "Undead_Phrase_3_corrupt";
        public static string Undead_Phrase_3_craft = null;
        public static readonly string Undead_Phrase_3_special = "Undead_Phrase_3_special";
        public static readonly string Undead_Phrase_3_finish = "Undead_Phrase_3_finish";
        public static List<string> Undead_Phrase_3_normal = new List<string>() { Undead_Phrase_3_soul, Undead_Phrase_3_curse, Undead_Phrase_3_corrupt, Undead_Phrase_3_special };

        public static readonly string Undead_Phrase_4_soul = "Undead_Phrase_4_soul";
        public static readonly string Undead_Phrase_4_curse = "Undead_Phrase_4_curse";
        public static readonly string Undead_Phrase_4_corrupt = "Undead_Phrase_4_corrupt";
        public static readonly string Undead_Phrase_4_craft = "Undead_Phrase_4_craft";
        public static string Undead_Phrase_4_special = null;
        public static readonly string Undead_Phrase_4_finish = "Undead_Phrase_4_finish";
        public static List<string> Undead_Phrase_4_normal = new List<string>() { Undead_Phrase_4_soul, Undead_Phrase_4_curse, Undead_Phrase_4_corrupt, Undead_Phrase_4_craft };

        public static readonly string Undead_Phrase_5_soul = "Undead_Phrase_5_soul";
        public static readonly string Undead_Phrase_5_curse = "Undead_Phrase_5_curse";
        public static readonly string Undead_Phrase_5_corrupt = "Undead_Phrase_5_corrupt";
        public static string Undead_Phrase_5_craft = null;
        public static readonly string Undead_Phrase_5_special = "Undead_Phrase_5_special";
        public static readonly string Undead_Phrase_5_finish = "Undead_Phrase_5_finish";
        public static List<string> Undead_Phrase_5_normal = new List<string>() { Undead_Phrase_5_soul, Undead_Phrase_5_curse, Undead_Phrase_5_corrupt, Undead_Phrase_5_special };

        public static List<string> Undead_Phrase(int phrase)
        {
            switch (phrase)
            {
                case 1:
                    return Undead_Phrase_1_normal;
                case 2:
                    return Undead_Phrase_2_normal;
                case 3:
                    return Undead_Phrase_3_normal;
                case 4:
                    return Undead_Phrase_4_normal;
                case 5:
                    return Undead_Phrase_5_normal;
                default:
                    MonoBehaviour.print("Undead_Error");
                    return null;
            }
        }


    }
}
