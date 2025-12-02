using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class Scene_Manager : MonoBehaviour
{
    // Call this function from a UI Button
    public void PlayGame()
    {
        // Replace "Level_1" with the exact name of your scene
        SceneManager.LoadScene("Level_1");
    }

    // Optional: quit button
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed!"); // Works only in build, not in editor
    }
}