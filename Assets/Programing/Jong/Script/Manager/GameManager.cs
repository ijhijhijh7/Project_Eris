using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update() // 테스트용 
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Debug.Log("일시 정지");
            PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadSceneByName("ManagerTest2");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadSceneByName("ManagerTest3");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LoadSceneByName("ManagerTest");
        }

    }

    public void LoadSceneByName(string sceneName) // 씬 이름으로 불러오기 , 리로딩도 이름으로 불러오면 가능 
    {
            SceneManager.LoadScene(sceneName);
    }

    public void PauseGame() // 일시정지 
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
