using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public enum DamageType
{
    Range,
    Melee,
    Suicide
}

public enum Facing
{
    left,
    right
}

public class Entity : MonoBehaviour
{
    public List<Effect> CurrentEffect = new List<Effect>();
    public EffectContainerHandler CurrentEffectHandler;
    public Action OnConsumeMark;

    public bool isConsumingMark;

    public void ConsumeMark()
    {
        isConsumingMark = true;
        OnConsumeMark?.Invoke();
        isConsumingMark = false;
    }

    public void AddEffect(Effect effect)
    {
        Debug.Log(effect);
        CurrentEffect.Add(effect);
        //update effect handler
        CurrentEffectHandler.UpdateEffect(CurrentEffect);
    }

    public void RemoveEffect(Effect effect)
    {
        CurrentEffect.Remove(effect);
        //update effect handler
        CurrentEffectHandler.UpdateEffect(CurrentEffect);
    }

    public void ReceiveEffect(GameObject pObject, Effect effect)
    {
        if (pObject != this.gameObject) return;
        effect.OnEffectStart();

    }

    
}
