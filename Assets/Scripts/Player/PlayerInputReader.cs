using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public bool JumpTriggered { get; private set; }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpTriggered = true;
                Debug.Log("[Input] Saut déclenché !");
            }
        }

        public void ResetJumpTrigger()
        {
            JumpTriggered = false;
        }
    }
}