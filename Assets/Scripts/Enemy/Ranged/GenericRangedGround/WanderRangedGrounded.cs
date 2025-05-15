using UnityEngine;

public class WanderRangedGrounded : BaseRangedGrounded
{
    private float _detectRangeInstance;
    private LayerMask _layerMask;
    bool _isPlayerDetected;

    public override void EnterState(ManagerRangedGrounded enemy)
    {
        enemy.enemyRb = enemy.GetComponent<Rigidbody2D>();
        _detectRangeInstance = enemy.detectionRange;
        _layerMask = LayerMask.GetMask("Wall", "Player");
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        _isPlayerDetected = enemy.PlayerDetection(enemy.transform.localScale);

        if (_isPlayerDetected)
        {
            enemy.hasPlayerDetected = true;
            AudioManager.instance.PlayFX(AudioManager.instance.enemyChaseAlert[0], false);
            enemy.SwitchState(enemy.chaseState);
        }

        if (enemy.transform.localScale.x == -1)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.left, enemy.wallDistanceLimit, _layerMask);
            Debug.DrawRay(enemy.transform.position, Vector2.left * enemy.wallDistanceLimit, Color.red);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.Flip();
            }
        }
        else if (enemy.transform.localScale.x == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, Vector2.right, enemy.wallDistanceLimit, _layerMask);
            Debug.DrawRay(enemy.transform.position, Vector2.right * enemy.wallDistanceLimit, Color.red);
            if (hit.collider == null) return;
            if (hit.collider.tag == "Wall")
            {
                Debug.LogWarning("Wall hit");
                enemy.Flip();
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
