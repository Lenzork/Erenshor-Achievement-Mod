namespace Erenshor_Achievement_Mod.Core.Class
{
    internal class AchievementStruct
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int RewardedAchievementPoints { get; set; }
        public int Category { get; set; }
        public bool Completed { get; set; }
    }
}
