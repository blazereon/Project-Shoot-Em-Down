using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    public InputAction CancelInput;
    public GameObject PausePanel;

    void Awake()
    {
        CancelInput = InputSystem.actions.FindAction("Cancel");
        GameManager.Current.OnPauseEvent += SetPause;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CancelInput.triggered)
        {
            PausePanel.SetActive(false);
            GameManager.Current.SetState(GameState.Playing);
        }
    }

    void SetPause()
    {
        PausePanel.SetActive(true);
    }

    void OnDestroy()
    {
        GameManager.Current.OnPauseEvent -= SetPause;
    }
}
