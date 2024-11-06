using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    [SerializeField] private TextAsset csvData; // csv파일을 인스펙터 창에서 설정하여 사용

    /// <summary>
    /// DialogueEvent[]에 들어갈 Dialogue[]인 dialogues 배열을 생성하는 Parser 함수
    /// </summary>
    /// <param name="csvFileName"></param>
    /// <returns></returns>
    public Dialogue[] Parser(string csvFileName)
    {
        // 대화 리스트를 생성
        List<Dialogue> dialogueList = new List<Dialogue>();

        // 한 줄(엔터)을 기준으로 csv파일을 잘라서 string배열화
        string[] data = csvData.text.Split(new char[] { '\n' });

        // i = 0 인 0번째는 (엑셀)표에서의 분류이므로 데이터로서는 필요하지 않으므로 1부터 시작
        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); // i번째 배열을 , 단위로 쪼개서 줄단위의 배열에 저장
            // [0] : 이벤트ID - Start / Phase1 / MonsterDie
            // [1] : 캐릭터ID
            // [2] : 캐릭터이름
            // [3] : 대사

            // Dialogue.cs파일의 class Dialogue 형식으로 각각 저장
            Dialogue dialogue = new Dialogue();
            dialogue.eventName = row[0]; // 이벤트를 저장
            dialogue.unitId = row[1]; // 대사를 하는 캐릭터의 ID저장
            dialogue.name = row[2]; // 대하를 하는 캐릭터의 이름 저장
            // Dialogue.cs파일의 class Dialogue의 대사를 저장하기 위해 string의 리스트를 생성
            List<string> contextList = new List<string>();

            // do - while 문 : 최초 한차례 무조건 실행시킨 후에 조건을 판별하고 이어서 진행할지 여부를 판단
            // csv 파일을 보면 캐릭터의 이름이 없이 대사가 입력되어있는 경우가 있으므로 그것을 판단하기 위해서 do - while문을 사용
            do
            {
                contextList.Add(row[3]);// 리스트에 row[2]에 있는 대사 한 줄을 저장
                if (++i < data.Length)// 미리 진행한 i가 data의 길이보다 작은지를 비교하고
                {
                    row = data[i].Split(new char[] { ',' }); // ++i를 진행해서 다음줄로 넘어가기
                }
                else // data의 길이보다 큰 경우에는 반복문을 실행할 필요가 없음
                {
                    break;
                }
            } while (row[1].ToString() == "");
            // do의 if문에서 다음줄로 넘어간 곳의 캐릭터의 ID가 공란인지를 확인하고
            // 공란이면 대사 한 줄을 저장

            dialogue.contexts = contextList.ToArray(); // 완성된 contextList를 배열로 변환하여 contexts에 저장

            dialogueList.Add(dialogue); // 함수 맨 처음에 제작한 dialogueList에 dialogue를 저장
        }
        return dialogueList.ToArray(); // 저장된 dialogueList를 배열화 하여 출력

    }
}
