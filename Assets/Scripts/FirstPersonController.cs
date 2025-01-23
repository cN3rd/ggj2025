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
        [SerializeField] private float moveSpeed = 10f;

        private InputSystemActions _inputSystemActions;
        private float _pitch;

        private void Start()
        {
            _inputSystemActions = new InputSystemActions();
            _inputSystemActions.Enable();
        }

        private void Update()
        {
            var movement = _inputSystemActions.Player.Move.ReadValue<Vector2>();
            var direction = new Vector3(movement.x, 0, movement.y).normalized;
            if (movement != Vector2.zero)
            {
                direction = transform.right * movement.x + transform.forward * movement.y;
            }
            
            characterController.Move(direction * (Time.deltaTime * moveSpeed));
        }

        private void LateUpdate()
        {
            Vector2 rotation = _inputSystemActions.Player.Look.ReadValue<Vector2>();
            
            // Adjust input sensitivity to match Unity's example
            // TODO: adjust according to input type
            rotation.y *= -1; // invert Y
            rotation *= 0.05f; // multiplier
            
            transform.Rotate(Vector3.up, rotation.x);
            
            _pitch += rotation.y;
            _pitch = Utilities.ClampAngle(_pitch, -90, 90);
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
