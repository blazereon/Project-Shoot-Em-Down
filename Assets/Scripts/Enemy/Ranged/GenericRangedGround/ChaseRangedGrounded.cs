using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


// Note to myself: This enemy has detection range and engagement range only (detection should be bigger than engagement)
// Once detected, enemy will move until it hits the engagement range, then start attacking
public class ChaseRangedGrounded : BaseRangedGrounded
{
    private bool _stopMoving;
    private float _distanceToPlayer;

    public override void EnterState(ManagerRangedGrounded enemy)
    {
        _stopMoving = false;
    }

    public override void UpdateState(ManagerRangedGrounded enemy)       // Handle Distance Detections
    {
        _distanceToPlayer = Vector2.Distance(EventSystem.Current.PlayerLocation, enemy.transform.position);

        if (enemy.startEngagementRange >= _distanceToPlayer)
        {
            _stopMoving = true;
            
        }
        else
        {
            _stopMoving = false;
        }

        if (enemy.detectionRange < _distanceToPlayer)
        {
            enemy.SwitchState(enemy.wanderState);
        }
    }

    public override void FixedUpdateState(ManagerRangedGrounded enemy)      // Handle any movement
    {
        if (!_stopMoving)
        {
            if (EventSystem.Current.PlayerLocation.x > enemy.transform.position.x)     // player on the right
            {
                if (!(enemy.transform.localScale.x > 0))    // enemy is not facing to the right
                {
                    enemy.Flip();
                }

                enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.chaseSpeed * Time.fixedDeltaTime;

            }
            else
            {
                if (!(enemy.transform.localScale.x < 0))    // enemy is not facing to the left
                {
                    enemy.Flip();
                }

                enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.chaseSpeed * Time.fixedDeltaTime;
            }

        }
        else
        {
            enemy.enemyRb.linearVelocity = Vector3.zero;

            if (enemy.enemyRb.linearVelocityX == 0)
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
    }
}
