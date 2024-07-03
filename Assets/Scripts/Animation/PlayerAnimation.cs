namespace Platformer.AnimationSystem
{
    // TODO: Animation enums for Player, Enemies, or any other entity must be inside its own vertical as discussed before.
    public enum PlayerAnimation
    {
        IDLE,
        RUNNING
    }
    
    public enum PlayerTriggerAnimation
    {
        JUMP,
        SLIDE,
        ATTACK,
        TAKE_DAMAGE,
        DEATH,
    }
}