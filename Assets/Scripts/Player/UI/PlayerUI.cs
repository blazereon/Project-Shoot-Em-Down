using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public HeartMeter HeartMeterInstance;
    public MomentumMeter MomentumMeterInstance;
    public PneumaticMeter PneumaticMeterInstance;
    public ChainMeter ChainMeterInstance;

    public WeaponsUI WeaponsUIInstance;

    public DashUI DashUIInstance;
    public KeenUI KeenUIInstance;
    public DestructiveBoltUI DestructiveBoltUIInstance;



    void Awake()
    {
        EventSystem.Current.OnUpdatePlayerStats += updatePlayerProperties;
        EventSystem.Current.OnUpdateDashAbilityUI += UpdateDash;
        EventSystem.Current.OnUpdateDestructiveBoltUI += UpdateDestructiveBolt;
        EventSystem.Current.OnUpdateKeenAbilityUI += UpdateKeen;
    }

    private void updatePlayerProperties(PlayerStats stats)
    {
        HeartMeterInstance.UpdateValue(stats.Health, stats.MaxHealth);
        MomentumMeterInstance.UpdateValue(stats.Momentum, stats.MaxMomentum);
        PneumaticMeterInstance.UpdateValue(stats.Pneumatic, stats.MaxPneumatic);
        ChainMeterInstance.UpdateValue(stats);
        WeaponsUIInstance.UpdateValue(stats);
    }

    private void UpdateDash(DashAbilityStatus status)
    {
        DashUIInstance.UpdateValue(status);
    }

    private void UpdateDestructiveBolt(DestructiveBoltStatus status)
    {
        DestructiveBoltUIInstance.UpdateValue(status);
    }

    private void UpdateKeen(KeenAbilityStatus status)
    {
        KeenUIInstance.UpdateValue(status);
    }

    void OnDestroy()
    {
        EventSystem.Current.OnUpdatePlayerStats -= updatePlayerProperties;
        EventSystem.Current.OnUpdateDashAbilityUI += UpdateDash;
        EventSystem.Current.OnUpdateDestructiveBoltUI += UpdateDestructiveBolt;
        EventSystem.Current.OnUpdateKeenAbilityUI += UpdateKeen;
    }
}
