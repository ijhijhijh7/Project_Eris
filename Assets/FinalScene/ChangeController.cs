using UnityEngine;

public class ChangeController : MonoBehaviour
{
    public void ChangeScene(string bossid, string eventName)
    {
        switch (bossid)
        {
            case "Boss_1":
                switch (eventName)
                {
                    case "Start":
                        GameManager.Instance.LoadSceneByName("Boss1SPhase1");
                        break;
                    case "Phase1":
                        GameManager.Instance.LoadSceneByName("Boss1SPhase2");
                        break;
                    case "Phase2":
                        GameManager.Instance.LoadSceneByName("Lobby");
                        break;
                }
                break;
            case "Boss_2_1":
                switch (eventName)
                {
                    case "Start":
                        GameManager.Instance.LoadSceneByName("Boss2SPhase1"); 
                        break;
                    case "Phase1":
                        GameManager.Instance.LoadSceneByName("Boss2SPhase2");
                        break;
                    case "Phase2":
                        GameManager.Instance.LoadSceneByName("Lobby");
                        break;
                }
                break;
            case "Boss_3":
                switch (eventName)
                {
                    case "Start":
                        GameManager.Instance.LoadSceneByName("Boss3SPhase1");
                        break;
                    case "Phase1":
                        GameManager.Instance.LoadSceneByName("Boss3SPhase2");
                        break;
                    case "Phase2":
                        GameManager.Instance.LoadSceneByName("BossClear");
                        break;
                }
                break;
            default:
                break;
        }
    }
}