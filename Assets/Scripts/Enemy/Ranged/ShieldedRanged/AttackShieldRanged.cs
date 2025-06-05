using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AttackShieldRanged: BaseShieldRanged
{
    private float _distanceToPlayer;
    private float _attackTimer;
    private bool _startAttack;

    private GameObject _spawnProjectile;
    private GameObject _projectileParent;
    private Projectile _projectileScript;
    private Coroutine _attackCoroutine;
    private Coroutine _turnCoroutine;
    private Vector2 _projectileTrajectory;

    public override void EnterState(ManagerShieldRanged enemy)
    {
        _attackTimer = 0f;
        _startAttack = true;
        _turnCoroutine = null;
    }

    public override void UpdateState(ManagerShieldRanged enemy)
    {
        // switch to stun
        if (enemy.IsStunned)
        {
            enemy.prevState = this;
            enemy.SwitchState(enemy.stunState);
        }
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
                enemy.hitDetect.weakSpotActive = false;
                enemy.SwitchState(enemy.chaseState);
            }
            else
            {
                _distanceToPlayer = Physics2D.Distance(EventSystem.Current.PlayerCollider, enemy.enemyCollider).distance;

                _attackTimer += Time.deltaTime;

                if (enemy.shootMode == ManagerShieldRanged.shootType.Single || enemy.shootMode == ManagerShieldRanged.shootType.AOEBurst)
                {
                    // Put weakspot activation and dertection here since we dont want to turn it on/off every atkSpd which is very short
                    enemy.hitDetect.weakSpotActive = true;

                    if (_attackTimer > enemy.attackSpd)
                    {
                        // Debug.Log("ws hit stat: " + enemy.hitDetect.weakSpotActive + " " + enemy.isWeakSpotActive + " " + enemy.hitDetect.WeakSpotHitDetected());
                        SingleShoot(enemy);
                        // enemy.hitDetect.weakSpotActive = false;

                        _attackTimer = 0f;

                    }
                }
                else
                {
                    if (_attackTimer > enemy.projectileInterval && !enemy.isFiringBurst)
                    {
                        // Put weakspot activation and dertection inside the timer since there's could be enough time inside burst shooting duration
                        enemy.hitDetect.weakSpotActive = true;

                        BurstShoot(enemy);
                        enemy.hitDetect.weakSpotActive = false;

                        _attackTimer = 0f;
                    }

                }
            }
        }
    }

    public override void FixedUpdateState(ManagerShieldRanged enemy)
    {
        if (_turnCoroutine == null)
        {
            _turnCoroutine = enemy.StartCoroutine(DelayTurn(enemy));
        }
    }

    private void SingleShoot(ManagerShieldRanged enemy)
    {
        // Single shoot tracks player in real time, thus, not affected by delayed shooting
        Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;

        if (enemy.shootMode == ManagerShieldRanged.shootType.Single)
        {

            _projectileTrajectory = (_playerVec3 - enemy.transform.position).normalized;

            // InstantiateProjectile(enemy, _projectileTrajectory);
            enemy.InstantiateProjectile(enemy.attackDmg, enemy.projectileSpd, _projectileTrajectory, enemy.projectile);

        }
        else if (enemy.shootMode == ManagerShieldRanged.shootType.AOEBurst)
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
    }

    private void BurstShoot(ManagerShieldRanged enemy)
    {
        enemy.hitDetect.weakSpotActive = true;

        Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;

        if (enemy.shootMode == ManagerShieldRanged.shootType.SingleFileBurst)
        {
            if (_attackCoroutine != null)
            {
                enemy.StopCoroutine(_attackCoroutine);
            }
            // enemy.StartCoroutine(SingleFileBurst(enemy, _playerVec3, enemy.projectileInterval));
            _attackCoroutine = enemy.StartCoroutine(enemy.SingleFileBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, enemy.attackSpd, _playerVec3, enemy.projectile));

        }
        else if (enemy.shootMode == ManagerShieldRanged.shootType.TrackingBurst)
        {
            if (_attackCoroutine != null)
            {
                enemy.StopCoroutine(_attackCoroutine);
            }
            // enemy.StartCoroutine(TrackingBurst(enemy, enemy.projectileInterval));
            _attackCoroutine = enemy.StartCoroutine(enemy.TrackingBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, enemy.attackSpd, enemy.transform.position, enemy.projectile));

        }
        else if (enemy.shootMode == ManagerShieldRanged.shootType.TwirlBurst)
        {
            if (_attackCoroutine != null)
            {
                enemy.StopCoroutine(_attackCoroutine);
            }

            Debug.Log("Atk coroutine status: " + _attackCoroutine);

            float _burstStep = enemy.burstSpread / enemy.burstCount;

            Vector2 _direction2player = (_playerVec3 - enemy.transform.position);
            float _startAngle = (Mathf.Atan2(_direction2player.y, _direction2player.x) * Mathf.Rad2Deg) - (enemy.burstSpread / 2);


            _attackCoroutine = enemy.StartCoroutine(enemy.TwirlBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, _startAngle, _burstStep, enemy.attackSpd, enemy.transform.right, enemy.projectile));
        }
    }

    IEnumerator DelayTurn(ManagerShieldRanged enemy)
    {

        yield return new WaitForSeconds(1.5f);

        if (EventSystem.Current.PlayerLocation.x > enemy.transform.position.x)     // player on the right
        {
            if (!(enemy.transform.localScale.x > 0))    // enemy is not facing to the right
            {
                enemy.Flip();

                if (_attackCoroutine != null)
                {
                    enemy.StopCoroutine(_attackCoroutine);
                    enemy.isFiringBurst = false;
                }
            }

            // enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.chaseSpeed * Time.fixedDeltaTime;

        }
        else
        {
            if (!(enemy.transform.localScale.x < 0))    // enemy is not facing to the left
            {
                enemy.Flip();

                if (_attackCoroutine != null)
                {
                    enemy.StopCoroutine(_attackCoroutine);
                    enemy.isFiringBurst = false;
                }
            }

            // enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.chaseSpeed * Time.fixedDeltaTime;
        }

        

        _turnCoroutine = null;
    }
}
