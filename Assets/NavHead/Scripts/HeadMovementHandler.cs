using UnityEngine;

public class HeadMovementHandler : MonoBehaviour
{
    public RectTransform circle;
    public float movementSensitivity = 20.0f;
    public Transform headTransform;


    // 6DoF movements
    private float initialYaw; // side-to-side movement
    private float initialRotateTilt; // inclination
    private float initialPitch;
    private float initialUpAndDown; // vertical movement
    private float initialLeftAndRight;
    private float initialForwardAndBackward; // depth

    void Start()
    {
        if (headTransform == null)
        {
            Debug.LogError("HeadTransform is null");
            return;
        }
        
        initialYaw = headTransform.eulerAngles.y; // save value for initial reference
        Debug.Log("Head movement started. Initial yaw value: "  + initialYaw);
    }

    void Update()
    {
        MoveLeftRight();
    }

    private void MoveLeftRight()
    {
        if (circle == null)
        {
            Debug.LogError("circle is null");
            return;
        }
        
        float currentYaw = headTransform.eulerAngles.y;
        float differenceInRotationAxeY = currentYaw - initialYaw;

        float movementAxeX = differenceInRotationAxeY * movementSensitivity * Time.deltaTime;

        Vector3 newPosition = circle.anchoredPosition;
        newPosition.x = Mathf.Clamp(newPosition.x + movementAxeX, -2.5f, 1.5f); //TODO: adjust the value
        circle.anchoredPosition = newPosition;
        
        Debug.Log($"Movement: Yaw difference: {differenceInRotationAxeY}, New position for X: {newPosition.x}");

        
        initialYaw = currentYaw;
    }
    
    private void MoveUpDown() { /* vertical movement */ }
    private void MoveForwardBackward() { /* depth */ }
    private void RotateTilt() { /* inclination */ }
    
}