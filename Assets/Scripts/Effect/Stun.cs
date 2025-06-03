public class Stun : Effect
{
    public override void OnEffectStart()
    {
        EntityHolder.AddEffect(this);
        EntityHolder.IsStunned = true;
    }
    public override void OnEffectEnd()
    {
        EntityHolder.RemoveEffect(this);
        EntityHolder.IsStunned = false;
    }
}