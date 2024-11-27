using System.Collections.Generic;
using MelonLoader;
using HarmonyLib;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal static class QuestAchievement
    {
        private static List<Achievement> checkingAchievements = new List<Achievement>();
        private static int completedQuests = 0;

        public static List<Achievement> CheckingAchievements { get => checkingAchievements; set => checkingAchievements = value; }

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
                    if (!ach.IsCompleted())
                    {
                        // Unique Checks
                        switch (ach.Name)
                        {
                            default:
                                if (completedQuests >= ach.Amount)
                                    ach.CompleteAchievement();
                                break;
                        }
                    }
                }
            }
        }
    }
}
