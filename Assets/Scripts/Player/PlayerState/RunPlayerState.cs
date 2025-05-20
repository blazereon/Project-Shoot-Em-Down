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
        if (!player.groundBox.isGrounded)
        {
            player.LandState.canCayote = true;
            player.SwitchState(player.LandState);
        }
        //return to the previous current state (idle most likely)
        if (!player.moveAction.IsPressed())
        {
            player.PopState();
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

    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        if (collision.collider.tag == "Wall")
        {
            player.SwitchState(player.WallGrabState);
        }
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }
}