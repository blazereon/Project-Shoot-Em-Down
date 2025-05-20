using System.Collections;
using UnityEngine;

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
            return;
        }

        if (UpgradeTier == 2)
        {
            //Enable Penetrating Dash
            return;
        }

        if (UpgradeTier == 3)
        {
            //Enable Fragile Mark
            return;
        }
    }

    public override void EmpowermentHandler()
    {
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
    }

    public void ResetCooldown(ManagerPlayerState player)
    {
        player.StopCoroutine(CooldownRoutineInstance);
        CooldownRoutineInstance = null;
        IsCooldownActive = false;
        DashCount = MaxDashCount;
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
        }
        IsCooldownActive = false;
    }

    IEnumerator EmpoweredDash()
    {
        yield return null;
    }
}