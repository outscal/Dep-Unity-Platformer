using Platformer.Player.Enumerations;

namespace Platformer.Player.Controllers
{
    public class PlayerCombatController
    {
        private PlayerController owner;

        public PlayerCombatController(PlayerController owner) => this.owner = owner;
        
        public bool CanAttack(bool isGrounded, PlayerState currentPlayerState)
        {
            return isGrounded && (currentPlayerState == PlayerState.IDLE || currentPlayerState == PlayerState.RUNNING);  
        } 
        
        public bool TakeDamage(int damageToInflict)
        {
            owner.CurrentHealth -= damageToInflict;
            if (owner.CurrentHealth <= 0)
            {
                owner.CurrentHealth = 0;
                owner.PlayerDied();
                return true;
            }
            return false;
        }
    }
}