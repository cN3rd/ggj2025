using UnityEngine;

public class PresentationMaskScript : MonoBehaviour
{
    [SerializeField] private Transform browAnchor;
    [SerializeField] private Transform characteristicsAnchor;
    [SerializeField] private Transform mouthAnchor;
    [SerializeField] private Transform decorationsAnchor;
    
    public void FillMask(MaskScript mask)
    {
        GameObject.Instantiate(mask.GetEyebrows().gameObject, browAnchor.position, browAnchor.rotation, browAnchor.transform);
        GameObject.Instantiate(mask.GetCharacteristics().gameObject, characteristicsAnchor.position, characteristicsAnchor.rotation, characteristicsAnchor.transform);
        GameObject.Instantiate(mask.GetMouth().gameObject, mouthAnchor.position, mouthAnchor.rotation, mouthAnchor.transform);
        GameObject.Instantiate(mask.GetDecorations().gameObject, decorationsAnchor.position, decorationsAnchor.rotation, decorationsAnchor.transform);
    }
}
