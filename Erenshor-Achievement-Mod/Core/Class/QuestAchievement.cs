using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal static class QuestAchievement
    {
        public static List<Achievement> checkingAchievements = new List<Achievement>();
        private static int completedQuests = 0;

        // Everything regarding to Combat Achievements
        [HarmonyPatch(typeof(GameData), "FinishQuest")]
        public static class Patch
        {
            private static void Postfix()
            {
                completedQuests++;

                // Check for Quest Achievements
                foreach (var ach in checkingAchievements)
                {
                    if (ach.IsCompleted() == false)
                    {
                        // Unique Checks
                        switch (ach.Name)
                        {
                            default:
                                if (completedQuests >= ach.Amount)
                                    ach.CompleteAchievement();

                                Melon<Mod>.Logger.Msg($"Geht in die Switch");
                                break;
                        }
                    }
                }
            }
        }
    }
}
