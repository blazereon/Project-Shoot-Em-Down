using System.Collections.Generic;
using UnityEngine;

public class EffectContainerHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    //Element 0 = Fragile Mark
    //Element 1 = Aether Mark
    //Element 2 = Stun
    public List<GameObject> Indicators;

    private void Awake()
    {
        //reset all
        foreach (GameObject indicator in Indicators)
        {
            indicator.SetActive(false);
        }
    }
    public void UpdateEffect(List<Effect> effect)
    {
        //reset all
        foreach (GameObject indicator in Indicators)
        {
            indicator.SetActive(false);
        }

        //update values
        foreach (Effect currentEffect in effect)
        {
            switch (currentEffect)
            {
                case FragileMark:
                    Indicators[0].SetActive(true);
                    break;
                case AetherMark:
                    Indicators[1].SetActive(true);
                    break;
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
