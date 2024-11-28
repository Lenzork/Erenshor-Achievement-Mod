using System;
using System.Data.SQLite;
using MelonLoader;
using static Erenshor_Achievement_Mod.Core.Class.Achievement;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Erenshor_Achievement_Mod.Core.Class
{
    static class Database
    {
        public static void CreateLocalDatabase()
        {
            if (!File.Exists("Achievements.db"))
                Melon<Mod>.Logger.BigError("Achievements.db was not found. Please download the newest Version from the Github.");
        }

        public static async Task LoadupAchievements()
        {
            // Load up all Achievements
            if (File.Exists("Achievements.db"))
            {

                if (Achievement.loadedAchievements.Count <= 0)
                    await Database.FetchAchievements(GameObject.Find("Player").GetComponent<Stats>().MyName);

                // Insert Character into Database
                await Database.InsertNewCharacterEntry(GameObject.Find("Player").GetComponent<Stats>().MyName);

                MelonEvents.OnGUI.Subscribe(AchievementWindow.DrawAchievementButton, 100);
            }
        }

        public static async Task InsertNewCharacterEntry(string characterName)
        {
            string connectionString = "Data Source=Achievements.db;Version=3;";
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString))
            {
                await m_dbConnection.OpenAsync();

                // Check if the character already exists
                string checkSql = "SELECT COUNT(*) FROM characters WHERE characterName = @characterName";
                using (SQLiteCommand checkCmd = new SQLiteCommand(checkSql, m_dbConnection))
                {
                    checkCmd.Parameters.AddWithValue("@characterName", characterName);
                    long count = (long)await checkCmd.ExecuteScalarAsync();

                    if (count > 0)
                    {
                        return;
                    }
                }

                // Insert new character entry if it doesn't exist
                string insertSql = "INSERT INTO characters (characterName) VALUES (@characterName)";
                using (SQLiteCommand insertCmd = new SQLiteCommand(insertSql, m_dbConnection))
                {
                    insertCmd.Parameters.AddWithValue("@characterName", characterName);
                    await insertCmd.ExecuteNonQueryAsync();
                }

                Melon<Mod>.Logger.Msg($"INSERTED NEW ENTRY FOR {characterName}");
            }
        }

        public static async Task InsertCompletedAchievement(string characterName, int achievementId)
        {
            string connectionString = "Data Source=Achievements.db;Version=3;";
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString))
            {
                await m_dbConnection.OpenAsync();

                string getCharacterIdSql = "SELECT id FROM characters WHERE characterName = @characterName";
                using (SQLiteCommand getCharacterIdCommand = new SQLiteCommand(getCharacterIdSql, m_dbConnection))
                {
                    getCharacterIdCommand.Parameters.AddWithValue("@characterName", characterName);
                    object result = await getCharacterIdCommand.ExecuteScalarAsync();

                    if (result != null)
                    {
                        int characterId = Convert.ToInt32(result);

                        string insertAchievementSql = "INSERT INTO characters_achievements (characterId, achievementId) VALUES (@characterId, @achievementId)";
                        using (SQLiteCommand insertAchievementCommand = new SQLiteCommand(insertAchievementSql, m_dbConnection))
                        {
                            insertAchievementCommand.Parameters.AddWithValue("@characterId", characterId);
                            insertAchievementCommand.Parameters.AddWithValue("@achievementId", achievementId);
                            await insertAchievementCommand.ExecuteNonQueryAsync();
                        }
                    }
                    else
                    {
                        Melon<Mod>.Logger.Msg($"Character with name {characterName} not found.");
                    }
                }
            }
        }

        public static async Task FetchAchievements(string characterName)
        {
            MelonEvents.OnGUI.Subscribe(AchievementWindow.DrawAchievementLoadingText, 100);

            string connectionString = "Data Source=Achievements.db;Version=3;";

            using (SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString))
            {
                await m_dbConnection.OpenAsync();

                using (SQLiteTransaction transaction = m_dbConnection.BeginTransaction())
                {
                    Melon<Mod>.Logger.Msg("Connection open");

                    string sql = @"
                SELECT a.id, a.displayName, a.name, a.description, a.amount, a.rewardedAchievementPoints, a.category,
                       CASE WHEN ca.achievementId IS NOT NULL THEN 1 ELSE 0 END AS isCompleted
                FROM achievements a
                LEFT JOIN (
                    SELECT achievementId 
                    FROM characters_achievements 
                    WHERE characterId = (SELECT id FROM characters WHERE characterName = @characterName)
                ) ca
                ON a.id = ca.achievementId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, m_dbConnection))
                    {
                        cmd.Parameters.AddWithValue("@characterName", characterName);

                        using (SQLiteDataReader reader = (SQLiteDataReader)await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int id = reader.GetInt32(0);
                                string displayName = reader.GetString(1);
                                string name = reader.GetString(2);
                                string description = reader.GetString(3);
                                int amount = reader.GetInt32(4);
                                int rewardedAchievementPoints = reader.GetInt32(5);
                                int category = reader.GetInt32(6);
                                bool isCompleted = reader.GetInt32(7) == 1;

                                Achievement achievement = new Achievement(id, displayName, name, description, amount, rewardedAchievementPoints, (AchievementCategory)category);

                                if (isCompleted)
                                {
                                    achievement.CompleteAchievement();
                                }

                                Achievement.loadedAchievements.Add(achievement);
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            foreach (var achievement in Achievement.loadedAchievements)
            {
                Melon<Mod>.Logger.Msg($"Loaded Achievement: {achievement.DisplayName}, Category: {achievement.Category}, Completed: {achievement.IsCompleted()}");
            }

            MelonEvents.OnGUI.Unsubscribe(AchievementWindow.DrawAchievementLoadingText);
        }

    }
}
