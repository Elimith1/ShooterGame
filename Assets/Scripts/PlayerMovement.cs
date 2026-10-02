using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float tempSpeed = 5f;

    private void Update()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            Vector3 directionWithCamera = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

            float originalMovementMagnitude = inputDirection.magnitude;
            Vector3 movementVector = directionWithCamera * tempSpeed * originalMovementMagnitude * Time.deltaTime;

            transform.position += movementVector;
        }
    }
}