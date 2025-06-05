using UnityEngine;

public class StunShieldRanged : BaseShieldRanged
{
    public override void EnterState(ManagerShieldRanged enemy)
    {
        enemy.StopAllCoroutines();
    }

    public override void UpdateState(ManagerShieldRanged enemy)
    {
        if (!enemy.IsStunned)
        {
            if (enemy.prevState == null)
            {
                enemy.SwitchState(enemy.wanderState);
            }
            else
            {
                BaseShieldRanged _switchToState = enemy.prevState;
                enemy.prevState = null;
                enemy.SwitchState(_switchToState);
            }
        }
    }

    public override void FixedUpdateState(ManagerShieldRanged enemy)
    {
        enemy.enemyRb.linearVelocityX = 0f;
    }
}
