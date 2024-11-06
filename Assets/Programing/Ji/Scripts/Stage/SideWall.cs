using System.Collections;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    // 데미지 확인 필요
    [SerializeField] float poisionDamage; // 독 데미지의 크기
    [SerializeField] float poisionDebuffTime; // 독 데미지 디버프 주기
    // 충돌한 플레이어의 플레이어 컨트롤러를 가져오기위해서 작성
    // 미리 유니티에서 넣어 놓는 것도 합칠때 생각해보는 것이 좋을 것 같음
    // 이 경우 OnCollsionEnter2D에 samplePlayer = collision.gameObject.GetComponent<SamplePlayer>(); 삭제할 것
    [SerializeField] PlayerRPG playerRpg; // 플레이어의 스크립트에 따라 다르게 변경할 것
    [SerializeField] Collision playerCollision; // 플레이어 프리팹에 있는 Collision의 충돌 체크를 불러오기 위해 연동

    bool isDebuff = false; // 디버프의 활성화 여부 설정

    private void Update()
    {
        if (playerCollision.onWall)
        {
            isDebuff = true;
            StartCoroutine(PoisonDebuff());
        }
        else if (playerCollision.onWall == false)
        {
            isDebuff = false;
            StopCoroutine(PoisonDebuff());
        }
    }

    /// <summary>
    /// 독 데미지가 들어가는 주기에 맞춰서 독 디버프를 실행하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator PoisonDebuff()
    {
        while (isDebuff)
        {
            playerRpg.TakeDamage(poisionDamage); // PlayerRPG의 TakeDamage로 플레이어 체력 감소
            // 플레이어의 체력 감소
            yield return new WaitForSeconds(poisionDebuffTime); // 일정 시간 멈춤
            //yield return new WaitForSeconds(poisionDebuffTime*Time.deltaTime); // 일정 시간 멈춤
        }
    }
}
