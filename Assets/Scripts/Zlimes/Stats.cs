using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats : MonoBehaviour
{
    public List<Stat> stats = new List<Stat>();

    void Awake()
    {
        CreateStats();
    }

    public void CreateStats()
    {
        for (int i = 0; i < 6; i++)
        {
            Stat stat = new Stat(i, 10+i, 20+i);
            stats.Add(stat);
        }
        
    }
}
