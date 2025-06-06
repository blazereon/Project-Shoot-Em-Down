using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashUI : MonoBehaviour
{
    public TextMeshProUGUI DashCountText;
    public Image DashIcon;
    public Image CooldownOverlay;
    public Image lvl1;
    public Image lvl2;
    public Image lvl3;


    public void UpdateValue(DashAbilityStatus status)
    {
        DashCountText.text = status.DashCount.ToString();

        // cooldown animation if the cooldown timer is active
        if (status.IsCooldownActive){
            CooldownOverlay.gameObject.SetActive(true);
            CooldownOverlay.fillAmount = 1f - (status.CooldownTimer / status.Cooldown);
        }
        else{
            CooldownOverlay.gameObject.SetActive(false);
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
            DashIcon.color = Color.cyan;
        }
    }
}