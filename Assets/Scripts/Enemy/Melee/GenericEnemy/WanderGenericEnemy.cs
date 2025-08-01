using UnityEngine;

public class WanderGenericEnemy : BaseGenericEnemy
{
    private float _detectRangeInstance;

    public override void EnterState(ManagerGenericEnemy genericEnemy)
    {
        _detectRangeInstance = genericEnemy.detectionRange;
    }

    public override void UpdateState(ManagerGenericEnemy genericEnemy)
    {
        genericEnemy.PlayMainAnimation("basicMeleeMoveLoop");
        
        bool _isPlayerDetected = genericEnemy.PlayerDetection(genericEnemy.facing);
        Debug.Log("Stun status: " + genericEnemy.IsStunned);

        if (_isPlayerDetected)
        {
            genericEnemy.hasPlayerDetected = true;
            AudioManager.instance.PlayFX(AudioManager.instance.enemyChaseAlert[0], false);
            genericEnemy.SwitchState(genericEnemy.chaseState);
        }

        // switch to stun
        if (genericEnemy.IsStunned)
        {
            genericEnemy.prevState = this;
            genericEnemy.SwitchState(genericEnemy.stunState);
        }

        Debug.Log(genericEnemy.facing);


        // wall logic
        if (genericEnemy.facing == Enemy.EnemyFacing.Left)
        {
            RaycastHit2D hit = Physics2D.Raycast(genericEnemy.transform.position, Vector2.left, genericEnemy.WallDistanceLimit);
            Debug.DrawRay(genericEnemy.transform.position, Vector2.left * genericEnemy.WallDistanceLimit, Color.cyan );
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                genericEnemy.Flip(Enemy.EnemyFacing.Right);
                genericEnemy.ScaleFlip();
            }
        }
        else if (genericEnemy.facing == Enemy.EnemyFacing.Right)
        {
            RaycastHit2D hit = Physics2D.Raycast(genericEnemy.transform.position, Vector2.right, genericEnemy.WallDistanceLimit);
            Debug.DrawRay(genericEnemy.transform.position, Vector2.right * genericEnemy.WallDistanceLimit, Color.cyan );
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                genericEnemy.Flip(Enemy.EnemyFacing.Left);
                genericEnemy.ScaleFlip();
            }
        }
    }

    public override void FixedUpdateState(ManagerGenericEnemy genericEnemy)
    {
        if (genericEnemy.facing == Enemy.EnemyFacing.Left)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.left.x * genericEnemy.WanderSpeed * Time.fixedDeltaTime;
        }
        else if (genericEnemy.facing == Enemy.EnemyFacing.Right)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.right.x * genericEnemy.WanderSpeed * Time.fixedDeltaTime;
        }
    }
}