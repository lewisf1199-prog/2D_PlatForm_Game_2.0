using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score & Gemstones")]
    public int score = 0;
    public int gemstonesCollected = 0;
    public int totalGemstones = 3;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gemstoneText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int points)
    {
        score += points;
        UpdateUI();
    }

    public void AddGemstone()
    {
        gemstonesCollected++;
        UpdateUI();

        if (gemstonesCollected >= totalGemstones)
        {
            Debug.Log("All 3 gemstones collected!");
        }
    }

    public bool HasAllGemstones()
    {
        return gemstonesCollected >= totalGemstones;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (gemstoneText != null)
        {
            gemstoneText.text = "Gemstones: " + gemstonesCollected + " / " + totalGemstones;
        }
    }
}