using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashPlayerState : BasePlayerState
{

    private float _dashTimer;
    private Facing _currentFacing;

    public override void EnterState(ManagerPlayerState player)
    {
        _currentFacing = player.facing;
        _dashTimer = 0;
        if (player.DashAbility.IsEnableDashDamage) player.DashAttackBox.SetActive(true);
        player.PlayerCurrentStats.Momentum = Mathf.Clamp(player.PlayerCurrentStats.Momentum + 25, 0, player.PlayerCurrentStats.MaxMomentum);
        EventSystem.Current.UpdatePlayerStats(player.PlayerCurrentStats);
        Physics2D.IgnoreLayerCollision(7, 9, true);
        AudioManager.instance.RandomSFX(AudioManager.instance.playerDash);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        if (_dashTimer >= player.DashDuration)
        {
            player.DashAttackBox.SetActive(false);
            Physics2D.IgnoreLayerCollision(7, 9, false);
            player.SwitchState(player.LandState);
            return;
        }

        _dashTimer += Time.deltaTime;
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

    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        if (collision.collider.tag == "Wall")
        {
            Debug.Log("Wall Grabbed while dashing");
            player.PlayerRb.linearVelocityX = 0;
            Physics2D.IgnoreLayerCollision(7, 9, false);
            player.SwitchState(player.WallGrabState);
            return;
        }
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }
}