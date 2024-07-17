using DG.Tweening;
using UnityEngine;

public class Island : Buyable
{
    private bool _bought;

    private void Awake()
    {
        _bought = false;
        transform.localScale = Vector3.zero;
    }

    public override void Buy()
    {
        _bought = true;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic, 0.5f);
    }
}