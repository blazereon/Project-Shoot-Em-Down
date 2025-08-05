using System.Collections;
using UnityEngine;

public class ChaseShieldMelee : BaseShieldMelee
{
    private float _distanceToPlayer;
    private Coroutine _turnCoroutine;
    private float _attackDistance;

    public override void EnterState(ManagerShieldMelee enemy)
    {
        _turnCoroutine = null;

        _attackDistance = enemy.enemyCollider.size.x + 0.3f;
    }

    public override void UpdateState(ManagerShieldMelee enemy)
    {
        enemy.PlayMainAnimation("shieldedMeleeMoveLoop");

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
            _distanceToPlayer = Vector2.Distance(EventSystem.Current.PlayerLocation, enemy.transform.position);

            if (_distanceToPlayer <= _attackDistance)
            {
                enemy.SwitchState(enemy.attackState);
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

    public override void FixedUpdateState(ManagerShieldMelee enemy)
    {
        if (enemy.IsStunned) return;
        if (_turnCoroutine == null)
        {
            _turnCoroutine = enemy.StartCoroutine(DelayTurn(enemy));
        }

        enemy.enemyRb.linearVelocityX = enemy.transform.localScale.x * enemy.chaseSpeed * Time.fixedDeltaTime;
    }

    IEnumerator DelayTurn(ManagerShieldMelee enemy)
    {

        yield return new WaitForSeconds(2.5f);

        if (EventSystem.Current.PlayerLocation.x > enemy.transform.position.x)     // player on the right
        {
            if (!(enemy.transform.localScale.x > 0))    // enemy is not facing to the right
            {
                enemy.ScaleFlip();
            }

            // enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.chaseSpeed * Time.fixedDeltaTime;

        }
        else
        {
            if (!(enemy.transform.localScale.x < 0))    // enemy is not facing to the left
            {
                enemy.ScaleFlip();
            }

            // enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.chaseSpeed * Time.fixedDeltaTime;
        }
        
        _turnCoroutine = null;
    }
}
