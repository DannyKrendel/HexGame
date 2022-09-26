using System.Collections.Generic;

namespace HexGame.Gameplay
{
    public class PlatformHighlighter
    {
        public List<Platform> HighlightedPlatforms { get; }

        public PlatformHighlighter()
        {
            HighlightedPlatforms = new List<Platform>();
        }

        public void Highlight(List<Platform> platforms)
        {
            ClearHighlight();
            
            foreach (var platform in platforms)
            {
                HighlightedPlatforms.Add(platform);
                platform.Highlight();
            }
        }

        public void ClearHighlight()
        {
            foreach (var platform in HighlightedPlatforms)
                platform.ClearHighlight();
            HighlightedPlatforms.Clear();
        }
    }
}