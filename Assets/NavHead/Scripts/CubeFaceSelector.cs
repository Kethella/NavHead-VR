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

    [Header("Instruction")] 
    public GameObject instructions;

    [Header("Materials das faces")] 
    public Renderer buttonLightRenderer;
    public Material lightOnMaterial;
    public Material lightOffMaterial;

    public Renderer ButtonSoundRenderer;
    public Material SoundOnMaterial;
    public Material SoundOffMaterial;

    public Renderer BackgroundRenderer;
    public Material BackgroundDayMaterial;
    public Material BackgroundNightMaterial;

    public Renderer ButtonTVRenderer;
    public Material TvOnMaterial;
    public Material TvOffMaterial;

    public Renderer ButtonRedRenderer;
    public Material RedOnMaterial;
    public Material RedOffMaterial;

    public Renderer ButtonInstructionsRenderer;
    public Material InstructionsOnMaterial;
    public Material InstructionsOffMaterial;

    void Start()
    {
        if (sunLight != null)
        {
            originalSunColor = sunLight.color;
        }

        // Initialize materials
        UpdateAllMaterials();
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

                if (sunLight != null)
                {
                    if (nightMode)
                    {
                        if (redAmbience)
                        {
                            // Red light on at night: turns on the red sun
                            sunLight.intensity = 1f;
                            sunLight.color = Color.red;
                        }
                        else
                        {
                            sunLight.intensity = 0.5f;
                            sunLight.color = new Color32(67, 57, 196, 255); // azul escuro
                        }
                    }
                    else
                    {
                        if (redAmbience)
                        {
                            // During the day with red light: red sun
                            sunLight.intensity = 1f;
                            sunLight.color = Color.red;
                        }
                        else
                        {
                            // Day
                            sunLight.intensity = 1f;
                            sunLight.color = originalSunColor;
                        }
                    }
                }
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
                    if (redAmbience)
                    {
                        sunLight.color = Color.red;
                        sunLight.intensity = 1f; // Ensures the sun turns on in night mode (when red light is on)
                    }
                    else
                    {
                        if (nightMode)
                        {
                            sunLight.intensity = 0.5f;
                            sunLight.color = new Color32(67, 57, 196, 255);
                        }
                        else
                        {
                            sunLight.intensity = 1f;
                            sunLight.color = originalSunColor;
                        }
                    }
                }
                break;

            case "Instructions":
                if (instructions != null)
                {
                    instructions.SetActive(!instructions.activeSelf);
                }
                break;

            default:
                Debug.LogWarning("Face without action.");
                break;
        }

        UpdateAllMaterials(); // Updates all faces whenever an action is performed
    }

    void UpdateAllMaterials()
    {
        UpdateLightFaceMaterial();
        UpdateSoundFaceMaterial();
        UpdateBackgroundMaterial();
        UpdateTVMaterial();
        UpdateRedAmbienceMaterial();
        UpdateInstructionsMaterial();
    }

    void UpdateLightFaceMaterial()
    {
        if (buttonLightRenderer != null)
        {
            buttonLightRenderer.material = lightOn ? lightOffMaterial : lightOnMaterial;
        }
    }

    void UpdateSoundFaceMaterial()
    {
        if (ButtonSoundRenderer != null)
        {
            ButtonSoundRenderer.material = musicOn ? SoundOffMaterial : SoundOnMaterial;
        }
    }

    void UpdateBackgroundMaterial()
    {
        if (BackgroundRenderer != null)
        {
            BackgroundRenderer.material = nightMode ? BackgroundDayMaterial : BackgroundNightMaterial;
        }
    }

    void UpdateTVMaterial()
    {
        if (ButtonTVRenderer != null)
        {
            ButtonTVRenderer.material = tvOn ? TvOffMaterial : TvOnMaterial;
        }
    }

    void UpdateRedAmbienceMaterial()
    {
        if (ButtonRedRenderer != null)
        {
            ButtonRedRenderer.material = redAmbience ? RedOffMaterial : RedOnMaterial;
        }
    }

    void UpdateInstructionsMaterial()
    {
        if (ButtonInstructionsRenderer != null)
        {
            bool isActive = instructions != null && instructions.activeSelf;
            ButtonInstructionsRenderer.material = isActive ? InstructionsOffMaterial : InstructionsOnMaterial;
        }
    }
}
