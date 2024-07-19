using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private PlayerStat stat;

    [Separator("References")]
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TextMeshProUGUI upgradeValueText;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private TextMeshProUGUI currencyAmountText;
    [SerializeField] private Button button;

    private void Start()
    {
        upgradeImage.sprite = stat.statUpgradeSprite;
        currencyIcon.sprite = stat.currency.currencySprite;

        stat.currency.onCurrencyValueChange += UpdateVisuals;
        UpdateVisuals(stat.currency.currencyAmount);
    }

    public void Buy()
    {
        if (stat.currency.CurrencyAmount >= stat.CurrentPrice)
        {
            stat.currency.DecreaseCurrency(stat.CurrentPrice);
            stat.UpgradeStat();
            UpdateVisuals(stat.currency.CurrencyAmount);
        }
    }

    private void UpdateVisuals(int newCurrentAmount)
    {
        if (newCurrentAmount >= stat.CurrentPrice)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }

        upgradeValueText.text = "+" + Mathf.RoundToInt((stat.currentLevel * stat.statIncreasePerLevel) * 100) +"%";
        currencyAmountText.text = stat.CurrentPrice.ToString();
    }
}