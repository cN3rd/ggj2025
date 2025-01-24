using UHG;
using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private TransitionDirector transitionDirector;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter DoorTrigger" + other.name);
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Body entered trigger " + other.gameObject.tag);
            transitionDirector.activateEyepeekCamera();
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
