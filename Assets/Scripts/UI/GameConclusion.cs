using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GameConclusion : MonoBehaviour
{
    public enum Type
    {
        Complete,
        Fail,
        None
    }

    public GameObject BackPanel;
    public GameObject LevelCompletePanel;
    public GameObject GameOverPanel;

    private Type _currentType;
    public Type CurrentType
    {
        get
        {
            return _currentType;
        }
        set
        {
            _currentType = value;
            switch (_currentType)
            {
                case Type.Complete:
                    CompleteState();
                    break;
                case Type.Fail:
                    FailState();
                    break;
                default:
                    NoneState();
                    break;
            }
        }
    }

    private void CompleteState()
    {
        BackPanel.SetActive(true);
        LevelCompletePanel.SetActive(true);
        GameOverPanel.SetActive(false);
    }

    private void FailState()
    {
        BackPanel.SetActive(true);
        LevelCompletePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    private void NoneState()
    {
        BackPanel.SetActive(false);
        LevelCompletePanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void QuitGame()
    {
        //Return to main menu
    }

    public void NextLevel()
    {
        EventSystem.Current.SavePlayerStat();
        GameManager.Current.LoadNextScene();
    }

    public void RestartLevel()
    {
        GameManager.Current.LoadScene(GameManager.Current.CurrentSceneCode);
    }
}