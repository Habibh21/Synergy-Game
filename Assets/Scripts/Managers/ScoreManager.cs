using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI coinText;

    [Header("Target Coin")]
    public int maxCoin = 5;

    private int coin = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int value)
    {
        coin += value;

        if (coin > maxCoin)
            coin = maxCoin;

        UpdateUI();
    }

    // Mengembalikan jumlah coin sekarang
    public int GetCoin()
    {
        return coin;
    }

    // Mengecek apakah semua coin sudah diambil
    public bool HasAllCoins()
    {
        return coin >= maxCoin;
    }

    void UpdateUI()
    {
        coinText.text = "COIN : " + coin + " / " + maxCoin;
    }
}