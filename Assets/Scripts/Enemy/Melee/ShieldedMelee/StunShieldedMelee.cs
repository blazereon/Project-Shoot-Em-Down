using UnityEngine;

public class StunShieldedMelee : BaseShieldMelee
{
    public override void EnterState(ManagerShieldMelee enemy)
    {
        Debug.Log("STUNNED STATE");
    }

    public override void UpdateState(ManagerShieldMelee enemy)
    {
        if (!enemy.IsStunned)
        {
            if (enemy.prevState == null)
            {
                enemy.SwitchState(enemy.wanderState);
            }
            else
            {
                BaseShieldMelee _switchToState = enemy.prevState;
                enemy.prevState = null;
                enemy.SwitchState(_switchToState);
            }
        }
    }

    public override void FixedUpdateState(ManagerShieldMelee enemy)
    {
        enemy.enemyRb.linearVelocityX = 0f;
    }
}
