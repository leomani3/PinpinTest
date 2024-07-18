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
        upgradeValueText.text = "x" + (stat.currentLevel * stat.statIncreasePerLevel);
        currencyIcon.sprite = stat.currency.currencySprite;
        currencyAmountText.text = stat.CurrentPrice.ToString();

        stat.currency.onCurrencyValueChange += UpdateVisuals;
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

        upgradeValueText.text = "x" + (stat.currentLevel * stat.statIncreasePerLevel);
        currencyAmountText.text = stat.CurrentPrice.ToString();
    }
}