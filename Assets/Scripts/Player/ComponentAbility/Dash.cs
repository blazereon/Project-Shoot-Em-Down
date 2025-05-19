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

    public bool IsDashAvailable()
    {
        if (DashCount > 0) return true;
        return false;
    }

    public void ConsumeDash(Player player)
    {
        if (DashCount <= 0)
        {
            Debug.Log("Dash in cooldown");
            return;
        }
        DashCount--;
        if (!IsCooldownActive) player.StartCoroutine(DashCooldown());
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