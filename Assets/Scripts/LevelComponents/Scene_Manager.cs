using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void PlayGame()
    {
        BugTracker.Instance.ResetGame();
        SceneManager.LoadScene("Level_1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed!");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}