using UnityEngine;

public class IdlePlayerCombatState : BasePlayerCombatState
{
    public override void EnterState(ManagerPlayerState player)
    {
        //nothing
    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        //nothing
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        if (player.attackAction.triggered)
        {
            //switch to attack mode
            player.PushCurrentCombatState();
            player.SwitchCombatState(player.AttackState);
        }
    }

    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        //nothing
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        //nothing
    }
}