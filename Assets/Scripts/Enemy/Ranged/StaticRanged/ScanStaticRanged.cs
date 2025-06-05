using UnityEngine;

public class ScanStaticRanged : BaseStaticRanged
{
    bool _isPlayerDetected = false;

    public override void EnterState(ManagerStaticRanged enemy)
    {
        Debug.Log("StR Enter Scan ");
    }

    public override void UpdateState(ManagerStaticRanged enemy)
    {
        // switch to stun
        if (enemy.IsStunned)
        {
            enemy.prevState = this;
            enemy.SwitchState(enemy.stunState);
        }
    }

    public override void FixedUpdateState(ManagerStaticRanged enemy)
    {
        if (enemy.hitDetect.PlayerDetected())
        {
            enemy.SwitchState(enemy.attackState);
        }
    }

}
