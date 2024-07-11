namespace Platformer.Utilities{
    public interface IDamagable
    {
        public int CurrentHealth();
        public int ClampHealth(int value);
        public void UpdateHealthService();
        public float CalculateHealthRatio();
        public void TakeDamage(int damage);
    }
}