using UnityEngine;

[CreateAssetMenu(fileName = "PetData", menuName = "ScriptableObjects/PetData")]
public class PetData : ScriptableObject
{
    public float ressourceDetectionRadius;
    public float moveSpeed;
    public Vector3Data playerPetPosition;
    public LayerMask ressourceLayer;
    public GameObjectPoolReference projectilePoolRef;
    public float delayBetweenAttacks;
}