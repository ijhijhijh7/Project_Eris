using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss02_1P : MonoBehaviour
{
    enum BossState
    {
        Idle, Attack, Die, win
    }

    // ���� ���� ������Ʈ
    [SerializeField] GameObject bossObject;
    // �÷��̾� ������
    [SerializeField] GameObject player;
    // ������ Rigidbody
    [SerializeField] Rigidbody2D bossRigid;
    // ���� �ִϸ��̼� 
    [SerializeField] Animator bossAnimator;
    // SwordAura �˱� ������
    [SerializeField] GameObject swordAura;
    // Bash ������ ����
    [SerializeField] GameObject bash;
    // FootWork ������ ª����
    [SerializeField] GameObject footWarkPre01;
    // FootWork ������ ���
    [SerializeField] GameObject footWarkPre02;
    // SwordSlash ������
    [SerializeField] GameObject SwordSlashPre;
    // ���� ���� ���� bool
    private bool skillStart = false;
    // SwordAura �˱� ���� ��ǥ
    [SerializeField] Transform swordAuraPoint;
    // �ʿ��� 1 ��ǥ
    [SerializeField] Transform bosswarp01;
    // �ʿ��� 2 ��ǥ
    [SerializeField] Transform bosswarp02;
    // �ʿ��� 3 ��ǥ
    [SerializeField] Transform bosswarp03;
    
    // ���� ����
    // ���� HP
    [SerializeField] float bossHP = 100;
    // ���� ���� HP
    float bossNowHP;
    // ���� �ٰŸ� ����
    [SerializeField] float attackRange;
    // ���� ���ǵ�
    [SerializeField] float bossSpeed;
    
    // ���� ���� ���� ����
    int bossPatternNum;
    // ���� ���� ����
    int bosscount;

    // ���¸� Idle�� ����
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
                // ���� �߿��� �ٸ� ������ ���� �ʵ��� ��
                break;
            case BossState.Die:
                Die();
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

        // ��� �ִϸ��̼�
        bossAnimator.Play("boss2 idle");
        // �÷��̾� ��ġ�� �ٶ󺸰�        
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
        // �÷��̾� �Ÿ� ���
        float playerDirection = Vector2.Distance(transform.position, player.transform.position);

        // �Ÿ��� ���� ������ ���� ��ȭ
        if (playerDirection <= attackRange)
        {
            // Debug.Log($"ª�� �Ÿ�");
            // 1,2 �� ����
            bossPatternNum = 1;
        }
        else if (playerDirection > attackRange)
        {
            bossPatternNum = 2;
        }
        if (bosscount == 3)
        {
            bossPatternNum = 2;
            bosscount = 0;
        }

        StartCoroutine(ExecuteAttackPattern());
    }
    
    private IEnumerator ExecuteAttackPattern()
    {
        skillStart = true;
        yield return new WaitForSeconds(2f);
        // ���� ����

        Mirrored();
        switch (bossPatternNum)
        {
            case 1:
                yield return StartCoroutine(Bash());
                Debug.Log("���� ����");
                break;
            case 2:
                yield return StartCoroutine(FootWork());
                Debug.Log("�ߵ� ����");
                break;
        }
        skillStart = false;
        state = BossState.Idle;
    }

    private IEnumerator Bash()
    {
        // �Ϲݺ���
        bosscount += 1;
        Debug.Log("�������!");
        // ���� �ִϸ��̼�
        bossAnimator.Play("boss1_attack2");
        yield return new WaitForSeconds(2f);
        // ����Ʈ ������ ����
        GameObject swordSopn01 = Instantiate(bash, swordAuraPoint.position, swordAuraPoint.rotation);
        yield return new WaitForSeconds(0.4f);
        GameObject swordSopn02 = Instantiate(bash, swordAuraPoint.position, swordAuraPoint.rotation * Quaternion.Euler(0, 0, 90));
        yield return new WaitForSeconds(1.5f);
        bossAnimator.Play("boss2 idle");
    }
    private IEnumerator FootWork()
    {
        // �ߵ�
        // �־�� �Ǵ� ��ǥ bosswarp01, bosswarp02, bosswarp03
        // 1,2 �߰��� 2,3 �߰��� �� ���� ������(FootWorkEffect01), 1�� 3���̿� �� �Ϲ� ���⺸�� �ι� �� ���� ������(FootWorkEffect02)
        // 1,2,3 �� ������ ����
        Mirrored();
        bossAnimator.Play("boss1_attack1");
        yield return new WaitForSeconds(1.5f);
        // �˻̴� �ִϸ��̼� ����
        Mirrored();
        yield return new WaitForSeconds(0.5f);
        // ���� ���� ��ġ�� Ȯ���ϰ� �÷��̾��� ��ġ�� ���� �����̵�
        // ���� �� ��ġ�� bosswarp01.position.x ��ó�϶�
        if (Mathf.Abs(transform.position.x - bosswarp01.position.x) <= 10f)
        {
            if (player.transform.position.x > bosswarp01.position.x && player.transform.position.x < bosswarp02.position.x)
            {
                // ������ bosswarp01�� �ְ� �÷��̾ 1,2 ���̿� ���� �� bosswarp02�� �̵�
                Debug.Log("011");
                transform.position = bosswarp02.position;
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp01.position + bosswarp02.position) / 2, Quaternion.identity);
            }
            else if (player.transform.position.x > bosswarp02.position.x && player.transform.position.x < bosswarp03.position.x)
            {
                Debug.Log("012");
                // ������ bosswarp01�� �ְ� �÷��̾ 2,3 ���̿� ���� �� bosswarp03���� �̵�
                transform.position = bosswarp03.position;
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre02, bosswarp02.position, Quaternion.identity);
            }
        }
        else if (Mathf.Abs(transform.position.x - bosswarp02.position.x) <= 10f)
        {
            if (player.transform.position.x > bosswarp01.position.x && player.transform.position.x < bosswarp02.position.x)
            {
                Debug.Log("021");
                // ������ bosswarp02�� �ְ� �÷��̾ 1,2 ���̿� ���� �� bosswarp01�� �̵�
                transform.position = bosswarp01.position;
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp01.position + bosswarp02.position) / 2, Quaternion.identity);
            }
            else if (player.transform.position.x > bosswarp02.position.x && player.transform.position.x < bosswarp03.position.x)
            {
                Debug.Log("022");
                // ������ bosswarp02�� �ְ� �÷��̾ 2,3 ���̿� ���� �� bosswarp03���� �̵�
                transform.position = bosswarp03.position;
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp02.position + bosswarp03.position) / 2, Quaternion.identity);
            }
        }
        else if (Mathf.Abs(transform.position.x - bosswarp03.position.x) <= 10f)
        {
            if (player.transform.position.x > bosswarp02.position.x && player.transform.position.x < bosswarp03.position.x)
            {
                Debug.Log("031");
                // ������ bosswarp03�� �ְ� �÷��̾ 2,3 ���̿� ���� �� bosswarp02�� �̵�
                transform.position = bosswarp02.position;
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre01, (bosswarp02.position + bosswarp03.position) / 2, Quaternion.identity);
            }
            else if (player.transform.position.x > bosswarp01.position.x && player.transform.position.x < bosswarp02.position.x)
            {
                Debug.Log("032");
                // ������ bosswarp03�� �ְ� �÷��̾ 1,2 ���̿� ���� �� bosswarp01�� �̵�
                transform.position = bosswarp01.position;
                yield return new WaitForSeconds(0.7f);
                Instantiate(footWarkPre02, bosswarp02.position, Quaternion.identity);
            }
        }
        // �� �ִ� �ִϸ��̼� ����
        yield return new WaitForSeconds(3f);
        Debug.Log("��");
    }

    private IEnumerator SkySwordAura()
    {
        float bossJumpPower = 110f;

        // animator.Play("���� ���� �ִϸ��̼�");     
        bossAnimator.Play("boss1_attack3");
        yield return new WaitForSeconds(1f);
        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.velocity = Vector2.zero;
        // ������ ���� �ö�
        bossRigid.AddForce(Vector2.up * bossJumpPower, ForceMode2D.Impulse);
        // ������ ������ ����
        GameObject swordSopn = Instantiate(SwordSlashPre, swordAuraPoint.position, swordAuraPoint.rotation);

        // �ִϸ��̼� �� �ö󰡴� �ð��� ���� ���ð�
        yield return new WaitForSeconds(0.2f);
        // ������ ��ġ ����
        bossRigid.velocity = Vector2.zero;
        bossRigid.bodyType = RigidbodyType2D.Kinematic;
        bossAnimator.speed = 0.5f;
        yield return new WaitForSeconds(2.2f);
        // ������ �Ѹ��� �˱� ����
        for (int i = 0; i < 6; i++)
        {
            Mirrored();
            SwordAuraSpon();
            yield return new WaitForSeconds(0.4f);
        }
        bossAnimator.speed = 1f;
        bossRigid.bodyType = RigidbodyType2D.Dynamic;
        bossRigid.gravityScale = 10;
        yield return new WaitForSeconds(0.7f);
        bossAnimator.Play("boss2 idle");
        yield return new WaitForSeconds(2f);
        bossRigid.gravityScale = 1;
    }
    public void TakeDamage(float damage)
    {
        bossNowHP -= damage;
        Debug.Log($"���� ü�� : {bossNowHP}");
        // ������ ü���� 0 ���ϰ� �Ǹ� ���¸� Die�� ����
        if (bossNowHP <= 0)
        {
            state = BossState.Die;
        }
    }
    private void Die()
    {
        // hp ���� �Ҹ� �� ��� �ִϸ��̼� ���� �� ������ �Ҹ�
        
        // ��� �ִϸ��̼� 
        bossAnimator.Play("boss2 die");

        // ������Ʈ ���� ó��
        Destroy(gameObject, 4f);
        // 1���� 1������ �߰� ��ȭ �� �ҷ�����
        SceneManager.LoadScene("Boss1DPhase");
        
    }
    private void Win()
    {
        // �÷��̾��� HP�� 0�̵Ǿ��ų� ���°� DIE�� �Ǿ�����

        // �¸� �ִϸ��̼�
        bossAnimator.Play("boss1 2 win");

        // �κ� �̵�
        //SceneManager.LoadScene("");
    }
    // �¿����
    private void Mirrored()
    {
        // �÷��̾ ������ ���ʿ� ������ ������ ��������, �����ʿ� ������ �������� �ٶ󺸰� ����
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
        // �÷��̾ ������ ���ʿ� ������ ������ ��������, �����ʿ� ������ �������� �ٶ󺸰� ����
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

        // �÷��̾ ������ �¿� �ִ��� �쿡 �ִ��� �Ǵ�
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