using System.Threading;
using UnityEngine;

public class WallGrabPlayerState : BasePlayerState
{
    private float _inputDelayTimer;
    private float _inputDelay = 0.2f;
    public override void EnterState(ManagerPlayerState player)
    {
        Debug.Log("wall grab state");
        player.PlayerRb.linearVelocity = Vector2.zero;

        AudioManager.instance.RandomSFX(AudioManager.instance.playerWallGrab);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        if (player.jumpAction.triggered)
        {
            player.SwitchState(player.WallJumpState);
        }
    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        player.PlayerRb.linearVelocityY = -30 * Time.fixedDeltaTime;

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
        
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        if (collision.collider.tag == "Wall")
        {
            Debug.Log("Exiting wall jump state");
            player.SwitchState(player.LandState);
        }
    }
}