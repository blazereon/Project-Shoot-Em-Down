using UnityEngine;

public class LandPlayerState : BasePlayerState
{
    public float coyoteTime = 1f;
    private float coyoteTimer = 0f;
    public bool canCayote = false;

    public override void EnterState(ManagerPlayerState player)
    {
        coyoteTimer = coyoteTime;
        Debug.Log("Coyote Time Start!: " + coyoteTimer);
    }
    public override void UpdateState(ManagerPlayerState player)
    {
            Debug.Log("Coyote Timer: " + coyoteTimer);
            coyoteTimer -= Time.deltaTime;

        if (player.jumpAction.triggered && (coyoteTimer >= 0f) && canCayote)
        {
            Debug.Log("Cayote jump!!");
            canCayote = false;
            player.SwitchState(player.JumpState);
            return;
        }

        if (player.dashAction.triggered && !player.isDashCooldown)
        {
            player.SwitchState(player.DashState);
            return;
        }

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