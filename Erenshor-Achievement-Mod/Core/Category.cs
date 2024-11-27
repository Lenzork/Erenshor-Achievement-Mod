using System.Collections.Generic;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal class Category
    {
        public List<AchievementStruct> Combat { get; set; }
        public List<AchievementStruct> Character { get; set; }
        public List<AchievementStruct> Quests { get; set; }
    }
}
