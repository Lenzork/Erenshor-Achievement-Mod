using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal class Achievement
    {
        // Stores all Achievements
        public static List<Achievement> loadedAchievements = new List<Achievement>();
        private static int GainedAchievementPoints = 0;

        public int Id {  get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public int RewardedAchievementPoints { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public AchievementCategory Category { get; set; }
        private bool Completed { get; set; }

        /*public Achievement(AchievementJson achievementJson)
        {
            DisplayName = achievementJson.DisplayName;
            Name = achievementJson.Name;
            RewardedAchievementPoints = achievementJson.RewardedAchievementPoints;
            Description = achievementJson.Description;
            Amount = achievementJson.Amount;
            Category = (AchievementCategory)achievementJson.Category;
            Completed = achievementJson.Completed;

            initTrigger(Category);
        }*/

        public Achievement(int id, string displayName, string name, string description, int amount, int rewardedAchievementPoints, AchievementCategory category)
        {
            Id = id;
            DisplayName = displayName;
            Name = name;
            RewardedAchievementPoints = rewardedAchievementPoints;
            Description = description;
            Amount = amount;
            Category = category;
            Completed = false;

            initTrigger(Category);
        }

        public enum AchievementCategory
        {
            Combat = 0,
            Character = 1,
            Quests = 2,
        }

        private void initTrigger(AchievementCategory category)
        {
            switch(category)
            {
                case AchievementCategory.Combat:
                    CombatAchievement.checkingAchievements.Add(this);
                    break;
                case AchievementCategory.Character:
                    CharacterAchievement.checkingAchievements.Add(this);
                    break;
                case AchievementCategory.Quests:
                    QuestAchievement.checkingAchievements.Add(this);
                    break;
            }
        }

        public void CompleteAchievement()
        {
            UpdateSocialLog.LogAdd($"Achievement {DisplayName} completed. You received {RewardedAchievementPoints} Achievement Points!", "yellow");
            Completed = true;
            GainedAchievementPoints += RewardedAchievementPoints;
            Database.InsertCompletedAchievement(GameObject.Find("Player").GetComponent<Stats>().MyName, Id);
        }

        public bool IsCompleted()
        {
            return Completed; 
        }

        public static int GetGainedAchievementPoints()
        {
            return GainedAchievementPoints;
        }

        public static void SetGainedAchievementPoints(int value)
        {
            GainedAchievementPoints = value;
        }
    }
}
