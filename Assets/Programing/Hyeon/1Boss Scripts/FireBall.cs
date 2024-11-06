using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    // 플레이어 데미지 판정
    bool spendDamage = false;
    // 플레이어 프리펩
    [SerializeField] GameObject player;
    // 파이어볼의 스피드
    [SerializeField] float fireBallSpeed;
    // 발사 방향
    private Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Collider2D fireBallCollider;
    [SerializeField] float fireBallDamage;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fireBallCollider = GetComponent<Collider2D>();

        // 파이어볼과 플레이어 사이의 방향 계산
        direction = new Vector2((player.transform.position.x - transform.position.x), 0).normalized;

        // SpriteRenderer와 Collider를 비활성화
        spriteRenderer.enabled = false;
        fireBallCollider.enabled = false;
       
        
        /* 나중에 플레이어 방향 계산해서 반전시킬때 또 쓰자
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1; // 왼쪽을 향할 경우 x 스케일을 반전
            transform.localScale = scale;
        }*/

        // 1.3초 후에 파이어볼을 활성화하고 이동 시작
        StartCoroutine(ActivateAfterDelay(1.3f));

        // 4초 후 자동 소멸
        Destroy(gameObject, 4f);
    }

    private IEnumerator ActivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // SpriteRenderer와 Collider를 활성화
        spriteRenderer.enabled = true;
        fireBallCollider.enabled = true;

        // 이동 시작
        rb.velocity = direction * fireBallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spendDamage)
        {
            if (collision.CompareTag("Player") && !spendDamage)
            {
                PlayerRPG playerRPG = collision.GetComponent<PlayerRPG>();
                if (playerRPG == null)
                {
                    Debug.Log("안들어옴");
                    return;
                }
                // 데미지 안 받았다면
                if (!spendDamage)
                {
                    // 플레이어에게 데미지를 주는 로직
                    playerRPG.TakeDamage(fireBallDamage);
                    Debug.Log($"플레이어에게 {fireBallDamage} 데미지를 입혔습니다.");
                }
                // 한번만 데미지를 주기 위해 spendDamage로 데미지 판정
                spendDamage = true;
            }
        }
    
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 소멸 애니메이션 적용 가능 (필요 시)
            Destroy(gameObject); // 애니메이션 후 소멸
        }
    }

}
