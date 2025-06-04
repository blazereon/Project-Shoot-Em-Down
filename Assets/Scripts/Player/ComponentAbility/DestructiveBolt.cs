
using System.Collections;
using UnityEngine;

public struct DestructiveBoltStatus
{
    public bool IsNextBulletEmpowered, IsCooldown;
    public float DestructiveBoltCooldownTimer, DestructiveBoltCooldown;
    
}
public class DestructiveBolt : ComponentAbility
{
    ManagerPlayerState player;
    public bool IsNextBulletEmpowered, IsCooldown = false;
    private float updateRate = 0.1f;
    public float DestructiveBoltCooldownTimer;
    public float DestructiveBoltCooldown = 5;

    Coroutine DestructiveBoltCooldownInstance;

    public DestructiveBolt(ManagerPlayerState player)
    {
        this.player = player;
        player.OnTriggerEmpowerment += SetEmpowered;
    }

    public override void EmpowermentHandler()
    {
        Debug.Log("Stunning field activated");
        if (UpgradeTier >= 2) EventSystem.Current.ReleaseStunningField();
        IsNextBulletEmpowered = true;
        Empowered = false;
        DestructiveBoltCooldownInstance = CoroutineHandler.Instance.StartCoroutine(DestructiveCooldownCoroutine());
    }

    public override void UpgradeComponentHandler()
    {

    }

    public DestructiveBoltStatus GetCurrentStatus()
    {
        return new DestructiveBoltStatus
        {
            IsNextBulletEmpowered = this.IsNextBulletEmpowered,
            IsCooldown = this.IsCooldown,
            DestructiveBoltCooldownTimer = this.DestructiveBoltCooldownTimer,
            DestructiveBoltCooldown = this.DestructiveBoltCooldown,
        };
    }

    IEnumerator DestructiveCooldownCoroutine()
    {
        IsCooldown = true;
        DestructiveBoltCooldownTimer = 0;
        while (DestructiveBoltCooldownTimer < DestructiveBoltCooldown)
        {
            yield return new WaitForSeconds(updateRate);
            DestructiveBoltCooldownTimer += updateRate;
        }
        DestructiveBoltCooldownTimer = 0;
        IsCooldown = false;
    }
}