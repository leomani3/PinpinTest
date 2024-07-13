using Lean.Pool;
using UnityEngine;

public class GameObjectPoolReferenceSetter : MonoBehaviour
{
    [SerializeField] private GameObjectPoolReference poolReference;
    private void Awake()
    {
        poolReference.pool = GetComponent<LeanGameObjectPool>();
    }
}