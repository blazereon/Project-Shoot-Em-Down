using UnityEngine;

public class Invincibility : Effect
{
    Coroutine EffectDurationInstance;

    public Invincibility(Entity entity, float duration)
    {
        EntityHolder = entity;
        Duration = duration;
    }

    public override void OnEffectStart()
    {
        EntityHolder.CanTakeDamage = false;
        EntityHolder.AddEffect(this);
        EffectDurationInstance = CoroutineHandler.Instance.StartCoroutine(EffectExpiration());
    }

    public override void OnEffectEnd()
    {
        EntityHolder.RemoveEffect(this);
        EntityHolder.CanTakeDamage = true;
    }

    public override Effect Clone()
    {
        return new Invincibility(null, Duration);
    }
}