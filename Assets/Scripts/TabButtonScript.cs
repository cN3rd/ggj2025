using UnityEngine;

public class TabButtonScript : MonoBehaviour
{
    [SerializeField] private TabManagerScript tabManager;
    [SerializeField] private ElementType elementType;
    [SerializeField] private GameObject ElementCollection;

    private void OnMouseDown() // onClick -> CHANGE IT MOVE IT DO WHATEVER THIS IS TEMPORARY
    {
        tabManager.SwitchTab(this);
    }

    public void Deactivate()
    {
        ElementCollection.SetActive(false);
    }
    public void Activate()
    {
        ElementCollection.SetActive(true);
    }

}
