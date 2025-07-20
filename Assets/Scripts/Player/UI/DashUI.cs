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

    public Color normalColor = Color.white;
    public Color empoweredColor = Color.cyan;
    public Color lockedColor = new Color(1f, 1f, 1f, 0.3f); // semi-transparent

    public void UpdateValue(DashAbilityStatus status)
    {
        // Always show the Dash icon
        DashIcon.gameObject.SetActive(true);
        DashCountText.text = status.DashCount.ToString();

        // Cooldown overlay
        if (status.IsCooldownActive)
        {
            CooldownOverlay.gameObject.SetActive(true);
            CooldownOverlay.fillAmount = 1f - (status.CooldownTimer / status.Cooldown);
            DashIcon.fillAmount = 1f - (status.CooldownTimer / status.Cooldown);
        }
        else
        {
            CooldownOverlay.gameObject.SetActive(false);
            DashIcon.fillAmount = 1f;
        }

        // Upgrade tier visuals
        switch (status.AbilityData.UpgradeTier)
        {
            case 0:
                lvl1.enabled = false;
                lvl2.enabled = false;
                lvl3.enabled = false;
                DashIcon.color = lockedColor;
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

        // Empowered state overrides color
        if (status.AbilityData.UpgradeTier > 0)
        {
            DashIcon.color = status.AbilityData.Empowered ? empoweredColor : normalColor;
        }
    }
}
