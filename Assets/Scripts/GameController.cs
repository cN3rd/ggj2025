using UnityEngine;

namespace UHG
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FirstPersonController player;
        [SerializeField] private TransitionDirector transitionDirector;




        public void disablePlayerControls()
        {
            player.enabled = false;
        }
        public void enablePlayerControls()
        {
            player.enabled = true;
        }
    }
}
