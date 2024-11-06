using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void StartButton()
    {
        GameManager.Instance.LoadSceneByName("GameStart");

    }
}
