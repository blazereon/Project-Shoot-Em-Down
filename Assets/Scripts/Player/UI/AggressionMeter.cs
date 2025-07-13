using UnityEngine;
using UnityEngine.UI;

public class AggressionMeter : MonoBehaviour
{
    public Image AggressionBar;
    public void UpdateValue(int aggrAmount, int maxAggrAmount)
    {
        float aggresssionPercentage = (float)aggrAmount / maxAggrAmount;
        AggressionBar.fillAmount = aggresssionPercentage;
    }
}
