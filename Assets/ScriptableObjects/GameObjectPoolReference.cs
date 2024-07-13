using Lean.Pool;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectPoolReference", menuName = "ScriptableObjects/GameObjectPoolReference")]
public class GameObjectPoolReference : ScriptableObject
{
    public LeanGameObjectPool pool;
}