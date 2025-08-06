using System;
using UnityEngine;

public class PlayerStatDelta
{
    // Absolute value
    [NonSerialized] public int? Health;
    [NonSerialized] public int? MaxHealth;
    [NonSerialized] public int? Violence;
    [NonSerialized] public int? SkillPoint;
    [NonSerialized] public int? MaxMomentum;
    [NonSerialized] public int? MaxPneumatic;
    [NonSerialized] public float? ProjectileSpeed;
    [NonSerialized] public int? MaxAggression;
    [NonSerialized] public int? MaxViolence;
    [NonSerialized] public int? MaxChain;
    [NonSerialized] public float? ChainDuration;
    [NonSerialized] public float? AttackRate;

    [NonSerialized] public int? Momentum;
    [NonSerialized] public int? Chain;
    [NonSerialized] public int? Pneumatic;
    [NonSerialized] public float? ChainTimer;
    [NonSerialized] public int? Aggression;
    [NonSerialized] public Player.AttackType? CurrentAttackType;

    // Increment Value
    [NonSerialized] public int? deltaHealth;
    [NonSerialized] public int? deltaMaxHealth;
    [NonSerialized] public int? deltaViolence;
    [NonSerialized] public int? deltaSkillPoint;
    [NonSerialized] public int? deltaMaxMomentum;
    [NonSerialized] public int? deltaMaxPneumatic;
    [NonSerialized] public float? deltaProjectileSpeed;
    [NonSerialized] public int? deltaMaxAggression;
    [NonSerialized] public int? deltaMaxViolence;
    [NonSerialized] public int? deltaMaxChain;
    [NonSerialized] public float? deltaChainDuration;
    [NonSerialized] public float? deltaAttackRate;

    [NonSerialized] public int? deltaMomentum;
    [NonSerialized] public int? deltaChain;
    [NonSerialized] public int? deltaPneumatic;
    [NonSerialized] public float? deltaChainTimer;
    [NonSerialized] public int? deltaAggression;
}

