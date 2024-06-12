using UnityEngine;

public class PrintLayer : MonoBehaviour
{
    private void Start()
    {
        print($"{name}: {LayerMask.LayerToName(gameObject.layer)}, {gameObject.layer}");
    }
}
