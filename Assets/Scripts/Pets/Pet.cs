using DG.Tweening;
using MyBox;
using System;
using TMPro;
using UnityEngine;

public class Pet : Buyable
{
    [SerializeField] private PetData petData;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI timeLeftText;

    private Collider[] _detectedColliders;
    private Ressource _targetRessource;
    private Animator _animator;
    private float _attackTimeCounter;
    private float _timeLeft;

    protected override void Awake()
    {
        base.Awake();
        canvas.enabled = _bought;
        _animator = GetComponentInChildren<Animator>();

        _attackTimeCounter = petData.delayBetweenAttacks;
    }

    private void Update()
    {
        if (_bought)
        {
            _detectedColliders = Physics.OverlapSphere(transform.position, petData.ressourceDetectionRadius, petData.ressourceLayer);
            foreach (Collider collider in _detectedColliders)
            {
                Ressource ressource = collider.GetComponent<Ressource>();
                if (ressource != null && ressource.Alive)
                {
                    _targetRessource = ressource;
                }
            }

            _attackTimeCounter += Time.deltaTime;
            if (_targetRessource != null)
            {
                if (_attackTimeCounter >= petData.delayBetweenAttacks)
                {
                    _animator.SetTrigger("Attack");
                    _attackTimeCounter = 0;
                    _targetRessource = null;
                }
            }

            _timeLeft -= Time.deltaTime;
            int minutes = TimeSpan.FromSeconds(_timeLeft).Minutes;
            int seconds = TimeSpan.FromSeconds(_timeLeft).Seconds;
            if (seconds > 10)
            {
                timeLeftText.text = minutes + ":" + seconds;
            }
            else
            {
                timeLeftText.text = minutes + ":0" + seconds;
            }
            if (_timeLeft <= 0)
            {
                OnTimerEnd();
            }
        }
    }

    public void FixedUpdate()
    {
        if (_bought)
        {
            transform.position = Vector3.MoveTowards(transform.position, petData.playerPetPosition.data, petData.moveSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(petData.playerPetPosition.data, transform.position) > Mathf.Epsilon)
            {
                Quaternion targetRotation = Quaternion.LookRotation(petData.playerPetPosition.data - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 20);
            }
        }
    }

    public void Attack()
    {
        if (_targetRessource != null)
        {
            Projectile spawnedProjectile = petData.projectilePoolRef.pool.Spawn(projectileSpawn.position, Quaternion.identity, petData.projectilePoolRef.pool.transform).GetComponent<Projectile>();
            spawnedProjectile.Travel(_targetRessource.transform.position.OffsetY(1));
        }
    }

    private void OnTimerEnd()
    {
        _attackTimeCounter = 0;
        _bought = false;
        _timeLeft = 0;
        Save();
        canvas.enabled = false;

        transform.DOScale(0, 1f).onComplete += () =>
        {
            transform.localScale = Vector3.one;

            foreach (BuyZone buyZone in connectedBuyZones)
            {
                buyZone.Reset();
            }
        };
    }

    public override void Buy(bool instant)
    {
        base.Buy(instant);

        if (_timeLeft <= 0)
            _timeLeft = petData.timeAlive;

        canvas.enabled = true;
    }

    protected override void Init()
    {
        base.Init();

        transform.localScale = Vector3.one;
    }

    protected override void Save()
    {
        PlayerPrefs.SetString(ID, _bought.ToString()+","+_timeLeft); 
    }

    protected override void Load()
    {
        string loadedString = PlayerPrefs.GetString(ID.ToString(), "false,0");
        string[] values = loadedString.Split(',');
        _bought = bool.Parse(values[0]);
        _timeLeft = float.Parse(values[1]);
    }
}