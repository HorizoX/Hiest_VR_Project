using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadMenuScene : MonoBehaviour
{
    public string menuSceneName = "MenuScene";
    public InputActionReference menuButtonAction;
    public bool menuButtonPressed = false;
    private bool menuSceneLoaded = false;
    public Transform headObject;

    void Start()
    {
        // Enable the input for menuButtonAction
        menuButtonAction.action.Enable();
    }

    void Update()
    {
        // Update menuButtonPressed
        menuButtonPressed = menuButtonAction.action.triggered;
        if (menuButtonPressed && !menuSceneLoaded)
        {
            LoadMenu();
            menuSceneLoaded = true;
            // Set the loaded menu scene's objects as child of the head object
            var menuScene = SceneManager.GetSceneByName(menuSceneName);
            var rootObjects = menuScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                rootObject.transform.parent = headObject;
            }
        }
        else if (menuButtonPressed && menuSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(menuSceneName);
            menuSceneLoaded = false;
        }

    }

    public void LoadMenu()
    {
        if (!menuSceneLoaded)
        {
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
            menuSceneLoaded = true;
        }
    }
}
