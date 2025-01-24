using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] private Transform anchorOrigin;
    [SerializeField] private ElementType elementType;
    [SerializeField] private Draggable currentElement;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Droppable"))
        {
            Draggable drag = other.GetComponent<Draggable>();
            if (drag.GetElementType() == elementType)
                drag.SetAnchor(anchorOrigin.position); // saves position while dragging over
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Droppable"))
        {
            other.GetComponent<Draggable>().RemoveAnchor();
        }
    }
    
    public void AddElementToMask(Draggable element) // sets element on to mask on mouseUp
    {
        if (elementType != element.GetElementType()) return;
        
        if (currentElement != null)
            currentElement.ReturnToOriginalParent();
        currentElement = element;
        element.transform.SetParent(transform);
    }
}
