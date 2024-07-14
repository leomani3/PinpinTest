using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrentData")]
public class CurrencyData : ScriptableObject
{
    public Action onCurrencyValueChange;

    public string currencyName;
    public Sprite currencySprite;
    public Sprite currencySpriteOutline;
    public int currencyAmount;

    public void Init()
    {
        SetCurrency(PlayerPrefs.GetInt(currencyName, 0));
    }

    public void IncreaseCurrency(int amount)
    {
        currencyAmount = PlayerPrefs.GetInt(currencyName, 0);
        currencyAmount += amount;
        PlayerPrefs.SetInt(currencyName, currencyAmount);

        onCurrencyValueChange?.Invoke();
    }

    public void DecreaseCurrency(int amount)
    {
        currencyAmount = PlayerPrefs.GetInt(currencyName, 0);
        currencyAmount = Mathf.Clamp(currencyAmount - amount, 0, currencyAmount);
        PlayerPrefs.SetInt(currencyName, currencyAmount);

        onCurrencyValueChange?.Invoke();
    }

    public void SetCurrency(int val)
    {
        currencyAmount = PlayerPrefs.GetInt(currencyName, 0);
        currencyAmount = val;
        PlayerPrefs.SetInt(currencyName, currencyAmount);

        onCurrencyValueChange?.Invoke();
    }
}