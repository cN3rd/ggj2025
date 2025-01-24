using UHG;
using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    //[SerializeField] private string componentTypeName = "Player";


    void Start()
    {
        Debug.Log("Start DoorTrigger");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            gameObject.GetComponentInParent<TransitionDirector>().activateVirtCamera();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Body entered trigger " + other.gameObject.tag);

        }
    }    
}
