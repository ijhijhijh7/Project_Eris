using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float destorytime;
    [SerializeField] float effectDamage;
    private bool spendDamage = false;
    private void Start()
    {
        Destroy(gameObject, destorytime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spendDamage)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerRPG playerRPG = collision.GetComponent<PlayerRPG>();
                if (playerRPG == null)
                {
                    Debug.Log("안들어옴");
                    return;
                }
                // 데미지 안 받았다면

                // 플레이어에게 데미지를 주는 로직
                playerRPG.TakeDamage(effectDamage);
                Debug.Log($"플레이어에게 {effectDamage} 데미지를 입혔습니다.");

                // 한번만 데미지를 주기 위해 spendDamage로 데미지 판정
                spendDamage = true;
            }
        }
    }
}
