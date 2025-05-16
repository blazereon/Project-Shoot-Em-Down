using UnityEngine;

public class AttackStaticRanged : BaseStaticRanged
{
    private float _distanceToPlayer;
    private float _attackTimer;
    private bool _startAttack;

    private GameObject _spawnProjectile;
    private GameObject _projectileParent;
    private Projectile _projectileScript;
    private Coroutine _attackCoroutine;
    private Vector2 _projectileTrajectory;

    public override void EnterState(ManagerStaticRanged enemy)
    {
        Debug.Log("StR Enter Attack ");
        _startAttack = true;
        enemy.isFiringBurst = false;

        AudioManager.instance.PlayFX(AudioManager.instance.enemyChaseAlert[0], false);
    }

    public override void UpdateState(ManagerStaticRanged enemy)
    {
        if (enemy.enemyCollider == null || EventSystem.Current.PlayerCollider == null)
        {
            _attackCoroutine = null;
            _startAttack = false;
            enemy.SwitchState(enemy.scanState);
        }
        else
        {
            _distanceToPlayer = Physics2D.Distance(EventSystem.Current.PlayerCollider, enemy.enemyCollider).distance;

            _attackTimer += Time.deltaTime;

            if (enemy.shootMode == ManagerStaticRanged.shootType.Single || enemy.shootMode == ManagerStaticRanged.shootType.AOEBurst)
            {
                if (_attackTimer > enemy.attackSpd)
                {

                    Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;

                    if (enemy.shootMode == ManagerStaticRanged.shootType.Single)
                    {

                        _projectileTrajectory = (_playerVec3 - enemy.transform.position).normalized;

                        // InstantiateProjectile(enemy, _projectileTrajectory);
                        enemy.InstantiateProjectile(enemy.attackDmg, enemy.projectileSpd, _projectileTrajectory, enemy.projectile);

                    }
                    else if (enemy.shootMode == ManagerStaticRanged.shootType.AOEBurst)
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

                    if (enemy.shootMode == ManagerStaticRanged.shootType.SingleFileBurst)
                    {
                        if (_attackCoroutine != null)
                        {
                            enemy.StopCoroutine(_attackCoroutine);
                        }
                        // enemy.StartCoroutine(SingleFileBurst(enemy, _playerVec3, enemy.projectileInterval));
                        _attackCoroutine = enemy.StartCoroutine(enemy.SingleFileBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, enemy.attackSpd, _playerVec3, enemy.projectile));

                    }
                    else if (enemy.shootMode == ManagerStaticRanged.shootType.TrackingBurst)
                    {
                        if (_attackCoroutine != null)
                        {
                            enemy.StopCoroutine(_attackCoroutine);
                        }
                        // enemy.StartCoroutine(TrackingBurst(enemy, enemy.projectileInterval));
                        _attackCoroutine = enemy.StartCoroutine(enemy.TrackingBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, enemy.attackSpd, enemy.transform.position, enemy.projectile));

                    }
                    else if (enemy.shootMode == ManagerStaticRanged.shootType.TwirlBurst)
                    {

                        if (_attackCoroutine != null)
                        {
                            enemy.StopCoroutine(_attackCoroutine);
                        }

                        float _burstStep = enemy.burstSpread / enemy.burstCount;

                        Vector2 _direction2player = (_playerVec3 - enemy.transform.position);
                        float _startAngle = (Mathf.Atan2(_direction2player.y, _direction2player.x) * Mathf.Rad2Deg) - (enemy.burstSpread / 2);


                        _attackCoroutine = enemy.StartCoroutine(enemy.TwirlBurst(_startAttack, enemy.attackDmg, enemy.projectileSpd, enemy.burstCount, _startAngle, _burstStep, enemy.attackSpd, enemy.transform.right, enemy.projectile));
                    }

                    _attackTimer = 0f;
                }

            }
        }        
    }

    public override void FixedUpdateState(ManagerStaticRanged enemy)
    {
        if (!enemy.hitDetect.PlayerDetected())
        {
            if (_attackCoroutine != null)
            {
                enemy.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
                _startAttack = false;
            }

            enemy.SwitchState(enemy.scanState);
        }
    }
}
