using UnityEngine;

public class CubeHeadRotator : MonoBehaviour
{
    public Transform headTransform;
    public float rotationSpeed = 1f;

    private float initialYaw;
    private float initialPitch;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (headTransform is null)
        {
            Debug.LogWarning("No head transform");
            return;
        }
        
        initialYaw = headTransform.eulerAngles.y;
        initialPitch = headTransform.eulerAngles.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (headTransform is null)
        {
            return;
        }
        
        float currentYaw = headTransform.eulerAngles.y; 
        float currentPitch = headTransform.eulerAngles.x;
        
        float yawDelta = Mathf.DeltaAngle(initialYaw, currentYaw);
        float pitchDelta = Mathf.DeltaAngle(initialPitch, currentPitch);
        
        transform.rotation = Quaternion.Euler(pitchDelta * rotationSpeed, yawDelta * rotationSpeed, 0f);
    }
}
