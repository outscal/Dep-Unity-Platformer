namespace Platformer.Player
{
    public class PlayerService
    {
        public PlayerController playerController { get; private set; }
        
        public PlayerService(PlayerScriptableObject playerScriptableObject) => SpawnPlayer(playerScriptableObject);

        private void SpawnPlayer(PlayerScriptableObject playerScriptableObject) => playerController = new PlayerController(playerScriptableObject);

        public void Update() => playerController.Update();
    }
}