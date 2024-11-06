using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
// using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class BossPattern : MonoBehaviour
{
    enum BossState
    {
        Idle, Move, Attack, Die, win
    }
    // 보스 게임 오브젝트
    [SerializeField] GameObject bossObject;
    // 보스 애니메이션
    [SerializeField] Animator animator;
    // 플레이어 프리펩
    [SerializeField] GameObject player;
    // 보스의 Rigidbody
    [SerializeField] Rigidbody2D bossRigid;
    // 보스 돌진 collider
    [SerializeField] GameObject bossTacklePoint;
    // 파이어볼 생성 좌표
    [SerializeField] Transform fireBallPoint;
    // 파이어볼 오른쪽 프리펩
    [SerializeField] GameObject fireBallRightPre;
    // 파이어볼 왼쪽 프리펩
    [SerializeField] GameObject fireBallLeftPre;

    //화염기둥 생성 좌표
    [SerializeField] Transform fireWallPoint;
    // 화염기둥 프리펩
    [SerializeField] GameObject fireWallPre;
    // 점프공격 이펙트 슬래쉬 프리펩
    [SerializeField] GameObject slash;

    // 패턴 시작 판정 bool
    private bool skillStart = false;
    // 벽 충돌 판정
    private bool isWall = false;
    // 사망 판정
    private bool isDie = false;
    // 달리기 판정
    private bool isRun = false;
    // 스킬 이펙트 프리펩

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] bossSound;

    // 공격 사거리
    [SerializeField] float attackRange;

    // 보스 스탯
    // 보스 HP
    public float bossHP;
    public float bossNowHP;
    // 보스 스피드
    [SerializeField] float bossSpeed;

    // 보스 몬스터 패턴 선택
    int bossPatternNum;
    // 플레이어 위치
    Vector2 playerPosition;

    // 상태를 Idle로 셋팅
    BossState state = BossState.Idle;

    // 등장시 대기상태, 패턴 중간 중간에 대기상태
    // 움직임은 기본적으로 플레이어를 따라감 조건 만족시 Attack 상황으로 변경

    // 스킬패턴 사용시 스킬 이펙트 프리펩 생성 

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        bossRigid = GetComponent<Rigidbody2D>();
        bossNowHP = bossHP;
    }
    private void Update()
    {
        switch (state)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Move:
                Move();
                break;
            case BossState.Attack:
                // 패턴 중에는 다른 동작을 하지 않도록 함
                break;
            case BossState.Die:
                if (!isDie)
                {
                    StartCoroutine(Die());
                }               
                break;
            case BossState.win:
                Win();
                break;
        }
        BossHPSearch();
    }

    private void Idle()
    {
        skillStart = false ;
        // Idle 애니메이션
        animator.Play("boss1 2 idel");
        
        // 플레이어 위치를 바라보게
        Mirrored();

        // 플레이어와의 거리 playerDirection
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // 플레이어와 거리를 계산하여 상태 변경
        if (playerDirection >= attackRange)
        {
            //Move 상태로 변경
            state = BossState.Move;
        }
        // 스킬 패턴이 시작을 안했을때
        else if (!skillStart)
        {
            StartCoroutine(WaitSkill());
        }
    }
    private void Move()
    {
        // 달리기 애니메이션 재생
        animator.Play("boss1 2 walk");
        // 
        if(!isRun)
        {
            isRun = true;
            audioSource.loop = true;
            audioSource.clip = bossSound[5];
            audioSource.Play();
        }
        
        Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, bossSpeed * Time.deltaTime),
            transform.position.y
        );
        transform.position = newPosition;
        // 이동 중에도 스프라이트가 플레이어를 바라보게 처리
        Mirrored();

        // 사거리 판정
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // 사거리 내로 들어오면 공격 전환
        if (playerDirection <= attackRange)
        {
            if (skillStart == false)
            {
                isRun = false;
                audioSource.loop = false;
                audioSource.Stop();
                StartCoroutine(WaitSkill());
            }
        }                   
    }

    // 거리 계산 패턴
    private IEnumerator WaitSkill()
    {
        // 스킬 패턴 시작
        skillStart = true;
        // 대기 애니메이션
        animator.Play("boss1 2 idel");

        // 공격 상태
        state = BossState.Attack;

        // 플레이어 방향 저장
        playerPosition = (player.transform.position - transform.position).normalized;

        yield return new WaitForSeconds(2f);

        // 플레이어 거리 계산
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // 거리에 따라 보스의 패턴 분화
        if (playerDirection <= attackRange / 1.5)
        {
            // Debug.Log($"짧은 거리");
            // 1,2 중 랜덤
            bossPatternNum = Random.Range(1, 3);
        }
        else
        {
            //Debug.Log($"긴 거리");
            // 3,4 중 랜덤
            bossPatternNum = Random.Range(3, 5);
        }

        StartCoroutine(ExecuteAttackPattern());
    }
    private IEnumerator ExecuteAttackPattern()
    {
        skillStart = true;

        switch (bossPatternNum)
        {
            case 1:
                yield return StartCoroutine(FireBarrier());
                Debug.Log("화염기둥 패턴");
                break;
            case 2:
                yield return StartCoroutine(JumpSlash());
                Debug.Log("점프공격 패턴");
                break;
            case 3:
                yield return StartCoroutine(SponFireBall());
                Debug.Log("화염구 패턴");
                break;
            case 4:
                yield return StartCoroutine(BodyTackle());
                Debug.Log("바디태클 패턴");
                break;
        }
        skillStart = false;
        state = BossState.Idle;
        yield return new WaitForSeconds(1f);
        
    }
    private IEnumerator Die()
    {       
        isDie = true;
        // hp 전부 소모 시 사망 애니메이션 송출 후 프리펩 소멸
        DataManager.Instance.LoadGameData();
        audioSource.PlayOneShot(bossSound[4]);
        // 사망 애니메이션 
        animator.Play("boss1 2 die");
        DataManager.Instance.data.isUnlock[1] = true;
        DataManager.Instance.SaveGameData();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Boss2DEnd");
        
        // 스킬 언락
        // 2보스 사망 이야기 씬 이동 이동                  
    }

    private void Win()
    {
        // 플레이어의 HP가 0이되었거나 상태가 DIE가 되었을때
        audioSource.PlayOneShot(bossSound[6]);
        // 승리 애니메이션
        animator.Play("boss1 2 win");
    }

    private IEnumerator BodyTackle()
    {
        // 몸통 박치기 패턴
        isWall = false;

        // 돌진 시작 위치
        Vector2 startPosition = transform.position;
        
        // 애니메이션 재생
        animator.Play("boss1 2 BodyTackle");
        
        yield return new WaitForSeconds(0.7f);
        // 돌진 목표 거리 설정
        float targetDistance = 80f; // 보스가 이동할 거리
        float currentDistance = 0f; // 현재 이동거리
        float tackleSpeed = 200f;  // 돌진 속도

        // 돌진 collider 활성화
        bossTacklePoint.SetActive(true);

        // while문으로 일정 거리를 돌진
        while (currentDistance < targetDistance && !isWall)
        {
            // 충돌 무시하고 목표 위치로 이동
            Vector2 targetPosition = new Vector2(bossRigid.position.x + (playerPosition.x * tackleSpeed * Time.deltaTime), bossRigid.position.y);
            bossRigid.MovePosition(targetPosition);
            currentDistance = Vector2.Distance(new Vector2(transform.position.x, startPosition.y), startPosition);

            yield return null;
        }
        bossRigid.velocity = Vector2.zero;
        audioSource.PlayOneShot(bossSound[3]);
        yield return new WaitForSeconds(0.7f);
        // 돌진 collider 비활성화
        bossTacklePoint.SetActive(false);
        isWall = false;
    }
    private IEnumerator JumpSlash()
    {
        //넓은 범위에 점프 공격
        // 점프의 파워 
        float bossJumpPower = 15f;
        
        // 점프하는 애니메이션
        animator.Play("boss1 2 JumpSlash");
        audioSource.PlayOneShot(bossSound[2]);
        yield return new WaitForSeconds(0.5f);
        // 점프 동작 
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.7f);     
        bossRigid.gravityScale = 5;
        // 보스 팔쪽 콜라이더만 피격판정
        SlashEffect();
        // 점프 공격 패턴 동안 대기 (3초 후 Idle 상태로 변경)
        yield return new WaitForSeconds(1f);
        bossRigid.gravityScale = 1;
        // 위쪽 힘만 가하면 느리게 올라가서 느리게 떨어짐
        // 나중에 기획에 피드백 받고 의도와 맞는지 QnA
    }

    private IEnumerator SponFireBall()
    {
        // 전방에 화염구 발사 패턴

        // 화염구 발사 애니메이션
        animator.Play("boss1 2 SponFireBall");
        FireBallFire();
        // 애니메이션 재생시간
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(bossSound[0]);
        yield return new WaitForSeconds(1.3f);

        // 화염구 프리펩 생성
        
        // 화염구가 생성되는 1.3 초동안 먼 사거리에서 빠르게 보스 뒤로 이동하면 화염구가 보스뒤로 이동하는 문제 발생
        // 플레이어가 1.3초 동안 먼 거리에서 보스뒤로 가기 불가능 하다고 판단. 이 문제는 알아두기만 하자

        // 불 발사 패턴 동안 대기 (1.5초 후 Idle 상태로 변경)
    }
    private IEnumerator FireBarrier()
    {
        float bossJumpPower = 20f;

        animator.Play("boss1 2 FireBarrier");
        yield return new WaitForSeconds(1f);

        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        // 보스가 위로 올라감
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        // 올라가고 차징하는 애니메이션
        animator.Play("boss1 2 FireBarrier");
        audioSource.PlayOneShot(bossSound[1]);
        yield return new WaitForSeconds(0.7f);
        // 보스의 위치 고정
        bossRigid.velocity = Vector2.zero;
        bossRigid.bodyType = RigidbodyType2D.Kinematic;
       
        // 보스의 주위에 화염 장벽 생성
        FireWallInstant();

        // 불기둥 생성 패턴 동안 대기
        yield return new WaitForSeconds(1f);

        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.gravityScale = 10;
        animator.Play("boss1 2 idel");
        yield return new WaitForSeconds(1f);
        bossRigid.gravityScale = 1;
    }
    

    // 보스에게 데미지를 주려면 BossPattern bossPattern = boss.GetComponent<BossPattern>();
    // bossPattern.TakeDamage(데미지);로 데미지를 줄 수 있음
    public void TakeDamage(float damage)
    {
        bossNowHP -= damage;
        Debug.Log($"현재 체력 : {bossNowHP}");
        // 보스의 체력이 0 이하가 되면 상태를 Die로 변경
        if (bossNowHP <= 0)
        {
            state = BossState.Die;
        }
    }

    // 좌우반전
    private void Mirrored()
    {
        // 플레이어가 보스의 왼쪽에 있으면 보스를 왼쪽으로, 오른쪽에 있으면 오른쪽을 바라보게 설정
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            // 보스가 왼쪽을 바라보도록 함
            bossObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // 보스가 오른쪽을 바라보도록 함
            bossObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // 화염구 발사 
    public void FireBallFire()
    {
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            // 보스가 왼쪽을 바라보도록 함
            GameObject fireBall = Instantiate(fireBallLeftPre, fireBallPoint.position, fireBallPoint.rotation);
        }
        else
        {
            // 보스가 오른쪽을 바라보도록 함
            GameObject fireBall = Instantiate(fireBallRightPre, fireBallPoint.position, fireBallPoint.rotation);
        }
        
    }

    public void FireWallInstant()
    {
        GameObject fireWall = Instantiate(fireWallPre, fireWallPoint.position, fireWallPoint.rotation);
    }

    public void SlashEffect()
    {
        GameObject fireWall = Instantiate(slash, fireBallPoint.position, fireBallPoint.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽과 충돌했는지 확인 Test => Boss 교체
        if (collision.gameObject.CompareTag("Boss"))
        {
            isWall = true;
        }
    }
    private void BossHPSearch()
    {
        if (bossNowHP <= 0)
        {
            bossNowHP = 0;
            state = BossState.Die;
        }
    }
}
