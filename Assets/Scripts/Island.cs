using DG.Tweening;
using UnityEngine;

public class Island : Buyable
{
    protected override void Init()
    {
        base.Init();

        transform.localScale = Vector3.zero;
    }
    public override void Buy(bool instant)
    {
       base.Buy(instant);

        if (instant)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic, 0.5f);
        }
    }
}