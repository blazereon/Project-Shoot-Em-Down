using UnityEngine;

public class StunRangedGrounded : BaseRangedGrounded
{
    public override void EnterState(ManagerRangedGrounded enemy)
    {
        enemy.StopAllCoroutines();
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        enemy.PlayMainAnimation("basicRangedStun_v1");  
        
        if (!enemy.IsStunned)
        {
            enemy.PlayMainAnimation("basicRangedStun_v3");
            
            if (enemy.prevState == null)
            {
                enemy.SwitchState(enemy.wanderState);
            }
            else
            {
                BaseRangedGrounded _switchToState = enemy.prevState;
                enemy.prevState = null;
                enemy.SwitchState(_switchToState);
            }
        }
    }

    public override void FixedUpdateState(ManagerRangedGrounded enemy)
    {
        enemy.enemyRb.linearVelocityX = 0f;
    }
}
