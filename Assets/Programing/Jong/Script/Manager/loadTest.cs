using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           GameManager.Instance.LoadSceneByName("ManagerTest");
        }
    }
}
