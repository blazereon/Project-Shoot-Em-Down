using System;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class RunPlayerState : BasePlayerState
{
    private float _momentumTimer = 0;
    private float _momentumIncrementRate = 0.1f;
    private int _momentumPerSecond = 20;
    public override void EnterState(ManagerPlayerState player)
    {

    }

    public override void UpdateState(ManagerPlayerState player)
    {
        player.PlayBodyAnimation("runLoop");
        player.PlayArmAnimation("runArmSwing");

        if (_momentumTimer > _momentumIncrementRate)
        {
            _momentumTimer = 0;
            if (player.PlayerCurrentStats.Momentum >= Math.Floor(player.PlayerCurrentStats.MaxMomentum * 0.75f)) return;
            player.PlayerCurrentStats.Momentum += (int)(_momentumPerSecond * _momentumIncrementRate);
            EventSystem.Current.UpdatePlayerStats(player.PlayerCurrentStats);
        }
        _momentumTimer += Time.deltaTime;
        
        if (!player.groundBox.isGrounded)
        {
            player.LandState.canCayote = true;
            player.SwitchState(player.LandState);
        }
        //return to the previous current state (idle most likely)
        if (!player.moveAction.IsPressed())
        {
            player.PopState();
            return;
        }

        //proceeds to jump state
        if (player.jumpAction.IsPressed() && player.groundBox.isGrounded)
        {
            player.PushCurrentState();
            player.SwitchState(player.JumpState);
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
        if (player.facing == Facing.right)
        {
            player.PlayerRb.linearVelocityX = player.Speed * Time.fixedDeltaTime * Vector3.right.x;
        }
        else if (player.facing == Facing.left)
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
// Animation Delay Function    
/*    private IEnumerator WaitForAnimationEnd(ManagerPlayerState player, string nextAnim, float delay)
{
    yield return new WaitForSeconds(delay);
    player.PlayAnimation(nextAnim);
}
*/
}