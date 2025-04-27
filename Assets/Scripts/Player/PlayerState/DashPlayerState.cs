using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashPlayerState : BasePlayerState
{
    private bool _isDashing = false;
    private float _dashDuration;
    private float _dashCooldown;
    private Facing _currentFacing;
    public override void EnterState(ManagerPlayerState player)
    {
        _dashDuration = player.DashDuration;
        _dashCooldown = player.DashRecovery;
        _currentFacing = player.facing;
        player.StartCoroutine(Dash(player));
        player.PlayerCurrentStats.Momentum = Mathf.Clamp(player.PlayerCurrentStats.Momentum + 25, 0, player.PlayerCurrentStats.MaxMomentum);
        EventSystem.Current.UpdatePlayerStats(player.PlayerCurrentStats);
    }

    public override void UpdateState(ManagerPlayerState player)
    {

    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        if (_currentFacing == Facing.right)
        {
            player.PlayerRb.linearVelocityX = player.DashForce;
        }
        else if (_currentFacing == Facing.left)
        {
            player.PlayerRb.linearVelocityX = -player.DashForce;
        }
        
        player.PlayerRb.linearVelocityY = 0;
    }

    IEnumerator Dash(ManagerPlayerState player)
    {
        yield return new WaitForSeconds(_dashDuration);
        player.isDashCooldown = true;
        player.PlayerRb.linearVelocityX = 0;
        player.SwitchState(player.LandState);
        yield return new WaitForSeconds(_dashCooldown);
        player.isDashCooldown = false;
    }

    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        if (collision.collider.tag == "Wall")
        {
            Debug.Log("Wall Grabbed while dashing");
            player.PlayerRb.linearVelocityX = 0;
            player.SwitchState(player.WallGrabState);
        }
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }
}