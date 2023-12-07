using Platformer.Main;
using Platformer.Player;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<PlayerView>() != null){
            GameService.Instance.PlayerService.TakeDamage(10);
        }
    }
}
