using UnityEngine;

public class CurrencyCollectable : Collectable
{
    [SerializeField] private CurrencyData currencyData;

    public override void PickUp()
    {
        base.PickUp();
        currencyData.IncreaseCurrency(value);
    }
}