using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using UnityEngine;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal static class CharacterAchievement
    {
        public static List<Achievement> checkingAchievements = new List<Achievement>();

        // Everything regarding to Combat Achievements
        [HarmonyPatch(typeof(Stats), "DoLevelUp")]
        public static class Patch
        {
            private static void Postfix(Stats __instance)
            {
                // Check for Character Achievements
                foreach (var ach in checkingAchievements)
                {
                    if (ach.IsCompleted() == false)
                    {
                        // Unique Checks
                        switch (ach.Name)
                        {
                            default:
                                if (ach.Amount <= __instance.Level)
                                    ach.CompleteAchievement();
                                break;
                        }
                    }
                }
            }
        }
    }
}
