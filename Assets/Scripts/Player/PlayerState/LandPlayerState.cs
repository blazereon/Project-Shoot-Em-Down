using UnityEngine;

public class LandPlayerState : BasePlayerState
{

    public override void EnterState(ManagerPlayerState player)
    {
    }
    public override void UpdateState(ManagerPlayerState player)
    {
        if (player.groundBox.isGrounded)
        {
            player.PopState();
        }
    }
    public override void FixedUpdateState(ManagerPlayerState player)
    {
        player.PlayerRb.linearVelocityY -= player.LandAcceleration;

        if (player.facing == Facing.right && player.moveAction.IsPressed())
        {
            player.PlayerRb.linearVelocityX = player.Speed * Time.fixedDeltaTime * Vector3.right.x;
        }
        else if (player.facing == Facing.left && player.moveAction.IsPressed())
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