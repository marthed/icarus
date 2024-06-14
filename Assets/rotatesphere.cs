using UnityEngine;

public class rotatesphere : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationSpeed = 2f; // Speed of rotation in degrees per second
    private float totalRotation = 0f; // Track the total rotation around the X axis

    void Update()
    {
        // Calculate rotation amount for this frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Check if adding the rotation amount would exceed 180 degrees
        if (totalRotation + rotationAmount > 180f)
        {
            // Adjust the rotation amount to stop at 180 degrees
            rotationAmount = 180f - totalRotation;
        }

        // Rotate the object around the X axis
        transform.Rotate(Vector3.right, rotationAmount);

        // Update the total rotation
        totalRotation += rotationAmount;

        // Stop rotating once 180 degrees is reached
        if (totalRotation >= 180f)
        {
            // Optionally, you can disable this script to stop further updates
            enabled = false;
        }
    }
}
