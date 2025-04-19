using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public HealthMeter HealthMeterInstance;
    public MomentumMeter MomentumMeterInstance;
    void Awake()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
    }

    private void updatePlayerProperties(PlayerStats stats)
    {
        Debug.Log("receivedPlayerUpdates");
        HealthMeterInstance.UpdateValue(stats.Health, stats.MaxHealth);
        MomentumMeterInstance.UpdateValue(stats.Momentum, stats.MaxMomentum);
    }

    void OnDestroy()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
    }
}
