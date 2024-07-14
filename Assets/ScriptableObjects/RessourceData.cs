using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "RessourceData", menuName = "ScriptableObjects/RessourceData")]
public class RessourceData : ScriptableObject
{
    [Separator("Values")]
    public RessourceType ressourceType;
    public float respawnDelay;
    public float maxHealth;
    public RessourcePhaseType phaseType;

    [Separator("Harvesting")]
    public GameObjectPoolReference chunkVFXPool;
    public GameObjectPoolReference collectablePool;
    public int collectableNumber;
    public int collectableValue;
}