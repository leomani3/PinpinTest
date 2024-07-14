using Pinpin;
using UnityEngine;

public class PlayerAE : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }
    public void Harvest()
    {
        _playerController.HarvestNearbyRessources();
    }
}