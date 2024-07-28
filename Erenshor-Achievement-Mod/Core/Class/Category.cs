using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erenshor_Achievement_Mod.Core.Class
{
    internal class Category
    {
        public List<AchievementStruct> Combat { get; set; }
        public List<AchievementStruct> Character { get; set; }
        public List<AchievementStruct> Quests { get; set; }
    }
}
