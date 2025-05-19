using UnityEngine;

public abstract class ComponentAbility
{
    public int UpgradeTier = 0;

    public void UpgradeComponent()
    {
        if (UpgradeTier == 3)
        {
            Debug.Log("Maximum upgrade for this component ability reached");
        }
        UpgradeTier++;
        UpgradeComponentHandler();
    }

    public abstract void UpgradeComponentHandler();
}
