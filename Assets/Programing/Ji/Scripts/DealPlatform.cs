using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;


/// <summary>
/// DealPlatform 오브젝트를 실행하는 스크립트
/// public void ShowPlatform() 함수를 사용하던가 혹은 DealPlatform를 지정하여 활성화 시키는 경우
/// 사용 가능
/// </summary>
public class DealPlatform : MonoBehaviour
{

    [SerializeField] float timer; // 인스펙터창에서 플랫폼이 보이는 시간을 조절
    float nowtTimer;
    // 특정시간 이후 깜빡이면서 사라지기 바라는 경우 코루틴으로 형성
    // 자연스러움을 위해 추가하였으나 기획의도가 아니라면 /**/표시 있는 구간을 삭제
    /**/
    [SerializeField] float spaceTime; // 깜빡임의 타이밍
    [SerializeField] float flashTime; // 깜빡이는 시간
    [SerializeField] SpriteRenderer spriteRenderer; // 발판의 이미지
    /**/

    /// <summary>
    /// 오브젝트 활성화 시 현재시간 설정
    /// </summary>
    private void OnEnable()
    {
        nowtTimer = timer;
        spriteRenderer.color = new Color(1, 1, 1, 1f); // 이미지의 투명도 100% 설정
    }
    private void Update()
    {
        nowtTimer -= Time.deltaTime; // Timer를 조절
        /**/
        if (nowtTimer < flashTime)
        {
            StartCoroutine(DisapperPlatform());
        }
        /**/
        if (nowtTimer < 0)
        {
            gameObject.SetActive(false); // 오브젝트 비활성화
            /**/
            StopCoroutine(DisapperPlatform());
            /**/
        }
    }

    /// <summary>
    /// PatternController.cs에서 플레이어가 약점을 공격할 수 있도록 일정시간동안
    /// DealPlatform을 표현하는 함수
    /// </summary>
    public void ShowPlatform()
    {
        gameObject.SetActive(true);
    }

    /**/
    /// <summary>
    /// 발판을 깜빡이는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DisapperPlatform()
    {
        while(true)
        {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f); // 이미지의 투명도 50% 설정
        yield return new WaitForSeconds(spaceTime); // 일정시간 유지
        spriteRenderer.color = new Color(1, 1, 1, 1f); // 이미지의 투명도 100% 설정
        yield return new WaitForSeconds(spaceTime); // 일정시간 유지
        }
    }
    /**/
}
