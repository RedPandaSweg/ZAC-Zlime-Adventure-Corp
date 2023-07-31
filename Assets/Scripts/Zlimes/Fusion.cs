using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion : MonoBehaviour
{
    public List<Zlime> zlimes = new List<Zlime>();

    public Zlime fusion;

    public GameObject zlimePrefab;

    public void FusionHa()
    {
        Debug.Log("Fusion, HA!");
        GameObject newZlime = Instantiate(zlimePrefab, this.transform);

        fusion = newZlime.GetComponent<Zlime>();

        //Color
        for (int i = 0; i < zlimes.Count; i++)
        {
            fusion.color.r += zlimes[i].color.r * zlimes[i].color.r;
            fusion.color.g += zlimes[i].color.g * zlimes[i].color.g;
            fusion.color.b += zlimes[i].color.b * zlimes[i].color.b;
        }
        fusion.color.r = Mathf.Sqrt((fusion.color.r) / zlimes.Count);
        fusion.color.g = Mathf.Sqrt((fusion.color.g) / zlimes.Count);
        fusion.color.b = Mathf.Sqrt((fusion.color.b) / zlimes.Count);
        fusion.color.a = 1f;
        newZlime.GetComponent<SpriteRenderer>().color = fusion.color;

        //Stats

        //MaxStats

        //Set Fusion Max Stats equal to Base
        for (int i = 0; i < fusion.statsMax.Count; i++)
        {
            fusion.statsMax[i] = zlimes[0].statsMax[i];
        }
        //Increase Max Stats by percent of partners
        for (int i = 0; i < zlimes.Count; i++)
        {
            for (int j = 0; j < zlimes[0].statsMax.Count; j++)
            {
                fusion.statsMax[j] += (zlimes[i].statsMax[j] * 0.1f);
            }
        }


        //CurrentStats

        //Set Fusion Current Stats equal to half of Base
        for (int i = 0; i < fusion.statsCurrent.Count; i++)
        {
            fusion.statsCurrent[i] = zlimes[0].statsCurrent[i] / 2;
        }
        //Increase Current Stats by percent of partners
        for (int i = 0; i < zlimes.Count; i++)
        {
            for (int j = 0; j < zlimes[0].statsCurrent.Count; j++)
            {
                fusion.statsCurrent[j] += (zlimes[i].statsCurrent[j] * 0.1f);
            }
        }

        //NEW:
        //Set Fusion Current Stats equal to half of Base
        for (int i = 0; i < fusion.GetComponent<Stats>().stats.Count; i++)
        {
            fusion.GetComponent<Stats>().stats[i].CurrentValue = zlimes[0].GetComponent<Stats>().stats[i].CurrentValue / 2;
        }
        //Increase Current Stats by percent of partners
        for (int i = 0; i < zlimes.Count; i++)
        {
            for (int j = 0; j < zlimes[0].GetComponent<Stats>().stats.Count; j++)
            {
                fusion.GetComponent<Stats>().stats[j].CurrentValue += (zlimes[i].GetComponent<Stats>().stats[j].CurrentValue * 0.1f);
            }
        }
    }
}
