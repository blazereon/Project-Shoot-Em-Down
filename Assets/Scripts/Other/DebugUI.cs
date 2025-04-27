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
        updatePlayerState(data.playerState);
    }
    public void updatePlayerState(BasePlayerState state)
    {
        PlayerStateTMP.text = "Player State: " + state;
    }
}
