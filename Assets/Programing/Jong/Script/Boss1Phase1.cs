using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Phase1 : MonoBehaviour
{

    // 지상 상태와 공중상태 2가지 지상에서 2패턴+ 백스텝  공중에서 2패턴 한쪽패턴에서 3번쓰면 다른패턴으로 전환
    // 지상일때 거리체크랑 공중일때 거리체크 및 이동방식이 바뀌어야 함 

    //백스텝(쿨타임있음) , 전방으로 크게 휘두름, 돌진 , 하늘에서 화염구 발사 , 공중에서 돌진하면서 여러번 베기, 2페이즈 돌입
    // 보스는 계속 플레이어를 추적하니까 일단 아이들은 필요 없을 듯?
    enum BossState
    {
        Flying, Walk, Attack, Die
    }
    // 보스 애니메이션
    [SerializeField] Animator animator;

    [SerializeField] GameObject player;

    [SerializeField] Rigidbody2D bossRigidbody;
    [SerializeField] GameObject RushCollider;

    // 보스 스탯  : 체력 이동속도 
    [SerializeField] int stateCount = 0;  // fly 나 walk 상태에서 카운트가 3이 되면(3번 오르면) 상태를 변경할때 쓸것 , 패턴을 실행 할 때 마다 하나씩 증가

    [SerializeField] float bossHP = 10;
    [SerializeField] float toPlayerDistance; // 보스와 플레이어의 거리 
    [SerializeField] float walkAttackRange;  // 지상사거리 

    [SerializeField] float flyAttackRange;  // 공중사거리 

    [SerializeField] float walkbossSpeed;    // 이동속도 
    [SerializeField] float flybossSpeed;    // 공중 이동속도 
    private bool isPatternOn = false; // 패턴중이면 
    private BossState preState; // 공격 진입하기 전의 상태 저장용
    private bool isflying = false;  // 공중 확인용 

    //공격 패턴용 필드
    [SerializeField] Transform atkPoint;
   // [SerializeField] Transform slashPoint;
    
    [SerializeField] GameObject slashPrefap2;

    [SerializeField] GameObject fireBallPrefab;
    [SerializeField] Transform[] fireBallPoints;

    [SerializeField] Transform forkPoint;
    [SerializeField] GameObject forkPrefab;

    [SerializeField] GameObject RtacklePrefab;
    [SerializeField] GameObject LtacklePrefab;
    [SerializeField] GameObject slash;


    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    Coroutine curCoroutine;
    BossState state = BossState.Walk;
    [SerializeField] BossState curBossState; // 보스의 현재 상태 확인용 
    // 보스에게 데미지를 주려면 BossPattern bossPattern = boss.GetComponent<BossPattern>();
    // bossPattern.TakeDamage(데미지);로 데미지를 줄 수 있음 , 이벤트로 호출하면 될듯

    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("BossDo");
        curBossState = state;
    }

    private void Update()
    {
        if (bossHP <= 0) 
        {
            StopCoroutine(BossDo());
            state = BossState.Die; 
            curBossState = state;
            StartCoroutine(Died());
              // 사망 
        }
    }
    IEnumerator BossDo()
    {

        // WaitForSeconds time = new WaitForSeconds(0.1f);  // 1초에 80번 호출
        curCoroutine = StartCoroutine(Walk());
        while (true)
        {

            toPlayerDistance = Vector2.Distance(player.transform.position, transform.position);

            if (curBossState != state)
            {
                curBossState = state;

                // 현재 실행 중인 코루틴이 있으면 정지
                if (curCoroutine != null)
                {
                    StopCoroutine(curCoroutine);
                }

                // 새로운 상태에 맞는 코루틴을 시작
                switch (state)
                {
                    case BossState.Walk:
                        curCoroutine = StartCoroutine(Walk());
                        break;
                    case BossState.Flying:
                        curCoroutine = StartCoroutine(Flying());
                        break;
                    case BossState.Attack:
                        curCoroutine = StartCoroutine(Attack());
                        break;
                    case BossState.Die:
                        // Die();
                        break;
                }
            }

            yield return null;
        }
    }


    IEnumerator Flying()
    {
        isflying = true;
        bossRigidbody.gravityScale = 0f; // 공중에서 공격할  땐 이동이 호출되지 않으니까 중력을 0으로 만들어서 계속 떠있게 함 
        // 지금 코드는 너무 휙 하고 올라가서 좀 별로 올라갈때 애니메이션이나 이펙트가 있으면 좀 나을듯 
        if (stateCount >= 3)
        {
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(Fork());
            yield return new WaitForSeconds(1.5f);
            stateCount = 0;
            state = BossState.Walk;
            Debug.Log("상태전환");
           
        }
        animator.Play("Fly");
        audioSource.clip = audioClips[8];
        audioSource.Play();
        audioSource.loop = true;
        while (state == BossState.Flying)
        {
            preState = curBossState;
            Mirrored();
           
            Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(transform.position.x, player.transform.position.x, flybossSpeed * Time.deltaTime),
            10f
            );
            transform.position = newPosition;


            if (toPlayerDistance <= flyAttackRange)
            {
                if (isPatternOn == false)
                {
                    preState = curBossState;
                    state = BossState.Attack;
                    stateCount++;
                }
            }


            yield return null;
        }


    }

    IEnumerator Walk()
    {
        isflying = false;
        bossRigidbody.gravityScale = 1f;
        if (stateCount >= 3)
        {
            animator.Play("Jump");
            audioSource.clip = audioClips[5];
            audioSource.Play();
            yield return new WaitForSeconds(1.2f);
            stateCount = 0;
            state = BossState.Flying;
            Debug.Log("상태전환");

        }
        audioSource.clip = audioClips[7];
        audioSource.Play();
        audioSource.loop = true;
        animator.Play("Walk");
        while (state == BossState.Walk)
        {
           
            preState = curBossState;
            Mirrored();
            Vector2 newPosition = new Vector2(
                Mathf.MoveTowards(transform.position.x, player.transform.position.x, walkbossSpeed * Time.deltaTime),
                transform.position.y
            );
            transform.position = newPosition;


            // 지상용 패턴 넣기   공중에서 지상 내려올때 패턴있으면 더 재밌을듯 ,공격 패턴이 실행되면 이동 루틴은 정지 해야할듯
            if (toPlayerDistance <= walkAttackRange)
            {
                if (isPatternOn == false)
                {
                    preState = state;
                    state = BossState.Attack;
                    stateCount++;
                }

            }


            yield return null;
        }

    }

    //백스텝(쿨타임있음) , 전방으로 크게 휘두름, 돌진 , 하늘에서 화염구 발사 , 공중에서 돌진하면서 여러번 베기, 2페이즈 돌입
    IEnumerator Attack()
    {
        audioSource.loop = false;
        WaitForSeconds time = new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(1f);
        if (isflying == false)
        {
            animator.Play("idle");
        }
        else if (isflying == true) 
        {
            animator.Play("FlyIdle");
        }  
        if (isflying == false) // 지상 패턴
        {
            if (isPatternOn == false)
            {
                int x = Random.Range(0, 3);
                switch (x)
                {
                    case 0:
                        StartCoroutine(BackStep());
                        break;
                    case 1:
                        StartCoroutine(Slash());
                        break;
                    case 2:
                        StartCoroutine(BodyTacle());
                        break;
                }
            }
        }
        else if (isflying == true) // 공중 패턴 
        {
            if (isPatternOn == false)
            {
                int y = Random.Range(0, 2);
                switch (y)
                {
                    case 0:
                        StartCoroutine(FireBall());
                        break;
                    case 1:
                        StartCoroutine(RushSlash());
                        break;
                }
            }
        }


        yield return time; // 스킬간의 텀 , 스킬의 실행 시간을 보장해줄 정도로 길어야 함 

        state = preState;  // 공격 이전의 상태로 돌아감 
        
    }
    IEnumerator BackStep()
    {
        int x = Random.Range(0, 2);
        Debug.Log(x);
        if (x == 0)
        {
            isPatternOn = true;
            animator.Play("BackStep");
            Debug.Log("백스텝");
            audioSource.clip = audioClips[9];
            audioSource.Play();
            yield return new WaitForSeconds(0.25f);
            if (player.transform.position.x < transform.position.x) // 플레이어의 반대 방향으로 날아감 
            {

                bossRigidbody.AddForce(Vector2.right * 50f, ForceMode2D.Impulse);
            }
            else
            {

                bossRigidbody.AddForce(Vector2.left * 50f, ForceMode2D.Impulse);
            }


            yield return new WaitForSeconds(0.25f);

            bossRigidbody.velocity = Vector2.zero; // 너무 안밀리게 속도 없앰 
            isPatternOn = false;
            yield return new WaitForSeconds(0.3f);
        }
        else if (x == 1) 
        {
            isPatternOn = true;
            animator.Play("Atk1");
            audioSource.clip = audioClips[0];

            yield return new WaitForSeconds(0.9f);
            audioSource.Play();
            slash.SetActive(true);
            Debug.Log("베기");
            yield return new WaitForSeconds(0.4f);
            slash.SetActive(false);
            yield return new WaitForSeconds(1f);

            isPatternOn = false;
        }
       
    }
    IEnumerator Slash()
    {
        isPatternOn = true;
        animator.Play("Atk1");
        audioSource.clip = audioClips[0];
        
        yield return new WaitForSeconds(0.9f);
        audioSource.Play();
        slash.SetActive(true);
        Debug.Log("베기");
        yield return new WaitForSeconds(0.4f);
        slash.SetActive(false);
        yield return new WaitForSeconds(1f);
        
        
        isPatternOn = false;
    }
    IEnumerator BodyTacle()
    {
        bool dir = false;
        if (player.transform.position.x < transform.position.x)
        {
            dir = true;
        }
        else
        {
            dir = false;
        }
        isPatternOn = true;
        animator.Play("Atk2");
        RushCollider.SetActive(true);
        Debug.Log("돌진");
        yield return new WaitForSeconds(1f);
        if (dir == true) // 플레이어의 방향으로 날아감 
        {

            audioSource.clip = audioClips[1];
            audioSource.Play();
            LtacklePrefab.SetActive(true);
            bossRigidbody.AddForce(Vector2.left * 150f, ForceMode2D.Impulse);
        }
        else
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
            RtacklePrefab.SetActive(true);
            bossRigidbody.AddForce(Vector2.right * 150f, ForceMode2D.Impulse);

        }


        yield return new WaitForSeconds(0.3f);
        RtacklePrefab.SetActive(false);
        LtacklePrefab.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        bossRigidbody.velocity = Vector2.zero; // 너무 안밀리게 속도 없앰 
        RushCollider.SetActive(false);

        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator FireBall()
    {
        isPatternOn = true;
        animator.Play("Atk3");
        yield return new WaitForSeconds(1f);
        audioSource.clip = audioClips[2];
        audioSource.Play();
        Debug.Log("화염구");
        if (player.transform.position.x < transform.position.x)
        {
            for (int i = 0; i < fireBallPoints.Length; i++)
            {
                GameObject obj = Instantiate(fireBallPrefab, fireBallPoints[i].position, fireBallPoints[i].rotation);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.left * 50f, ForceMode2D.Impulse);
                Destroy(obj, 2.3f); 
            }

        }
        
        else
        {
            for (int i = 0; i < fireBallPoints.Length; i++)
            {
                GameObject obj = Instantiate(fireBallPrefab, fireBallPoints[i].position, fireBallPoints[i].rotation);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * 50f, ForceMode2D.Impulse);
                Destroy(obj, 2.3f);
            }

        }
        yield return new WaitForSeconds(0.5f);
        isPatternOn = false;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator RushSlash()
    {
        isPatternOn = true;
        animator.Play("Atk4");
        Debug.Log("공중돌진베기");
        yield return new WaitForSeconds(0.3f);
        audioSource.clip = audioClips[3];
        audioSource.Play();
        if (player.transform.position.x < transform.position.x) // 플레이어의 방향으로 날아감 
        {

            for (int i = 0; i < 3; i++) 
            {
                yield return new WaitForSeconds(0.2f);
                bossRigidbody.AddForce(Vector2.left * 30f, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.3f);
                bossRigidbody.velocity = Vector2.zero;
                slashPrefap2.SetActive(true);

                yield return new WaitForSeconds(0.3f);
                slashPrefap2.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                bossRigidbody.AddForce(Vector2.right * 30f, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.3f);
                bossRigidbody.velocity = Vector2.zero;
                slashPrefap2.SetActive(true);

                yield return new WaitForSeconds(0.3f);
                slashPrefap2.SetActive(false);
            }
        }
        yield return new WaitForSeconds(1.5f);
        
       
        isPatternOn = false;

    }
    IEnumerator Fork() 
    {

        audioSource.loop = false;
        Vector2 pos = new Vector2(forkPoint.position.x , 0f);
        isPatternOn = true;
        yield return new WaitForSeconds(1f);
        GameObject obj = Instantiate(forkPrefab, pos, forkPoint.rotation);
        Destroy(obj, 0.8f);

        bossRigidbody.AddForce(Vector2.down * 300f, ForceMode2D.Impulse);
        animator.Play("Landing");
        
        
        

        yield return new WaitForSeconds(0.5f);
       
        bossRigidbody.velocity = Vector2.zero;
        audioSource.clip = audioClips[6];
        audioSource.Play();
        yield return new WaitForSeconds(1.5f);
        
        isPatternOn = false;
        
    }
    // 2페이즈?


    IEnumerator Died() 
    {
        audioSource.loop = false;
        // 사망 애니메이션  , 씬 전환 넣기 
        animator.Play("Change");
        audioSource.clip = audioClips[4];
        audioSource.Play();
        
        yield return new WaitForSeconds(2f);
        GameManager.Instance.LoadSceneByName("Boss2DPhase");
    }
    //private void Die()  // 사망하면 2페이즈로 가게 
    //{
    //    audioSource.loop = false;
    //    // 사망 애니메이션  , 씬 전환 넣기 
    //    animator.Play("Change");
    //    audioSource.clip = audioClips[4];
    //    audioSource.Play();
    //    Destroy(gameObject, 2f);
    //    GameManager.Instance.LoadSceneByName("Boss2DPhase");
    //}

    public void TakeDamage(float damage) // 업데이트나 이벤트로 처리하면 될듯
    {
        bossHP -= damage;

        // 보스의 체력이 0 이하가 되면 상태를 Die로 변경
        if (bossHP <= 0)
        {
            state = BossState.Die;
            StartCoroutine(Died());
        }
    }

    private void Mirrored()
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
