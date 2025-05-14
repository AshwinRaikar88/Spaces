using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float minPitch = -80f;  // Prevents flipping
    public float maxPitch = 80f;

    private float pitch = 0f;
    private float yaw = 0f;

    void Update()
    {
        float verticalInput = 0f;
        float horizontalInput = 0f;

        // Vertical input for pitch (up/down)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        // Horizontal input for yaw (left/right)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        // Update pitch and clamp
        pitch += verticalInput * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Update yaw
        yaw += horizontalInput * rotationSpeed * Time.deltaTime;

        // Apply rotation (pitch on X-axis, yaw on Y-axis)
        transform.localEulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
