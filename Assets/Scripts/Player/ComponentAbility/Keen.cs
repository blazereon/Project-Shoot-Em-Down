using System;
using System.Collections;
using UnityEngine;

public struct KeenAbilityStatus
{
    public ComponentAbilityData AbilityData;
    public float KeenCooldown;
    public float TandemCooldown;
    public float TandemExpiration;
    public float KeenCooldownTimer, TandemCooldownTimer, TandemExpirationTimer;
    public bool IsTandemCooldown;
    public bool IsTandemTriggered;
    public bool IsKeenCooldown;
}

public class Keen : ComponentAbility
{
    public Player PlayerInstance;
    public Action OnConsumeMark;

    Coroutine TandemCooldownInstance;
    Coroutine TandemExpirationInstance;
    Coroutine KeenCooldownInstance;

    public float KeenCooldown = 10f;
    public float TandemCooldown = 6f;
    public float TandemExpiration = 3f;
    public float KeenCooldownTimer, TandemCooldownTimer, TandemExpirationTimer = 0;
    public float updateTimerRate = 0.2f;

    public bool IsTandemCooldown = false;
    public bool IsTandemTriggered = false;
    public bool IsKeenCooldown = false;

    public Keen(Player player)
    {
        PlayerInstance = player;
    }

    public override void EmpowermentHandler()
    {
        OnConsumeMark?.Invoke();
        Empowered = false;
    }

    public override void UpgradeComponentHandler()
    {
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
    }

    public void TriggerTandem()
    {
        if (IsTandemCooldown)
        {
            Debug.Log("Tandem is still in cooldown");
            return;
        }
        Debug.Log("Tandem Triggered");
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
        TandemExpirationTimer = 0;
        if (IsTandemTriggered == false)
        {

            TandemExpirationInstance = CoroutineHandler.Instance.StartCoroutine(TandemExpirationCoroutine());
        }
    }

    public void ConsumeKeen()
    {
        if (IsKeenCooldown)
        {
            Debug.Log("Keen is still in cooldown!");
            return;
        }

        //Apply Invincibility Effect
        var _InvincibilityInstance = new Invincibility(PlayerInstance, 1f);
        _InvincibilityInstance.EntityHolder = PlayerInstance;

        if (Empowered)
        {
            _InvincibilityInstance.Duration = 1.5f;
            Empowered = false;
        }
        else
        {
            _InvincibilityInstance.Duration = 1f;
        }

        EventSystem.Current.ApplyEffect(PlayerInstance.gameObject, _InvincibilityInstance);
        KeenCooldownInstance = CoroutineHandler.Instance.StartCoroutine(KeenCooldownCoroutine());
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
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
        CoroutineHandler.Instance.StartCoroutine(TandemCooldownCoroutine());
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
    }

    private KeenAbilityStatus GetCurrentStatus()
    {
        return new KeenAbilityStatus
        {
            AbilityData = GetComponentAbilityData(),

            KeenCooldown = this.KeenCooldown,
            TandemCooldown = this.TandemCooldown,
            TandemExpiration = this.TandemExpiration,

            KeenCooldownTimer = this.KeenCooldownTimer,
            TandemCooldownTimer = this.TandemCooldownTimer,
            TandemExpirationTimer = this.TandemExpirationTimer,

            IsTandemCooldown = this.IsTandemCooldown,
            IsTandemTriggered = this.IsTandemTriggered,
            IsKeenCooldown = this.IsKeenCooldown
        };
    }

    IEnumerator KeenCooldownCoroutine()
    {
        IsKeenCooldown = true;
        while (KeenCooldownTimer <= TandemCooldown)
        {
            yield return new WaitForSeconds(updateTimerRate);
            KeenCooldownTimer += updateTimerRate;
            EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
        }
        KeenCooldownTimer = 0;
        IsKeenCooldown = false;
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
    }

    IEnumerator TandemCooldownCoroutine()
    {
        IsTandemCooldown = true;
        while (TandemCooldownTimer <= TandemCooldown)
        {
            yield return new WaitForSeconds(updateTimerRate);
            TandemCooldownTimer += updateTimerRate;
            EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
        }
        IsTandemCooldown = false;
        TandemCooldownTimer = 0;
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
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
            EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
        }
        IsTandemTriggered = false;
        EventSystem.Current.UpdateKeenAbilityUI(GetCurrentStatus());
        yield return null;
    }

}