using UnityEngine;


public struct ComponentAbilityData
{
    public int UpgradeTier;
    public bool Empowered;
}
public abstract class ComponentAbility
{
    public int UpgradeTier = 0;
    public bool Empowered = false;

    public void UpgradeComponent()
    {
        if (UpgradeTier == 3)
        {
            Debug.Log("Maximum upgrade for this component ability reached");
        }
        UpgradeTier++;
        UpgradeComponentHandler();
    }

    public void SetEmpowered(ComponentAbility abilityToEmpower)
    {
        if (abilityToEmpower == this)
        {
            Empowered = true;
            EmpowermentHandler();
        }
    }

    public ComponentAbilityData GetComponentAbilityData()
    {
        return new ComponentAbilityData
        {
            UpgradeTier = this.UpgradeTier,
            Empowered = this.Empowered
        };
    }

    public abstract void EmpowermentHandler();
    public abstract void UpgradeComponentHandler();
}
