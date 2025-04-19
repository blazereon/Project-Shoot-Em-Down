using UnityEngine;
using UnityEngine.UI;

public class MomentumMeter : MonoBehaviour
{
    public Image MomentumBar;

    public void UpdateValue(int momentumAmount, int MaxMomentumAmount)
    {
        float momentumPercentage = (float)momentumAmount / MaxMomentumAmount;
        MomentumBar.fillAmount = momentumPercentage;
    }
    
}
