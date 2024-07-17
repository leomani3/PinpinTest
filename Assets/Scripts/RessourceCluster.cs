using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class RessourceCluster : MonoBehaviour
{
    private Vector2 minMaxScale;

    private List<Ressource> _ressources = new List<Ressource>();

    [ButtonMethod]
    public void RandomAllRessourcesRotations()
    {
        _ressources = GetComponentsInChildren<Ressource>().ToList();
        foreach (Ressource _ressources in _ressources)
        {
            _ressources.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }

    [ButtonMethod]
    public void RandomAllRessourcesSize()
    {
        _ressources = GetComponentsInChildren<Ressource>().ToList();
        foreach (Ressource _ressources in _ressources)
        {
            _ressources.transform.localScale = Vector3.one * Random.Range(minMaxScale.x, minMaxScale.y);
        }
    }
}