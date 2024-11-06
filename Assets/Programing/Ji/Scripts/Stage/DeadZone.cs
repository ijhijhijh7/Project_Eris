using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] PlayerController playerController; // 플레이어의 스크립트에 따라 다르게 변경할 것
    [SerializeField] PlayerRPG playerRPG;
    /// <summary>
    /// DeadZone에 trigger로 판정
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /* tag가 DeadZone인 경우 
             * 플레이어의 사망
             * 플레이어가 가지고 있는 사망함수를 가져오기위해
             * collision.플레이어cs로 플레이어의 컴포넌트를 선언하고 사망함수 가져오기
             */
            playerRPG.TakeDamage(playerRPG.maxHp);
            //playerController.Die();
        }
    }
}
