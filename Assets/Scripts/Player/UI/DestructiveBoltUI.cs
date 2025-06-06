using UnityEngine;
using UnityEngine.UI;

public class DestructiveBoltUI : MonoBehaviour
{
    public Image DestBoltIcon;
    public Image CooldownOverlay;
    public Image lvl1;
    public Image lvl2;
    public Image lvl3;
    

    public void UpdateValue(DestructiveBoltStatus status)
    {
        // cooldown animation XD
        CooldownOverlay.gameObject.SetActive(status.IsCooldown);

        if (status.IsCooldown)
        {
            CooldownOverlay.fillAmount = 1f - (status.DestructiveBoltCooldownTimer / status.DestructiveBoltCooldown);
        }
        
        switch (status.AbilityData.UpgradeTier)
        {
            case 0:
                lvl1.enabled = false;
                lvl2.enabled = false;
                lvl3.enabled = false;
                break;
            case 1:
                lvl1.enabled = true;
                lvl2.enabled = false;
                lvl3.enabled = false;
                break;
            case 2:
                lvl1.enabled = true;
                lvl2.enabled = true;
                lvl3.enabled = false;
                break;
            case 3:
                lvl1.enabled = true;
                lvl2.enabled = true;
                lvl3.enabled = true;
                break;
        }

        // Empowered state visual que
        if (status.AbilityData.Empowered)
        {
            DestBoltIcon.color = Color.cyan;
        }
        else
        {
            DestBoltIcon.color = Color.white;
        }
    }
}