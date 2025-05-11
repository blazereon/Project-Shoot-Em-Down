using TMPro;
using UnityEngine;

public class ChainMeter : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ChainMeterTMP;

    public void UpdateValue(PlayerStats playerStats)
    {
        ChainMeterTMP.text = "" + playerStats.Chain;
        
    }

}