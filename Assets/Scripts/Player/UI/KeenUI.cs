using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeenUI : MonoBehaviour
{
    public Image KeenIcon;
    public Image CooldownOverlay;
    public Image lvl1;
    public Image lvl2;
    public Image lvl3;


    public void UpdateValue(KeenAbilityStatus status)
    {

        // cooldown animation if the cooldown timer is active
        CooldownOverlay.gameObject.SetActive(status.IsKeenCooldown);

        if (status.IsKeenCooldown){
            CooldownOverlay.fillAmount = 1f - (status.KeenCooldownTimer / status.KeenCooldown);
        }

        switch(status.AbilityData.UpgradeTier){
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
            KeenIcon.color = Color.cyan;
        }
    }
}