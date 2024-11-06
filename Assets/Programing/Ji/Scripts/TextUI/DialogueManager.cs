using System.Collections;
using TMPro;
using UnityEngine;
/* 대사 스크립트 관련 유튜브 영상
 * https://www.youtube.com/watch?v=DPWvoUlHbjg&list=PLUZ5gNInsv_NG_UKZoua8goQbtseAo8Ow&index=11
 * https://www.youtube.com/watch?v=1fRbGvQlIEQ
 * https://www.youtube.com/watch?v=_04sCWLHoXU
 * https://www.youtube.com/watch?v=qJjfYvEYKiE
 * 
 * 대사를 타자형식으로 출력하는 것 관련 유튜브 영상
 * https://www.youtube.com/watch?v=OjcPuEVQT6s
 */

/// <summary>
/// 전반적인 대화 시스템의 텍스트와 이미지의 출력을 담당하는 매니저
/// Assets > Programing > Ji > Prefab > TextUI의 DialogueManager로 프리팹화하여 제작
/// 각 대화 상대의 이미지 파일을 수정하고 데이터를 인스펙터 창에서 매칭시켜 사용
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header ("Don't Change")]
    // DatabaseManager.cs의 GetDialogues() 함수를 사용하기 위한 선언
    DatabaseManager databaseManager;
    private Dialogue[] nowDialogue;
    [SerializeField] string unitId_Boss; // .csv파일의 보스의 unitId를 인스펙터창에서 입력
    [SerializeField] string unitId_Player; // .csv파일의 플레이어의 unitId를 인스펙터창에서 입력

    ChangeController changeController;

    [Header("Status")]
    [SerializeField] float typingSpace; // 타이핑 출력 간격


    [Header("UI")]
    [SerializeField] private GameObject uiBoss; // 보스 캐릭터의 대화 시 출력 오브젝트
    [SerializeField] private GameObject uiPlayer; // 플레이어 캐릭터의 대화 시 출력 오브젝트
    [SerializeField] private GameObject imgDialogue; // 대화창 이미지 오브젝트
    [SerializeField] private GameObject uiTextName; // 대화하는 캐릭터의 이름 출력 오브젝트
    [SerializeField] private GameObject uiTextDialogue; // 캐릭터가 대사 출력 오브젝트

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textName; // 대화하는 캐릭터의 이름 text
    [SerializeField] private TextMeshProUGUI textContext; // 캐릭터의 대사 text

    [Header("Image")]
    [SerializeField] private SpriteRenderer imgBoss; // 보스 캐릭터의 대화 시 출력 이미지
    [SerializeField] private SpriteRenderer imgPlayer; // 플레이어 캐릭터의 대화 시 출력 이미지

    private bool isTyping = false; // 대사의 타이핑이 끝나는 것을 판단하여 다음 대사로 넘어가도록 제어하는 변수

    int count = 0; // nowDialogue[count]로 사용
    int num = 0; // nowDialogue[count].contexts[num] 으로 사용

    private void Awake()
    {
        changeController = transform.GetChild(0).GetComponent<ChangeController>();
        databaseManager = transform.GetChild(3).GetComponent<DatabaseManager>();

        uiBoss.SetActive(true); // 보스 캐릭터의 대화 시 출력 이미지
        uiPlayer.SetActive(true); // 플레이어 캐릭터의 대화 시 출력 이미지
        imgDialogue.SetActive(true); // 대화창 이미지
        uiTextName.SetActive(true); // 대화하는 캐릭터의 이름 출력 오브젝트
        uiTextDialogue.SetActive(true); // 캐릭터가 대사 출력 오브젝트
    }
    private void Start()
    {
        nowDialogue = databaseManager.dialogues; // DatabaseManager에서 Awake()에서 저장된 Dialogues 배열을 불러와서 사용
        // 씬이 시작하자마자 첫번째 대사(이름, 이미지, 대사)를 출력
        ShowImgName(nowDialogue, count);
        ShowTextName(nowDialogue, count);
        // ShowTextContexts(nowDialogue, count, num);
        StartCoroutine(ShowTextTyping());
    }

    private void Update()
    {
        if (!isTyping) // 타이핑이 끝난 경우에
        {
            // 스페이스바를 누르는 경우
            if (Input.GetKeyDown(KeyCode.Space))
            {
                num++;
                // 다음 대사가 존재하는지 판단하여
                if (num < nowDialogue[count].contexts.Length)
                {
                    // 존재하는 경우 대사만 출력
                    // ShowTextContexts(nowDialogue, count, num);
                    StartCoroutine(ShowTextTyping());
                }
                else // 다음 대사가 존재하지 않는 경우
                {
                    count++; // 다음 화자로 넘어가고
                    num = 0; // 대사 횟수를 초기화

                    // 이벤트가 끝나지 않았으면
                    if (nowDialogue[count].eventName.ToString() != "end")
                    {
                        // 알맞은 대사(캐릭터 이름, 대사, 캐릭터이미지)를 출력
                        ShowTextName(nowDialogue, count);
                        ShowImgName(nowDialogue, count);
                        StartCoroutine(ShowTextTyping()); // 대사는 타이핑 효과를 넣기 위해 코루틴으로 구성
                    }
                    else // 이벤트가 끝났다면
                    {
                        changeController.ChangeScene(nowDialogue[0].unitId, nowDialogue[0].eventName);
                        //GameManager.Instance.LoadSceneByName("Boss1SPhase1");
                        //Debug.Log("씬전환"); // 추후 씬 전환하는 명령어로 변경하여 사용할 것
                    }
                }
                StopCoroutine(ShowTextTyping());
            }
        }
    }

    IEnumerator ShowTextTyping()
    {
        isTyping = true;
        string context = nowDialogue[count].contexts[num];
        for (int i = 0; i < context.Length; i++)
        {
            // 각종 효과 조절을 위한 <을 열었을 경우 출력을 제어하는 조건문
            if (context[i] == '<')
            {
                while (true)
                {
                    if (i == context.Length)
                    {
                        break;
                    }
                    if (context[i] == '>')
                    {
                        break;
                        // > 가 나올떼에도 출력하지 않아야 함
                    }
                    i++;
                }
            }
            if (context[i] == '>')
            {
                // > 인 경우 for문의 위로 가서 다음 < 가 출력되지 않는지 추가 확인이 필요
                continue;
            }
            if (context.Contains('*'))
            {
                context = context.Replace('*', ',');
            }
            if (context.Contains('@'))
            {
                context = context.Replace('@', '\n');
            }
            textContext.text = context.Substring(0, i).ToString();
            // Substring(시작지점, 끝지점)을 지정하여 문장의 하나씩 출력되는 기능
            yield return new WaitForSeconds(typingSpace); // 출력딜레이 시간
        }
        isTyping = false;
    }

    /// <summary>
    /// 현재 데이터의 count에 알맞은 이름을 출력
    /// 여기서 count는 키를 입력하여 대사를 넘긴 횟수
    /// </summary>
    /// <param name="nowDialogue"></param>
    /// <param name="count"></param>
    public void ShowTextName(Dialogue[] nowDialogue, int count)
    {
        textName.text = nowDialogue[count].name.ToString();
    }

    /// <summary>
    /// 현재 데이터의 count에 알맞는 캐릭터Id를 찾아서 ID에 맞는 캐릭터는 선명하게
    /// 아닌 캐릭터는 어둡게 표시하기
    /// 데이터 파일의 unitId를 사용하여 변경하였으며, 변경하고자 하는 캐릭터의 unitId를 확인하고 변경하여 사용할 것
    /// </summary>
    /// <param name="nowDialogue"></param>
    /// <param name="count"></param>
    public void ShowImgName(Dialogue[] nowDialogue, int count)
    {
        ;
        if (nowDialogue[count].unitId == unitId_Boss)
        {
            imgBoss.material.color = new Color(1, 1, 1);
            imgPlayer.material.color = new Color(55 / 255f, 55 / 255f, 55 / 255f);
        }
        else if (nowDialogue[count].unitId == unitId_Player)
        {
            imgPlayer.material.color = new Color(1, 1, 1);
            imgBoss.material.color = new Color(55 / 255f, 55 / 255f, 55 / 255f);
        }
    }
}

