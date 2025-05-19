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
    public float Cooldown;
    public float CooldownUpdateInterval;
    private float _dashTimer = 0f;

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

    public bool isDashAvailable()
    {
        if (DashCount > 0) return true;
        return false;
    }

    public void ConsumeDash(Player player, out bool status)
    {
        if (DashCount <= 0)
        {
            Debug.Log("Dash in cooldown");
            status = false;
            return;
        }
        DashCount--;
        if (IsCooldownActive) player.StartCoroutine(DashCooldown());
        status = true;
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
            _dashTimer += CooldownUpdateInterval;
            if (_dashTimer >= Cooldown)
            {
                _dashTimer = 0;
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