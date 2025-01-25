using UHG;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Cinemachine;


public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [FormerlySerializedAs("transitionDirector")] [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private GameObject targetCamera;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter DoorTrigger" + other.name);
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Body entered trigger " + other.gameObject.tag);
            transitionManager.TransitionToTargetCamera(targetCamera);
        }
    }
}
