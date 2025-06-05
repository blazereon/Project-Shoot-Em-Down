using UnityEngine;

public class StunGenericEnemy : BaseGenericEnemy
{
    public override void EnterState(ManagerGenericEnemy genericEnemy)
    {
        Debug.Log("STUNNED STATE");
    }

    public override void UpdateState(ManagerGenericEnemy genericEnemy)
    {
        if (!genericEnemy.IsStunned)
        {
            if (genericEnemy.prevState == null)
            {
                genericEnemy.SwitchState(genericEnemy.wanderState);
            }
            else
            {
                BaseGenericEnemy _switchToState = genericEnemy.prevState;
                genericEnemy.prevState = null;
                genericEnemy.SwitchState(_switchToState);
            }
        }
    }

    public override void FixedUpdateState(ManagerGenericEnemy genericEnemy)
    {
        genericEnemy.GenericEnemyRb.linearVelocityX = 0f;
    }
}
