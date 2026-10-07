using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 18f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float jumpHeight = 6f;
        [SerializeField] private float gravity = -25f;
        [SerializeField] private float groundedGravity = -2f;

        [Header("References")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private PlayerInputReader inputReader;

        private float _verticalVelocity;

        private void Awake()
        {
            if (controller == null) controller = GetComponent<CharacterController>();
            if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
            if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        }

        private void Update()
        {

            HandleMovement();
        }

        private void HandleMovement()
        {
            bool isGrounded = controller.isGrounded;

            if (isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = groundedGravity;
            }

            // Calcul du déplacement horizontal
            Vector2 input = inputReader.MoveInput;
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
            controller.Move(moveDirection * (moveSpeed * Time.deltaTime));

            // Rotation
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Gestion du saut
            if (inputReader.JumpTriggered)
            {
                if (isGrounded)
                {
                    // V = sqrt(2 * hauteur * -gravite)
                    _verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
                }
                
                inputReader.ResetJumpTrigger();
            }

            // Gravité et déplacement vertical
            _verticalVelocity += gravity * Time.deltaTime;
            controller.Move(new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
        }
    }
}