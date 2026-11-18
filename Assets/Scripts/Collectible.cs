using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum ItemType { Coin, Gemstone }

    [Header("Settings")]
    [SerializeField] private ItemType collectibleType = ItemType.Coin;
    [SerializeField] private int coinScoreValue = 10;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collected && other.CompareTag("Player"))
        {
            collected = true;

            if (collectibleType == ItemType.Coin)
            {
                GameManager.instance?.AddCoin(coinScoreValue);
            }
            else if (collectibleType == ItemType.Gemstone)
            {
                GameManager.instance?.AddGemstone();
            }

            Destroy(gameObject);
        }
    }
}