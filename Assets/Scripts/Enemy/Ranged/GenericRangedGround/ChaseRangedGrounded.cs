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
                Debug.Log("EXITING WANDER STATE: " + _distanceToPlayer);
                enemy.hasPlayerDetected = false;
                enemy.SwitchState(enemy.wanderState);
            }

            // switch to stun
            if (enemy.IsStunned)
            {
                enemy.prevState = this;
                enemy.SwitchState(enemy.stunState);
            }
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
