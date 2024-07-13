using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrentData")]
public class CurrencyData : ScriptableObject
{
    public string currencyName;
    public Sprite currencySprite;
    public GameObject currencyModel;
}