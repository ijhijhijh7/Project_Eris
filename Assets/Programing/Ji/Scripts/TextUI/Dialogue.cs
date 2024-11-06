using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 인스펙터 창에서 수정 가능하도록 설정
public class Dialogue
{
    [Tooltip("이벤트 이름")] // 인스펙터 창에서 확인 가능하도록 설정
    public string eventName; // 이벤트의 이름을 설정 - 분리에 용이함
    [Tooltip("대사 치는 캐릭터 아이디")]
    public string unitId;
    [Tooltip("대사 치는 캐릭터 이름")] 
    public string name;
    [Tooltip("대사 내용")]
    public string[] contexts; // 대사는 string[]로 설정
}
/* 참고자료에서는 사용하였으나 사용하지 않게 된 코드
[System.Serializable] // 인스펙터 창에서 수정 가능하도록 설정
/// <summary>
/// 여러 캐릭터가 대화를 진행해야하므로 Dialgue Class를 배열로 제작해야 함
/// </summary>
public class DialogueEvent
{
    public string eventName; // 이벤트의 이름을 설정 - 분리에 용이함
    public Vector2 line; // x ~ y번째 까지 대사를 추출 해오도록 사용 ex. csv파일의 2(x값)번째부터 5(y값)까지의 대사 추출
    public Dialogue[] dialogues; // Dialogue 클래스를 배열로 다루기
                                 // System.Serializable 선언되어있으므로 인스펙터창에서 수정가능
}*/

