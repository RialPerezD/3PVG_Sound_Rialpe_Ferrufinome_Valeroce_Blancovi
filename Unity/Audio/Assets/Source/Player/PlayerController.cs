using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuration")] public float speed = 5f;
    public float sensitivity = 200f;
    public new Transform camera;

    private float _rotationX = 0f;
    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        camera.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        _controller.Move(mover * (speed * Time.deltaTime));

        _controller.Move(Physics.gravity * Time.deltaTime);
    }
}