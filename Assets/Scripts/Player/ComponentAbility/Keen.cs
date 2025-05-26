using System;
using System.Collections;
using UnityEngine;

public class Keen : ComponentAbility
{
    public Player player;
    public Action OnConsumeMark;
     
    Coroutine TandemCooldownInstance;
    Coroutine TandemExpirationInstance;

    public float TandemCooldown = 6f;
    public float TandemExpiration = 3f;
    public float TandemCooldownTimer, TandemExpirationTimer = 0;
    public float updateTimerRate = 0.2f;

    public bool IsTandemCooldown = false;
    public bool IsTandemTriggered = false;

    public override void EmpowermentHandler()
    {
        OnConsumeMark?.Invoke();
    }

    public override void UpgradeComponentHandler()
    {

    }

    public void TriggerTandem()
    {
        if (IsTandemCooldown)
        {
            Debug.Log("Tandem is still in cooldown");
            return;
        }
        Debug.Log("Tandem Triggered");
        
        TandemExpirationTimer = 0;
        if (IsTandemTriggered == false)
        {

            TandemExpirationInstance = CoroutineHandler.Instance.StartCoroutine(TandemExpirationCoroutine());
        }
    }

    public void ConsumeTandem()
    {
        if (IsTandemCooldown)
        {
            Debug.Log("Tandem is still in cooldown");
            return;
        }
        TandemExpirationTimer = 0;
        if (IsTandemTriggered == true)
        {
            IsTandemTriggered = false;
            CoroutineHandler.Instance.StopCoroutine(TandemExpirationInstance);
        }

        if (!IsTandemCooldown)
        {
            CoroutineHandler.Instance.StartCoroutine(TandemCooldownCoroutine());
        }
    }

    IEnumerator TandemCooldownCoroutine()
    {
        IsTandemCooldown = true;
        while (TandemCooldownTimer <= TandemCooldown)
        {
            yield return new WaitForSeconds(updateTimerRate);
            TandemCooldownTimer += updateTimerRate;
        }
        IsTandemCooldown = false;
        yield return null;
    }

    IEnumerator TandemExpirationCoroutine()
    {
        IsTandemTriggered = true;
        Debug.Log("Tandem passive triggered");
        while (TandemExpirationTimer <= TandemExpiration)
        {
            yield return new WaitForSeconds(updateTimerRate);
            TandemExpirationTimer += updateTimerRate;
        }
        IsTandemTriggered = false;
        yield return null;
    }

}