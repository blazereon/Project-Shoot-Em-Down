using UnityEngine;
using System.Collections;

public class RangedEnemyComponent : MonoBehaviour
{
    public void InstantiateProjectile(IEnemyProjectileSource enemy, Vector2 trajectory)
    {
        AudioManager.instance.RandomSFX(AudioManager.instance.enemyAttackRanged);
        GameObject _spawnProjectile = GameObject.Instantiate(enemy.projectile, enemy.transform.position, enemy.transform.rotation);
        Projectile _projectileScript = _spawnProjectile.GetComponent<Projectile>();

        _projectileScript.ProjectileCurrentProperties.AttackDamage = enemy.attackDmg;
        _projectileScript.ProjectileCurrentProperties.ProjectileSpeed = enemy.projectileSpd;
        _projectileScript.ProjectileCurrentProperties.Trajectory = trajectory;
        _projectileScript.ProjectileCurrentProperties.FiredBy = ProjectileOwner.Enemy;
    }

    public IEnumerator SingleFileBurst(IEnemyProjectileSource enemy, Vector3 playerPos, float bulletInterval)
    {
        for (int i = 0; i < enemy.burstCount; i++)
        {
            Vector2 _projectileTrajectory = (playerPos - enemy.transform.position).normalized;
            InstantiateProjectile(enemy, _projectileTrajectory);
            yield return new WaitForSeconds(bulletInterval);
        }
    }

    public IEnumerator TrackingBurst(IEnemyProjectileSource enemy, float bulletInterval)
    {
        for (int i = 0; i < enemy.burstCount; i++)
        {
            Vector3 _playerPos = EventSystem.Current.PlayerLocation;
            Vector2 _projectileTrajectory = (_playerPos - enemy.transform.position).normalized;
            InstantiateProjectile(enemy, _projectileTrajectory);
            yield return new WaitForSeconds(bulletInterval);
        }
    }
}
