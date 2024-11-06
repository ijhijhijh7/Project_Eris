using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerRPG : MonoBehaviour
{
    [Header("PlayerStat")]
    [SerializeField] public float attackDamage;
    [SerializeField] public float dronDamage;
    [SerializeField] public float curHp;
    [SerializeField] public float maxHp;

    [Header("BossSort")]
    [SerializeField] Boss02_1P Boss1_1;
    [SerializeField] Boss02_2P Boss1_2;
    [SerializeField] Boss1Phase1 Boss2_1;
    [SerializeField] BossPattern Boss2_2;
    [SerializeField] Boss3Controller Boss3;

    private PlayerController playerController;
    private Collision coll;

    private void Awake()
    {
        curHp = maxHp;
        // PlayerController 컴포넌트 참조
        playerController = GetComponent<PlayerController>();
        coll = GetComponent<Collision>();

        Boss1_1 = FindObjectOfType<Boss02_1P>();
        Boss1_2 = FindObjectOfType<Boss02_2P>();
        Boss2_1 = FindObjectOfType<Boss1Phase1>();
        Boss2_2 = FindObjectOfType<BossPattern>();
        Boss3 = FindObjectOfType<Boss3Controller>();
    }

    private void Update()
    {
        StampedeUpdate();
    }

    public void DealDamageToBoss(string bossType, float damage)
    {
        switch (bossType)
        {
            case "Boss1_1":
                Boss1_1.TakeDamage(damage);
                break;
            case "Boss1_2":
                Boss1_2.TakeDamage(damage);
                break;
            case "Boss2_1":
                Boss2_1.TakeDamage(damage);
                break;
            case "Boss2_2":
                Boss2_2.TakeDamage(damage);
                break;
            case "Boss3_1":
                Boss3.TakeDamage(damage);
                break;
            case "Boss3_2":
                Boss3.TakeDamage(damage);
                break;
            default:
                Debug.LogWarning("존재하지 않는 보스입니다.");
                break;
        }
        Debug.Log($"{damage}의 피해를 입혔습니다.");
    }

    public void TakeDamage(float damage)
    {
        curHp -= damage;
        Debug.Log($"현재 체력 : {curHp}");

        // 플레이어의 체력이 0 이하가 되면 상태를 Die로 변경
        if (curHp <= 0)
        {
            playerController.Die();
        }
    }

    public void StampedeUpdate()
    {
        if(coll.onPlatform && coll.onCeiling)
        {
            playerController.Die();
        }
    }
}
