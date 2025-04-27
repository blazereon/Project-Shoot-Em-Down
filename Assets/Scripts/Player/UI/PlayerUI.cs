using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public HealthMeter HealthMeterInstance;
    public MomentumMeter MomentumMeterInstance;
    public PneumaticMeter PneumaticMeterInstance;

    void Awake()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
    }

    private void updatePlayerProperties(PlayerStats stats)
    {
        HealthMeterInstance.UpdateValue(stats.Health, stats.MaxHealth);
        MomentumMeterInstance.UpdateValue(stats.Momentum, stats.MaxMomentum);
        PneumaticMeterInstance.UpdateValue(stats.Pneumatic, stats.MaxPneumatic);
    }

    void OnDestroy()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
    }
}
