using UnityEngine;

public class WallJumpPlayerState : BasePlayerState
{
    
    public override void EnterState(ManagerPlayerState player)
    {
        player.PlayerCurrentStats.Momentum -= 10;
        EventSystem.Current.UpdatePlayerStats(player.PlayerCurrentStats);
        
        Debug.Log("Entered wall jump");
        if (player.facing == Facing.left)
        {
            player.facing = Facing.right;
            player.PlayerRb.AddForce(new Vector2(5, player.JumpForce), ForceMode2D.Impulse);
        }
        else if (player.facing == Facing.right)
        {
            player.facing = Facing.left;
            player.PlayerRb.AddForce(new Vector2(-5, player.JumpForce), ForceMode2D.Impulse);
        }

        AudioManager.instance.RandomSFX(AudioManager.instance.playerWallJump);
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
            player.PushCurrentState();
            player.SwitchState(player.DashState);
            return;
        }
    }
    public override void FixedUpdateState(ManagerPlayerState player)
    {
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