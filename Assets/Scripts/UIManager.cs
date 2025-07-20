using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum UIState
{
    Pause,
    SkillTree,
    None
}
public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static UIManager _current;

    public static UIManager Current
    {
        get
        {
            if (_current == null)
            {
                Debug.Log("UI Manager is null");
            }
            return _current;
        }
    }
    private UIState _currentState;

    public UIState CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
    }

    public GameObject PauseMenu;
    public GameObject SkillTreeMenu;



    public InputAction PauseAction, SkillTreeAction, CancelAction;

    private void Awake()
    {
        _current = this;
    }

    void Start()
    {
        PauseAction = InputSystem.actions.FindAction("Pause");
        SkillTreeAction = InputSystem.actions.FindAction("SkillTree");
        CancelAction = InputSystem.actions.FindAction("Cancel");
        CurrentState = UIState.None;
    }

    void Update()
    {
        UpdateState();
    }


    void UpdateState()
    {

        switch (_currentState)
        {
            case UIState.None:
                NoneState();
                break;
            case UIState.Pause:
                PauseState();
                break;
            case UIState.SkillTree:
                SkillTreeState();
                break;
            default:
                Debug.LogError("Invalid UIState");
                break;
        }
    }

    private void SkillTreeState()
    {
        GameManager.Current.SetState(GameState.Paused);
        SkillTreeMenu.SetActive(true);

        if (!CancelAction.triggered) return;
        CurrentState = UIState.None;
        return;
    }

    private void PauseState()
    {
        GameManager.Current.SetState(GameState.Paused);
        PauseMenu.SetActive(true);
        //Release Pause Menu
        if (!CancelAction.triggered) return;
        CurrentState = UIState.None;
        return;
    }

    private void NoneState()
    {
        ClearUIExceptPlayerUI();
        GameManager.Current.SetState(GameState.Playing);

        if (SkillTreeAction.triggered)
        {
            CurrentState = UIState.SkillTree;
            return;
        }

        if (PauseAction.triggered)
        {
            CurrentState = UIState.Pause;
            return;
        }
    }

    private void ClearUIExceptPlayerUI()
    {
        PauseMenu.SetActive(false);
        SkillTreeMenu.SetActive(false);
    }

}
