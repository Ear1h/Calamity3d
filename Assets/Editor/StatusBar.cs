using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusBar : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/StatusBar")]
    public static void ShowExample()
    {
        StatusBar wnd = GetWindow<StatusBar>();
        wnd.titleContent = new GUIContent("StatusBar");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
