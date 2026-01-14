using UnityEngine;

public class UserInputReader : MonoBehaviour
{
    private UserInput _userInput;

    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        _userInput = new UserInput();
        _userInput.Enable();
    }

    private void Update()
    {
        Direction = _userInput.Game.CameraMove.ReadValue<Vector2>();
    }
}
