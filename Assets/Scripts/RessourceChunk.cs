
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RessourceChunk : MonoBehaviour
{
    [SerializeField] private Vector3Data playerPositionData;
    [SerializeField] private float moveSpeed;

    protected int value;
    protected GameObjectPoolReference _parentPool;

    public void Spawn(GameObjectPoolReference parentPool)
    {
        _parentPool = parentPool;


        transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), .75f);
        transform.DOJump(transform.position + new Vector3(Random.Range(-2, 2), Random.Range(1, 2), Random.Range(-2, 2)), 1, 1, 0.75f).OnComplete(ChasePlayer);
    }

    public void ChasePlayer()
    {
        StartCoroutine(ChasePlayerCoroutine());
    }

    private IEnumerator ChasePlayerCoroutine()
    {
        while (Vector3.Distance(transform.position, playerPositionData.data) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerPositionData.data - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 20);

            transform.position = Vector3.MoveTowards(transform.position, playerPositionData.data, moveSpeed * Time.fixedDeltaTime);

            yield return null;
        }

        PickUp();
    }

    public virtual void PickUp()
    {
        _parentPool.pool.Despawn(gameObject);
    }
}