using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    public Image Healthbar;

    public void UpdateValue(int healthAmount, int MaxHealthAmount)
    {
        float healthPercentage = (float)healthAmount / MaxHealthAmount;
        Healthbar.fillAmount = healthPercentage;
    }
    
}
