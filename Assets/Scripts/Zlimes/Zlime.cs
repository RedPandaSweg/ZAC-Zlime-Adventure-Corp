using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zlime : MonoBehaviour
{
    //Genetics
    public string rank = "D";
    public Color color;

    public float growthPotential = 0.5f;

    public float mass = 100f;
    public float massMax = 100f;
    public float mojo = 0f;
    public float mojoMax = 0f;
    public float armor = 0f;

    public float xp = 0f;

    public List<float> statsCurrent = new List<float>();
    public List<float> statsMax = new List<float>();
    //public List<Trait> traits = new List<Trait>();

    public ZlimePanel zlimePanel;

    private void Awake()
    {
        statsCurrent = new List<float> { Random(), Random(), Random(), Random(), Random(), Random() };
        statsMax = new List<float> { RandomMax(), RandomMax(), RandomMax(), RandomMax(), RandomMax(), RandomMax() };
    }

    void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    internal void AddSkill(string skillName)
    {
        throw new NotImplementedException();
    }

    public float Random()
    {
        return UnityEngine.Random.Range(1f, 10f);
    }
    public float RandomMax()
    {
        return UnityEngine.Random.Range(12f, 20f);
    }
}
