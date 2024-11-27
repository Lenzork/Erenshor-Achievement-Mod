using Erenshor_Achievement_Mod.Core;
using Erenshor_Achievement_Mod.Core.Class;
using MelonLoader;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Erenshor_Achievement_Mod
{
    public class Mod : MelonMod
    {
        public override void OnLateInitializeMelon()
        {
            Database.CreateLocalDatabase();
        }

        public override async void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            // Only load up when in Playable Scene
            if (!SceneValidator.IsValidScene(sceneName))
            {
                MelonEvents.OnGUI.Unsubscribe(AchievementWindow.DrawAchievementButton);
                AchievementWindow.SetShowAchievementWindow(false);
                Achievement.loadedAchievements.Clear();
                CombatAchievement.CheckingAchievements.Clear();
                QuestAchievement.CheckingAchievements.Clear();
                CharacterAchievement.CheckingAchievements.Clear();
                Achievement.SetGainedAchievementPoints(0);
            }

            if(GameObject.Find("Player") != null && SceneValidator.IsValidScene(sceneName))
                await LoadupAchievements();
        }
        
        private static async Task LoadupAchievements()
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

        public override void OnGUI()
        {
            if (AchievementWindow.GetShowAchievementWindow())
            {
                AchievementWindow.SetWindowRect(GUI.Window(0, AchievementWindow.GetWindowRect(), AchievementWindow.AchievementWindowFunction, "Achievements"));
            }
        }
    }
}
