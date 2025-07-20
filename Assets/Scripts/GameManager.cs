using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    Playing,
    Loading,
    Paused
}

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created'

    public static GameManager Current;

    public GameState CurrentGameState;
    public InputActionMap PlayerInput;
    public InputActionMap UIInput;

    public Action OnPauseEvent;

    void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        PlayerInput = InputSystem.actions.FindActionMap("Player");
        UIInput = InputSystem.actions.FindActionMap("UI");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    public void SetState(GameState gameState)
    {
        CurrentGameState = gameState;
    }

    void UpdateState()
    {
        switch (CurrentGameState)
        {
            case GameState.Playing:
                PlayingState();
                break;
            case GameState.Paused:
                OnPauseEvent?.Invoke();
                PauseState();
                break;
            case GameState.Loading:
                LoadingState();
                break;
            default:
                Debug.LogError("Invalid Game State");
                break;
        }
    }

    private void PauseState()
    {
        
        Time.timeScale = 0;
        PlayerInput.Disable();
        UIInput.Enable();
    }

    private void PlayingState()
    {
        Time.timeScale = 1;
        PlayerInput.Enable();
        UIInput.Disable();
    }

    private void LoadingState()
    {
        Time.timeScale = 0;
        PlayerInput.Disable();
        UIInput.Disable();
    }
}
