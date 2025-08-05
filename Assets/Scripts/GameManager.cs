using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public Dictionary<String, String> SceneList = new();
    public GameState CurrentGameState;
    public InputActionMap PlayerInput;
    public InputActionMap UIInput;
    public LoadingScreen LoadingScreenInstance;

    public GameObject PlayerObject;

    [NonSerialized] public PlayerStats PlayerSavedStats;
    [NonSerialized] public bool IsPlayerNew = true;



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

        SceneList.Add("Tutorial", "Assets/Levels/Tutorial/TutorialScene.unity");
        SceneList.Add("LV1", "Assets/Levels/Level1/LV1_Scene.unity");
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
        LoadingScreenInstance.IsLoadingPanelActive = false;
    }

    private void LoadingState()
    {
        Time.timeScale = 0;
        PlayerInput.Disable();
        UIInput.Disable();
        LoadingScreenInstance.IsLoadingPanelActive = true;
    }

    public void LoadScene(String name)
    {
        String _sceneName;
        bool isSceneAvailable = SceneList.TryGetValue(name, out _sceneName);
        
        if (!isSceneAvailable)
        {
            Debug.LogError("Invalid Scene Code: " + name);
            return;
        }
        LoadingScreenInstance.LoadingValue = 0f;
        CurrentGameState = GameState.Loading;
        Time.timeScale = 0;
        PlayerInput.Disable();
        UIInput.Disable();
        LoadingScreenInstance.IsLoadingPanelActive = true;
        StartCoroutine(LoadSceneCoroutine(_sceneName));
    }

    IEnumerator LoadSceneCoroutine(String SceneName)
    {
        yield return new WaitForSecondsRealtime(0.6f);
        LoadingScreenInstance.LoadingValue = 0.4f;
        yield return new WaitForSecondsRealtime(0.4f);
        LoadingScreenInstance.LoadingValue = 0.6f;
        yield return new WaitForSecondsRealtime(0.6f);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncLoad.isDone)
        {
            LoadingScreenInstance.LoadingValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }

        CurrentGameState = GameState.Playing;
    }
}
