using UnityEngine;

public class MapCameraLogic : MonoBehaviour
{
    private Camera mapCamera;
    private float zoomSpeed = 1f;
    private bool isCloseZoom;
    private float targetZoom;
    private const float CLOSE_ZOOM = 30f;
    private const float FAR_ZOOM = 60f;

    private void Awake()
    {
        mapCamera = GetComponent<Camera>();
        isCloseZoom = true;
        targetZoom = CLOSE_ZOOM;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Zoom"))
        {
            isCloseZoom = !isCloseZoom;
            targetZoom = isCloseZoom ? CLOSE_ZOOM : FAR_ZOOM;
        }
        mapCamera.orthographicSize = Mathf.Lerp(mapCamera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }
}
