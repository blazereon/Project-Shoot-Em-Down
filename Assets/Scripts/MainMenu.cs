using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Current.LoadScene("Tutorial");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
