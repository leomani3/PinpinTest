using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private CurrencyData currencyData;

    [Separator("References")]
    [SerializeField] private TextMeshProUGUI currencyAmountText;
    [SerializeField] private Image currencyImage;
    [SerializeField] private Image currencyImageOutline;

    private void Awake()
    {
        currencyImage.sprite = currencyData.currencySprite;
        currencyImageOutline.sprite = currencyData.currencySpriteOutline;

        currencyData.onCurrencyValueChange += OnCurrencyValueChange;
    }

    public void OnCurrencyValueChange()
    {
        SetText(PlayerPrefs.GetInt(currencyData.currencyName, 0).ToString());
    }

    public void SetText(string text)
    {
        currencyAmountText.text = text;
    }
}