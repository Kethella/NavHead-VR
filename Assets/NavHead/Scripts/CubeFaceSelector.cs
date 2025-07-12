using UnityEngine;

// This script allows selecting faces of a cube either by gazing at them or pressing a  keyboard key
// Each face triggers a different environmental or UI effect
public class CubeFaceSelector : MonoBehaviour
{
    public Transform headTransform;
    
    // Face alignment ("Gaze") detection settings
    public float gazeDuration = 4f;
    public float gazeDistance = 5f;
    public LayerMask faceLayer; // Only detect faces in this layer

    // "Gaze" timer and current face being looked at
    private float gazeTimer = 0f;
    private GameObject currentFace = null;

    // State flags for different interactive elements
    private bool lightOn = false;
    private bool musicOn = true;
    private bool nightMode = false;
    private bool tvOn = false;
    private bool redAmbience = false;

    private Color originalSunColor; // Save the original color of the sun for resetting

    private bool isAlignMode = true; // True = Face Aligment ("gaze") mode, False = Keyboard (S key) mode

    // UI and Environment References
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

    // Materials for visual feedback on faces
    [Header("Materials of the faces")]
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

    // UI indicators to show which selection mode is active
    [Header("Selection Mode UI")]
    public GameObject AlignSelectionCanvas;
    public GameObject BlowSelectionCanvas;

    // Cooldown to prevent repeated accidental selection
    private GameObject lastSelectedFace = null;
    private float lastSelectionTime = -Mathf.Infinity;
    private float faceCooldown = 8f; // seconds
    
    void Start()
    {
        // Store the sun's original color for later restoration
        if (sunLight != null)
        {
            originalSunColor = sunLight.color;
        }

        // Set initial visual states
        UpdateAllMaterials();
        UpdateSelectionModeUI();
    }

    void Update()
    {
        HandleModeSwitch(); // Toggle between face alignment and Keyboard mode

        if (isAlignMode)
        {
            HandleGazeSelection(); // Use head movement to select
        }
        else
        {
            HandleKeyboardSelection(); // Use keyboard input to select
        }
    }
    
    // Toggle between face alignment and Keyboard mode when 'L' is pressed
    void HandleModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isAlignMode = !isAlignMode;
            gazeTimer = 0f;
            currentFace = null;
            UpdateSelectionModeUI();
            Debug.Log("Mode switched: " + (isAlignMode ? "Align (Gaze)" : "Tecla S"));
        }
    }
    
    // Show/hide selection mode UI canvases
    void UpdateSelectionModeUI()
    {
        if (AlignSelectionCanvas != null)
            AlignSelectionCanvas.SetActive(isAlignMode);
        if (BlowSelectionCanvas != null)
            BlowSelectionCanvas.SetActive(!isAlignMode);
    }

    // Select a face by looking at it for a fixed duration
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

    // Select the closest face facing the user when 'S' is pressed
    void HandleKeyboardSelection()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject closestFace = FindFaceFacingHead();
            if (closestFace != null)
            {
                SelectFace(closestFace);
            }
        }
    }

    // Find which face is best aligned with the user's view direction
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
    
    // Execute the logic for the selected face
    void SelectFace(GameObject face)
    {
        Debug.Log($"Face '{face.name}' selected!");
        
        // Prevent rapid reselection of the same fac
        if (face == lastSelectedFace && Time.time - lastSelectionTime < faceCooldown)
        {
            Debug.Log($"Ignored repeated selection of '{face.name}' due to cooldown.");
            return;
        }

        lastSelectedFace = face;
        lastSelectionTime = Time.time;
        
        // Trigger action based on face's tag
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
                            sunLight.intensity = 1f;
                            sunLight.color = Color.red;
                        }
                        else
                        {
                            sunLight.intensity = 0.5f;
                            sunLight.color = new Color32(67, 57, 196, 255);
                        }
                    }
                    else
                    {
                        if (redAmbience)
                        {
                            sunLight.intensity = 1f;
                            sunLight.color = Color.red;
                        }
                        else
                        {
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
                        sunLight.intensity = 1f;
                    }
                    else
                    {
                        sunLight.intensity = nightMode ? 0.5f : 1f;
                        sunLight.color = nightMode ? new Color32(67, 57, 196, 255) : originalSunColor;
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

        UpdateAllMaterials(); // Refresh button visuals after state change
    }
    
    // Update visual feedback materials based on states
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
