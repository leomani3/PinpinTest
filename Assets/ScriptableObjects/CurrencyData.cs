using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrentData")]
public class CurrencyData : ScriptableObject
{
    public string currencyName;
    public Sprite currencySprite;
    public int currencyAmount;

    public void IncreaseCurrency(int amount)
    {
        currencyAmount = PlayerPrefs.GetInt(currencyName, 0);
        currencyAmount += amount;
        PlayerPrefs.SetInt(currencyName, currencyAmount);
    }

    public void DecreaseCurrency(int amount)
    {
        currencyAmount = PlayerPrefs.GetInt(currencyName, 0);
        currencyAmount = Mathf.Clamp(currencyAmount - amount, 0, currencyAmount);
        PlayerPrefs.SetInt(currencyName, currencyAmount);
    }

    public void SetCurrency(int val)
    {
        currencyAmount = PlayerPrefs.GetInt(currencyName, 0);
        currencyAmount = val;
        PlayerPrefs.SetInt(currencyName, currencyAmount);
    }
}