using UnityEngine;
using UnityEngine.UIElements;

public class ControlsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    UIDocument _doc;
    Button _exitBtn;
    GameObject _mainUI;
    GameObject _controlsUI;


    private void OnEnable()
    {
        
        _doc = GetComponent<UIDocument>();
        VisualElement root = _doc.rootVisualElement;
        _exitBtn = root.Q<Button>("ExitBtn");
        if (_exitBtn != null) _exitBtn.clicked += OnExitClicked;
        _mainUI = GameObject.FindGameObjectWithTag("MainUI");
        
    }

    void Start()
    {
        _mainUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Btn Clicked");
        _mainUI.SetActive(true);
        _doc.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (_exitBtn != null) _exitBtn.clicked -= OnExitClicked;
    }
}
