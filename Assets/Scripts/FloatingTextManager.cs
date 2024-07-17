using MyBox;
using UnityEngine;

public class FloatingTextManager : Singleton<FloatingTextManager>
{
    [SerializeField] private GameObjectPoolReference floatingTextPoolRef;

    public void Spawn(Vector3 worldPos, string txt)
    {
        FloatingText text = floatingTextPoolRef.pool.Spawn(floatingTextPoolRef.pool.transform).GetComponent<FloatingText>();
        text.Spawn(worldPos, txt, floatingTextPoolRef.pool);
    }
}