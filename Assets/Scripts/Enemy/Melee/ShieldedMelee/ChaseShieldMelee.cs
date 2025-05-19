using System.Collections;
using UnityEngine;

public class ChaseShieldMelee : BaseShieldMelee
{
    private bool _isPlayerInSight = false;
    private bool _isChaseMode = false;

    private int _rayNumber = 200;
    private int _rayMaxAngle = 180;
    private float _chaseDistance = 5f;
    private float _attackDistance = 1.3f;

    private LayerMask _layerMask;
    public override void EnterState(ManagerShieldMelee enemy)
    {
        _layerMask = LayerMask.GetMask("Wall", "Player");
        _isPlayerInSight = true;
        _isChaseMode = true;
        enemy.StartCoroutine(CheckPlayer(enemy));
    }

    public override void UpdateState(ManagerShieldMelee enemy)
    {

        if (!_isChaseMode)
        {
            Debug.Log("End of chase mode");
            enemy.hasPlayerDetected = false;
            enemy.SwitchState(enemy.wanderState);
        }

        _isPlayerInSight = CheckIfPlayerInSight(enemy);

        Debug.Log("Enemy-Player Distance: " + Vector2.Distance(enemy.transform.position, EventSystem.Current.PlayerLocation));

        if (_isPlayerInSight && (Vector2.Distance(enemy.transform.position, EventSystem.Current.PlayerLocation) <= _attackDistance))
        {
            enemy.SwitchState(enemy.attackState);
        }

        Debug.Log("Player In Sight: " + _isPlayerInSight);


    }

    public override void FixedUpdateState(ManagerShieldMelee enemy)
    {
        if (EventSystem.Current.PlayerLocation.x > enemy.transform.position.x)     // player on the right
        {
            if (!(enemy.transform.localScale.x > 0))    // enemy is not facing to the right
            {
                enemy.ScaleFlip();
            }

            enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.chaseSpeed * Time.fixedDeltaTime;

        }
        else
        {
            if (!(enemy.transform.localScale.x < 0))    // enemy is not facing to the left
            {
                enemy.ScaleFlip();
            }

            enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.chaseSpeed * Time.fixedDeltaTime;
        }
    }

    public bool CheckIfPlayerInSight(ManagerShieldMelee genericEnemy)
    {

        RaycastHit2D hitLeft = Physics2D.Raycast(genericEnemy.transform.position, Vector2.left, _chaseDistance, _layerMask);
        if (hitLeft.collider != null)
        {
            Debug.DrawRay(genericEnemy.transform.position, Vector2.left * _chaseDistance, Color.blue);
            if (hitLeft.collider.tag == "Player") return true;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(genericEnemy.transform.position, Vector2.right, _chaseDistance, _layerMask);
        if (hitRight.collider != null)
        {
            Debug.DrawRay(genericEnemy.transform.position, Vector2.right * _chaseDistance, Color.blue);
            if (hitRight.collider.tag == "Player") return true;
        }

        return false;
    }

    IEnumerator CheckPlayer(ManagerShieldMelee enemy)
    {
        while (_isChaseMode)
        {
            yield return new WaitForSeconds(1.5f);
            if (!_isPlayerInSight)
            {
                _isChaseMode = false;
                break;
            }
            var _ploc = EventSystem.Current.PlayerLocation;
            if (_ploc.x > enemy.transform.position.x)
            {
                enemy.facing = Enemy.EnemyFacing.Right;
            }
            else if (_ploc.x < enemy.transform.position.x)
            {
                enemy.facing = Enemy.EnemyFacing.Left;
            }
            _isChaseMode = true;
        }
    }
}
