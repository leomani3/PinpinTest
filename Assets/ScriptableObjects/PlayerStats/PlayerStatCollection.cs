using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatCollection", menuName = "ScriptableObjects/PlayerStatCollection")]
public class PlayerStatCollection : ScriptableObject
{
    public List<PlayerStat> stats = new List<PlayerStat>();

    public void Init()
    {
        foreach (PlayerStat stat in stats)
        {
            stat.Init();
        }
    }

    public void Save()
    {
        foreach (PlayerStat stat in stats)
        {
            stat.Save();
        }
    }

    public float GetStat(PlayerStatType type)
    {
        foreach (PlayerStat stat in stats)
        {
            if (stat.statType == type)
            {
                return stat.currentValue;
            }
        }

        Debug.LogWarning("Unknown stat. Returning 0.");
        return 0;
    }
}