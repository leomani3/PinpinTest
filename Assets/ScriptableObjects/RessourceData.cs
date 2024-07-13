using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "RessourceData", menuName = "ScriptableObjects/RessourceData")]
public class RessourceData : ScriptableObject
{
    public float respawnDelay;
    public float maxHealth;
    public GameObjectPoolReference chunkVFXPool;

    [Separator("Collectables")]
    public GameObjectPoolReference collectablePool;
    public int collectableNumber;
    public int collectableValue;
}