using DG.Tweening;
using MyBox;
using Pinpin;
using System.Collections;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class BuyZone : MonoBehaviour
{
    [SerializeField] private CurrencyData currency;
    [SerializeField] private int neededAmount;
    [SerializeField] private GameObjectPoolReference chunkPoolReference;
    [SerializeField] private Buyable buyable;

    [Separator("References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform textTarget;
    [SerializeField] private Canvas canvas;
    [SerializeField] private SpriteRenderer squareSprite;
    [SerializeField] private Vector3Data playerPos;

    private int _currentAmount;
    private bool _bought;
    private Camera _mainCam;
    private Tween _squareScaleTween;
    private Vector3 _initalSquareSpriteScale;
    private bool _playerInside;

    private void Awake()
    {
        _mainCam = Camera.main;
        _bought = false;
        _initalSquareSpriteScale = squareSprite.transform.localScale;
        UpdateText();
    }

    private void Update()
    {
        if (!_bought && _mainCam != null)
        {
            text.transform.position = _mainCam.WorldToScreenPoint(textTarget.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null && !_bought) 
        {
            _squareScaleTween.Kill();
            _squareScaleTween = squareSprite.transform.DOScale(_initalSquareSpriteScale * 1.25f, 0.15f);

            _playerInside = true;

            StartCoroutine(TransferCurrencyCo());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null && !_bought)
        {
            _squareScaleTween.Kill();
            _squareScaleTween = squareSprite.transform.DOScale(_initalSquareSpriteScale, 0.15f);

            _playerInside = false;
        }
    }

    public IEnumerator TransferCurrencyCo()
    {
        while (_playerInside && _currentAmount < neededAmount && currency.CurrencyAmount > 0)
        {
            RessourceChunk spawnedChunk = chunkPoolReference.pool.Spawn(playerPos.data, Quaternion.identity, chunkPoolReference.pool.transform).GetComponent<RessourceChunk>();
            spawnedChunk.transform.DOJump(transform.position, 2, 1, 0.5f).onComplete += () => chunkPoolReference.pool.Despawn(spawnedChunk.gameObject);
            spawnedChunk.transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 0.5f);

            _currentAmount++;
            currency.DecreaseCurrency(1);

            UpdateText();

            yield return new WaitForSeconds(0.1f);
        }

        if (_currentAmount >= neededAmount) 
        {
            Buy();
        }
    }

    public void Buy()
    {
        _bought = true;

        if (buyable != null)
            buyable.Buy();

        canvas.enabled = false;

        _squareScaleTween.Kill();
        _squareScaleTween = squareSprite.transform.DOScale(0, 1f);
    }

    private void UpdateText()
    {
        text.text = _currentAmount + "/" + neededAmount + " <sprite=\"" + currency.currencyName + "\" name=\"" + currency.currencyName + "\">";
    }
}