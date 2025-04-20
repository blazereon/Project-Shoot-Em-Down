using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class RunPlayerState : BasePlayerState
{
    public override void EnterState(ManagerPlayerState player)
    {
        
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        //return to the previous current state (idle most likely)
        if (!player.moveAction.IsPressed())
        {
            player.SwitchState();
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
            return;
        }
    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        if (player.facing == Facing.right)
        {
            player.PlayerRb.linearVelocityX = player.Speed * Time.fixedDeltaTime * Vector3.right.x;
        }
        else if (player.facing == Facing.left)
        {
            player.PlayerRb.linearVelocityX = player.Speed * Time.fixedDeltaTime * Vector3.left.x;
        }
    }
}