using UnityEngine;

/// <summary>
/// 발판의 움직임에 따라 발판이 천장과 바닥(배경의 이미지보다 더 위에 지정되어있어야함)의 위치의
/// y값 범위를 넘어가는 경우 발판의 위치를 재조정하는 스크립트
/// </summary>
public class LenPlatformLoop : MonoBehaviour
{
    // MoveLenPlatform에서 현재 라인의 상승, 하강여부를 판별하기 위해 필요
    [SerializeField] MoveLenPlatform moveLenPlatform;
    // 자식오브젝트로 생성한 발판의 갯수(num)와 지정했던 간격(space)으로 오브젝트의 위치를 지정하기 위해 필요
    [SerializeField] CreatePlatform createPlatform;

    Transform pCiling; // 천장
    Transform dGround; // 바닥
    Transform makingPos; // 발판 재배치의 시작 지점 위치 설정
    int num; // 발판의 갯수
    float space; // 발판의 간격 

    private void Awake()
    {

    }
    private void Start()
    {
        // PatternController.cs에서 설정한 수치를 불러오기
        pCiling = moveLenPlatform.pCiling;
        dGround = moveLenPlatform.pGround;
        // CreatePlatform에서 설정한 수치를 불러오기
        makingPos = createPlatform.SetPos;
        num = createPlatform.num - 1; // 발판의 개수가 num 이므로 곱해지는 값은 num - 1
        space = createPlatform.space;
    }
    private void Update()
    {
        // 상승중
        if (moveLenPlatform.isUpMove)
        {
            // 천장의 y값보다 오브젝트의 y 값이 높아지면
            if (gameObject.transform.position.y > pCiling.transform.position.y)
            {
                // 게임오브젝트의 위치 수정
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                            -(makingPos.position.y + space * num));
            }
        }
        // 하강중
        if (!moveLenPlatform.isUpMove)
        {
            // 바닥의 y값보다 오브젝트의 y값이 낮아지면
            if (gameObject.transform.position.y < dGround.transform.position.y)
            {
                // 게임 오브젝트의 위치 수정
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                            makingPos.position.y + space * num);
            }
        }
    }
}

