using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private UserInputReader _reader;

    private float _speed = 20f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = _reader.Direction;
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * input.y + right * input.x;

        _camera.transform.position += moveDirection * _speed * Time.deltaTime;
    }
}
