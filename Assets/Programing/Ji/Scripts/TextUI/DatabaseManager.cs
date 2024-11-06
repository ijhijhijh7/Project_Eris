using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] string csvFileName; // CSV 파일의 이름을 지정
    [SerializeField] public Dialogue[] dialogues; // Dialogue로 dialogues배열을 만들어서 DialogueParser한 데이터를 배열로 저장

    // Dictionary를 <string, Dialogue> 로 제작
    // dialogueDic에 데이터를 저장하여 사용
    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool isFinish = false; // 데이터 파싱 후 제대로 저장이 되었는지 여부를 확인할 수 있는 변수

    private void Awake()
    {
        if (instance == null) // instance가 null 값이면
        {
            instance = this; // 현재 인스턴스로 저장
            // DatabaseManager와 DialogueParser.cs는 같은 오브젝트에 넣어 한번에 theParser를 찾을 수 있도록 선언
            DialogueParser theParser = GetComponent<DialogueParser>();

            dialogues = theParser.Parser(csvFileName); // DialogueParser의 Parser함수를 실행
            // dialogues에 csv파일의 데이터가 전부 담기게 됨
            
            isFinish = true; // 데이터의 저장이 완료됨
        }

    }
}
