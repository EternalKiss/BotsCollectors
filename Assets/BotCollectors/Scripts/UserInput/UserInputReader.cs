using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputReader : MonoBehaviour
{
    [SerializeField] private Base _base;

    private UserInput _userInput;

    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        _userInput = new UserInput();
        _userInput.Enable();
    }

    private void OnEnable()
    {
        _userInput.Game.Scun.performed += OnScunPerformed;
    }

    private void OnDisable()
    {
        _userInput.Game.Scun.performed -= OnScunPerformed;
    }

    private void Update()
    {
        Direction = _userInput.Game.CameraMove.ReadValue<Vector2>();
    }

    private void OnScunPerformed(InputAction.CallbackContext context)
    {
        _base.Scun();
    }
}
