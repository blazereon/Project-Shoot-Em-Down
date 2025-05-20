using System.Collections;
using UnityEngine;

public abstract class Effect
{
    public float Duration;
    public abstract void OnEffectStart();
    public abstract void OnEffectEnd();

    public IEnumerator EffectExpiration()
    {
        Debug.Log("Effect will expire in: " + Duration + " seconds");
        yield return new WaitForSeconds(Duration);
        Debug.Log("Ending effect");
        OnEffectEnd();
    }
}