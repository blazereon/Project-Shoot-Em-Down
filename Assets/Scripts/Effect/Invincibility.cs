using UnityEngine;

public class Invincibility : Effect
{
    Coroutine EffectDurationInstance;
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


}