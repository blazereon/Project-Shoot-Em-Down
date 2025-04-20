using UnityEngine;
using UnityEngine.UI;

public class MomentumMeter : MonoBehaviour
{
    public Image MomentumBar75;
    public Image MomentumBar25;

    public void UpdateValue(int momentumAmount, int MaxMomentumAmount)
    {
        float momentumPercentage = (float) momentumAmount / MaxMomentumAmount;
        if (momentumPercentage >= 0.75f)
        {
            MomentumBar75.fillAmount = 0.75f;
            MomentumBar25.fillAmount = (momentumAmount - (MaxMomentumAmount * 0.75f)) / (MaxMomentumAmount * 0.25f);
            return;
        }
        MomentumBar75.fillAmount = momentumPercentage;
        MomentumBar25.fillAmount = 0f;
    }
}
