using Erenshor_Achievement_Mod.Core.Class;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Erenshor_Achievement_Mod
{
    //TODO: MAYBE SQLITE VERWENDEN FÜR DATENBANK UND EINEN TABLE FÜR DIE ACHIEVEMENTS MACHEN & EINE FÜR DIE PLAYERS WELCHE DIE ACHIEVEMENTS ABSCHLIEßEN

    public class Mod : MelonMod
    {
        public override async void OnLateInitializeMelon()
        {
            await Database.CreateLocalDatabase();
        }

        public override async void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            // Only load up when in Playable Scene
            if (!IsValidScene(sceneName))
            {
                MelonEvents.OnGUI.Unsubscribe(AchievementWindow.DrawAchievementButton);
                AchievementWindow.SetShowAchievementWindow(false);
                Achievement.loadedAchievements.Clear();
                CombatAchievement.checkingAchievements.Clear();
                QuestAchievement.checkingAchievements.Clear();
                CharacterAchievement.checkingAchievements.Clear();
                Achievement.SetGainedAchievementPoints(0);
            }

            if(GameObject.Find("Player") != null && IsValidScene(sceneName))
                await LoadupAchievements();
        }

        private static bool IsValidScene(string sceneName)
        {
            return sceneName == "Stowaway";
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

        public override async void OnFixedUpdate()
        {

            if (Input.GetKeyDown(KeyCode.F3))
            {
                await LoadupAchievements();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                foreach (var item in Achievement.loadedAchievements)
                {
                    item.CompleteAchievement();
                }
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
