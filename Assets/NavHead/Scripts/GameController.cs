using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cube;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private bool cubeVisible = false;

    void Start()
    {
        if (cube != null)
        {
            initialPosition = cube.transform.position;
            initialRotation = cube.transform.rotation;
            initialScale = cube.transform.localScale;

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
                // reset position and activate
                cube.transform.position = initialPosition;
                cube.transform.rotation = initialRotation;
                cube.transform.localScale = initialScale;
                cube.SetActive(true);
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