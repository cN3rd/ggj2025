using UnityEngine;

public class MaskScript : MonoBehaviour
{
    [SerializeField] private MaskDraggableElement eyebrowsElement;
    [SerializeField] private MaskDraggableElement characteristicsElement;
    [SerializeField] private MaskDraggableElement mouthElement;
    [SerializeField] private MaskDraggableElement decorationsElement;
    [SerializeField] private PresentationMaskScript presentationMask;
    

    public void SetElement(MaskDraggableElement element)
    {
        switch (element.GetElementType())
        {
            case ElementType.Eyebrows:
                eyebrowsElement = element;
                break;
            case ElementType.HeadOfMask:
                characteristicsElement = element;
                break;
            case ElementType.Mouth:
                mouthElement = element;
                break;
            case ElementType.Decorations:
                decorationsElement = element;
                break;
        }
    } // play it on transitionOutEnd

    public MaskDraggableElement GetEyebrows()
    {
        return eyebrowsElement;
    }
    public MaskDraggableElement GetCharacteristics()
    {
        return characteristicsElement;
    }
    public MaskDraggableElement GetMouth()
    {
        return mouthElement;
    }
    public MaskDraggableElement GetDecorations()
    {
        return decorationsElement;
    }
}
