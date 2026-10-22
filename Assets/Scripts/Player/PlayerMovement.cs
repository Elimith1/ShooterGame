using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 15f;
        [SerializeField] private float jumpSpeed = 23f;
        [SerializeField] private float gravity = 30f;
        [SerializeField] private float rotationTime = 0.1f;

        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private PlayerInputReader inputReader;

        private Vector2 _moveValue;
        private Vector3 _movementDirection;
        private float _rotationVelocity;
        private float _verticalMovement = 0f;

        private void Awake()
        {
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
            if (mainCamera == null && Camera.main != null) mainCamera = Camera.main.transform;
        }

        private void Update()
        {
            _moveValue = inputReader.MoveInput;

            HandleGroundMovement();
            HandleVerticalMovement();

            characterController.Move(_movementDirection);
        }

        private void HandleGroundMovement()
        {
            /*Demandé à gemini: normaliser la direction du mouvement du joueur*/
            _movementDirection = new Vector3(_moveValue.x, 0f, _moveValue.y);

            if (_movementDirection.magnitude > 1.0f) _movementDirection = _movementDirection.normalized;

            if (_movementDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(_movementDirection.x, _movementDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, rotationTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                float tempSpeed = speed;
                if (!characterController.isGrounded) tempSpeed /= 2;

                Vector3 directionWithCamera = (Quaternion.Euler(0f, angle, 0f) * Vector3.forward).normalized;
                float originalMovementMagnitude = _movementDirection.magnitude;
                _movementDirection.x = directionWithCamera.x * tempSpeed * originalMovementMagnitude * Time.deltaTime;
                _movementDirection.z = directionWithCamera.z * tempSpeed * originalMovementMagnitude * Time.deltaTime;
            }
            else
            {
                _movementDirection = Vector3.zero;
            }
        }

        private void HandleVerticalMovement()
        {
            if (!characterController.isGrounded)
                _verticalMovement -= gravity * Time.deltaTime;

            if (inputReader.JumpTriggered)
            {
                if (characterController.isGrounded)
                    _verticalMovement = jumpSpeed;

                inputReader.ResetJumpTrigger();
            }

            _movementDirection.y = _verticalMovement * Time.deltaTime;
        }
    }
}