using UnityEngine;

public class CubeFaceSelector : MonoBehaviour
{
    public Transform headTransform;
    public float gazeDuration = 4f;
    public float gazeDistance = 5f;
    public LayerMask faceLayer;

    private float gazeTimer = 0f;
    private GameObject currentFace = null;

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
                    gazeTimer = 0f; // Reset after selection
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
            Vector3 toHead = (headTransform.position - child.position).normalized;
            Vector3 faceNormal = child.forward; // The "outward" direction of the face

            float dot = Vector3.Dot(faceNormal, toHead);
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
        Debug.Log($"🟩 Face '{face.name}' selecionada!");
        //TODO
        // Trigger custom events or methods
    }
}
