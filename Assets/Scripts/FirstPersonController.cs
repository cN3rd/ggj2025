using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace UHG
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform headLookTransform;
        [SerializeField] private float maxSpeed = 4f;
        [SerializeField] private float moveAccel = 10f;
        

        private InputSystemActions _inputSystemActions;
        private float _pitch;
        private float _speed;
        private float _verticalVelocity;
        private Vector3 _lastInputDirection;

        private void Start()
        {
            _inputSystemActions = new InputSystemActions();
            _inputSystemActions.Enable();
        }

        private void Update()
        {
            float targetSpeed = maxSpeed;
            var input = _inputSystemActions.Player.Move.ReadValue<Vector2>();
            // if there is no input, set the target speed to 0
            if (input == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

            float speedOffset = .5f;

            // accelerate or decelerate to target speed
            if (currentSpeed < targetSpeed - speedOffset || currentSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * moveAccel);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
            
            if (input != Vector2.zero)
            {
                inputDirection = transform.right * input.x + transform.forward * input.y;
                _lastInputDirection = inputDirection;
            }
            else
            {
                inputDirection = _lastInputDirection;
            }

            Vector3 _playerMove;
            if (inputDirection != Vector3.zero)
                _playerMove = inputDirection.normalized * _speed;
            else 
                _playerMove = _lastInputDirection * _speed;
            // move the player
            characterController.Move(_playerMove * Time.deltaTime);
        }

        private void LateUpdate()
        {

            Vector2 rotation = _inputSystemActions.Player.Look.ReadValue<Vector2>();
            
            // Adjust input sensitivity to match Unity's example
            // TODO: adjust according to input type
            rotation.y *= -1; // invert Y
            rotation *= 0.25f; // multiplier
            
            transform.Rotate(Vector3.up, rotation.x);
            
            _pitch += rotation.y;
            _pitch = Utilities.ClampAngle(_pitch, -80, 80);
            headLookTransform.localRotation = Quaternion.Euler(_pitch, 0, 0);
        }
    }

    [BurstCompile]
    internal static class Utilities
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return math.clamp(angle, min, max);
        }
    }
}
