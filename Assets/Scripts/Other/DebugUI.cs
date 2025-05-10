using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI PlayerStateTMP;

    void Awake()
    {
        EventSystem.Current.OnUpdatePlayerDebug += UpdateData;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateData(PlayerDebug data)
    {
        PlayerStateTMP.text = string.Format("Player State: {0}\n Chain Multiplier: {1}\n Chain Timer: {2}", 
            data.playerState, 
            data.playerStats.Chain, 
            data.playerStats.ChainTimer < 1 ? 0 : data.playerStats.ChainTimer);
    }
}
