using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BtGraphWindow : EditorWindow
{
    private Toolbar toolBar;
    private BtGraphConfig config;
    private BtGraphView graphView;

    [MenuItem("Tools/Behaviour Tree Graph Editor")]
    public static void Open()
    {
        var window = GetWindow<BtGraphWindow>();
        window.titleContent = new GUIContent("Behaviour Tree Graph");
        window.minSize = new Vector2(300, 500);
    }

    public static void Open(BtGraphConfig config)
    {
        var window = GetWindow<BtGraphWindow>();
        window.titleContent = new GUIContent("Behaviour Tree Graph");
        window.minSize = new Vector2(300, 500);
        window.LoadConfig(config);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new BtGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        toolBar = new Toolbar();

        toolBar.Add(new ToolbarButton(CreateNewGraph) { text = "New" });
        toolBar.Add(new ToolbarButton(LoadGraphDialog) { text = "Load" });
        toolBar.Add(new ToolbarButton(Save) { text = "Save" });

        rootVisualElement.Add(toolBar);
    }

    private void CreateNewGraph()
    {
        var path = EditorUtility.SaveFilePanelInProject(
            "Create Behaviour Tree",
            "NewBehaviourTree",
            "asset",
            "Choose where to save the Behaviour Tree asset");

        if (string.IsNullOrEmpty(path)) return;

        var graph = CreateInstance<BtGraphConfig>();
        graph.Id = System.Guid.NewGuid().ToString();
        AssetDatabase.CreateAsset(graph, path);
        AssetDatabase.SaveAssets();

        LoadConfig(graph);
    }

    private void LoadGraphDialog()
    {
        var path = EditorUtility.OpenFilePanel("Load Behaviour Tree", "Assets", "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);
        var conf = AssetDatabase.LoadAssetAtPath<BtGraphConfig>(path);

        if (conf == null)
        {
            EditorUtility.DisplayDialog("Error", "Selected file is not a BehaviourTreeGraphConfig asset.", "OK");
            return;
        }

        LoadConfig(conf);
    }

    private void LoadConfig(BtGraphConfig _config)
    {
        config = _config;
        graphView.SetGraph(config);
    }

    private void Save()
    {
        if (config == null) return;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
    }
}

[CustomEditor(typeof(BtGraphConfig))]
public class BehaviourTreeGraphConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(8);
        if (GUILayout.Button("Open in Graph Editor", GUILayout.Height(28)))
            BtGraphWindow.Open((BtGraphConfig)target);
    }
}