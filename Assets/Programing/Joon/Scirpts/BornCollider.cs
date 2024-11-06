using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornCollider : MonoBehaviour
{
    public GameObject[] bones; // 스켈레톤 캐릭터의 본 리스트를 배열로 지정
    private BoxCollider2D boxCollider;

    private PlayerController playerController;
    private Vector3 originalSize;
    private Vector2 originalOffset;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>(); // 없다면 추가
        }

        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
        playerController = gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        //플레이어 상태가 Idle or Run이 아닐 때 콜라이더 업데이트
        if (playerController.curState != PlayerController.PlayerState.Idle &&
            playerController.curState != PlayerController.PlayerState.Run)
        {
            UpdateColliderBounds();
        }
        else
        {
            boxCollider.offset = originalOffset;
            boxCollider.size = originalSize;
        }
        
    }

    private void UpdateColliderBounds()
    {
        if (bones == null || bones.Length == 0)
        {
            return;
        }

        /*float minX = bones[0].transform.position.x;
        float maxX = bones[0].transform.position.x;*/
        float minY = bones[0].transform.position.y;
        float maxY = bones[0].transform.position.y;

        // 모든 본의 x, y 좌표를 비교하여 범위를 찾음
        foreach (var bone in bones)
        {
            Vector2 bonePos = bone.transform.position;

            /*if (bonePos.x < minX) minX = bonePos.x;
            if (bonePos.x > maxX) maxX = bonePos.x;*/
            if (bonePos.y < minY) minY = bonePos.y; 
            if (bonePos.y > maxY) maxY = bonePos.y + 1.5f;
        }

        // 콜라이더의 중심 위치와 크기 계산
        Vector2 center = new Vector2(transform.position.x, (minY + maxY) / 2);
        Vector2 size = new Vector2(boxCollider.size.x, maxY - minY);

        boxCollider.offset = transform.InverseTransformPoint(center); // 로컬 좌표계로 변환
        boxCollider.size = size;
    }
}
