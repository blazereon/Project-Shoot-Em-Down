using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangedGrounded : BaseRangedGrounded
{
    private float _distanceToPlayer;
    private float attackTimer;

    private GameObject _spawnProjectile;
    private GameObject _projectileParent;
    private Projectile _projectileScript;

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
                // _projectileParent.transform.position = enemy.transform.position;

                _spawnProjectile = GameObject.Instantiate(enemy.projectile, enemy.transform.position, enemy.transform.rotation);
                _projectileScript = _spawnProjectile.GetComponent<Projectile>();

                // _spawnProjectile.transform.parent = _projectileParent.transform;

                Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;
                Vector2 _projectileTrajectory = (_playerVec3 - enemy.transform.position).normalized;

                _projectileScript.atkDmg = enemy.attackDmg;
                _projectileScript.speed = enemy.projectileSpd;
                _projectileScript.trajectory = _projectileTrajectory;

                // _spawnProjectile.GetComponent<Rigidbody2D>().linearVelocity(_projectileTrajectory * enemy.projectileSpd);

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
}
