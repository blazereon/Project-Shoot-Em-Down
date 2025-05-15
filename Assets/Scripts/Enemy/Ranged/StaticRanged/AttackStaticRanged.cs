using UnityEngine;

public class AttackStaticRanged : BaseStaticRanged
{
    public override void EnterState(ManagerStaticRanged enemy)
    {
        Debug.Log("StR Enter Attack ");
    }

    public override void UpdateState(ManagerStaticRanged enemy)
    {
        // code
    }

    public override void FixedUpdateState(ManagerStaticRanged enemy)
    {
        if (!enemy.hitDetect.PlayerDetected())
        {
            enemy.SwitchState(enemy.scanState);
        }
    }
}
