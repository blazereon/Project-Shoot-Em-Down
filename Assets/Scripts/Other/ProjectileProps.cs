using System;
using Unity.VisualScripting;
using UnityEngine;


public enum ProjectileOwner
{
    Player,
    Enemy
}

public enum LayerDestinations
{
    Player,
    Enemy
}

[Serializable]
public struct ProjectileProps 
{
    public float ProjectileSpeed;
    public int AttackDamage;
    public Vector2 Trajectory;
    public LayerMask DestroyOnly;
    public ProjectileOwner FiredBy;
    public LayerDestinations Destination;
}