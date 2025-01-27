using UnityEditor;

public class DomainReloadTrigger : EditorWindow
{
    [MenuItem("Tools/Force Domain Reload")]
    public static void ForceDomainReload()
    {
        AssetDatabase.Refresh(); // Triggers a domain reload
    }
}