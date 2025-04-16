using UnityEngine;

public class GenericEnemyWanderState : GenericEnemyBaseState
{
    private float _detectRangeInstance;
    private LayerMask _layerMask;

    public override void EnterState(GenericEnemyManager genericEnemy)
    {
        _detectRangeInstance = genericEnemy.detectionRange;
        _layerMask = LayerMask.GetMask("Wall", "Player");
    }

    public override void UpdateState(GenericEnemyManager genericEnemy)
    {
        bool _isPlayerDetected = genericEnemy.PlayerDetection();

        if (_isPlayerDetected)
        {
            genericEnemy.SwitchState(genericEnemy.chaseState);
        }

        Debug.Log(genericEnemy.facing);


        //wall logic
        if (genericEnemy.facing == EnemyFacing.Left)
        {
            RaycastHit2D hit = Physics2D.Raycast(genericEnemy.transform.position, Vector2.left, genericEnemy.WallDistanceLimit);
            Debug.DrawRay(genericEnemy.transform.position, Vector2.left * genericEnemy.WallDistanceLimit, Color.cyan );
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                genericEnemy.Flip(EnemyFacing.Right);
            }
        }
        else if (genericEnemy.facing == EnemyFacing.Right)
        {
            RaycastHit2D hit = Physics2D.Raycast(genericEnemy.transform.position, Vector2.right, genericEnemy.WallDistanceLimit);
            Debug.DrawRay(genericEnemy.transform.position, Vector2.right * genericEnemy.WallDistanceLimit, Color.cyan );
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                genericEnemy.Flip(EnemyFacing.Left);
            }
        }
    }

    public override void FixedUpdateState(GenericEnemyManager genericEnemy)
    {
        if (genericEnemy.facing == EnemyFacing.Left)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.left.x * genericEnemy.WanderSpeed * Time.fixedDeltaTime;
        }
        else if (genericEnemy.facing == EnemyFacing.Right)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.right.x * genericEnemy.WanderSpeed * Time.fixedDeltaTime;
        }
    }
}