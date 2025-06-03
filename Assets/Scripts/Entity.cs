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

public abstract class Entity : MonoBehaviour
{
    public List<Effect> CurrentEffect = new List<Effect>();
    public Action OnConsumeMark;

    public bool isConsumingMark;
    public bool IsStunned;
    public bool CanTakeDamage = true;

    public void ConsumeMark()
    {
        isConsumingMark = true;
        OnConsumeMark?.Invoke();
        isConsumingMark = false;
    }

    public void AddEffect(Effect effect)
    {
        CurrentEffect.Add(effect);
        //update effect handler
        // CurrentEffectHandler.UpdateEffect(CurrentEffect);
        UpdateUIData();
    }

    public void RemoveEffect(Effect effect)
    {
        CurrentEffect.Remove(effect);
        //update effect handler
        // CurrentEffectHandler.UpdateEffect(CurrentEffect);
        UpdateUIData();
    }

    public void ReceiveEffect(GameObject pObject, Effect effect)
    {
        if (pObject != this.gameObject) return;
        effect.OnEffectStart();
    }

    public abstract void UpdateUIData();

    
}
