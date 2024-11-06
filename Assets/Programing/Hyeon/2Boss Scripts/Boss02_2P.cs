using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss02_2P : MonoBehaviour
{
    enum BossState
    {
        Idle, Attack, Die, win
    }

    // 보스 게임 오브젝트
    [SerializeField] GameObject bossObject;
    // 플레이어 프리펩
    [SerializeField] GameObject player;
    // 보스의 Rigidbody
    [SerializeField] Rigidbody2D bossRigid;
    // 보스 애니메이션 
    [SerializeField] Animator bossAnimator;
    // SwordAura 검기 프리펩
    [SerializeField] GameObject swordAura;
    // Bash 프리펩 생성
    [SerializeField] GameObject bash;
    // FootWork 프리펩 짧은거
    [SerializeField] GameObject footWarkPre01;
    // FootWork 프리펩 긴거
    [SerializeField] GameObject footWarkPre02;
    // SwordSlash 프리펩
    [SerializeField] GameObject SwordSlashPre;
    // 패턴 시작 판정 bool
    private bool skillStart = false;
    // 사망 판정
    private bool isDie = false;
    // SwordAura 검기 생성 좌표
    [SerializeField] Transform swordAuraPoint;
    // 맵에서 1 좌표
    [SerializeField] Transform bosswarp01;
    // 맵에서 2 좌표
    [SerializeField] Transform bosswarp02;
    // 맵에서 3 좌표
    [SerializeField] Transform bosswarp03;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] bosssound;
    // 보스 스탯
    // 보스 HP
    [SerializeField] float bossHP = 100;
    // 보스 현재 HP
    float bossNowHP;
    // 보스 근거리 공격
    [SerializeField] float attackRange;
    // 보스 스피드
    [SerializeField] float bossSpeed;
    
    // 보스 몬스터 패턴 선택
    int bossPatternNum;
    // 보스 패턴 스택
    int bosscount;

    // 상태를 Idle로 셋팅
    BossState state = BossState.Idle;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        bossRigid = GetComponent<Rigidbody2D>();
        bossNowHP = bossHP;
        transform.position = bosswarp03.position;
        Mirrored();
    }

    private void Update()
    {
        switch (state)
        {
            case BossState.Idle:
                Idle();
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
        skillStart = false;

        // 대기 애니메이션
        bossAnimator.Play("boss2 idle");
        // 플레이어 위치를 바라보게        
        Mirrored();

        if (!skillStart)
        {
            WaitSkill();           
        }
    }

    private void WaitSkill()
    {
        skillStart = true;
        state = BossState.Attack;
        // 플레이어 거리 계산
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // 거리에 따라 보스의 패턴 분화
        if (playerDirection <= attackRange)
        {
            // Debug.Log($"짧은 거리");
            // 1,2 중 랜덤
            bossPatternNum = 1;
        }
        else if (playerDirection > attackRange)
        {
            bossPatternNum = Random.Range(2, 4);
        }
        // 근거리 공격 3스택이면 원거리 공격 진행
        if (bosscount == 3)
        {
            bossPatternNum = Random.Range(2, 4);
            bosscount = 0;
        }

        StartCoroutine(ExecuteAttackPattern());
    }
    
    private IEnumerator ExecuteAttackPattern()
    {
        skillStart = true;
        yield return new WaitForSeconds(1f);
        // 공격 상태

        Mirrored();
        switch (bossPatternNum)
        {
            case 1:
                yield return StartCoroutine(Bash());
                Debug.Log("베기 패턴");
                break;
            case 2:
                yield return StartCoroutine(FootWork());
                Debug.Log("발도 패턴");
                break;
            case 3:
                yield return StartCoroutine(SkySwordAura());
                Debug.Log("공중 검기 날리기");
                break;
        }
        skillStart = false;
        state = BossState.Idle;
    }

    private IEnumerator Bash()
    {
        // 일반베기
        bosscount += 1;
        Debug.Log("베어가르기!");
        // 베는 애니메이션
        bossAnimator.Play("boss1_attack2");
        yield return new WaitForSeconds(1.5f);
        // 이펙트 프리펩 생성
        GameObject swordSopn01 = Instantiate(bash, swordAuraPoint.position, swordAuraPoint.rotation);
        audioSource.PlayOneShot(bosssound[1]);
        yield return new WaitForSeconds(0.4f);
        audioSource.PlayOneShot(bosssound[1]);
        GameObject swordSopn02 = Instantiate(bash, swordAuraPoint.position, swordAuraPoint.rotation * Quaternion.Euler(0, 0, 90));
        yield return new WaitForSeconds(1f);
        bossAnimator.Play("boss2 idle");
    }
    private IEnumerator FootWork()
    {
        // 발도
        // 있어야 되는 좌표 bosswarp01, bosswarp02, bosswarp03
        // 1,2 중간과 2,3 중간에 들어갈 베기 프리펩(FootWorkEffect01), 1과 3사이에 들어갈 일반 베기보다 두배 긴 베기 프리펩(FootWorkEffect02)
        // 1,2,3 세 구간이 있음
        Mirrored();
        // 검뽑는 애니메이션 실행
        bossAnimator.Play("boss1_attack1");
        yield return new WaitForSeconds(1f);
        Mirrored();
        yield return new WaitForSeconds(0.5f);
        // 현재 보스 위치를 확인하고 플레이어의 위치에 따라 순간이동
        // 보스 의 위치가 bosswarp01.position.x 근처일때
        if (Mathf.Abs(transform.position.x - bosswarp01.position.x) <= 10f)
        {
            if (player.transform.position.x > bosswarp01.position.x && player.transform.position.x < bosswarp02.position.x)
            {
                // 보스가 bosswarp01에 있고 플레이어가 1,2 사이에 있을 때 bosswarp02로 이동
                Debug.Log("011");
                transform.position = bosswarp02.position;
                audioSource.PlayOneShot(bosssound[0]);
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp01.position + bosswarp02.position) / 2, Quaternion.identity);
            }
            else if (player.transform.position.x > bosswarp02.position.x && player.transform.position.x < bosswarp03.position.x)
            {
                Debug.Log("012");
                // 보스가 bosswarp01에 있고 플레이어가 2,3 사이에 있을 때 bosswarp03으로 이동
                transform.position = bosswarp03.position;
                audioSource.PlayOneShot(bosssound[0]);
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre02, bosswarp02.position, Quaternion.identity);
            }
        }
        else if (Mathf.Abs(transform.position.x - bosswarp02.position.x) <= 10f)
        {
            if (player.transform.position.x > bosswarp01.position.x && player.transform.position.x < bosswarp02.position.x)
            {
                Debug.Log("021");
                // 보스가 bosswarp02에 있고 플레이어가 1,2 사이에 있을 때 bosswarp01로 이동
                transform.position = bosswarp01.position;
                audioSource.PlayOneShot(bosssound[0]);
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp01.position + bosswarp02.position) / 2, Quaternion.identity);
            }
            else if (player.transform.position.x > bosswarp02.position.x && player.transform.position.x < bosswarp03.position.x)
            {
                Debug.Log("022");
                // 보스가 bosswarp02에 있고 플레이어가 2,3 사이에 있을 때 bosswarp03으로 이동
                transform.position = bosswarp03.position;
                audioSource.PlayOneShot(bosssound[0]);
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp02.position + bosswarp03.position) / 2, Quaternion.identity);
            }
        }
        else if (Mathf.Abs(transform.position.x - bosswarp03.position.x) <= 10f)
        {
            if (player.transform.position.x > bosswarp02.position.x && player.transform.position.x < bosswarp03.position.x)
            {
                Debug.Log("031");
                // 보스가 bosswarp03에 있고 플레이어가 2,3 사이에 있을 때 bosswarp02로 이동
                transform.position = bosswarp02.position;
                audioSource.PlayOneShot(bosssound[0]);
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp02.position + bosswarp03.position) / 2, Quaternion.identity);
            }
            else if (player.transform.position.x > bosswarp01.position.x && player.transform.position.x < bosswarp02.position.x)
            {
                Debug.Log("032");
                // 보스가 bosswarp03에 있고 플레이어가 1,2 사이에 있을 때 bosswarp01로 이동
                transform.position = bosswarp01.position;
                audioSource.PlayOneShot(bosssound[0]);
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre02, bosswarp02.position, Quaternion.identity);
            }
        }
        // 검 넣는 애니메이션 실행
        yield return new WaitForSeconds(2f);
        Debug.Log("끝");
    }

    private IEnumerator SkySwordAura()
    {
        float bossJumpPower = 200f;

        // animator.Play("공중 점프 애니메이션");     
        bossAnimator.Play("boss1_attack3");
        yield return new WaitForSeconds(1.2f);
        audioSource.PlayOneShot(bosssound[1]);
        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.velocity = Vector2.zero;
        // 보스가 위로 올라감
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        // 슬레쉬 프리펩 생성
        GameObject swordSopn = Instantiate(SwordSlashPre, swordAuraPoint.position, swordAuraPoint.rotation);

        // 애니메이션 및 올라가는 시간을 위한 대기시간
        yield return new WaitForSeconds(0.2f);
        // 보스의 위치 고정
        bossRigid.velocity = Vector2.zero;
        bossRigid.bodyType = RigidbodyType2D.Kinematic;
        bossAnimator.speed = 0.5f;
        yield return new WaitForSeconds(2f);
        // 보스가 뿌리는 검기 생성
        for (int i = 0; i < 6; i++)
        {
            Mirrored();
            SwordAuraSpon();
            audioSource.PlayOneShot(bosssound[2]);
            yield return new WaitForSeconds(0.4f);
        }
        bossAnimator.speed = 1f;
        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.gravityScale = 10;
        yield return new WaitForSeconds(0.7f);
        bossAnimator.Play("boss2 idle");
        yield return new WaitForSeconds(1f);
        bossRigid.gravityScale = 1;
    }
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
    private IEnumerator Die()
    {
        isDie = true;
        // hp 전부 소모 시 사망 애니메이션 송출 후 프리펩 소멸
        DataManager.Instance.LoadGameData();
        // 사망 애니메이션 
        bossAnimator.Play("boss2 die");
        DataManager.Instance.data.isUnlock[0] = true;
        DataManager.Instance.SaveGameData();
        // 오브젝트 삭제 처리
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Boss1DEnd");
        // 스킬 언락
        // 1보스 엔드 씬 불러오기       
    }

    private void Win()
    {
        // 플레이어의 HP가 0이되었거나 상태가 DIE가 되었을때

        // 승리 애니메이션
        bossAnimator.Play("boss1 2 win");
        // 로비
        //SceneManager.LoadScene("");
    }
    // 좌우반전
    private void Mirrored()
    {
        // 플레이어가 보스의 왼쪽에 있으면 보스를 왼쪽으로, 오른쪽에 있으면 오른쪽을 바라보게 설정
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            bossObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            bossObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void MinusMirrored()
    {
        // 플레이어가 보스의 왼쪽에 있으면 보스를 왼쪽으로, 오른쪽에 있으면 오른쪽을 바라보게 설정
        if (player.transform.position.x < bossObject.transform.position.x)
        {
            bossObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            bossObject.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }
    public void SwordAuraSpon()
    {     
        GameObject swordSopn = Instantiate(swordAura, swordAuraPoint.position, Quaternion.identity);
        swordSopn.transform.LookAt(player.transform);
        SwordAura type = swordSopn.GetComponentInChildren<SwordAura>();

        // 플레이어가 보스의 좌에 있는지 우에 있는지 판단
        if (player.transform.position.x < swordAuraPoint.transform.position.x)
        {
            type.direction = -1;
        }
        else if (player.transform.position.x > swordAuraPoint.transform.position.x)
        {
            type.direction = 1;
        }
        else
        {
            type.direction = 0;
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
