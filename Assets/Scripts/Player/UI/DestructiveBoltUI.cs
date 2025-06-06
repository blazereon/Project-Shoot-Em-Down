using UnityEngine;
using UnityEngine.UI;

public class DestructiveBoltUI : MonoBehaviour
{
    public Image DestBoltIcon;
    public Image CooldownOverlay;
    

    public void UpdateValue(DestructiveBoltStatus status)
    {
        // cooldown animation XD
        CooldownOverlay.fillAmount = status.DestructiveBoltCooldownTimer / status.DestructiveBoltCooldown;

        // Empowered state visual que
        DestBoltIcon.enabled = status.IsNextBulletEmpowered;
    }
}