using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Security.Cryptography;

public class AttackRangedGrounded : BaseRangedGrounded
{
    private float _distanceToPlayer;
    private float attackTimer;

    private GameObject _spawnProjectile;
    private GameObject _projectileParent;
    private Projectile _projectileScript;
    private Vector2 _projectileTrajectory;

    public override void EnterState(ManagerRangedGrounded enemy)
    {
        attackTimer = 0f;
        // _projectileParent = new GameObject("ProjectileParent");
        // _projectileParent.tag = "ProjectileParent";
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        _distanceToPlayer = Vector2.Distance(EventSystem.Current.PlayerLocation, enemy.transform.position);

        attackTimer += Time.deltaTime;

        if (enemy.startEngagementRange < _distanceToPlayer)
        {
            enemy.SwitchState(enemy.chaseState);
        }
        else
        {
            if (attackTimer > enemy.attackSpd)
            {

                Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;

                if (enemy.shootMode == ManagerRangedGrounded.shootType.Single)
                {

                    _projectileTrajectory = (_playerVec3 - enemy.transform.position).normalized;

                    InstantiateProjectile(enemy, _projectileTrajectory);

                }
                else if (enemy.shootMode == ManagerRangedGrounded.shootType.SingleFileBurst)
                {

                    enemy.StartCoroutine(SingleFileBurst(enemy, _playerVec3, enemy.projectileInterval));

                }
                else if (enemy.shootMode == ManagerRangedGrounded.shootType.TrackingBurst)
                {

                    enemy.StartCoroutine(TrackingBurst(enemy, enemy.projectileInterval));

                }
                else
                {

                    float _burstStep = enemy.burstSpread / enemy.burstCount;

                    Vector2 _direction2player = (_playerVec3 - enemy.transform.position);
                    float _startAngle = (Mathf.Atan2(_direction2player.y, _direction2player.x) * Mathf.Rad2Deg) - (enemy.burstSpread / 2);

                    for (int i = 0; i < enemy.burstCount; i++)
                    {
                        _projectileTrajectory = (Quaternion.Euler(0, 0, _startAngle + (i * _burstStep)) * enemy.transform.right);
                        InstantiateProjectile(enemy, _projectileTrajectory);
                    }

                }

                attackTimer = 0f;

            }
        }

    }

    public override void FixedUpdateState(ManagerRangedGrounded enemy)
    {
        if (EventSystem.Current.PlayerLocation.x > enemy.transform.position.x)     // player on the right
        {
            if (!(enemy.transform.localScale.x > 0))    // enemy is not facing to the right
            {
                enemy.Flip();
            }

        }
        else
        {
            if (!(enemy.transform.localScale.x < 0))    // enemy is not facing to the left
            {
                enemy.Flip();
            }
        }
    }

    public void InstantiateProjectile (ManagerRangedGrounded enemy, Vector2 trajectory)
    {
        _spawnProjectile = GameObject.Instantiate(enemy.projectile, enemy.transform.position, enemy.transform.rotation);

        _projectileScript = _spawnProjectile.GetComponent<Projectile>();

        _projectileScript.atkDmg = enemy.attackDmg;
        _projectileScript.speed = enemy.projectileSpd;
        _projectileScript.trajectory = trajectory;

        _projectileScript.destroyOnly.Add(LayerMask.GetMask("Wall"));
        _projectileScript.destroyOnly.Add(LayerMask.GetMask("Ground"));
    }

    IEnumerator SingleFileBurst (ManagerRangedGrounded enemy, Vector3 playerPos, float bulletInterval)
    {
        for (int i = 0; i < enemy.burstCount; i++)
        {
            _projectileTrajectory = (playerPos - enemy.transform.position).normalized;
            InstantiateProjectile(enemy, _projectileTrajectory);

            yield return new WaitForSeconds(bulletInterval);
        }
    }

    IEnumerator TrackingBurst (ManagerRangedGrounded enemy,  float bulletInterval)
    {
        for (int i = 0; i < enemy.burstCount; i++)
        {
            Vector3 _playerPos = EventSystem.Current.PlayerLocation;
            _projectileTrajectory = (_playerPos - enemy.transform.position).normalized;
            InstantiateProjectile(enemy, _projectileTrajectory);

            yield return new WaitForSeconds(bulletInterval);
        }
    }
}
