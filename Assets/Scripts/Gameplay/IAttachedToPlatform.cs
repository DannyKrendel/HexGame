namespace HexGame.Gameplay
{
    public interface IAttachedToPlatform : IHexGridElement
    {
        Platform ParentPlatform { get; }
        void AttachToPlatform(Platform platform);
    }
}