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
    public CurrencyData currencyData;

    [Separator("Harvesting")]
    public GameObjectPoolReference chunkVFXPool;
    public GameObjectPoolReference collectablePool;
    public int currencyDropPer1Damage;
    public int chunkNumber;

    [Separator("Floating text")]
    public GameObjectPoolReference floatingTextPoolRef;
    public Vector3 floatingTextSpawnOffset;
}