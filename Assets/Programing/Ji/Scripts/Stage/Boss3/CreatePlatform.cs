using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 게임이 시작하면 발판의 위치를 플레이어의 크기에 맞추어 간격을 재설정하는 스크립트
/// 배경보다 더 바깥쪽에 발판이 배치될 범위를 지정하여 사용
/// 배경의 크기와 발판의 크기에 따라서 발판의 개수와 control값을 인스펙터창에서 조절하여
/// 일정한 간격으로 재배치되도록 조정이 필요함
/// </summary>
public class CreatePlatform : MonoBehaviour
{
    [SerializeField] BoxCollider2D playerCollider; // 플레이어의 박스콜라이더의 크기에 맞추어 발판의 간격을 설정
    [SerializeField] public Transform SetPos; // 발판 배치의 시작 지점 위치 설정 - 바닥쪽 배경의 크기 밖의 바닥에 설정
    public float space; // 발판의 간격 
    [SerializeField] float control; // 간격의 임의 조정값
    [SerializeField] public int num; // 발판의 갯수
    GameObject[] child; // 배치할 발판 오브젝트의 배열

    private void Awake()
    {
        num = gameObject.transform.childCount; // 사용할 발판의 개수

        child = new GameObject[num]; // 배치할 발판 오브젝트 배열 생성
        for(int i = 0; i < child.Length; i++)
        {
            child[i] = gameObject.transform.GetChild(i).gameObject; // 자식 오브젝트를 배열에 배치
        }
        space = playerCollider.size.y * control; // 발판의 간격  = 플레이어의 콜라이더의 y값 * 조정값
    }

    private void Start()
    {
        for(int i = 0;i < child.Length;i++) // 배열의 처음부터 끝까지
        {
            // 배열의 오브젝트의 위치를 x값은 부모와 동일하게
            // y값은 시작지점 기준으로 간격 * 횟수로 벌어지게
            child[i].transform.position = new Vector2(gameObject.transform.position.x, SetPos.position.y + space * i);
        }
    }
}
