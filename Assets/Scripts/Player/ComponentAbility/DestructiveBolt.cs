
using UnityEngine;

public class DestructiveBolt : ComponentAbility
{
    ManagerPlayerState player;

    public DestructiveBolt(ManagerPlayerState player)
    {
        this.player = player;
        player.OnTriggerEmpowerment += SetEmpowered;
    }

    public override void EmpowermentHandler()
    {
        Debug.Log("Stunning field activated");
        EventSystem.Current.ReleaseStunningField();
        Empowered = false;
    }

    public override void UpgradeComponentHandler()
    {
        
    }
}