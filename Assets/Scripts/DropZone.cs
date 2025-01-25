using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] private Transform anchorOrigin;
    [SerializeField] private ElementType elementType;
    [SerializeField] private MaskDraggableElement currentElement;
    [SerializeField] private PresentationMaskScript mask;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Droppable"))
        {
            MaskDraggableElement drag = other.GetComponent<MaskDraggableElement>();
            if (drag.GetElementType() == elementType)
                drag.SetAnchor(anchorOrigin.position); // saves position while dragging over
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Droppable"))
        {
            other.GetComponent<MaskDraggableElement>().RemoveAnchor();
        }
    }
    
    public void AddElementToMask(MaskDraggableElement element) // sets element on to mask on mouseUp
    {
        if (elementType != element.GetElementType()) return;
        
        if (currentElement != null)
            currentElement.ReturnToOriginalParent();
        currentElement = element;
        element.transform.SetParent(transform);
        mask.SetElement(element);
    }
}
