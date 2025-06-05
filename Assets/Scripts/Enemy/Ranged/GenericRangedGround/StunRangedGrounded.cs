using UnityEngine;

public class StunRangedGrounded : BaseRangedGrounded
{
    public override void EnterState(ManagerRangedGrounded enemy)
    {
        enemy.StopAllCoroutines();
    }

    public override void UpdateState(ManagerRangedGrounded enemy)
    {
        if (!enemy.IsStunned)
        {
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
