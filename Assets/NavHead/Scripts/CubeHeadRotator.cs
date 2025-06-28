using UnityEngine;
using UnityEngine.UI;

public class CubeHeadController : MonoBehaviour
{
    public Image[] selectionSlices;
    public Transform headTransform; // Main Camera
    public Transform groundReference;
    public float rotationSpeed = 90f;
    public float rotationThreshold = 7f;
    public float zoomThreshold = 7f;
    public float zoomSpeed = 0.5f;
    public float selectionDistanceThreshold = 0.25f;
    public float holdDuration = 2f;

    private Vector3 neutralEuler;
    private Vector3 neutralPosition;

    private float currentPitch = 0f;
    private float currentYaw = 0f;

    private float holdTimer = 0f;
    private bool isSelecting = false;

    private bool calibrated = false;
    private float calibrationTimer = 0f;
    private float calibrationDelay = 1f;

    private enum RotationDirection { None, Left, Right }
    private RotationDirection currentRotationDirection = RotationDirection.None;

    void Update()
    {
        if (!calibrated)
        {
            calibrationTimer += Time.deltaTime;
            if (calibrationTimer >= calibrationDelay)
            {
                neutralEuler = headTransform.eulerAngles;
                neutralPosition = headTransform.position;
                calibrated = true;
                Debug.Log("✅ Head pose calibrated.");
            }
            return;
        }

        HandleGestures();
    }

    void HandleGestures()
    {
        Vector3 currentEuler = headTransform.eulerAngles;
        Vector3 deltaEuler = new Vector3(
            Mathf.DeltaAngle(neutralEuler.x, currentEuler.x), // Pitch
            Mathf.DeltaAngle(neutralEuler.y, currentEuler.y), // Yaw
            Mathf.DeltaAngle(neutralEuler.z, currentEuler.z)  // Roll
        );

        float absYaw = Mathf.Abs(deltaEuler.y);
        float absPitch = Mathf.Abs(deltaEuler.x);
        float absRoll = Mathf.Abs(deltaEuler.z);

        bool updated = false;

        if (absYaw > rotationThreshold || absPitch > rotationThreshold || absRoll > zoomThreshold)
        {
            if (absYaw > absPitch && absYaw > absRoll)
            {
                float yawDir = Mathf.Sign(deltaEuler.y);
                currentYaw += yawDir * rotationSpeed * Time.deltaTime;
                Debug.Log($"↪ Dominant Yaw: {yawDir}");
                updated = true;
            }
            else if (absPitch > absYaw && absPitch > absRoll)
            {
                float pitchDir = Mathf.Sign(deltaEuler.x);
                currentPitch -= pitchDir * rotationSpeed * Time.deltaTime;
                currentPitch = Mathf.Clamp(currentPitch, -89f, 89f);
                Debug.Log($"↕ Dominant Pitch: {pitchDir}");
                updated = true;
            }
            else if (absRoll > absYaw && absRoll > absPitch)
            {
                float zoomDir = Mathf.Sign(deltaEuler.z);
                transform.localScale += Vector3.one * zoomDir * zoomSpeed * Time.deltaTime;
                transform.localScale = Vector3.ClampMagnitude(transform.localScale, 3f);
                transform.localScale = Vector3.Max(transform.localScale, Vector3.one * 0.3f);
                Debug.Log($"🔎 Dominant Zoom: {(zoomDir > 0 ? "In" : "Out")}");
            }
        }

        if (updated)
        {
            if (groundReference != null)
            {
                Vector3 up = groundReference.up;
                Vector3 right = Vector3.Cross(up, Vector3.forward).normalized;
                Quaternion yawRotation = Quaternion.AngleAxis(currentYaw, up);
                Quaternion pitchRotation = Quaternion.AngleAxis(currentPitch, yawRotation * right);

                transform.rotation = pitchRotation * yawRotation;
            }
            else
            {
                transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            }
        }

        if (Mathf.Abs(deltaEuler.y) < rotationThreshold * 0.5f)
        {
            currentRotationDirection = RotationDirection.None;
            Debug.Log("Stop rotation: head stabilized.");
        }
    }
    
    public void ResetToInitialState(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // Reset transform
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;

        // Reset lógica interna
        currentPitch = 0f;
        currentYaw = 0f;
        calibrated = false;
        calibrationTimer = 0f;

        Debug.Log("🔄 Cube state reset.");
    }
}
