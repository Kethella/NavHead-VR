using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cube;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private CubeHeadController cubeHeadController;

    private bool cubeVisible = false;

    void Start()
    {
        if (cube != null)
        {
            initialPosition = cube.transform.position;
            initialRotation = cube.transform.rotation;
            initialScale = cube.transform.localScale;

            cubeHeadController = cube.GetComponent<CubeHeadController>();

            cube.SetActive(false); // hidden at the beginning
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cubeVisible = !cubeVisible;

            if (cubeVisible)
            {
                cube.SetActive(true);

                if (cubeHeadController != null)
                {
                    cubeHeadController.ResetToInitialState(initialPosition, initialRotation, initialScale);
                }

                Debug.Log("🟩 Cube activated.");
            }
            else
            {
                cube.SetActive(false);
                Debug.Log("🟥 Cube deactivated.");
            }
        }
    }
}
