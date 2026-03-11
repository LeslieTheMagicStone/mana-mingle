using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target; // 角色的Transform
    public float cameraDistance = 4.0f; // 摄像头距离角色的默认距离

    void LateUpdate()
    {
        Vector3 desiredCameraPos = target.position - transform.forward * cameraDistance;
        RaycastHit hit;

        if (Physics.Raycast(target.position, -transform.forward, out hit, cameraDistance))
        {
            // 如果有碰撞，调整摄像头的位置到碰撞点前一点
            transform.position = hit.point + transform.forward * 0.5f;
        }
        else
        {
            // 如果没有碰撞，使用默认的摄像头位置
            transform.position = desiredCameraPos;
        }
    }
}

