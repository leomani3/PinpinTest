using UnityEngine;

[CreateAssetMenu(fileName = "PetData", menuName = "ScriptableObjects/PetData")]
public class PetData : ScriptableObject
{
    [SerializeField] public float ressourceDetectionRadius;
    [SerializeField] public float moveSpeed;
    [SerializeField] public Vector3Data playerPetPosition;
}