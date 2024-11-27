using System.Linq;

namespace Erenshor_Achievement_Mod.Core
{
    public static class SceneValidator
    {
        private static string[] ValidScenes = new string[]
        {
            "Stowaway",
            "Brake",
            "Bonepits",
            "Vitheo",
            "Krakengard",
            "FernallaField",
            "SaltedStrand",
            "Elderstone",
            "Azure",
            "Rottenfoot",
            "Braxonian",
            "Silkengrass",
            "Underspine",
            "Loomingwood",
            "Duskenlight",
            "Windwashed",
            "Blight",
            "Malaroth",
            "Braxonia",
            "Soluna",
            "Ripper",
            "Abyssal",
            "VitheosEnd",
            "Azynthi",
            "AzynthiClear",
            "DuskenPortal",
            "Rockshade",
            "ShiveringTomb",
            "Undercity",
            "Jaws"
        };

        public static bool IsValidScene(string sceneName)
        {
            return ValidScenes.Contains(sceneName);
        }
    }
}
