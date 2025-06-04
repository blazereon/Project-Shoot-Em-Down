public class Stun : Effect
{
    public Stun(Entity entity, float duration)
    {
        this.EntityHolder = entity;
        this.Duration = duration;
    }
    public override void OnEffectStart()
    {
        EntityHolder.AddEffect(this);
        EntityHolder.IsStunned = true;
        CoroutineHandler.Instance.StartCoroutine(EffectExpiration());
    }
    public override void OnEffectEnd()
    {
        EntityHolder.RemoveEffect(this);
        EntityHolder.IsStunned = false;
    }

    public override Effect Clone()
    {
        return new Stun(EntityHolder, Duration);
    }
}