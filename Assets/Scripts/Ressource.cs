using DG.Tweening;
using MyBox;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ressource : MonoBehaviour
{
    [SerializeField] RessourceData ressourceData;
    [SerializeField] List<GameObject> phaseChunks = new List<GameObject>();

    private float[] _phasesLife;
    private int _currentPhase;
    private float _currentLife;
    private float _currentPhaseLife;

    private bool _alive;
    private float _timer;

    private void Awake()
    {
        _alive = true;
        _currentPhase = 0;
        _currentLife = ressourceData.maxHealth;

        _phasesLife = new float[phaseChunks.Count];
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
        _alive = true;

        foreach (GameObject chunk in phaseChunks)
        {
            chunk.SetActive(true);
        }

        _currentPhase = 0;
        _currentPhaseLife = _phasesLife[_currentPhase];
        _currentLife = ressourceData.maxHealth;

        transform.DOShakeScale(0.5f, 0.3f, 3, 90, true);
    }

    [ButtonMethod]
    public void test()
    {
        ReceiveHit(1);
    }

    public void ReceiveHit(float damage)
    {
        if (_currentLife > 0)
        {
            transform.DOShakeScale(0.15f, 0.15f, 3, 90, true);

            float damageLeft = damage;
            while (damageLeft > 0)
            {
                if (damageLeft >= _currentPhaseLife)
                {
                    damageLeft -= _currentPhaseLife;

                    Drop();
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

    public void Drop()
    {
        phaseChunks[_currentPhase].SetActive(false);
        ressourceData.chunkVFXPool.pool.Spawn(phaseChunks[_currentPhase].transform.position, Quaternion.identity, ressourceData.chunkVFXPool.pool.transform);

        for (int i = 0; i < ressourceData.collectableNumber; i++)
        {
            Collectable spawnedCollectable = ressourceData.collectablePool.pool.Spawn(phaseChunks[_currentPhase].transform.position, Quaternion.identity, ressourceData.collectablePool.pool.transform).GetComponent<Collectable>();
            spawnedCollectable.Spawn(ressourceData.collectableValue, ressourceData.collectablePool);
        }
    }

    public void DestroyRessource()
    {
        _alive = false;
        _timer = 0;
    }
}