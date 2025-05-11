using UnityEngine;

public class CubeFaceSelector : MonoBehaviour
{
    public Transform headTransform;
    public float gazeDuration = 4f;
    public float gazeDistance = 5f;
    public LayerMask faceLayer;

    private float gazeTimer = 0f;
    private GameObject currentFace = null;

    private bool lightOn = false;
    private bool musicOn = true;
    private bool nightMode = false;
    private bool tvOn = false;
    private bool redAmbience = false;

    private Color originalSunColor;

    [Header("Lights")]
    public GameObject lightOnObject;
    public GameObject lightOffObject;

    [Header("Music")]
    public AudioSource ambientMusic;

    [Header("Backgrounds")]
    public GameObject backgroundDay;
    public GameObject backgroundNight;

    [Header("TV")]
    public GameObject screenOn;
    public GameObject screenOff;

    [Header("Ambience")]
    public Light sunLight;

    void Start()
    {
        if (sunLight != null)
        {
            originalSunColor = sunLight.color;
        }
    }

    void Update()
    {
        HandleGazeSelection();
        HandleKeyboardSelection();
    }

    void HandleGazeSelection()
    {
        Ray ray = new Ray(headTransform.position, headTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, gazeDistance, faceLayer))
        {
            if (hit.collider.gameObject == currentFace)
            {
                gazeTimer += Time.deltaTime;

                if (gazeTimer >= gazeDuration)
                {
                    SelectFace(currentFace);
                    gazeTimer = 0f;
                }
            }
            else
            {
                currentFace = hit.collider.gameObject;
                gazeTimer = 0f;
            }
        }
        else
        {
            currentFace = null;
            gazeTimer = 0f;
        }
    }

    void HandleKeyboardSelection()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameObject closestFace = FindFaceFacingHead();
            if (closestFace != null)
            {
                SelectFace(closestFace);
            }
        }
    }

    GameObject FindFaceFacingHead()
    {
        GameObject bestFace = null;
        float bestDot = -1f;

        foreach (Transform child in transform)
        {
            Vector3 toFace = (child.position - headTransform.position).normalized;
            float dot = Vector3.Dot(child.forward, toFace);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestFace = child.gameObject;
            }
        }

        return bestFace;
    }

    void SelectFace(GameObject face)
    {
        Debug.Log($"Face '{face.name}' selected!");

        switch (face.tag)
        {
            case "ButtonLight":
                lightOn = !lightOn;
                if (lightOnObject != null) lightOnObject.SetActive(lightOn);
                if (lightOffObject != null) lightOffObject.SetActive(!lightOn);
                break;

            case "ButtonSound":
                musicOn = !musicOn;
                if (ambientMusic != null)
                {
                    if (musicOn) ambientMusic.Play();
                    else ambientMusic.Pause();
                }
                break;

            case "ButtonNight":
                nightMode = !nightMode;
                if (backgroundDay != null) backgroundDay.SetActive(!nightMode);
                if (backgroundNight != null) backgroundNight.SetActive(nightMode);
                break;

            case "ButtonTV":
                tvOn = !tvOn;
                if (screenOn != null) screenOn.SetActive(tvOn);
                if (screenOff != null) screenOff.SetActive(!tvOn);
                break;

            case "ButtonAmbience":
                redAmbience = !redAmbience;
                if (sunLight != null)
                {
                    sunLight.color = redAmbience ? Color.red : originalSunColor;
                }
                break;

            default:
                Debug.LogWarning("Face without action.");
                break;
        }
    }
}
