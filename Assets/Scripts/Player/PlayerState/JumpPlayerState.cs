using UnityEngine;

public class JumpPlayerState : BasePlayerState
{
    public override void EnterState(ManagerPlayerState player)
    {
        Debug.Log("Jump State!!");
        player.PlayerRb.linearVelocityY = 0;
        player.PlayerRb.AddForce(new Vector2(0, player.JumpForce), ForceMode2D.Impulse);

        AudioManager.instance.RandomSFX(AudioManager.instance.playerJump);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        if (player.PlayerRb.linearVelocityY <= player.LandStart)
        {
            player.SwitchState(player.LandState);
            return;
        }

        //proceeds to dash
        if (player.dashAction.IsPressed() && player.DashAbility.IsDashAvailable())
        {
            player.DashAbility.ConsumeDash(player);
            player.PushCurrentState();
            player.SwitchState(player.DashState);
            return;
        }
    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
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