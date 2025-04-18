using UnityEngine;

public class WanderRangedGrounded : BaseRangedGrounded
{
    private float _detectRangeInstance;
    private LayerMask _layerMask;

    public override void EnterState(ManagerRangedGrounded enemy)
    {
        enemy.enemyRb = enemy.GetComponent<Rigidbody2D>();
        _detectRangeInstance = enemy.detectionRange;
        _layerMask = LayerMask.GetMask("Wall", "Player");
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        bool _isPlayerDetected = enemy.PlayerDetection(enemy.transform.localScale);

        if (_isPlayerDetected)
        {
            enemy.SwitchState(enemy.chaseState);
        }

        if (enemy.transform.localScale.x == -1)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.left, enemy.wallDistanceLimit);
            Debug.DrawRay(enemy.transform.position, Vector2.left * enemy.wallDistanceLimit, Color.red);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.transform.localScale *= -1;
            }
        }
        else if (enemy.transform.localScale.x == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.right, enemy.wallDistanceLimit);
            Debug.DrawRay(enemy.transform.position, Vector2.right * enemy.wallDistanceLimit, Color.red);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.transform.localScale *= -1;
            }
        }

    }

    public override void FixedUpdateState(ManagerRangedGrounded enemy)
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
