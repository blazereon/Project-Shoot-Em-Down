using UnityEngine;

public class StunStaticRanged : BaseStaticRanged
{
    public override void EnterState(ManagerStaticRanged enemy)
    {
        enemy.StopAllCoroutines();
    }

    public override void UpdateState(ManagerStaticRanged enemy)
    {
        if (!enemy.IsStunned)
        {
            if (enemy.prevState == null)
            {
                enemy.SwitchState(enemy.scanState);
            }
            else
            {
                BaseStaticRanged _switchToState = enemy.prevState;
                enemy.prevState = null;
                enemy.SwitchState(_switchToState);
            }
        }
    }

    public override void FixedUpdateState(ManagerStaticRanged enemy)
    {
        
    }
}
