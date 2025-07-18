using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

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

    public void OnAxis2D(InputAction.CallbackContext context)
    {
        Debug.Log("OnAxis2D called");
        if (!context.performed)
        {
            Debug.Log("Context not performed, returning.");
            return;
        }
        Debug.Log("Context performed, processing input.");
        Vector2 input = context.ReadValue<Vector2>();
        inputX = input.x;
        inputZ = input.y;
        Debug.Log($"X Input translated: {inputX} \nZ Input translated: {inputZ}");
    }

    public void OnGamepadZTranslate(InputAction.CallbackContext context)
    {
        inputZ = context.ReadValue<float>();
        Debug.Log($"Gamepad Z Input: {inputZ}");
    }

    private void Update()
    {
        Vector3 move = new(inputX, 0, inputZ);
        characterController.Move(speed * Time.deltaTime * move);
    }
}
