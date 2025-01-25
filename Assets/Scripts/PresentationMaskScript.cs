using System.Collections.Generic;
using Data;
using TriInspector;
using UnityEngine;

public class PresentationMaskScript : MonoBehaviour
{
    [SerializeField] private Transform anchorHeadOfMask;
    [SerializeField] private Transform anchorEyebrows;
    [SerializeField] private Transform anchorNoseShape;
    [SerializeField] private Transform anchorMouth;
    [SerializeField] private Transform anchorDecorations;
    [SerializeField] private Transform anchorShape;

    private List<CharacterAffinity> _affinities;
    
    public void SetElement(MaskDraggableElement element)
    {
        switch (element.GetElementType())
        {
            case ElementType.HeadOfMask:
                if (anchorHeadOfMask.childCount > 0)
                    Destroy(anchorHeadOfMask.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, anchorHeadOfMask.position, anchorHeadOfMask.rotation, anchorHeadOfMask.transform);
                break;
            case ElementType.Eyebrows:
                if (anchorEyebrows.childCount > 0)
                    Destroy(anchorEyebrows.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, anchorEyebrows.position, anchorEyebrows.rotation, anchorEyebrows.transform);
                break;
            case ElementType.NoseShape:
                if (anchorNoseShape.childCount > 0)
                    Destroy(anchorNoseShape.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, anchorNoseShape.position, anchorNoseShape.rotation, anchorNoseShape.transform);
                break;
            case ElementType.Mouth:
                if (anchorMouth.childCount > 0)
                    Destroy(anchorMouth.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, anchorMouth.position, anchorMouth.rotation, anchorMouth.transform);
                break;
            case ElementType.Decorations:
                if (anchorDecorations.childCount > 0)
                    Destroy(anchorDecorations.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, anchorDecorations.position, anchorDecorations.rotation, anchorDecorations.transform);
                break;
            case ElementType.Shape:
                if (anchorShape.childCount > 0)
                    Destroy(anchorShape.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, anchorShape.position, anchorShape.rotation, anchorShape.transform);
                break;
        }
    }

    [Button]
    public List<CharacterAffinity> GetAffinities()
    {
        _affinities = new List<CharacterAffinity>();
        if (anchorHeadOfMask.childCount > 0)
            _affinities.Add(anchorHeadOfMask.GetChild(0).GetComponent<MaskDraggableElement>().getAffinityType());
        if (anchorEyebrows.childCount > 0)
            _affinities.Add(anchorEyebrows.GetChild(0).GetComponent<MaskDraggableElement>().getAffinityType());
        if (anchorNoseShape.childCount > 0)
            _affinities.Add(anchorNoseShape.GetChild(0).GetComponent<MaskDraggableElement>().getAffinityType());
        if (anchorMouth.childCount > 0)
            _affinities.Add(anchorMouth.GetChild(0).GetComponent<MaskDraggableElement>().getAffinityType());
        if (anchorDecorations.childCount > 0)
            _affinities.Add(anchorDecorations.GetChild(0).GetComponent<MaskDraggableElement>().getAffinityType());
        if (anchorShape.childCount > 0)
            _affinities.Add(anchorShape.GetChild(0).GetComponent<MaskDraggableElement>().getAffinityType());
        
        Debug.Log("affinities: " + string.Join(", ", _affinities));
        return _affinities;
    }
}
