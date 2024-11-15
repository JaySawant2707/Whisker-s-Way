using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinPoints = 10;
    bool wasCollected = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().UpdateScore(coinPoints);
            Destroy(gameObject);
        }
    }
}
