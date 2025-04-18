using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


// Note to myself: This enemy's has detection range and engagement range only (detection should be bigger than engagement)
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
        _distanceToPlayer = Vector2.Distance(enemy.player.transform.position, enemy.transform.position);
        if (enemy.startEngagementRange >= _distanceToPlayer)
        {
            _stopMoving = true;
            // enemy.SwitchState(enemy.attackState);
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
            if (enemy.player.transform.position.x > enemy.transform.position.x)     // player on the right
            {
                if (!(enemy.transform.localScale.x > 0))    // enemy is not facing to the right
                {
                    enemy.transform.localScale *= -1;
                }

                enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.chaseSpeed * Time.fixedDeltaTime;

            }
            else
            {
                if (!(enemy.transform.localScale.x < 0))    // enemy is not facing to the left
                {
                    enemy.transform.localScale *= -1;
                }

                enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.chaseSpeed * Time.fixedDeltaTime;
            }

        }
        else
        {
            enemy.enemyRb.linearVelocityX = 0;
        }
    }
}
