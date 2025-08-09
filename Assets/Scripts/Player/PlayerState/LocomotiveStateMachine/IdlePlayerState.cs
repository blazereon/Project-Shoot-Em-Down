using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class IdlePlayerState : BasePlayerState
{
    private float _momentumTimer = 0;
    private float _momentumIncrementRate = 0.1f;
    private int _momentumDecayPerSecond = 14;
    public override void EnterState(ManagerPlayerState player)
    {
        // player.GunArmInstance.UpdateArm(true, player.DestructiveBoltAbility.UpgradeTier);
    }

    public override void UpdateState(ManagerPlayerState player)
    {
        player.PlayBodyAnimation("idle");
        player.PlayArmAnimation("idleArmSwing");

        if (_momentumTimer > _momentumIncrementRate)
        {
            _momentumTimer = 0;
            player.PlayerCurrentStats.Momentum -= (int)(_momentumDecayPerSecond * _momentumIncrementRate);
            EventSystem.Current.UpdatePlayerStats(player.PlayerCurrentStats);
        }
        _momentumTimer += Time.deltaTime;
        
        //proceeds to run
        if (player.moveAction.IsPressed())
        {
            player.PushCurrentState();
            player.SwitchState(player.RunState);
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
        player.PlayerRb.linearVelocityX = 0;
    }

    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }
}