using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalScale : MonoBehaviour
{
    [SerializeField] Transform Boss;
    private Vector3 originalScale;

    private void Awake()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    void Start()
    {
        // 이펙트의 초기 스케일 저장
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 부모의 x 스케일이 -1이면 이펙트의 스케일을 원래대로 유지
        if (Boss.localScale.x < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
}
