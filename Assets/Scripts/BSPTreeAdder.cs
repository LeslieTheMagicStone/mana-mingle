using UnityEditor;
using UnityEngine;

public class BSPTreeAdder : EditorWindow
{
    [MenuItem("Window/Add BSPTree to MeshCollider")]
    public static void ShowWindow()
    {
        GetWindow<BSPTreeAdder>("Add BSPTree to MeshCollider");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add BSPTree to MeshCollider"))
        {
            AddBSPTree();
        }
        if (GUILayout.Button("Remove BSPTree to MeshCollider"))
        {
            RemoveBSPTree();
        }
        if (GUILayout.Button("Check MeshCollider"))
        {
            CheckForMissingMeshCollider();
        }
    }

    private void AddBSPTree()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<MeshCollider>() != null && obj.GetComponent<BSPTree>() == null)
            {
                obj.AddComponent<BSPTree>();
            }
        }
    }

    private void RemoveBSPTree()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<MeshCollider>() != null && obj.GetComponent<BSPTree>() != null)
            {
                DestroyImmediate(obj.GetComponent<BSPTree>());
            }
        }
    }

    private void CheckForMissingMeshCollider()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<BSPTree>() != null && obj.GetComponent<MeshCollider>() == null)
            {
                Debug.Log("GameObject " + obj.name + " has a BSPTree but no MeshCollider.");
            }
        }
    }
}
