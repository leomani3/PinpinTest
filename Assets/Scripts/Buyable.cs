
using System.Collections.Generic;
using UnityEngine;

public class Buyable : MonoBehaviour
{
    [SerializeField] protected string ID;

    [SerializeField] protected List<BuyZone> connectedBuyZones;

    protected bool _bought;

    protected virtual void Awake()
    {
        Load();

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

        Save();
    }

    protected virtual void Save()
    {
        PlayerPrefs.SetString(ID.ToString(), _bought.ToString());
    }

    protected virtual void Load()
    {
        _bought = bool.Parse(PlayerPrefs.GetString(ID.ToString(), "false"));
        print(gameObject.name + " bought : " + _bought);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        Save();
    }
}