using UnityEngine;

// This script manages interaction through head movements.
public class gameController : MonoBehaviour
{
    
    public HeadMovementHandler headMovement;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        headMovement.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
