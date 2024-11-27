using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal static class CombatAchievement 
    {

        private static List<Achievement> checkingAchievements = new List<Achievement>();
        private static int kills = 0;

        public static List<Achievement> CheckingAchievements { get => checkingAchievements; set => checkingAchievements = value; }

        // Everything regarding to Combat Achievements
        [HarmonyPatch(typeof(Character), "DoDeath")]
        public static class Patch
        {
            private static void Postfix(Character __instance)
            {
                if (!__instance.isNPC)
                    return;

                if (__instance.gameObject.GetComponent<NPC>().AggroTable[0].Player != GameObject.Find("Player").GetComponent<Character>()) 
                    return;

                if (__instance.Alive)
                    return;

                kills++;

                // Check for Kill Achievements
                foreach (var ach in checkingAchievements)
                {
                    if(ach.Amount <= kills && !ach.IsCompleted())
                    {
                        // Unique Checks
                        switch(ach.Name)
                        {
                            case "KILL_VEVILLO_POLTER":
                                if (__instance.gameObject.name == "Vevillo Polter")
                                {
                                    ach.CompleteAchievement();
                                }
                                break;
                            case "KILL_PLAXITHERIS":
                                if (__instance.gameObject.name == "Plaxitheris")
                                {
                                    ach.CompleteAchievement();
                                }
                                break;
                            case "KILL_MOURNING":
                                if (__instance.gameObject.name == "Mourning")
                                {
                                    ach.CompleteAchievement();
                                }
                                break;
                            default:
                                ach.CompleteAchievement();
                                break;
                        }
                    }
                }
            }
        }


    }
}
