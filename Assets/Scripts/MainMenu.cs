using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public 
    void Start()
    {
        
    }
    public void PlayGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Current.LoadScene("Tutorial");
    }

    public void ContinueGame()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
