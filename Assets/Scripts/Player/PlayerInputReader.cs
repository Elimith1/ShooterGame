using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction fireAction;
        private InputAction missileAction;

        private Vector2 moveValue;
        private bool jumpTriggered;
        private bool isFiring;
        private bool isMissileTriggered;

        public Vector2 MoveInput => moveValue;
        public bool JumpTriggered => jumpTriggered;
        public bool IsFiring => isFiring;
        public bool IsMissileTriggered => isMissileTriggered;

        private void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            fireAction = InputSystem.actions.FindAction("Fire");
            missileAction = InputSystem.actions.FindAction("Missile");
        }

        private void Update()
        {
            if (moveAction != null) moveValue = moveAction.ReadValue<Vector2>();

            if (jumpAction != null && jumpAction.triggered) jumpTriggered = true;

            if (fireAction != null) isFiring = fireAction.IsPressed();

            if (missileAction != null && missileAction.triggered) isMissileTriggered = true;
        }

        public void ResetJumpTrigger() => jumpTriggered = false;
        public void ResetMissileTrigger() => isMissileTriggered = false;
    }
}