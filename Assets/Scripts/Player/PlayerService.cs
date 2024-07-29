namespace Platformer.Player
{
    public class PlayerService
    {
        private PlayerController playerController;
        
        public PlayerService(PlayerScriptableObject playerScriptableObject) => SpawnPlayer(playerScriptableObject);

        private void SpawnPlayer(PlayerScriptableObject playerScriptableObject) => playerController = new PlayerController(playerScriptableObject);

        public void Update() => playerController.Update();
    }
}