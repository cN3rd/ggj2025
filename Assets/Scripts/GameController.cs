using UnityEngine;

namespace UHG
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FirstPersonController player;

        private InputSystemActions _input;

        private void Awake()
        {
            _input = new InputSystemActions();
            player.SetPlayerActions(_input.Player);
        }

        public void EnablePlayerControls() => _input.Player.Enable();
        public void DisablePlayerControls() => _input.Player.Disable();

        public void EnableVNControls() => _input.VN.Enable();
        public void DisableVNControls() => _input.VN.Disable();

        public void EnableMaskMakingControls() => _input.Maskmaking.Enable();
        public void DisableMaskMakingControls() => _input.Maskmaking.Disable();
    }
}
