using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniatureViewScript : MonoBehaviour
{
    public InputActionReference actionReference; // Reference to the input action to be checked
    public GameObject firstCameraParent; // Reference to the first camera parent object
    public GameObject secondCameraParent; // Reference to the second camera parent object

    private bool isSwitched = false; // Indicates if the camera is currently switched or not

    void Start()
    {
        // Enable the input for actionReference
        actionReference.action.Enable();
        firstCameraParent.SetActive(true);
        secondCameraParent.SetActive(false);
    }

    void Update()
    {
        if (actionReference.action.triggered)
        {
            SwitchCameras(); // If the input action was triggered, switch the cameras
            Debug.Log("Cameras Switched");
        }
    }

    void SwitchCameras()
    {
        isSwitched = !isSwitched;

        if (isSwitched)
        {
            firstCameraParent.SetActive(false);
            secondCameraParent.SetActive(true);
        }
        else
        {
            firstCameraParent.SetActive(true);
            secondCameraParent.SetActive(false);
        }
    }
}
