using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float minPitch = -80f;  // Prevents flipping
    public float maxPitch = 80f;

    private float pitch = 0f;

    void Update()
    {
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        pitch += verticalInput * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0f);
    }
}
