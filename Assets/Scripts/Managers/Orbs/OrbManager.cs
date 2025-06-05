using System;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public int PoolCount;

    public GameObject Orb;
    public GameObject AggressionOrb;
    public Stack<GameObject> AggressionOrbs;
    public Stack<GameObject> PneumaOrbs;
    public Dictionary<GameObject, Orb> OrbDictionary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AggressionOrbs = new Stack<GameObject>();
        PneumaOrbs = new Stack<GameObject>();

        //Instantiates Pneuma Orb object
        for (int i = 0; i < PoolCount; i++)
        {
            GameObject newOrb = Instantiate(Orb);
            newOrb.SetActive(false);
            PneumaOrbs.Push(newOrb);
            OrbDictionary[newOrb] = newOrb.GetComponent<Orb>();
        }
        
        //Instantiates Aggression Orb object
        for (int i = 0; i < PoolCount; i++)
        {
            GameObject newOrb = Instantiate(Orb);
            newOrb.SetActive(false);
            PneumaOrbs.Push(newOrb);
            OrbDictionary[newOrb] = newOrb.GetComponent<Orb>();
        }
    }

    void GetOrb(OrbType type, int value, Vector2 position)
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
        orb.value = value;
        orb.transform.position = position;
    }

    void ReturnOrb(GameObject orbObject)
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
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
