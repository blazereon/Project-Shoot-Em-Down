using System.Collections;
using UnityEngine;
public enum OrbType
{
    Aggression,
    Pneuma
}

public class Orb : MonoBehaviour
{
    private float OrbMovementSpeed = 2f;
    public OrbType Type;
    public int value;
    public float IdleDuration;
    private bool _isIdle = true;

    void Update()
    {
        if (_isIdle == false)
        {
            transform.Translate(EventSystem.Current.PlayerLocation * OrbMovementSpeed * Time.deltaTime);
        }
    }

    void OnInstantiateOrb()
    {
        _isIdle = true;
        StartCoroutine(IdleCoroutine());
    }

    IEnumerator IdleCoroutine()
    {
        _isIdle = true;
        yield return new WaitForSeconds(IdleDuration);
        _isIdle = false;
    }
}