using UnityEngine;
using UnityEngine.UI;

// This script allows the cube to react to head movement (via the Main Camera) for rotation and zooming
public class CubeHeadController : MonoBehaviour
{
    public Image[] selectionSlices;
    public Transform headTransform; // Main Camera
    public Transform groundReference;
    
    // Rotation and zoom parameters
    public float rotationSpeed = 15f;
    public float rotationThreshold = 10f;
    public float zoomThreshold = 12f;
    public float zoomSpeed = 0.3f;
    public float selectionDistanceThreshold = 0.25f;
    public float holdDuration = 2f;

    // Initial reference orientation and position for calibration
    private Vector3 neutralEuler;
    private Vector3 neutralPosition;

    // Current rotation values
    private float currentPitch = 0f;
    private float currentYaw = 0f;

    private float holdTimer = 0f;
    private bool isSelecting = false;

    // Calibration state tracking
    private bool calibrated = false;
    private float calibrationTimer = 0f;
    private float calibrationDelay = 1f; // Delay before capturing the neutral pose

    private enum RotationDirection { None, Left, Right }
    private RotationDirection currentRotationDirection = RotationDirection.None;

    void Update()
    {
        // Wait until calibration time has passed to capture the neutral pose
        if (!calibrated)
        {
            calibrationTimer += Time.deltaTime;
            if (calibrationTimer >= calibrationDelay)
            {
                // Capture the neutral rotation and position of the head
                neutralEuler = headTransform.eulerAngles;
                neutralPosition = headTransform.position;
                calibrated = true;
                Debug.Log("Head pose calibrated.");
            }
            return;
        }
        
        // Once calibrated, continuously check for gestures
        HandleGestures();
    }

    // Detects head movement gestures and applies cube transformations accordingly
    void HandleGestures()
    {
        Vector3 currentEuler = headTransform.eulerAngles;
        Vector3 deltaEuler = new Vector3(
            Mathf.DeltaAngle(neutralEuler.x, currentEuler.x), // Pitch (up/down)
            Mathf.DeltaAngle(neutralEuler.y, currentEuler.y), // Yaw (left/right)
            Mathf.DeltaAngle(neutralEuler.z, currentEuler.z)  // Roll (tilt/zoom)
        );

        float absYaw = Mathf.Abs(deltaEuler.y);
        float absPitch = Mathf.Abs(deltaEuler.x);
        float absRoll = Mathf.Abs(deltaEuler.z);

        bool updated = false;

        // Determine which axis has the dominant movement
        if (absYaw > rotationThreshold || absPitch > rotationThreshold || absRoll > zoomThreshold)
        {
            // Yaw dominates: rotate horizontally
            if ((absYaw > absPitch + 2f) && (absYaw > absRoll + 2f))
            {
                float yawDir = Mathf.Sign(deltaEuler.y);
                currentYaw += yawDir * rotationSpeed * Time.deltaTime;
                Debug.Log($"Dominant Yaw: {yawDir}");
                updated = true;
            }
            // Pitch dominates: rotate vertically
            else if ((absPitch > absYaw + 2f) && (absPitch > absRoll + 2f))
            {
                float pitchDir = Mathf.Sign(deltaEuler.x);
                currentPitch -= pitchDir * rotationSpeed * Time.deltaTime;
                currentPitch = Mathf.Clamp(currentPitch, -89f, 89f);
                Debug.Log($"Dominant Pitch: {pitchDir}");
                updated = true;
            }
            // Roll dominates: apply zoom
            else if ((absRoll > absYaw + 2f) && (absRoll > absPitch + 2f))
            {
                float zoomDir = Mathf.Sign(deltaEuler.z);
                transform.localScale += Vector3.one * zoomDir * zoomSpeed * Time.deltaTime;
                transform.localScale = Vector3.ClampMagnitude(transform.localScale, 3f);
                transform.localScale = Vector3.Max(transform.localScale, Vector3.one * 0.3f);
                Debug.Log($"Dominant Zoom: {(zoomDir > 0 ? "In" : "Out")}");
            }
        }

        // If rotation values were updated, apply them to the cube's transform
        if (updated)
        {
            if (groundReference != null)
            {
                // Calculate rotation using ground reference for consistent "up" direction
                Vector3 up = groundReference.up;
                Vector3 right = Vector3.Cross(up, Vector3.forward).normalized;
                Quaternion yawRotation = Quaternion.AngleAxis(currentYaw, up);
                Quaternion pitchRotation = Quaternion.AngleAxis(currentPitch, yawRotation * right);

                transform.rotation = pitchRotation * yawRotation;
            }
            else
            {
                // Fallback rotation if no ground reference is available
                transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            }
        }

        // If head stabilizes (yaw movement is minimal), stop rotation updates
        if (Mathf.Abs(deltaEuler.y) < rotationThreshold * 0.5f)
        {
            currentRotationDirection = RotationDirection.None;
            Debug.Log("Stop rotation: head stabilized.");
        }
    }
    
    // Reset cube's transform and internal state to original values
    public void ResetToInitialState(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Reset transform values
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;

        // Reset internal tracking and calibration
        currentPitch = 0f;
        currentYaw = 0f;
        calibrated = false;
        calibrationTimer = 0f;

        Debug.Log("Cube state reset.");
    }
}
