using DG.Tweening;
using MyBox;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ressource : MonoBehaviour
{
    [SerializeField] RessourceData ressourceData;
    [SerializeField] List<GameObject> phases = new List<GameObject>();

    private float[] _phasesLife;
    private int _currentPhase;
    private float _currentLife;
    private float _currentPhaseLife;
    private Collider _collider;

    private bool _alive;
    private float _timer;

    public RessourceType RessourceType => ressourceData.ressourceType;
    public bool Alive => _alive;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _alive = true;
        _currentPhase = 0;
        _currentLife = ressourceData.maxHealth;

        _phasesLife = new float[phases.Count];
        for (int i = 0; i < _phasesLife.Length; i++)
        {
            _phasesLife[i] = ressourceData.maxHealth / _phasesLife.Length;
        }

        _currentPhaseLife = _phasesLife[_currentPhase];
    }

    private void Update()
    {
        if (!_alive)
        {
            _timer += Time.deltaTime;
            if (_timer >= ressourceData.respawnDelay)
            {
                Spawn();
            }
        }
    }

    public void Spawn()
    {
        _collider.enabled = true;
        _alive = true;

        foreach (GameObject chunk in phases)
        {
            chunk.SetActive(true);
        }

        _currentPhase = 0;
        _currentPhaseLife = _phasesLife[_currentPhase];
        _currentLife = ressourceData.maxHealth;

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic, 0.5f);
        //transform.DOShakeScale(0.5f, 0.5f, 3, 90, true);
    }

    public void ReceiveHit(float damage)
    {
        if (_currentLife > 0)
        {
            transform.DOShakeScale(0.2f, 0.25f, 3, 90, true);

            ressourceData.currencyData.IncreaseCurrency(ressourceData.currencyDropPerHit);

            FloatingTextManager.Instance.Spawn(transform.position + ressourceData.floatingTextSpawnOffset, "+" + ressourceData.currencyDropPerHit.ToString() + " <sprite=\"" + ressourceData.currencyData.currencyName + "\" name=\"" + ressourceData.currencyData.currencyName + "\">");

            float damageLeft = damage;
            while (damageLeft > 0)
            {
                if (damageLeft >= _currentPhaseLife)
                {
                    damageLeft -= _currentPhaseLife;

                    SpawnChunks();
                    if (_currentPhase + 1 < _phasesLife.Length)
                    {
                        _currentPhase++;
                        _currentPhaseLife = _phasesLife[_currentPhase];
                    }
                }
                else
                {
                    _currentPhaseLife -= damageLeft;
                    damageLeft = 0;
                }
            }
            _currentLife -= damage;

            if (_currentLife <= 0)
                DestroyRessource();
        }
    }

    public void SpawnChunks()
    {
        SetPhase(_currentPhase);

        ressourceData.chunkVFXPool.pool.Spawn(phases[_currentPhase].transform.position, Quaternion.identity, ressourceData.chunkVFXPool.pool.transform);

        for (int i = 0; i < ressourceData.chunkNumber; i++)
        {
            RessourceChunk spawnedCollectable = ressourceData.collectablePool.pool.Spawn(phases[_currentPhase].transform.position, Quaternion.identity, ressourceData.collectablePool.pool.transform).GetComponent<RessourceChunk>();
            spawnedCollectable.Spawn(ressourceData.collectablePool);
        }
    }

    private void SetPhase(int phaseIndex)
    {
        switch (ressourceData.phaseType)
        {
            case RessourcePhaseType.ChunkSeparation:
                phases[_currentPhase].SetActive(false);
                break;
            case RessourcePhaseType.ModelChange:
                phases[_currentPhase].SetActive(false);

                if (_currentPhase + 1 < phases.Count)
                {
                    phases[_currentPhase+1].SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void DestroyRessource()
    {
        _collider.enabled = false;

        _alive = false;
        _timer = 0;
    }
}