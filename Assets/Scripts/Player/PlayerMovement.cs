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

            // Récupération du Vector2 mis à jour dans PlayerInputReader
            Vector2 input = inputReader.MoveInput;

            // Calcul du déplacement selon la caméra
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = forward * input.y + right * input.x;

            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }

            // Application du mouvement horizontal selon la direction calculée
            controller.Move(moveDirection * (moveSpeed * Time.deltaTime));

            // Rotation du joueur dans la direction du mouvement
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // 3. Gestion du saut
            if (inputReader.JumpTriggered)
            {
                if (isGrounded)
                {
                    _verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
                }
                
                // On réinitialise l'état du saut
                inputReader.ResetJumpTrigger();
            }

            // Application de la gravité et mouvement vertical
            _verticalVelocity += gravity * Time.deltaTime;
            controller.Move(new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
        }
    }
}