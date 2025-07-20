using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public GameObject PausePanel;

    public Button ResumeButton;

    void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResumeButton.onClick.AddListener(delegate
        {
            SetResume();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetPause()
    {
        PausePanel.SetActive(true);
    }

    void SetResume()
    {
        UIManager.Current.CurrentState = UIState.None;
    }

    void OnDestroy()
    {
        GameManager.Current.OnPauseEvent -= SetPause;
    }
}
