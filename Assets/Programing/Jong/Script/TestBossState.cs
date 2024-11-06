using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossState : MonoBehaviour
{   

    // 지상 상태와 공중상태 2가지 지상에서 2패턴+ 백스텝  공중에서 2패턴 한쪽패턴에서 3번쓰면 다른패턴으로 전환
    // 지상일때 거리체크랑 공중일때 거리체크 및 이동방식이 바뀌어야 함 
    enum BossState
    {
        Idle, Move, Attack, Die
    }
    // 보스 게임 오브젝트
    [SerializeField] GameObject bossObject;
    [SerializeField] Transform bossTransform; // 보스의 이동과 회전의 기준이 될 트랜스폼 
    [SerializeField] Rigidbody2D bossRigid;


    // 보스 애니메이션
    [SerializeField] Animator animator;
    // 플레이어 프리펩
    [SerializeField] GameObject player;
    // 스킬 이펙트 프리펩



    // 보스 스탯
    public float bossHP = 10;
    [SerializeField] float attackRange;  // 사거리 
    [SerializeField] float bossSpeed;    // 이동속도 
    [SerializeField] float bossCoolTime; // 패턴의 쿨 타임
    [SerializeField] bool isOnPattern = false;    // 패턴 진행중일 때 

    // 보스 몬스터 패턴 선택
    int bossPatternNum;
    // 상태를 Idle로 셋팅
    BossState state = BossState.Idle;
    // 등장시 대기상태, 패턴 중간 중간에 대기상태
    // 움직임은 기본적으로 플레이어를 따라감 조건 만족시 Attack 상황으로 

    // 스킬 패턴 사용시 스킬 이펙트 프리펩 생성 
    [SerializeField] GameObject slashPrefab;
    [SerializeField] Transform slashPoint;

    [SerializeField] GameObject fireballPrefab;

    WaitForSeconds time; // 패턴 쿨타임 용 시간 
    [SerializeField] BossState curBossState; // 보스의 현재 상태 확인용 

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        time = new WaitForSeconds(bossCoolTime);// 패턴 쿨타임 용 시간 
    }


    private void Update()
    {
        curBossState = state; // 보스의 현재 상태 확인용 

        switch (state)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Move:
                Move();
                break;
            case BossState.Die:
                Die();
                break;
        }

    }
    private void Idle()
    {
        // Idle 애니메이션
        // animator.Play();
        animator.Play("idle");
        // 플레이어 위치를 바라보게
        Mirrored();

        // 플레이어와의 거리 distanceToPlayer
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distanceToPlayer);
        // 플레이어와 거리를 계산하여 상태 변경
        if (distanceToPlayer > attackRange)
        {
            state = BossState.Move;
        }
        else if (distanceToPlayer < attackRange)
        {
            // 플레이어와 너무 가까울 때는 바로 공격! 
            if (isOnPattern == false) // 실행중인 패턴이 없으면 
            {
                StartCoroutine("Attack");
            }
        }
    }


    private void Move()
    {
        animator.Play("run");
        Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, bossSpeed * Time.deltaTime),
            transform.position.y
        );
        transform.position = newPosition;
        // 이동 중에도 스프라이트가 플레이어를 바라보게 처리
        Mirrored();

        // 사거리 내로 들어오면 공격 상태로 전환
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (isOnPattern == false) // 실행중인 패턴이 없으면 
            {
                StartCoroutine("Attack");
            }
        }
    }

    /*if (플레이어와의 거리가 일정거리 이상이면)
     *몬스터의 상태 Move로 변경
     * state = BossState.Move;
     * 
     * else 아니라면
     * 몬스터의 상태 Attack으로 변경
     * state = BossState.Attack;
    */


    IEnumerator Attack()
    {
        state = BossState.Attack;

        isOnPattern = true;
        bossPatternNum = Random.Range(3, 4);
        Debug.Log($"{bossPatternNum}번 패턴 실행");
        switch (bossPatternNum)
        {
            case 1:
                StartCoroutine("Rush");
                break;
            case 2:
                StartCoroutine("BackStep");
                break;
            case 3:
                StartCoroutine("Slash");
                break;
            case 4:
                StartCoroutine("FireBall");
                break;
            case 5:
                StartCoroutine("MoreSlash");
                break;


        }
        yield return time;
        isOnPattern = false;

    }
    private void Die()
    {
        // hp 전부 소모 시 사망 애니메이션 송출 후 프리펩 소멸

        // 사망 애니메이션 
        // animator.Play();

        // 오브젝트 삭제 처리
        //Destroy(gameObject, 2f);
    }
    // 백스텝(쿨타임있음) , 전방으로 크게 휘두름, 돌진 , 하늘에서 화염구 발사 , 공중에서 돌진하면서 여러번 베기 , 2페이즈 돌입 
    // 모든 패턴에는 대기시간이 필요하니까 코루틴으로 작성하는게 좋을듯!
    IEnumerator Rush()  // 기획에서 함수이름을 bodytacle? 같은거면 좋겠다고 함
    {
        // 플레이어 방향으로 돌진
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        Vector2 startPosition = transform.position;

        // 돌진 거리와 속도 설정
        float targetDistance = 30f;
        float rushSpeed = 70f;
        float currentDistance = 0f;

        // Rigidbody2D에 힘을 가하여 돌진
        bossRigid.AddForce(playerDirection * rushSpeed, ForceMode2D.Impulse);

        // 목표 지점까지 이동
        while (currentDistance < targetDistance)
        {
            currentDistance = Vector2.Distance(transform.position, startPosition);

            // 목표 거리 근처에서만 drag 설정
            if (currentDistance >= targetDistance * 0.9f)
            {
                bossRigid.drag = 2f; // 거리의 90% 도달 시 속도 줄이기
            }

            // 목표 거리에 도달하면 멈춤
            if (currentDistance >= targetDistance)
            {
                bossRigid.velocity = Vector2.zero;
                break;
            }

            yield return null;
        }

        Debug.Log("돌진 완료!");
        bossRigid.drag = 0; // drag 초기화
        yield return time; // 쿨타임 대기

        state = BossState.Idle;
    }
    private IEnumerator BodyTackle()
    {
        // 몸통 박치기 패턴

        // 플레이어 위치 탐색
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        // 돌진 시작 위치
        Vector2 startPosition = transform.position;

        // 애니메이션 재생
        animator.Play("jump");

        // 돌진 목표 거리 설정
        float targetDistance = 30f; // 보스가 이동할 거리 (원하는 값으로 설정)
        float currentDistance = 0f; // 현재 이동거리
        float tackleSpeed = 70f;  // 돌진 속도

        // 돌진 시작
        Debug.Log("돌진 시작---!");

        // while문으로 일정 거리를 돌진
        while (currentDistance < targetDistance)
        {
            // 충돌 무시하고 목표 위치로 이동
            bossRigid.MovePosition(bossRigid.position + playerDirection * tackleSpeed * Time.deltaTime);
            currentDistance = Vector2.Distance(transform.position, startPosition);

            if (currentDistance >= targetDistance)
            {
                bossRigid.velocity = Vector2.zero;
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(3f);
      
        Debug.Log("돌진 끝---!");
        state = BossState.Idle;
    }

    IEnumerator BackStep()
    {

        // 백스텝에 필요한 행동 작성 
        Debug.Log("백스텝!");
        yield return time; // 쿨타임 만큼 기다렸다가 다른 패턴이 동작할수 있게


        state = BossState.Idle;
    }
    IEnumerator Slash()
    {

        //필요한 행동 작성 
        GameObject obj = Instantiate(slashPrefab, slashPoint.position, slashPoint.rotation);
        Destroy(obj,0.5f);
        Debug.Log("슬래쉬!");
        yield return time; // 쿨타임 만큼 기다렸다가 다른 패턴이 동작할수 있게



        state = BossState.Idle;
    }
    IEnumerator FireBall()
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;

        transform.position = new Vector2(transform.position.x, transform.position.y + 15f);  // 리지드바디로 올려버리니까 좀 어색함 + 화면 밖 벗어날 텐데 ?  , 
        animator.Play("jump");
        yield return time;
        GameObject obj1 = Instantiate(fireballPrefab, slashPoint.position, slashPoint.rotation);
        Rigidbody2D obj1rb = obj1.GetComponent<Rigidbody2D>();
        obj1rb.AddForce(Vector2.left*10f,ForceMode2D.Impulse);
        Destroy(obj1, 2f);
        //GameObject obj2 = Instantiate(fireballPrefab, slashPoint.position, slashPoint.rotation);
        //Rigidbody2D obj2rb = obj2.GetComponent<Rigidbody2D>();
        //obj2rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
        //Destroy(obj2, 2f);
        //GameObject obj3 = Instantiate(fireballPrefab, slashPoint.position, slashPoint.rotation);
        //Rigidbody2D obj3rb = obj3.GetComponent<Rigidbody2D>();
        //obj3rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
        //Destroy(obj3, 2f);

        // 점프하고 화염구 발사 
        Debug.Log("화염구!");
        yield return time; // 쿨타임 만큼 기다렸다가 다른 패턴이 동작할수 있게 , 대기시간이 짧으면 공중에서 move로 바뀌니까 공중공격 같은거는 대기시간이 좀더 길어야 할듯 



        state = BossState.Idle;
    }
    IEnumerator MoreSlash()
    {


        // 백스텝에 필요한 행동 작성 
        Debug.Log("공중베기!");
        yield return time; // 쿨타임 만큼 기다렸다가 다른 패턴이 동작할수 있게



        state = BossState.Idle;
    }



    // 보스에게 데미지를 주려면 BossPattern bossPattern = boss.GetComponent<BossPattern>();
    // bossPattern.TakeDamage(데미지);로 데미지를 줄 수 있음 , 이벤트로 호출하면 될듯
    public void TakeDamage(float damage)
    {
        bossHP -= damage;

        // 보스의 체력이 0 이하가 되면 상태를 Die로 변경
        if (bossHP <= 0)
        {
            state = BossState.Die;
        }
    }

    private void Mirrored()
    {
        // 플레이어가 보스의 왼쪽에 있으면 보스를 왼쪽으로, 오른쪽에 있으면 오른쪽을 바라보게 설정
        if (player.transform.position.x < transform.position.x)
        {
            // 보스가 왼쪽을 바라보도록 함
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // 보스가 오른쪽을 바라보도록 함
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
