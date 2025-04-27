using UnityEngine;
using UnityEngine.UI;

public class PneumaticMeter : MonoBehaviour
{
    public Image PneumaBar;

    public void UpdateValue(int PneumaAmount, int MaxPneumaAmount)
    {
        float PneumaticPercentage = (float)PneumaAmount / MaxPneumaAmount;
        PneumaBar.fillAmount = PneumaticPercentage;
    }
    
}
