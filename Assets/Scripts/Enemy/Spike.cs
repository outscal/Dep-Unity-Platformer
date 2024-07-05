using Platformer.Main;
using Platformer.Player;
using Platformer.Utilities;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    
    private void OnTriggerEnter2D(Collider2D other){
        if (other.TryGetComponent<PlayerView>(out var damagableObject))
        {
            damagableObject.TakeDamage(damage);
        }
    }
}
