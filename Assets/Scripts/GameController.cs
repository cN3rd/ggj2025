using UnityEngine;
using UnityEngine.Serialization;

namespace UHG
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FirstPersonController player;

        public void DisablePlayerControls() => player.enabled = false;

        public void EnablePlayerControls() => player.enabled = true;
    }
}
