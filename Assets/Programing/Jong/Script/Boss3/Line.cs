using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] LineRenderer lineRenderer;

    // 플레이어를 조준하는 레이저 코드 
    void Start()
    {
        lineRenderer.positionCount = 2;    
    }

   
    void Update()
    {
        lineRenderer.SetPosition(0, startPoint.position);

        Vector3 direction = (endPoint.position - startPoint.position).normalized; // 방향을 계산 
        Vector3 endPointover = startPoint.position + direction * 30f; // 라인의 길이 
        lineRenderer.SetPosition(1, endPointover);  // 계산된 방향의 길이만큼 뒤 까지 라인을 그림
    }
}
