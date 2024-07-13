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

    private void Awake()
    {
        _currentPhase = 0;
        _currentLife = ressourceData.maxHealth;

        _phasesLife = new float[phaseChunks.Count];
        for (int i = 0; i < _phasesLife.Length; i++)
        {
            _phasesLife[i] = ressourceData.maxHealth / _phasesLife.Length;
        }

        _currentPhaseLife = _phasesLife[_currentPhase];
    }

    public void Spawn()
    {

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
            print("ReceiveHit : " + damage);

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
        }
    }

    public void Drop()
    {
        print("drop");
        phaseChunks[_currentPhase].SetActive(false);
        ressourceData.chunkVFXPool.pool.Spawn(phaseChunks[_currentPhase].transform.position, Quaternion.identity, ressourceData.chunkVFXPool.pool.transform);
    }
}