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
        if (headTransform is null)
        {
            Debug.LogError("HeadTransform is null");
            return;
        }
        
        // Rotation
        initialYaw = headTransform.eulerAngles.y; // save value for initial reference
        initialPitch = headTransform.eulerAngles.x;
        initialRotateTilt = headTransform.eulerAngles.z;
        
        // Position
        initialUpAndDown = headTransform.position.y;
        initialLeftAndRight = headTransform.position.x;
        initialForwardAndBackward = headTransform.position.z;
    }

    void Update()
    {
        MoveLeftRight();
        MoveUpDown();
        //MoveForwardBackward();
        //RotateTilt();
    }

    private void MoveLeftRight()
    {
        if (circle is null)
        {
            return;
        }
        
        float currentYaw = headTransform.eulerAngles.y;
        float yawDelta = Mathf.DeltaAngle(initialYaw, currentYaw);
        float moveX = yawDelta * movementSensitivity * Time.deltaTime;
        
        Vector3 newPosition = circle.anchoredPosition;
        newPosition.x = Mathf.Clamp(newPosition.x + moveX, -1.5f, 1.5f);
        circle.anchoredPosition = newPosition;
        
        initialYaw = currentYaw;
    }

    private void MoveUpDown()
    {
        if (circle is null)
        {
            return;
        }
        
        float currentY = headTransform.position.y;
        float yOffset = currentY - initialUpAndDown;
        float moveY = yOffset * movementSensitivity * Time.deltaTime * 100;
        
        Vector3 newPosition = circle.anchoredPosition;
        newPosition.y = Mathf.Clamp(newPosition.y + moveY, -1.5f, 1.5f);
        circle.anchoredPosition = newPosition;
    }

    // TODO
    private void MoveForwardBackward()
    {
    }
    
    // TODO
    private void RotateTilt()
    {
    }
    
}