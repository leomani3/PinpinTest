using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObjectPoolReference hitVFXPool;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask ressourceLayer;

    private Tween _moveTween;
    private Collider[] _detectedColliders;

    public void Travel(Vector3 targetPos)
    {
        transform.LookAt(targetPos);

        _moveTween = transform.DOMove(targetPos, (targetPos - transform.position).magnitude / moveSpeed);
        _moveTween.OnComplete(OnTravelEnd);
    }

    private void OnTravelEnd()
    {
        //VFX
        hitVFXPool.pool.Spawn(transform.position, Quaternion.identity, hitVFXPool.pool.transform);

        //Mine ressources
        _detectedColliders = Physics.OverlapSphere(transform.position, explosionRadius, ressourceLayer);
        foreach (Collider collider in _detectedColliders)
        {
            Ressource ressource = collider.GetComponent<Ressource>();
            if (ressource != null && ressource.Alive)
            {
                ressource.ReceiveHit(999);
            }
        }

        transform.localScale = Vector3.zero;
    }
}