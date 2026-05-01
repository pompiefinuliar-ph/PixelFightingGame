using System.Drawing;

namespace PixelFightingGame
{
    public class Stage
    {
        public string StageName { get; set; }
        public ElementType BoostedElement { get; set; }
        public float BoostMultiplier { get; set; }
        public Color StageColor { get; set; }
        public string ImagePath { get; set; } // --- NEW: Holds the stage background image ---

        public Stage(string name, ElementType element, float multiplier, Color color, string imagePath)
        {
            StageName = name;
            BoostedElement = element;
            BoostMultiplier = multiplier;
            StageColor = color;
            ImagePath = imagePath;
        }
    }
}