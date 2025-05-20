using UnityEngine;

public class WanderExplosive : BaseExplosive
{
    private float _detectRangeInstance;
    private LayerMask _layerMask;

    public override void EnterState(ManagerExplosive enemy)
    {
        _detectRangeInstance = enemy.detectionRange;
        _layerMask = LayerMask.GetMask("Wall", "Player");
    }

    public override void UpdateState(ManagerExplosive enemy)
    {
        bool _isPlayerDetected = enemy.PlayerDetection(enemy.facing);

        if (_isPlayerDetected)
        {
            enemy.SwitchState(enemy.chaseState);
        }

        Debug.Log(enemy.facing);


        //wall logic
        if (enemy.facing == Enemy.EnemyFacing.Left)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.left, enemy.WallDistanceLimit);
            Debug.DrawRay(enemy.transform.position, Vector2.left * enemy.WallDistanceLimit, Color.cyan);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.Flip(Enemy.EnemyFacing.Right);
            }
        }
        else if (enemy.facing == Enemy.EnemyFacing.Right)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.right, enemy.WallDistanceLimit);
            Debug.DrawRay(enemy.transform.position, Vector2.right * enemy.WallDistanceLimit, Color.cyan);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.Flip(Enemy.EnemyFacing.Left);
            }
        }
    }

    public override void FixedUpdateState(ManagerExplosive enemy)
    {
        if (enemy.facing == Enemy.EnemyFacing.Left)
        {
            enemy.enemyRb.linearVelocityX = Vector2.left.x * enemy.WanderSpeed * Time.fixedDeltaTime;
        }
        else if (enemy.facing == Enemy.EnemyFacing.Right)
        {
            enemy.enemyRb.linearVelocityX = Vector2.right.x * enemy.WanderSpeed * Time.fixedDeltaTime;
        }
    }
}
