using UnityEngine;

public class PresentationMaskScript : MonoBehaviour
{
    [SerializeField] private Transform shapeAnchor;
    [SerializeField] private Transform browAnchor;
    [SerializeField] private Transform characteristicsAnchor;
    [SerializeField] private Transform mouthAnchor;
    [SerializeField] private Transform decorationsAnchor;
    
    public void SetElement(MaskDraggableElement element)
    {
        switch (element.GetElementType())
        {
            case ElementType.Eyebrows:
                if (browAnchor.childCount > 0)
                    Destroy(browAnchor.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, browAnchor.position, browAnchor.rotation, browAnchor.transform);
                break;
            case ElementType.HeadOfMask:
                if (characteristicsAnchor.childCount > 0)
                    Destroy(characteristicsAnchor.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, characteristicsAnchor.position, characteristicsAnchor.rotation, characteristicsAnchor.transform);
                break;
            case ElementType.Mouth:
                if (mouthAnchor.childCount > 0)
                    Destroy(mouthAnchor.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, mouthAnchor.position, mouthAnchor.rotation, mouthAnchor.transform);
                break;
            case ElementType.Decorations:
                if (decorationsAnchor.childCount > 0)
                    Destroy(decorationsAnchor.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, decorationsAnchor.position, decorationsAnchor.rotation, decorationsAnchor.transform);
                break;
            case ElementType.Shape:
                if (shapeAnchor.childCount > 0)
                    Destroy(shapeAnchor.GetChild(0).gameObject);
                GameObject.Instantiate(element.gameObject, shapeAnchor.position, shapeAnchor.rotation, shapeAnchor.transform);
                break;
        }

    }
}
