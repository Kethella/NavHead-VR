using UnityEngine;

public class CubeHeadController : MonoBehaviour
{
    public Transform headTransform; // XR Camera
    public float rotationSpeed = 90f;
    public float rotationThreshold = 10f;
    public float zoomThreshold = 10f;
    public float zoomSpeed = 0.5f;
    public float selectionDistanceThreshold = 0.25f;
    public float holdDuration = 2f;

    private Vector3 neutralEuler;
    private Vector3 neutralPosition;

    private bool isYawRotating = false;
    private bool isPitchRotating = false;
    private bool isZooming = false;

    private float holdTimer = 0f;
    private bool isSelecting = false;

    private bool calibrated = false;
    private float calibrationTimer = 0f;
    private float calibrationDelay = 1f; // calibrate position at start

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
                Debug.Log("Head pose calibrated.");
            }
            return;
        }

        HandleRotation();
        HandleZoom();
        HandleSelection();
    }

    void HandleRotation()
    {
        Vector3 currentEuler = headTransform.eulerAngles;
        Vector3 deltaEuler = new Vector3(
            Mathf.DeltaAngle(neutralEuler.x, currentEuler.x), // pitch
            Mathf.DeltaAngle(neutralEuler.y, currentEuler.y), // yaw
            Mathf.DeltaAngle(neutralEuler.z, currentEuler.z)  // roll
        );

        // YAW (left-right)
        if (!isYawRotating && Mathf.Abs(deltaEuler.y) > rotationThreshold)
        {
            isYawRotating = true;
        }
        else if (isYawRotating && Mathf.Abs(deltaEuler.y) <= rotationThreshold * 0.5f)
        {
            isYawRotating = false;
        }

        if (isYawRotating)
        {
            float direction = Mathf.Sign(deltaEuler.y);
            transform.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);
        }

        // PITCH (up-down)
        if (!isPitchRotating && Mathf.Abs(deltaEuler.x) > rotationThreshold)
        {
            isPitchRotating = true;
        }
        else if (isPitchRotating && Mathf.Abs(deltaEuler.x) <= rotationThreshold * 0.5f)
        {
            isPitchRotating = false;
        }

        if (isPitchRotating)
        {
            float direction = Mathf.Sign(deltaEuler.x);
            transform.Rotate(Vector3.right, -direction * rotationSpeed * Time.deltaTime);
        }
    }

    void HandleZoom()
    {
        Vector3 currentEuler = headTransform.eulerAngles;
        float roll = Mathf.DeltaAngle(neutralEuler.z, currentEuler.z);

        if (!isZooming && Mathf.Abs(roll) > zoomThreshold)
        {
            isZooming = true;
        }
        else if (isZooming && Mathf.Abs(roll) <= zoomThreshold * 0.5f)
        {
            isZooming = false;
        }

        if (isZooming)
        {
            float direction = Mathf.Sign(roll);
            transform.localScale += Vector3.one * direction * zoomSpeed * Time.deltaTime;
        }
    }

    void HandleSelection()
    {
        float forwardDistance = Vector3.Distance(headTransform.position, neutralPosition);

        if (forwardDistance > selectionDistanceThreshold)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDuration && !isSelecting)
            {
                isSelecting = true;
                Debug.Log("🎯 Face selected!");
                // TODO: start actios here
            }
        }
        else
        {
            holdTimer = 0f;
            isSelecting = false;
        }
    }
}
