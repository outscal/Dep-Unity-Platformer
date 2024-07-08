namespace Platformer.Utilities{
    // TODO: The fact that Controllers are not implementing this interface is weird. Need a sync regarding this.
    public interface IDamagable{
        public void TakeDamage(int damage);
    }
}