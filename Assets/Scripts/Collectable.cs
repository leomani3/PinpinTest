
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Vector3Data playerPositionData;
    [SerializeField] private float moveSpeed;

    protected int value;

    public void Spawn(int val)
    {
        value = val;



        transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 1);
        transform.DOJump(transform.position + new Vector3(Random.Range(-3, 3), Random.Range(1, 3), Random.Range(-3, 3)), 1, 1, 1).OnComplete(Pickup);
    }

    public void Pickup()
    {
        StartCoroutine(PickUpCoroutine());
    }

    private IEnumerator PickUpCoroutine()
    {
        while (Vector3.Distance(transform.position, playerPositionData.data) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerPositionData.data - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 20);

            transform.position = Vector3.MoveTowards(transform.position, playerPositionData.data, moveSpeed * Time.fixedDeltaTime);

            yield return null;
        }
    }
}