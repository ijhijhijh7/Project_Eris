using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePlatform : MonoBehaviour
{
    public float velocity;
    public Rigidbody2D playerRigidbody;
    public bool isLeft; // 플레이어가 서있는 방향을 판별

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // 플레이어의 리지드 바디를 가져오기
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            // 충돌한 물체의 x 위치값이 기존 물체의 x값보다 크거나 같은지를 비교하여 bool변수 저장
            isLeft = collision.gameObject.transform.position.x < transform.position.x;
            if (isLeft) // 플레이어가 물체의 기준보다 왼쪽에 있는 경우
            {
                // 플레이어의 속력을 주어서 밀려나도록 설정
                playerRigidbody.velocity = new Vector2(velocity, playerRigidbody.velocity.y);
            }
            else // 플레이어가 물체의 기준보다 오른쪽에 있는 경우
            {
                // 플레이어의 속력을 주어서 밀려나도록 설정 - 반대쪽으로
                playerRigidbody.velocity = new Vector2(-velocity, playerRigidbody.velocity.y);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (isLeft) // 플레이어가 물체의 기준보다 왼쪽에 있는 경우
            {
                // 플레이어의 속력을 주어서 밀려나도록 설정
                playerRigidbody.velocity = new Vector2(velocity, playerRigidbody.velocity.y);
            }
            else // 플레이어가 물체의 기준보다 오른쪽에 있는 경우
            {
                // 플레이어의 속력을 주어서 밀려나도록 설정 - 반대쪽으로
                playerRigidbody.velocity = new Vector2(-velocity, playerRigidbody.velocity.y);
            }
        }
    }
}
