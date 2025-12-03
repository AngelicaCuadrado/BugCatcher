using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement; // Required for loading scenes
using System;


public class MainMenuController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("The name of the scene to load when Start is pressed")]
    [SerializeField] private string gameSceneName = "Level_1";

    [Header("UI References")]
    [Tooltip("Optional: Drag your Tutorial/Controls UI Document here if you want to toggle it")]
    [SerializeField] private GameObject controlsPanel;

    private UIDocument _doc;
    private GameObject _controlsDoc;
    private Button _btnStart;
    private Button _btnOptions;
    private Button _btnControls;
    private Button _btnExit;

    private void OnEnable()
    {
        _doc = GetComponent<UIDocument>();
        _controlsDoc = GameObject.FindGameObjectWithTag("ControlsUI");
        _controlsDoc.SetActive(false);

        VisualElement root = _doc.rootVisualElement;

        // 1. Query the buttons by their UXML names
        _btnStart = root.Q<Button>("BtnStart");
        _btnOptions = root.Q<Button>("BtnOptions");
        _btnControls = root.Q<Button>("BtnControls");
        _btnExit = root.Q<Button>("BtnExit");

        // 2. Subscribe to the Click events
        if (_btnStart != null) _btnStart.clicked += OnStartClicked;
        if (_btnOptions != null) _btnOptions.clicked += OnOptionsClicked;
        if (_btnControls != null) _btnControls.clicked += OnControlsClicked;
        if (_btnExit != null) _btnExit.clicked += OnExitClicked;
    }

    private void Start()
    {
        // Optional: Additional initialization if needed
    }

    private void OnDisable()
    {
        // 3. Good practice: Unsubscribe to prevent memory leaks/errors
        if (_btnStart != null) _btnStart.clicked -= OnStartClicked;
        if (_btnOptions != null) _btnOptions.clicked -= OnOptionsClicked;
        if (_btnControls != null) _btnControls.clicked -= OnControlsClicked;
        if (_btnExit != null) _btnExit.clicked -= OnExitClicked;
    }

    // --- Event Handlers ---

    private void OnStartClicked()
    {
        Debug.Log("Start Game Clicked");
        // Ensure you have added your Game Scene to File > Build Settings
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("Game Scene Name is empty in Inspector!");
        }
    }

    private void OnOptionsClicked()
    {
        Debug.Log("Options Clicked");
    }

    private void OnControlsClicked()
    {
        Debug.Log("Controls Clicked");
        _controlsDoc.SetActive(true);

    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Game");

#if UNITY_EDITOR
        // Stop playing if inside the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quit the application
            Application.Quit();
#endif
    }
}