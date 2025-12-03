using UnityEngine;

public class Ladybug : MonoBehaviour
{
    internal LadybugFlockController controller;

    void Start()
    {
        // Find controller by tag if not already assigned
        if (controller == null)
        {
            GameObject controllerObj = GameObject.FindGameObjectWithTag("LadybugFlockController");
            if (controllerObj != null)
            {
                controller = controllerObj.GetComponent<LadybugFlockController>();
            }
        }

        if (controller != null)
        {
            controller.AddToFlock(this);
        }
    }

    void Update()
    {
        // Add movement/flocking here later if needed
    }

    private void OnDestroy()
    {
        if (controller != null)
        {
            controller.RemoveFromFlock(this);
        }
    }
}