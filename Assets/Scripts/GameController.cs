using UnityEngine;

namespace UHG
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FirstPersonController player;
        [SerializeField] private TransitionDirector transitionDirector;

        public void DisablePlayerControls() => player.enabled = false;

        public void EnablePlayerControls() => player.enabled = true;
    }
}
