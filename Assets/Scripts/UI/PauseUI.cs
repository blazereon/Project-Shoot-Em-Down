using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public InputAction CancelInput;
    public GameObject PausePanel;

    public Button ResumeButton;

    void Awake()
    {
        CancelInput = InputSystem.actions.FindAction("Cancel");
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Current.OnPauseEvent += SetPause;

        ResumeButton.onClick.AddListener(delegate
        {
            SetResume();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (CancelInput.triggered)
        {
            SetResume();
        }
    }

    void SetPause()
    {
        PausePanel.SetActive(true);
    }

    void SetResume()
    {
        PausePanel.SetActive(false);
        GameManager.Current.SetState(GameState.Playing);
    }

    void OnDestroy()
    {
        GameManager.Current.OnPauseEvent -= SetPause;
    }
}
