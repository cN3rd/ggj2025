using UHG;
using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
    [SerializeField] private TransitionManager transitionManager;
    private void OnMouseDown()
    {
        transitionManager.ReturnToPlayer();
    }
}
