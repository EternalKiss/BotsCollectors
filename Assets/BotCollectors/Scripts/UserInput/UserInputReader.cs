using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputReader : MonoBehaviour
{
    [SerializeField] private BaseRegistry _baseRegistry;

    private UserInput _userInput;

    public Vector2 Direction { get; private set; }

    public event Action<Ray> Clicked;

    private void Awake()
    {
        _userInput = new UserInput();
        _userInput.Enable();
    }

    private void OnEnable()
    {
        _userInput.Game.Scun.performed += OnScunPerformed;
        _userInput.Game.Mouse.performed += OnMouseLeftButtonClick;
    }

    private void OnDisable()
    {
        _userInput.Game.Scun.performed -= OnScunPerformed;
        _userInput.Game.Mouse.performed -= OnMouseLeftButtonClick;
    }

    private void Update()
    {
        Direction = _userInput.Game.CameraMove.ReadValue<Vector2>();
    }

    private void OnScunPerformed(InputAction.CallbackContext context)
    {
        _baseRegistry.ScanAll();
    }
    private void OnMouseLeftButtonClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Clicked?.Invoke(ray);
    }
}
