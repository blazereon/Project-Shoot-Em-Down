using System.Collections;
using UnityEngine;

public class ChaseExplosive : BaseExplosive
{
    private bool _isPlayerInSight = false;
    private bool _isChaseMode = false;
    private float _chaseDistance = 5f;

    private LayerMask _layerMask;
    public override void EnterState(ManagerExplosive enemy)
    {
        _layerMask = LayerMask.GetMask("Wall", "Player");
        _isPlayerInSight = true;
        _isChaseMode = true;
        enemy.StartCoroutine(CheckPlayer(enemy));
    }

    public override void UpdateState(ManagerExplosive enemy)
    {

        if (!_isChaseMode)
        {
            Debug.Log("End of chase mode");
            enemy.SwitchState(enemy.wanderState);
        }

        _isPlayerInSight = CheckIfPlayerInSight(enemy);

        Debug.Log("Enemy-Player Distance: " + Vector2.Distance(enemy.transform.position, EventSystem.Current.PlayerLocation));

        if (_isPlayerInSight && (Vector2.Distance(enemy.transform.position, EventSystem.Current.PlayerLocation) <= enemy.startSelfDetructDistance))
        {
            enemy.SwitchState(enemy.attackState);
        }

        Debug.Log("Player In Sight: " + _isPlayerInSight);

        // switch to stun
        if (enemy.IsStunned)
        {
            enemy.prevState = this;
            enemy.SwitchState(enemy.stunState);
        }
    }

    public override void FixedUpdateState(ManagerExplosive enemy)
    {
        if (enemy.facing == Enemy.EnemyFacing.Left)
        {
            enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.ChasingSpeed * Time.fixedDeltaTime;
        }
        else if (enemy.facing == Enemy.EnemyFacing.Right)
        {
            enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.ChasingSpeed * Time.fixedDeltaTime;
        }
    }

    public bool CheckIfPlayerInSight(ManagerExplosive enemy)
    {

        RaycastHit2D hitLeft = Physics2D.Raycast(enemy.transform.position, Vector2.left, _chaseDistance, _layerMask);
        if (hitLeft.collider != null)
        {
            Debug.DrawRay(enemy.transform.position, Vector2.left * _chaseDistance);
            if (hitLeft.collider.tag == "Player") return true;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(enemy.transform.position, Vector2.right, _chaseDistance, _layerMask);
        if (hitRight.collider != null)
        {
            Debug.DrawRay(enemy.transform.position, Vector2.right * _chaseDistance);
            if (hitRight.collider.tag == "Player") return true;
        }

        return false;
    }

    IEnumerator CheckPlayer(ManagerExplosive enemy)
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
