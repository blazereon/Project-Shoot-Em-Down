using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public static OrbManager Current;
    public int PoolCount;
    public GameObject Orb;
    private Stack<GameObject> AggressionOrbs = new Stack<GameObject>();
    private Stack<GameObject> PneumaOrbs = new Stack<GameObject>();
    public Dictionary<GameObject, Orb> OrbDictionary = new Dictionary<GameObject, Orb>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Current == null) Current = this;
    }
    void Start()
    {

        //Instantiates Pneuma Orb object
        for (int i = 0; i < PoolCount; i++)
        {
            GameObject newOrb = Instantiate(Orb);
            newOrb.SetActive(false);
            PneumaOrbs.Push(newOrb);
            OrbDictionary[newOrb] = newOrb.GetComponent<Orb>();
            OrbDictionary[newOrb].Type = OrbType.Pneuma;
            OrbDictionary[newOrb].orbManagerInstance = this;
        }

        //Instantiates Aggression Orb object
        for (int i = 0; i < PoolCount; i++)
        {
            GameObject newOrb = Instantiate(Orb);
            newOrb.SetActive(false);
            AggressionOrbs.Push(newOrb);
            OrbDictionary[newOrb] = newOrb.GetComponent<Orb>();
            OrbDictionary[newOrb].Type = OrbType.Aggression;
            OrbDictionary[newOrb].orbManagerInstance = this;
        }
    }

    public void GetOrb(OrbType type, int value, Vector2 position)
    {
        GameObject orbObject;
        Orb orb;
        switch (type)
        {
            case OrbType.Pneuma:
                orbObject = PneumaOrbs.Pop();
                orb = OrbDictionary[orbObject];
                break;
            case OrbType.Aggression:
                orbObject = AggressionOrbs.Pop();
                orb = OrbDictionary[orbObject];
                break;
            default:
                Debug.LogError("Run out of orbs. Add more orbs to the pool");
                return;
        }

        orbObject.SetActive(true);
        orb.OnInstantiateOrb(position);
        orb.value = value;
    }

    public void ReturnOrb(GameObject orbObject)
    {
        var orb = OrbDictionary[orbObject];
        switch (orb.Type)
        {
            case OrbType.Pneuma:
                PneumaOrbs.Push(orbObject);
                break;
            case OrbType.Aggression:
                AggressionOrbs.Push(orbObject);
                break;
        }

        orbObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
