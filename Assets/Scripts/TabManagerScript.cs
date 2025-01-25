using UnityEngine;

public class TabManagerScript : MonoBehaviour
{
    [SerializeField] private TabButtonScript startingTabButton;
    private TabButtonScript _activeTab;
    void Start()
    {
        startingTabButton.Activate();
        _activeTab = startingTabButton;
    }

    public void SwitchTab(TabButtonScript button)
    {
        _activeTab.Deactivate();
        button.Activate();
        _activeTab = button;
    }
}
