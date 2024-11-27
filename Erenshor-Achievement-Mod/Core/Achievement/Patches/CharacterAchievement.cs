using System.Collections.Generic;
using HarmonyLib;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal static class CharacterAchievement
    {
        private static List<Achievement> checkingAchievements = new List<Achievement>();

        public static List<Achievement> CheckingAchievements { get => checkingAchievements; set => checkingAchievements = value; }

        // Everything regarding to Combat Achievements
        [HarmonyPatch(typeof(Stats), "DoLevelUp")]
        public static class Patch
        {
            private static void Postfix(Stats __instance)
            {
                // Check for Character Achievements
                foreach (var ach in checkingAchievements)
                {
                    if (!ach.IsCompleted())
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
