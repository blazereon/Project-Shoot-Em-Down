using UnityEngine;

public class AttackRangedGrounded : BaseRangedGrounded
{
    private float _distanceToPlayer;
    public override void EnterState(ManagerRangedGrounded enemy)
    {
        
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        _distanceToPlayer = Vector2.Distance(enemy.player.transform.position, enemy.transform.position);

        Debug.Log("ATTAAAACKK!!!" + _distanceToPlayer);
        if (enemy.startEngagementRange < _distanceToPlayer )
        {
            enemy.SwitchState(enemy.chaseState);
        }

    }

    public override void FixedUpdateState(ManagerRangedGrounded enemy)
    {
        
    }
}
