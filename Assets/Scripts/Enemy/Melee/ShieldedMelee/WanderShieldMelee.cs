using UnityEngine;

public class WanderShieldMelee : BaseShieldMelee
{
    private float _detectRangeInstance;
    bool _isPlayerDetected;

    public override void EnterState(ManagerShieldMelee enemy)
    {
        _detectRangeInstance = enemy.detectionRange;
        
    }

    public override void UpdateState(ManagerShieldMelee enemy)
    {
        enemy.PlayMainAnimation("shieldedMeleeMoveLoop");
        
        _isPlayerDetected = enemy.PlayerDetection(enemy._shieldTransform.localScale);

        if (_isPlayerDetected)
        {
            
            enemy.hasPlayerDetected = true;
            AudioManager.instance.PlayFX(AudioManager.instance.enemyChaseAlert[0], false);
            Debug.Log("player detected, chasing");
            enemy.SwitchState(enemy.chaseState);
        }

        // switch to stun
        if (enemy.IsStunned)
        {
            enemy.prevState = this;
            enemy.SwitchState(enemy.stunState);
        }

        if (enemy.transform.localScale.x == -1)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy._shieldTransform.position, Vector2.left, enemy.wallDistanceLimit);
            Debug.DrawRay(enemy._shieldTransform.position, Vector2.left * enemy.wallDistanceLimit, Color.red);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.ScaleFlip();
            }
        }
        else if (enemy.transform.localScale.x == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy._shieldTransform.position, Vector2.right, enemy.wallDistanceLimit);
            Debug.DrawRay(enemy._shieldTransform.position, Vector2.right * enemy.wallDistanceLimit, Color.red);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.ScaleFlip();
            }
        }

        
    }

    public override void FixedUpdateState(ManagerShieldMelee enemy)
    {
        if (enemy.transform.localScale.x == -1)
        {
            enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.wanderSpeed * Time.fixedDeltaTime;
        }
        else if (enemy.transform.localScale.x == 1)
        {
            enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.wanderSpeed * Time.fixedDeltaTime;
        }
    }
}
