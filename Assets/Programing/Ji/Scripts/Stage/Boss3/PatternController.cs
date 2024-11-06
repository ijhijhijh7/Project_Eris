using UnityEngine;


/// <summary>
/// 몬스터의 공격 패턴 중 하나로
/// 랜덤으로 줄 발판들을 위 또는 아래로 움직이도록 설정
/// </summary>
public class PatternController : MonoBehaviour
{
    // 보스의 상태에 따라서
    // 보스의 체력이 25% 이상인 경우 한 개의 라인 씩 번갈아 가며 상하이동을 제어
    // 보스의 체력이 25% 이하인 경우 두 개의 라인 씩 번갈아 가며 상하이동을 제어

    //보스의 현재 상태에 따라서 UpDate()에서 분기
    // 보스의 체력이 25% 이상인 경우 - normal
    // 보스의 체력이 25% 이하인 경우 - overdive
    // 보스의 체력상태에 따라 나뉘므로 보스의 체력 상태를 불러오거나, 보스의 체력 관련 스크립트에서 작성하여 받아오도록
    // 수정이 필요
    public enum BossState { normal, overdirve }
    public BossState nowState; // 보스의 현재 상태  

    // 보스가 공격하는 패턴을 보스의 개별 스크립트에서 받아서 사용하도록 수정이 필요
    //public bool isAttackP; // 패턴 변경 공격 상태 여부

    GameObject[] lines; // 자식오브젝트들을 받아오는 배열 생성

    // 인스펙터창에서 수정하기 쉽게 PatternController.cs에서 스피드를 설정하고
    // 실제 사용은 MoveLenPlatform.cs에서 사용됨
    [SerializeField] public float moveSpeed;
    // 인스펙터창에서 수정하기 쉽게 PatternController.cs에서 플랫폼이 생성되고 사라질 천장과 바닥을 설정하고
    // 실제 사용은 LenPlatformLoop.cs에서 사용됨
    [SerializeField] public Transform pCiling; // 천장 - 배경이미지보다 더 넓게 설정할 것
    [SerializeField] public Transform pGround; // 바닥 - 배경이미지보다 더 넓게 설정할 것

    private void Awake()
    {
        // 배열에 자식 오브젝트 설정
        lines = new GameObject[gameObject.transform.childCount]; // 배치할 발판 오브젝트 배열 생성
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = gameObject.transform.GetChild(i).gameObject; // 각 라인을 배열로 만들어 사용
        }
    }
    /*
    private void Update()
    {
        // test용으로 함수를 실행시키기 위한 조건으로 추후 if문에 보스의 체력상태를 넣어서 실행되도록 수정하여 사용
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            isAttackP = true; // 패턴 변경 공격 상태 여부를 받아오기 이걸 맨 위의 이프로 사용하기
            switch (nowState)
            {
                case BossState.normal:
                    if (isAttackP)
                    {
                        setNormalChoice();
                    }
                    break;
                case BossState.overdirve:
                    if (isAttackP)
                    {
                        setOverdirveChoice();
                    }
                    break;
            }
        }
    }
    */
    /// <summary>
    /// 보스의 기본 상태일 때 패턴이 움직이는 방향을 설정
    /// 한 개의 랜덤 숫자를 받아서 한 개의 라인만 기존의 이동 방향에서 반대로 설정
    /// </summary>
    public void setNormalChoice()
    {
        int num = Random.Range(0, 4);
        Debug.Log($"패턴변경 {num}");
        switch (num)
        {
            case 0:
                lines[0].GetComponent<MoveLenPlatform>().isUpMove = !lines[0].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 1:
                lines[1].GetComponent<MoveLenPlatform>().isUpMove = !lines[1].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 2:
                lines[2].GetComponent<MoveLenPlatform>().isUpMove = !lines[2].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 3:
                lines[3].GetComponent<MoveLenPlatform>().isUpMove = !lines[3].GetComponent<MoveLenPlatform>().isUpMove;
                break;
        }
        //isAttackP = false; // 공격 패턴의 종료를 선언
    }

    /// <summary>
    /// 보스의 광폭화 상태일 때 패턴이 움직이는 방향을 설정
    /// 두 개의 랜덤 숫자를 받아서 겹치지 않는지 확인한 후 두 개의 라인을 기존의 이동 방향에서 반대로 설정
    /// </summary>
    public void setOverdirveChoice()
    {
        int num1 = Random.Range(0, 4);
        int num2 = Random.Range(0, 4);
        while (num1 == num2) // 두 수가 같으면
        {
            // 다를 때 까지 랜덤 수 뽑기
            num2 = Random.Range(0, 4);
        }
        // num1 숫자에 맞춰서 레일 변경
        switch (num1)
        {
            case 0:
                lines[0].GetComponent<MoveLenPlatform>().isUpMove = !lines[0].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 1:
                lines[1].GetComponent<MoveLenPlatform>().isUpMove = !lines[1].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 2:
                lines[2].GetComponent<MoveLenPlatform>().isUpMove = !lines[2].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 3:
                lines[3].GetComponent<MoveLenPlatform>().isUpMove = !lines[3].GetComponent<MoveLenPlatform>().isUpMove;
                break;
        }
        // num2 숫자에 맞춰서 레일 변경
        switch (num2)
        {
            case 0:
                lines[0].GetComponent<MoveLenPlatform>().isUpMove = !lines[0].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 1:
                lines[1].GetComponent<MoveLenPlatform>().isUpMove = !lines[1].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 2:
                lines[2].GetComponent<MoveLenPlatform>().isUpMove = !lines[2].GetComponent<MoveLenPlatform>().isUpMove;
                break;
            case 3:
                lines[3].GetComponent<MoveLenPlatform>().isUpMove = !lines[3].GetComponent<MoveLenPlatform>().isUpMove;
                break;
        }
        //isAttackP = false; // 공격패턴의 종료를 선언
    }





}
