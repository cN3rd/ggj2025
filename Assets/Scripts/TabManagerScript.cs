using UnityEngine;

public class TabManagerScript : MonoBehaviour
{
    [SerializeField] private TabButtonScript buttonEyebrows;
    [SerializeField] private TabButtonScript buttonCharacteristics;
    [SerializeField] private TabButtonScript buttonMouth;
    [SerializeField] private TabButtonScript buttonDecorations;
    private TabButtonScript activeTab;
    void Start()
    {
        buttonEyebrows.Activate();
        activeTab = buttonEyebrows;
    }

    public void SwitchTab(TabButtonScript button)
    {
        activeTab.Deactivate();
        button.Activate();
        activeTab = button;
    }
}
