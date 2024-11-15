using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal class AchievementWindow
    {
        private static bool showAchievementWindow = false;
        private static Rect windowRect = new Rect(20, 20, 600, 600);
        private static Achievement.AchievementCategory selectedCategory = Achievement.AchievementCategory.Combat;

        public static void DrawAchievementButton()
        {
            if (GUI.Button(new Rect(10, 10, 150, 30), "Show Achievements"))
            {
                showAchievementWindow = !showAchievementWindow;
            }
        }

        public static void DrawAchievementLoadingText()
        {
            GUI.Label(new Rect(20, 20, 1000, 200), "<b><color=cyan><size=12>Loading Achievements..</size></color></b>");
        }

        public static bool GetShowAchievementWindow()
        {
            return showAchievementWindow;
        }

        public static void SetShowAchievementWindow(bool value)
        {
            showAchievementWindow = value;
        }

        public static Rect GetWindowRect()
        {
            return windowRect;
        }

        public static void SetWindowRect(Rect value)
        {
            windowRect = value;
        }

        public static void AchievementWindowFunction(int windowID)
        {
            GUILayout.BeginHorizontal();

            // Linke Spalte: Kategorien
            GUILayout.BeginVertical(GUILayout.Width(200));
            if (GUILayout.Button("Combat"))
            {
                selectedCategory = Achievement.AchievementCategory.Combat;
            }
            if (GUILayout.Button("Character"))
            {
                selectedCategory = Achievement.AchievementCategory.Character;
            }
            if (GUILayout.Button("Quest"))
            {
                selectedCategory = Achievement.AchievementCategory.Quests;
            }
            GUILayout.EndVertical();

            // Mittlere Spalte: Achievements
            GUILayout.BeginVertical();

            // Get achievements of selected category
            var achievementsOfSelectedCategory = Achievement.loadedAchievements
                .Where(a => a.Category == selectedCategory)
                .OrderByDescending(a => a.IsCompleted()) // Sort by Completed status
                .ToList();

            foreach (var achievement in achievementsOfSelectedCategory)
            {
                Color originalColor = GUI.color;
                GUI.color = achievement.IsCompleted() ? Color.yellow : Color.white;

                GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(windowRect.width - 220)); // Adjust width to fit inside the window

                GUILayout.BeginHorizontal();
                GUILayout.Label(achievement.DisplayName, GUILayout.Width(windowRect.width - 340));
                GUILayout.FlexibleSpace();
                GUILayout.Label($"Points: {achievement.RewardedAchievementPoints}", GUILayout.Width(100));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.Label(achievement.Description);

                GUILayout.EndVertical();

                GUILayout.Space(10);

                GUI.color = originalColor;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            // Achievementpoints anzeigen
            GUILayout.BeginArea(new Rect(10, windowRect.height - 30, 200, 20));
            GUILayout.Label($"Achievementpoints: {Achievement.GetGainedAchievementPoints()}");
            GUILayout.EndArea();

            GUI.DragWindow();
        }
    }
}
