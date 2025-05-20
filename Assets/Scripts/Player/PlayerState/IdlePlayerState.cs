using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class IdlePlayerState : BasePlayerState
{
    public override void EnterState(ManagerPlayerState player)
    {
        
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        //proceeds to run
        if (player.moveAction.IsPressed())
        {
            player.PushCurrentState();
            player.SwitchState(player.RunState);
            return;
        }

        //proceeds to jump state
        if (player.jumpAction.IsPressed() && player.groundBox.isGrounded)
        {
            player.PushCurrentState();
            player.SwitchState(player.JumpState);
            return;
        }

        //proceeds to dash
        if (player.dashAction.IsPressed() && player.DashAbility.IsDashAvailable())
        {
            player.PushCurrentState();
            player.SwitchState(player.DashState);
            return;
        }

        if (player.attackAction.triggered)
        {
            player.PushCurrentState();
            player.SwitchState(player.AttackState);
            return;
        }

    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        player.PlayerRb.linearVelocityX = 0;
    }

    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }
}