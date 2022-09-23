namespace HexGame.Gameplay
{
    public interface ISelectable
    {
        bool IsSelected { get; }
        
        void Select();
        void Deselect();
    }
}