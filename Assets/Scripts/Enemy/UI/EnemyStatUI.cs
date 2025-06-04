using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct EnemyStatProps {
    public int Health;
    public int MaxHealth;
    public List<Effect> Effects;
}

public class EnemyStatUI : MonoBehaviour
{
    public HealthMeter HealthMeterInstance;
    public EffectContainerHandler EffectContainerHandlerInstance;

    public void UpdateValues(EnemyStatProps props)
    {
        HealthMeterInstance.UpdateValue(props.Health, props.MaxHealth);
        EffectContainerHandlerInstance.UpdateEffect(props.Effects);
    }
}
