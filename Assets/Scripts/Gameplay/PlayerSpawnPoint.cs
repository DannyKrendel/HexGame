namespace HexGame.Gameplay
{
    public class PlayerSpawnPoint : HexGridElement, ISpawnPoint<Player>
    {
        public void Spawn(Player player)
        {
            player.Movement.Move(Coordinates, true);
        }
    }
}