using UnityEngine;

public class StunExplosive : BaseExplosive
{
    public override void EnterState(ManagerExplosive enemy)
    {
        // enemy.StopAllCoroutines();
    }

    public override void UpdateState(ManagerExplosive enemy)
    {
        if (!enemy.IsStunned)
        {
            if (enemy.prevState == null)
            {
                enemy.SwitchState(enemy.wanderState);
            }
            else
            {
                BaseExplosive _switchToState = enemy.prevState;
                enemy.prevState = null;
                enemy.SwitchState(_switchToState);
            }
        }
    }

    public override void FixedUpdateState(ManagerExplosive enemy)
    {
        
    }
}
