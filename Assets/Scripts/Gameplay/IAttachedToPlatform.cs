namespace HexGame.Gameplay
{
    public interface IAttachedToPlatform : IHexGridElement
    {
        void AttachToPlatform(Platform platform);
    }
}