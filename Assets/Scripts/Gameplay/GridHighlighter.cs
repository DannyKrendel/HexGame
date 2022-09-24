using System.Collections.Generic;

namespace HexGame.Gameplay
{
    public class GridHighlighter
    {
        public List<HexCell> HighlightedCells { get; }

        public GridHighlighter()
        {
            HighlightedCells = new List<HexCell>();
        }

        public void Highlight(List<HexCell> cells)
        {
            ClearHighlight();
            
            foreach (var cell in cells)
            {
                HighlightedCells.Add(cell);
                cell.Highlight();
            }
        }

        public void ClearHighlight()
        {
            foreach (var cell in HighlightedCells)
                cell.ClearHighlight();
            HighlightedCells.Clear();
        }
    }
}