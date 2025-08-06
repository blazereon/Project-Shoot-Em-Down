using NUnit.Framework;
using UnityEngine;

public class JumpPlayerState : BasePlayerState
{
    private float holdDuration;
    private float holdDurationTimer = 0f;
    private float currentJumpTime = 0f;
    private float jumpTimer = 0f;

    private bool isHold = false;
    public override void EnterState(ManagerPlayerState player)
    {
        Debug.Log("Jump State!!");
        holdDuration = player.ShortJumpTime * 0.75f;
        holdDurationTimer = 0f;
        jumpTimer = 0f;
        currentJumpTime = player.ShortJumpTime;
        isHold = false;
        // player.PlayerRb.linearVelocityY = 0;
        // player.PlayerRb.AddForce(new Vector2(0, player.JumpForce), ForceMode2D.Impulse);

        AudioManager.instance.RandomSFX(AudioManager.instance.playerJump);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        player.PlayBodyAnimation("jumpLoop");
        player.PlayArmAnimation("idleArmSwing");
        player.PlayerRb.linearVelocityY = player.JumpForce;

        if (player.jumpAction.IsPressed())
        {
            holdDurationTimer += Time.deltaTime;
        }

        if (holdDurationTimer > holdDuration)
        {
            currentJumpTime = player.LongJumpTime;
            isHold = true;
        }

        if (jumpTimer >= currentJumpTime)
        {
            holdDurationTimer = 0f;
            jumpTimer = 0f;
            currentJumpTime = player.ShortJumpTime;
            player.SwitchState(player.LandState);
            return;
        }

        if (isHold && !player.jumpAction.IsPressed())
        {
            holdDurationTimer = 0f;
            jumpTimer = 0f;
            currentJumpTime = player.ShortJumpTime;
            player.SwitchState(player.LandState);
            return;
        }

        //proceeds to dash
        if (player.dashAction.IsPressed() && player.DashAbility.IsDashAvailable())
        {
            player.SwitchState(player.DashState);
            return;
        }

        jumpTimer += Time.deltaTime;
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

        if (collision.collider.tag == "Ground")
        {
            player.SwitchState(player.LandState);
        }
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }
}