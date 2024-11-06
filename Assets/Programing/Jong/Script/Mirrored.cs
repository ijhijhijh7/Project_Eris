using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirrored : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Mirror();
    }
    void Update()
    {
        
    }
    private void Mirror()
    {
        // 플레이어가 보스의 왼쪽에 있으면 보스를 왼쪽으로, 오른쪽에 있으면 오른쪽을 바라보게 설정
        if (player.transform.position.x < transform.position.x)
        {
            // 보스가 왼쪽을 바라보도록 함
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // 보스가 오른쪽을 바라보도록 함
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
