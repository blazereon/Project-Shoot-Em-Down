using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public enum DamageType
{
    Range,
    Melee
}

public enum Facing
{
    left,
    right
}

public class Entity : MonoBehaviour
{
    public Action OnConsumeMark;

    public bool isConsumingMark;

    public void ConsumeMark()
    {
        isConsumingMark = true;
        OnConsumeMark?.Invoke();
        isConsumingMark = false;
    }

    public void ReceiveEffect(GameObject pObject, Effect effect)
    {
        if (pObject != this.gameObject) return;

        effect.OnEffectStart();
    }
}
