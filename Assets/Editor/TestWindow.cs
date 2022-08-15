using UnityEditor;

public class TestWindow : EditorWindow
{
    [MenuItem("Test/Test Window")]
    public static void Open() => GetWindow<TestWindow>();

    private void CreateGUI()
    {

    }
}