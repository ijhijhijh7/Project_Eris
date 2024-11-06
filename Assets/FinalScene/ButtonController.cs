using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // 게임 시작 버튼
    public void GameStart()
    {
        DataManager.Instance.LoadGameData(); // 불러오기
        DataManager.Instance.data.isUnlock[0] = false;
        DataManager.Instance.data.isUnlock[1] = false;
        DataManager.Instance.data.isUnlock[2] = false;
        DataManager.Instance.SaveGameData();
        GameManager.Instance.LoadSceneByName("Lobby");
    }

    // 게임 이어하기 버튼
    public void SaveGameStart()
    {
        DataManager.Instance.LoadGameData();
        GameManager.Instance.LoadSceneByName("Lobby");
    }

    // 게임 종료 버튼
    public void EndGame()
    {
        Application.Quit();
    }
}
