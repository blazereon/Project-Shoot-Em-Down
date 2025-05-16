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
    private float _attackTimer;
    private bool _startAttack;

    private GameObject _spawnProjectile;
    private GameObject _projectileParent;
    private Projectile _projectileScript;
    private Coroutine _attackCoroutine;
    private Vector2 _projectileTrajectory;

    public override void EnterState(ManagerRangedGrounded enemy)
    {
        _attackTimer = 0f;
        _startAttack = true;
        // _projectileParent = new GameObject("ProjectileParent");
        // _projectileParent.tag = "ProjectileParent";
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        if (enemy.enemyCollider == EventSystem.Current.PlayerCollider)
        {
            Debug.LogWarning("Player and Enemy colliders are the same! " + enemy.enemyCollider + " " + EventSystem.Current.PlayerCollider);
        }
        if (enemy.enemyCollider == null || EventSystem.Current.PlayerCollider == null)
        {
            enemy.hasPlayerDetected = false;
            enemy.SwitchState(enemy.wanderState);
        }
        else
        {
            _distanceToPlayer = Physics2D.Distance(EventSystem.Current.PlayerCollider, enemy.enemyCollider).distance;

            _attackTimer += Time.deltaTime;

            if (enemy.startEngagementRange < _distanceToPlayer)
            {
                enemy.SwitchState(enemy.chaseState);
            }
            else
            {
                _distanceToPlayer = Physics2D.Distance(EventSystem.Current.PlayerCollider, enemy.enemyCollider).distance;

                _attackTimer += Time.deltaTime;

                if (enemy.shootMode == ManagerRangedGrounded.shootType.Single || enemy.shootMode == ManagerRangedGrounded.shootType.AOEBurst)
                {
                    if (_attackTimer > enemy.attackSpd)
                    {

                        Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;

                        if (enemy.shootMode == ManagerRangedGrounded.shootType.Single)
                        {

                            _projectileTrajectory = (_playerVec3 - enemy.transform.position).normalized;

                            // InstantiateProjectile(enemy, _projectileTrajectory);
                            enemy.InstantiateProjectile(enemy.attackDmg, enemy.projectileSpd, _projectileTrajectory, enemy.projectile);

                        }
                        else if (enemy.shootMode == ManagerRangedGrounded.shootType.AOEBurst)
                        {

                            float _burstStep = enemy.burstSpread / enemy.burstCount;

                            Vector2 _direction2player = (_playerVec3 - enemy.transform.position);
                            float _startAngle = (Mathf.Atan2(_direction2player.y, _direction2player.x) * Mathf.Rad2Deg) - (enemy.burstSpread / 2);

                            for (int i = 0; i < enemy.burstCount; i++)
                            {
                                _projectileTrajectory = (Quaternion.Euler(0, 0, _startAngle + (i * _burstStep)) * enemy.transform.right);
                                // InstantiateProjectile(enemy, _projectileTrajectory);
                                enemy.InstantiateProjectile(enemy.attackDmg, enemy.projectileSpd, _projectileTrajectory, enemy.projectile);
                            }

                        }

                        _attackTimer = 0f;

                    }
                }
                else
                {
                    if (_attackTimer > enemy.projectileInterval && !enemy.isFiringBurst)
                    {
                        Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;

                        if (enemy.shootMode == ManagerRangedGrounded.shootType.SingleFileBurst)
                        {
                            if (_attackCoroutine != null)
                            {
                                enemy.StopCoroutine(_attackCoroutine);
                            }
                            // enemy.StartCoroutine(SingleFileBurst(enemy, _playerVec3, enemy.projectileInterval));
                            _attackCoroutine = enemy.StartCoroutine(enemy.SingleFileBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, enemy.attackSpd, _playerVec3, enemy.projectile));

                        }
                        else if (enemy.shootMode == ManagerRangedGrounded.shootType.TrackingBurst)
                        {
                            if (_attackCoroutine != null)
                            {
                                enemy.StopCoroutine(_attackCoroutine);
                            }
                            // enemy.StartCoroutine(TrackingBurst(enemy, enemy.projectileInterval));
                            _attackCoroutine = enemy.StartCoroutine(enemy.TrackingBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, enemy.attackSpd, enemy.transform.position, enemy.projectile));

                        }

                        _attackTimer = 0f;
                    }

                }
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
        AudioManager.instance.RandomSFX(AudioManager.instance.enemyAttackRanged);
        _spawnProjectile = GameObject.Instantiate(enemy.projectile, enemy.transform.position, enemy.transform.rotation);

        _projectileScript = _spawnProjectile.GetComponent<Projectile>();

        _projectileScript.ProjectileCurrentProperties.AttackDamage = enemy.attackDmg;
        _projectileScript.ProjectileCurrentProperties.ProjectileSpeed = enemy.projectileSpd;
        _projectileScript.ProjectileCurrentProperties.Trajectory = trajectory;

        _projectileScript.ProjectileCurrentProperties.FiredBy = ProjectileOwner.Enemy;
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
