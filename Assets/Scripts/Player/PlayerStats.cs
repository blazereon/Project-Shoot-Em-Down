using System;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    [SerializeField]
    //Can be edited using inspector menu
    public int Health;
    public int MaxHealth;
    public int Violence;
    public int MaxMomentum;
    public int MaxPneumatic;
    public float ProjectileSpeed;
    public int MaxAggression;
    public int MaxViolence;
    public int MaxChain;
    public float ChainDuration;
    //This should not be edited thru inspector and must only be accessed via code
    [NonSerialized] public int Momentum;
    [NonSerialized] public int Chain;
    [NonSerialized] public int Pneumatic;
    [NonSerialized] public float ChainTimer;
    [NonSerialized] public int Aggression;
    [NonSerialized] public Player.AttackType CurrentAttackType;
}