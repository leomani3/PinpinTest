using DG.Tweening;
using Lean.Pool;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float fadeInDuration;
    [SerializeField] private Vector3 fadeInOffset;

    [SerializeField] private float delayBeforeFadeOut;
    [SerializeField] private float fadeOutDuration;

    private Tween _moveTween;
    private Tween _opacityTween;
    private Camera _mainCam;
    private TextMeshProUGUI _text;
    private Vector3 _worldPos;
    private bool _active;
    private LeanGameObjectPool _parentPool;

    private void Awake()
    {
        _mainCam = Camera.main;
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Spawn(Vector3 worldPos, string txt, LeanGameObjectPool parentPool)
    {
        _active = true;

        _worldPos = worldPos;
        _parentPool = parentPool;

        _moveTween.Kill();
        _opacityTween.Kill();
        _text.text = txt;

        //before
        _text.transform.localPosition = Vector3.zero;
        _text.color = _text.color.WithAlphaSetTo(0);

        //fade in
        _moveTween = _text.transform.DOLocalMove(_worldPos + fadeInOffset, fadeInDuration);
        _opacityTween = _text.DOFade(1, fadeInDuration);

        //fade out
        _opacityTween = _text.DOFade(0, fadeOutDuration).SetDelay(delayBeforeFadeOut).OnComplete(Despawn);
    }

    private void Despawn()
    {
        _active = false;
        _parentPool.Despawn(gameObject);
    }

    private void Update()
    {
        if (_active && _worldPos != null)
        {
            transform.position = _mainCam.WorldToScreenPoint(_worldPos);
        }
    }
}