using UnityEngine;

// This effect does nothing by itself, just existing so we know that we can instakill the entity
public class Instakill : Effect
{
    public Instakill(Entity entity, float duration)
    {
        this.EntityHolder = entity;
        this.Duration = duration;
    }

    public override void OnEffectStart()
    {
        EntityHolder.AddEffect(this);
        CoroutineHandler.Instance.StartCoroutine(EffectExpiration());
    }
    public override void OnEffectEnd()
    {
        EntityHolder.RemoveEffect(this);

    }

    public override Effect Clone()
    {
        return new Instakill(null, Duration);
    }
}
