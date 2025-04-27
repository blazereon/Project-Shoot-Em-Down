using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    private static DebugUI _instance;
    public static DebugUI Current { get { return _instance; }}

    [SerializeField] public TextMeshProUGUI PlayerStateTMP;
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatePlayerState(BasePlayerState state)
    {
        PlayerStateTMP.text = "Player State: " + state;
    }
}
