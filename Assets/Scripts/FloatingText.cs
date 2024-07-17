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

    private void Awake()
    {
        _mainCam = Camera.main;
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Spawn(Vector3 worldPos, string txt)
    {
        _worldPos = worldPos;

        _moveTween.Kill();
        _opacityTween.Kill();
        _text.text = txt;

        _text.color = _text.color.WithAlphaSetTo(0);

        _moveTween = _text.transform.DOLocalMove(_worldPos + fadeInOffset, fadeInDuration);
        _opacityTween = _text.DOFade(1, fadeInDuration);

        _opacityTween = _text.DOFade(0, fadeOutDuration).SetDelay(delayBeforeFadeOut);
    }

    private void Update()
    {
        if (_worldPos != null)
        {
            transform.position = _mainCam.WorldToScreenPoint(_worldPos);
        }
    }
}