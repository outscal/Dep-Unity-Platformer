using Platformer.Main;
using Platformer.Player;
using Platformer.Utilities;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // TODO: Will other enemies also take damage from this spike?
    [SerializeField] private int damage = 10;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.TryGetComponent<IDamagable>(out var damagableObject))
        {
            damagableObject.TakeDamage(damage);
        }
    }
}
