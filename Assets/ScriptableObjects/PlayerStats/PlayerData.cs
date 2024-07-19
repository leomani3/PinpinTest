using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Separator("Movement")]
    public PlayerStatCollection statCollection;
    public Vector3Data playerPositionData;
    public float rotationSpeed;
    public float ressourceDetectionRadius;
    public float distanceFromPlayer;
    public float maxSlopeAngle;
    public LayerMask ressourceLayer;
    public LayerMask groundLayer;

    [Separator("Pet")]
    public Vector3Data playerPetPosition;
    public float nbFrameDelay;
}