using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;

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
            player.SwitchState(player.RunState);
            return;
        }

        //proceeds to jump state
        if (player.jumpAction.IsPressed() && player.groundBox.isGrounded)
        {
            player.SwitchState(player.JumpState);
            return;
        }

        //proceeds to dash
        if (player.dashAction.IsPressed() && !player.isDashCooldown)
        {
            player.SwitchState(player.DashState);
        }
    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        player.PlayerRb.linearVelocityX = 0;
    }
}