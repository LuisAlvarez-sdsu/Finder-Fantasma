using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

   private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player"))
    {
        CoinManager.Instance.AddCoins(value);
        other.GetComponent<Player>()?.CollectCoin();
        Destroy(gameObject);
    }
    }
}