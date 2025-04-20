using UnityEngine;

public class JumpPlayerState : BasePlayerState
{
    public override void EnterState(ManagerPlayerState player)
    {
        Debug.Log("Jump State!!");
        if (player.groundBox.isGrounded) player.PlayerRb.AddForce(new Vector2(0, player.JumpForce), ForceMode2D.Impulse);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        //if player is grounded and not jumping
        if (player.groundBox.isGrounded && (player.PlayerRb.linearVelocityY <= 0))
        {
            Debug.Log("Exit Jump State");
            player.SwitchState();
        }

        if (player.dashAction.IsPressed() && !player.isDashCooldown)
        {
            Debug.Log("Jump to Dash state");
            player.SwitchState(player.DashState);
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