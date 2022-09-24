namespace HexGame.Gameplay
{
    public interface IHighlight
    {
        bool IsHighlighted { get; }
        
        void Highlight();
        void ClearHighlight();
    }
}