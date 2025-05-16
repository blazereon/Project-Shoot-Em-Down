using UnityEngine;

public interface IEnemyProjectileSource
{
    GameObject projectile { get; }
    Transform transform { get; }
    int attackDmg { get; }
    float projectileSpd { get; }
    int burstCount { get; }

}