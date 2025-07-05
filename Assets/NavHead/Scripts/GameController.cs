using UnityEngine;

// This script manages the visibility and reset behavior of a cube GameObject.
public class GameController : MonoBehaviour
{
    public GameObject cube;
    
    // Variables to store the initial transform values of the cube
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private CubeHeadController cubeHeadController;

    private bool cubeIsVisible = false;

    void Start()
    {
        if (cube != null)
        {
            // Store its initial position, rotation, and scale of the cube
            initialPosition = cube.transform.position;
            initialRotation = cube.transform.rotation;
            initialScale = cube.transform.localScale;

            cubeHeadController = cube.GetComponent<CubeHeadController>();

            cube.SetActive(false); // hidden the cube at the beginning
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Toggle cube visibility when the "C" key is pressed
        {
            cubeIsVisible = !cubeIsVisible;

            if (cubeIsVisible)
            {
                cube.SetActive(true);

                if (cubeHeadController != null)  // Reset the cube's transform to its original state

                {
                    cubeHeadController.ResetToInitialState(initialPosition, initialRotation, initialScale);
                }

                Debug.Log("Cube activated.");
            }
            else
            {
                cube.SetActive(false);
                Debug.Log("Cube deactivated.");
            }
        }
    }
}
