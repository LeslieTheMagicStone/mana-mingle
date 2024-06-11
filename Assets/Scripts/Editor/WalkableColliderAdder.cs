using UnityEditor;
using UnityEngine;

public class WalkableColliderAdder : EditorWindow
{
    [MenuItem("Window/Add Walkable Collider")]
    public static void ShowWindow()
    {
        GetWindow<WalkableColliderAdder>("Add Walkable Collider");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add Walkable Collider"))
        {
            AddWalkableCollider();
        }
    }

    private void AddWalkableCollider()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null && !collider.isTrigger && obj.layer == LayerMask.NameToLayer("Default"))
            {
                obj.layer = LayerMask.NameToLayer("Walkable");
            }
        }
    }
}
