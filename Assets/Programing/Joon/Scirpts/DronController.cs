using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab; // 발사할 총알의 프리팹
    [SerializeField] Transform bulletSpawnPoint; // 총알의 발사 위치
    [SerializeField] float bulletSpeed = 10f; // 총알의 속도
    [SerializeField] float fireGap = 0.1f; // 연속 발사 간격
    [SerializeField] bool isFiring = false; // 발사 상태

    [SerializeField] Animator DronAnimator;
    //private static int dronIdleHash = Animator.StringToHash("DroneIdle");
    //private static int dronAttackHash = Animator.StringToHash("DroneAttack");
    [SerializeField] Coroutine FireCoroutine;
    [SerializeField] GameObject GFX;
    private void Awake()
    {
        // 같은 GameObject에 있는 Animator를 자동으로 할당
        if (DronAnimator == null)
        {
            DronAnimator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // V 키 입력으로 발사 온오프
        if (Input.GetKeyDown(KeyCode.A))
        {
            isFiring = !isFiring;
            if (isFiring)
            {
                FireCoroutine = StartCoroutine(FireBullets());
                DronAnimator.SetBool("IsAttacking", true); // 공격 애니메이션 시작
            }
            else
            {
                if (FireCoroutine != null)
                {
                    StopCoroutine(FireCoroutine);
                }
                DronAnimator.SetBool("IsAttacking", false); // Idle 애니메이션으로 돌아감
            }
        }
    }

    // 총알 발사 코루틴
    private IEnumerator FireBullets()
    {

        while (isFiring)
        {
            for (int i = 0; i < 3; i++)
            {
                FireSingleBullet();
                //SoundManager.Instance.DronAttackSound();
                yield return new WaitForSeconds(fireGap); // 발사 간격
            }
            yield return new WaitForSeconds(1f); // 세 발 발사 후 재발사 대기 시간
        }
    }

    // 단일 총알 발사
    private void FireSingleBullet()
    {
        Quaternion bulletRotation = Quaternion.identity;
        if (GFX.transform.localScale.x == 1f) // 오른쪽을 바라볼 때
        {
            bulletRotation = Quaternion.identity;
        }
        else if (GFX.transform.localScale.x == -1f) // 왼쪽을 바라볼 때
        {
            bulletRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        
        Vector2 bulletdirection = Vector2.zero;   //총알 방향을 가르키는 변수(초기화)

        if (bulletScript != null)
        {
            if (GFX.transform.localScale.x == 1f) // 오른쪽을 바라볼 때
            {
                bulletdirection = Vector2.right;
            }
            else if (GFX.transform.localScale.x == -1f) // 왼쪽을 바라볼 때
            {
                bulletdirection = Vector2.left;
            }
            bulletScript.SetSpeed(bulletdirection * bulletSpeed); // 총알 속도 설정
        }
    }
}
