using System;
using UnityEngine;

public class CheatEventSystem
{
    public static CheatEventSystem current;

    public static CheatEventSystem Current
    {
        get
        {
            if (current == null)
            {
                current = new CheatEventSystem();
            }
            return current;
        }
    }

    public event Action<PlayerStatDelta> OnPlayerModifyStat;
    public event Action<CompAbilityType> OnPlayerUpgradeAbility;

    public void InvokePlayerCheat(PlayerStatDelta playerStatDelta)
    {
        OnPlayerModifyStat?.Invoke(playerStatDelta);
    }

    public void InvokeUpgradeAbilityCheat(CompAbilityType type)
    {
        OnPlayerUpgradeAbility?.Invoke(type);
    }
}
