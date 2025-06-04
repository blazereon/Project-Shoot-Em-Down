
using UnityEngine;

public class DestructiveBolt : ComponentAbility
{
    ManagerPlayerState player;
    public bool IsNextBulletEmpowered = false;

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
    }

    public override void UpgradeComponentHandler()
    {

    }
}