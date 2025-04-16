using UnityEngine;

public class WanderGenericEnemy : BaseGenericEnemy
{
    private float _detectRangeInstance;
    private LayerMask _layerMask;

    public override void EnterState(ManagerGenericEnemy genericEnemy)
    {
        _detectRangeInstance = genericEnemy.detectionRange;
        _layerMask = LayerMask.GetMask("Wall", "Player");
    }

    public override void UpdateState(ManagerGenericEnemy genericEnemy)
    {
        bool _isPlayerDetected = genericEnemy.PlayerDetection(genericEnemy.facing);

        if (_isPlayerDetected)
        {
            genericEnemy.SwitchState(genericEnemy.chaseState);
        }

        Debug.Log(genericEnemy.facing);


        //wall logic
        if (genericEnemy.facing == Enemy.EnemyFacing.Left)
        {
            RaycastHit2D hit = Physics2D.Raycast(genericEnemy.transform.position, Vector2.left, genericEnemy.WallDistanceLimit);
            Debug.DrawRay(genericEnemy.transform.position, Vector2.left * genericEnemy.WallDistanceLimit, Color.cyan );
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                genericEnemy.Flip(Enemy.EnemyFacing.Right);
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