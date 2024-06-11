using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusRaycaster : MonoBehaviour
{
    public float rayLength = 100f; // Max length of the ray
    public LayerMask interactableLayer; // Layer mask to filter which objects to interact with

    void Update()
    {
        PerformRaycast();
    }

    void PerformRaycast()
    {
        // Create a ray from the controller's position forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            Debug.Log($"Hit: {hit.collider.gameObject.name}");
            // Implement your interaction logic here (e.g., highlighting the object, triggering an action, etc.)
        }

        // Optionally, draw the ray in the Scene view for debugging
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.green);
    }
}
