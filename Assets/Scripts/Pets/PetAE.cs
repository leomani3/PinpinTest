using UnityEngine;

public class PetAE : MonoBehaviour
{
    private Pet _pet;

    private void Awake()
    {
        _pet = GetComponentInParent<Pet>();
    }

    public void LaunchProjectile()
    {
        _pet.Attack();
    }
}