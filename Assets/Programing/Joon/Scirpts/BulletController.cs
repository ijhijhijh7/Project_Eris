using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private Animator bulletAnimator;

    [SerializeField] float destroyDelay = 2f; // 파괴될 시간

    private void Awake()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();

        // Animator 컴포넌트 가져오기
        bulletAnimator = GetComponent<Animator>();

        // 중력 영향을 받지 않도록 설정
        rb.gravityScale = 0;
    }
    private void Start()
    {
        // 파괴 지연 코루틴 시작
        StartCoroutine(DestroyAfterDelay(destroyDelay));
    }

    public void SetSpeed(Vector2 speed)
    {
        if (rb != null)
        {
            rb.velocity = speed; // Rigidbody2D의 velocity를 설정하여 앞으로 이동
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            return; // Player 태그일 경우 무시
        }

        /*// 피격 이펙트 생성
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }*/

        Destroy(gameObject); // 오브젝트 파괴
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지연 시간 대기

        // 오브젝트가 아직 파괴되지 않았을 경우에만 파괴
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
