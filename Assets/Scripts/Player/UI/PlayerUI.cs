using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public HeartMeter HeartMeterInstance;
    public MomentumMeter MomentumMeterInstance;
    public PneumaticMeter PneumaticMeterInstance;
    public ChainMeter ChainMeterInstance;

    
    void Awake()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
    }

    private void updatePlayerProperties(PlayerStats stats)
    {
        HeartMeterInstance.UpdateValue(stats.Health, stats.MaxHealth);
        MomentumMeterInstance.UpdateValue(stats.Momentum, stats.MaxMomentum);
        PneumaticMeterInstance.UpdateValue(stats.Pneumatic, stats.MaxPneumatic);
        ChainMeterInstance.UpdateValue(stats);
    }

    void OnDestroy()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
    }
}
