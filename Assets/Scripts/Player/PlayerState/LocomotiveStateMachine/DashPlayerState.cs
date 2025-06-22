using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashPlayerState : BasePlayerState
{

    private float _dashTimer;
    private Facing _currentFacing;

    public override void EnterState(ManagerPlayerState player)
    {
        player.DashAbility.ConsumeDash(player);
        //Special case of empowerment ability invoke method due to dash being player's main ability
        if (player.IsEmpowerementInvoke)
        {
            Debug.Log("Empowered dash triggered");
            player.TriggerEmpowerment(player.DashAbility);
        }

        _currentFacing = player.facing;
        _dashTimer = 0;
        
        //Activates melee damage if the dash ability is empowered
        if (player.DashAbility.Empowered) player.DashAttackBox.SetActive(true);

        //increases momentum as dash is activated
        player.PlayerCurrentStats.Momentum = Mathf.Clamp(player.PlayerCurrentStats.Momentum + 25, 0, player.PlayerCurrentStats.MaxMomentum);

        EventSystem.Current.UpdatePlayerStats(player.PlayerCurrentStats);

        //If dash component ability is in tier 2, ignores layer collision with colliding enemy and shields (Boss not included)
        if (player.DashAbility.Empowered && player.DashAbility.UpgradeTier >= 2)
        {
            Physics2D.IgnoreLayerCollision(7, 13, true);
            Physics2D.IgnoreLayerCollision(7, 9, true);
        }

        AudioManager.instance.RandomSFX(AudioManager.instance.playerDash);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        if (_dashTimer >= player.DashDuration) // if dash expires
        {
            player.DashAttackBox.SetActive(false);
            Physics2D.IgnoreLayerCollision(7, 13, false);
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
        if (collision.collider.tag == "Wall") //if player hits a wall while dashing
        {
            Debug.Log("Wall Grabbed while dashing");
            player.PlayerRb.linearVelocityX = 0;
            Physics2D.IgnoreLayerCollision(7, 13, false);
            Physics2D.IgnoreLayerCollision(7, 9, false);
            player.SwitchState(player.WallGrabState);
            return;
        }

        if (collision.collider.tag == "Shield") //if player hits a shield
        {
            player.DashAttackBox.SetActive(false);
            Physics2D.IgnoreLayerCollision(7, 13, false);
            player.PlayerRb.linearVelocityX = 0;
            player.SwitchState(player.LandState);
            return;
        }
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {

    }
}