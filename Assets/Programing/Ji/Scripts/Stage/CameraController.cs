using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player; // 플레이어 오브젝트 설정
    [SerializeField] CinemachineVirtualCamera LCam; // 플레이어가 왼쪽을 바라볼 때(isLeft = true) 사용하는 카메라
    [SerializeField] CinemachineVirtualCamera RCam; // 플레이어가 오른쪽을 바라볼 때(isLeft = false) 사용하는 카메라
    public bool isLeft;

    private void Update()
    {
        // 왼쪽을 바라보고 있으면
        if (isLeft == true) 
        {
            LCam.Priority = 20; // LCam을 활성화
        }
        // 오른쪽을 바라보고 있으면
        else
        {
            LCam.Priority = 5; // RCam을 활성화
        }
    }
}
