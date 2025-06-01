using System.Collections;
using UnityEngine;

public struct DashAbilityStatus
{

    public ComponentAbilityData AbilityData;
    public int DashCount;
    public int MaxDashCount;
    public int EmpoweredDashCount;
    public int MaximumEmpoweredDashCount;

    public bool IsCooldownActive;
    public float Cooldown;
    public float CooldownTimer;
}


public class Dash : ComponentAbility
{
    //Regular dash
    public int DashCount = 1;
    public int MaxDashCount = 1;

    //Empowered dash
    public int EmpoweredDashCount = 1;
    public int MaximumEmpoweredDashCount = 1;

    Coroutine CooldownRoutineInstance;

    //Cooldown variables
    public bool IsCooldownActive = false;
    public float Cooldown = 5;
    public float CooldownUpdateInterval = 0.2f;
    private float _cooldownTimer = 0f;

    public override void UpgradeComponentHandler()
    {
        if (UpgradeTier == 1)
        {
            MaxDashCount = 2;
            MaximumEmpoweredDashCount = 2;
            EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
            return;
        }

        if (UpgradeTier == 2)
        {
            //Enable Penetrating Dash
            EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
            return;
        }

        if (UpgradeTier == 3)
        {
            //Enable Fragile Mark
            EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
            return;
        }
    }

    public override void EmpowermentHandler()
    {
        EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
        //Nothing
    }

    public bool IsDashAvailable()
    {
        if (DashCount > 0) return true;
        return false;
    }

    public void ConsumeDash(ManagerPlayerState player)
    {
        if (DashCount <= 0)
        {
            Debug.Log("Dash in cooldown");
            return;
        }
        DashCount--;
        if (Empowered) EmpoweredDashCount--;
        if (EmpoweredDashCount < 1)
        {
            EmpoweredDashCount = MaximumEmpoweredDashCount;
            Empowered = false;
        }
        if (!IsCooldownActive) CooldownRoutineInstance = player.StartCoroutine(DashCooldown());
        EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
    }

    public void ResetCooldown(ManagerPlayerState player)
    {
        player.StopCoroutine(CooldownRoutineInstance);
        CooldownRoutineInstance = null;
        IsCooldownActive = false;
        DashCount = MaxDashCount;
        EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
    }

    private DashAbilityStatus GetCurrentStatus()
    {
        return new DashAbilityStatus
        {
            AbilityData = GetComponentAbilityData(),
            DashCount = this.DashCount,
            MaxDashCount = this.MaxDashCount,
            EmpoweredDashCount = this.EmpoweredDashCount,
            MaximumEmpoweredDashCount = this.MaximumEmpoweredDashCount,
            IsCooldownActive = this.IsCooldownActive,
            Cooldown = this.Cooldown,
            CooldownTimer = this._cooldownTimer
        };
    }

    IEnumerator DashCooldown()
    {
        // player.isDashCooldown = true;
        // yield return new WaitForSeconds(player.DashRecovery);
        // player.isDashCooldown = false;
        IsCooldownActive = true;
        while (DashCount < MaxDashCount)
        {
            yield return new WaitForSeconds(CooldownUpdateInterval);
            Debug.Log("Dash cooldown: " + _cooldownTimer);
            _cooldownTimer += CooldownUpdateInterval;
            if (_cooldownTimer >= Cooldown)
            {
                _cooldownTimer = 0;
                DashCount++;
            }
            EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
        }
        IsCooldownActive = false;
        EventSystem.Current.UpdateDashAbilityUI(GetCurrentStatus());
    }
}