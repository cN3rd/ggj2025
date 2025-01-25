using Data;
using UnityEngine;

public enum ElementType
{
    HeadOfMask,
    Eyebrows,
    NoseShape,
    Mouth,
    Decorations,
    Shape
}

public class MaskDraggableElement : MonoBehaviour
{
    [SerializeField] private ElementType elementType;
    [SerializeField] private CharacterAffinity affinityType;
    [SerializeField] private DropZone dropZone;
    private Color _mouseOverColor = Color.yellow;
    private bool _dragging = false;
    private Vector3 _dropAnchor;
    private bool _hasAnchor = false;
    private GameObject _originalParent;
    private Vector3 _originalPosition;

    public ElementType GetElementType()
    {
        return elementType;
    }

    public CharacterAffinity getAffinityType()
    {
        return affinityType;
    }

    public void ReturnToOriginalParent()
    {
        gameObject.transform.SetParent(_originalParent.transform);
        transform.position = _originalPosition;
    }

    private void OnEnable()
    {
        _originalParent = transform.parent.gameObject;
        _originalPosition = transform.position;
    }

    public void SetAnchor(Vector3 anchor)
    {
        _hasAnchor = true;
        _dropAnchor = anchor;
    }

    public void RemoveAnchor()
    {
        _hasAnchor = false;
    }

    public void RemoveFromMask()
    {
        if (_hasAnchor)
        {
            RemoveAnchor();
        }
    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = _mouseOverColor;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.gray;
    }

    private void OnMouseDown()
    {
        _dragging = true;
    }

    private void OnMouseUp()
    {
        _dragging = false;
        if (_hasAnchor)
        {
            dropZone.AddElementToMask(this);
            transform.position = _dropAnchor;
        }
        else
        {
            transform.position = _originalPosition;
        }
    }

    void Update()
    {
        if (_dragging)
        {
            LayerMask layerMask = LayerMask.GetMask("Raycast BG");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                transform.position = hit.point;
            }
        }
    }
}
