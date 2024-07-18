using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private PetData petData;
    [SerializeField] private Transform projectileSpawn;

    private Collider[] _detectedColliders;
    private Ressource _targetRessource;
    private Animator _animator;
    private float _timeCpt;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
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

        _timeCpt += Time.deltaTime;
        if (_targetRessource != null)
        {
            if (_timeCpt >= petData.delayBetweenAttacks)
            {
                _animator.SetTrigger("Attack");
                _timeCpt = 0;
                _targetRessource = null;
            }
        }
    }

    public void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, petData.playerPetPosition.data, petData.moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(petData.playerPetPosition.data, transform.position) > Mathf.Epsilon)
        {
            Quaternion targetRotation = Quaternion.LookRotation(petData.playerPetPosition.data - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 20);
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
}