using UnityEngine;
using UnityEngine.SceneManagement;
using Supercyan.FreeSample;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;
    public Canvas backgroundCanvas;
    [Range(0f, 1f)] public float pausedVolume = 0.2f;
    private float originalVolume;

    private PlayerController playerScript;
    private OrbitCamera cameraScript;

    public static bool IsPaused = false;

    void Start()
    {
        playerScript = FindFirstObjectByType<PlayerController>();
        cameraScript = FindFirstObjectByType<OrbitCamera>();

        if (backgroundMusic == null)
        {
            AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (var source in sources)
            {
                if (source.isPlaying && source.loop)
                {
                    backgroundMusic = source;
                    break;
                }
            }
        }

        if (backgroundMusic != null)
        {
            originalVolume = backgroundMusic.volume;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        if (playerScript != null) playerScript.enabled = true;
        if (cameraScript != null) cameraScript.enabled = true;

        if (backgroundMusic != null) backgroundMusic.volume = originalVolume;
        if (backgroundCanvas != null) backgroundCanvas.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        if (playerScript != null) playerScript.enabled = false;
        if (cameraScript != null) cameraScript.enabled = false;

        if (backgroundMusic != null) backgroundMusic.volume = pausedVolume;
        if (backgroundCanvas != null) backgroundCanvas.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}