using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAssist : MonoBehaviour
{
    //캐릭터의 떨어지는 속도를 가중시키기 위한 변수
    [SerializeField] float fallMultiplier = 20f;
    //캐릭터의 점프키 입력을 땠을 때 중력의 영향을 키워 더 빠르게 떨어지도록 구현
    [SerializeField] float lowJumpMultiplier = 20f;
    Rigidbody2D rb;
    [SerializeField] Collision coll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb.velocity.y < 0 && coll.onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // 착지 후 y 속도 초기화
        }

        //캐릭터가 하강중일 때
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //캐릭터가 상승중이지만 점프 버튼을 땠을 때
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.C))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
