using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private CharacterController characterController;

    private float inputX = 0f;
    private float inputZ = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void OnKeyboardXTranslate(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<float>();
    }

    public void OnGamepadZTranslate(InputAction.CallbackContext context)
    {
        inputZ = context.ReadValue<float>();
    }

    private void Update()
    {
        Vector3 move = new(inputX, 0, inputZ);
        characterController.Move(speed * Time.deltaTime * move);
    }
}
