using MyBox;
using UnityEngine;

public enum PlayerStatType
{
    MoveSpeed,
    WoodAmage,
    StoneDamage
}

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStat : ScriptableObject
{
    public PlayerStatType statType;
    public string statID;
    public float baseValue;
    public float currentValue;
    public int currentLevel;

    [Separator("Upgrade")]
    public Sprite statUpgradeSprite;
    public float statIncreasePerLevel;
    public CurrencyData currency;
    public int basePrice;
    public float basePriceMultiplicator;
    public float priceMultiplicatorPerLevel;

    public int CurrentPrice => Mathf.RoundToInt(basePrice * (basePriceMultiplicator +(currentLevel * priceMultiplicatorPerLevel)));

    public void Init()
    {
        string[] values = PlayerPrefs.GetString(statID, "0,"+baseValue).Split(',');
        currentLevel = int.Parse(values[0]);
        currentValue = float.Parse(values[1]);
    }

    public void Save()
    {
        PlayerPrefs.SetString(statID, currentLevel+","+ currentValue);
    }

    public void UpgradeStat()
    {
        currentLevel++;
        currentValue = baseValue + (currentLevel * statIncreasePerLevel);

        Save();
    }
}