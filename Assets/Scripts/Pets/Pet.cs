using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private PetData petData;

    public void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, petData.playerPetPosition.data, petData.moveSpeed * Time.fixedDeltaTime);
    }

    public void GetDetectedRessource()
    {

    }
}