using System;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    [SerializeField]
    //Can be edited using inspector menu
    public int Health;
    public int MaxHealth;
    public int MeleeDamage;
    public int RangedDamage;
    public int Violence;
    public int SkillPoint;
    public int MaxMomentum;
    public int MaxPneumatic;
    public int MaxXp;
    public float ProjectileSpeed;
    public int MaxAggression;
    public int MaxViolence;
    public int MaxChain;
    public float ChainDuration;
    public float AttackRate;
    //This should not be edited thru inspector and must only be accessed via code
    [NonSerialized] public int Xp;
    [NonSerialized] public int Momentum;
    [NonSerialized] public int Chain;
    [NonSerialized] public int Pneumatic;
    [NonSerialized] public float ChainTimer;
    [NonSerialized] public int Aggression;
    [NonSerialized] public Player.AttackType CurrentAttackType;
    [NonSerialized] public DashAbilityStatus DashAbility;
    [NonSerialized] public KeenAbilityStatus KeenAbility;
    [NonSerialized] public DestructiveBoltStatus DestructiveBoltAbility;
}