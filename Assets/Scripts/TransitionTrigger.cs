using UHG;
using UnityEngine;
using UnityEngine.Serialization;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [FormerlySerializedAs("transitionDirector")] [SerializeField] private InOutTransitioner inOutTransitioner;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter DoorTrigger" + other.name);
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Body entered trigger " + other.gameObject.tag);
            inOutTransitioner.ActivateTargetCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Body exited trigger " + other.gameObject.tag);
        }
    }
}
