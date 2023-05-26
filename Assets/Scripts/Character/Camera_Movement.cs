using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField]
    private float Sensivity = 250f;
    public static float sensitivityMouse;

    private float xRotation;

    [SerializeField]
    private Transform Body;

    private void Start()
    {
        sensitivityMouse = Sensivity;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityMouse * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Body.Rotate(Vector3.up * mouseX);

    }
}
