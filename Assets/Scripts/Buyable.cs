
using System.Collections.Generic;
using UnityEngine;

public class Buyable : MonoBehaviour
{
    [SerializeField] private int ID;

    [SerializeField] private List<BuyZone> connectedBuyZones;

    protected bool _bought;

    protected virtual void Awake()
    {
        _bought = bool.Parse(PlayerPrefs.GetString(ID.ToString(), "false"));

        if (_bought)
        {
            Buy(true);
        }
        else
        {
            Init();
        }
    }

    private void Start()
    {
        foreach (BuyZone buyZone in connectedBuyZones)
        {
            buyZone.SetActive(!_bought);
        }
    }

    protected virtual void Init()
    {
        _bought = false;
    }

    public virtual void Buy(bool instant)
    {
        _bought = true;

        PlayerPrefs.SetString(ID.ToString(), _bought.ToString());
    }
}